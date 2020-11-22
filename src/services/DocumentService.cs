using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using model;
using modelDTOs;
using persistenDatabase;
using services.Commons;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace services
{
    public interface IDocumentService
    {
        Task<IEnumerable<Document>> GetAll();
        Task<DocumentDTO> Details(int? id);
        Task Create(DocumentCreateDTO model);
        Task<Document> GetById(int? id);
        Task<Document> GetById(string _urlName);
        Task DeleteConfirmed(int _id);
        bool DocumentExists(int id);
        bool DuplicaName(string _stringName);
        Task<IEnumerable<FileDocument>> FilesDocuments();

    }
    public class DocumentService : IDocumentService
    {
        //Variables
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUploadedFileIIS _uploadedFileIIS;
        private readonly IFormatStringUrl _formatStringUrl;
        private readonly string _account = "filesDocuments";

        public DocumentService(ApplicationDbContext context,
                                IMapper mapper,
                                IHostingEnvironment hostingEnvironment,
                                IFormatStringUrl formatStringUrl,
                                IUploadedFileIIS uploadedFileIIS)
        {
            _context = context;
            _mapper = mapper;
            _formatStringUrl = formatStringUrl;
            _uploadedFileIIS = uploadedFileIIS;
        }

        public async Task<IEnumerable<Document>> GetAll()
        {
            return (await _context.Documents
                          .Include(x => x.FileDocument)
                          .OrderByDescending(x => x.CreateDate)
                          .ToListAsync());
        }

        public async Task<DocumentDTO> Details(int? id)
        {
            var _documentDTO = _mapper.Map<DocumentDTO>(
                    await _context.Documents
                    .FirstOrDefaultAsync(m => m.ID == id)
                );

            return _mapper.Map<DocumentDTO>(_documentDTO);
        }

        public async Task Create(DocumentCreateDTO model)
        {

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                int _id;
                var _dateCreate = DateTime.Now;
                var _urlName = _formatStringUrl.FormatString(model.Name);

                var _nameProyect = await _context.Masters
                    .FirstOrDefaultAsync(x => x.Id == model.MasterId);

                if (_nameProyect != null)
                {
                    var _model = new Document
                    {
                        Name = model.Name.Trim(),
                        UrlName = _urlName,
                        NameProyect = _nameProyect.NameMaster,
                        Description = model.Description,
                        CreateDate = _dateCreate,
                        MasterId = model.MasterId
                    };


                    await _context.AddAsync(_model);
                    await _context.SaveChangesAsync();

                    _mapper.Map<DocumentDTO>(_model);
                    _id = _model.ID;
                }
                else
                {
                    var _model = new Document
                    {
                        Name = model.Name.Trim(),
                        UrlName = _urlName,
                        NameProyect = "others",
                        Description = model.Description,
                        CreateDate = _dateCreate
                    };

                    await _context.AddAsync(_model);
                    await _context.SaveChangesAsync();

                    _mapper.Map<DocumentDTO>(_model);
                    _id = _model.ID;
                }

                if (model.RouteFile != null)
                {

                    foreach (var item in model.RouteFile)
                    {
                        string _nameFile = System.IO.Path.GetFileNameWithoutExtension(item.FileName);
                        string _nameFileFormat = _formatStringUrl.FormatString(_nameFile);

                        var _routeFile = _uploadedFileIIS.UploadedFileImage(item, _nameFileFormat, _account);

                        var _fileDocuments = new FileDocument
                        {
                            NameFile = model.Name,
                            RouteFile = _routeFile,
                            DocumentoId = _id
                        };

                        await _context.AddAsync(_fileDocuments);
                        await _context.SaveChangesAsync();
                    }

                }


                transaction.Commit();

            }catch (Exception ex)
            {
                transaction.Rollback();
            }

        }

        public async Task<Document> GetById(int? id)
        {
            return await _context.Documents
                .Include(x => x.FileDocument)
                .FirstOrDefaultAsync(x => x.ID == id);
        }

        public async Task<Document> GetById(string _urlName)
        {
            return await _context.Documents
                .Include(x => x.FileDocument)
                .FirstOrDefaultAsync(x => x.UrlName == _urlName);
        }

        public async Task DeleteConfirmed(int _id)
        {

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                //Eliminar los documentos existentes

                var _document = await _context.Documents
                    .AsNoTracking()
                    .Include(x=>x.FileDocument)
                    .FirstOrDefaultAsync(x => x.ID == _id);

                if (_document.FileDocument != null)
                {
                    foreach (var files in _document.FileDocument)
                    {

                        _uploadedFileIIS.DeleteConfirmed(files.RouteFile, _account);

                        _context.Remove(new FileDocument
                        {
                            ID = files.ID
                        });

                        await _context.SaveChangesAsync();

                    }
                }

                _context.Remove(new Document
                {
                    ID = _id
                });

                await _context.SaveChangesAsync();

            transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
            }
        }

        public bool DocumentExists(int id)
        {
            return _context.Documents.Any(e => e.ID == id);
        }

        public bool DuplicaName(string _stringName)
        {
            return _context.Documents.Any(e => e.Name == _stringName);
        }

        public async Task<IEnumerable<FileDocument>> FilesDocuments()
        {
            return (await _context.FileDocuments
                          .AsNoTracking()
                          .ToListAsync());
        }
    }
}
