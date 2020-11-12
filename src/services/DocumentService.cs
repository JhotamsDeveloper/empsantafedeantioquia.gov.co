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
        Task Edit(int id, DocumentEditDTO model);
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
                var _dateCreate = DateTime.Now;
                var _urlName = _formatStringUrl.FormatString(model.Name);

                var _model = new Document
                {
                    Name = model.Name.Trim(),
                    UrlName = _urlName,
                    NameProyect = model.NameProyect,
                    Description = model.Description,
                    CreateDate = _dateCreate,
                };


                await _context.AddAsync(_model);
                await _context.SaveChangesAsync();

                _mapper.Map<DocumentDTO>(_model);

                if (model.RouteFile != null)
                {
                    ICollection<string> _documents = _uploadedFileIIS.UploadedMultipleFileImage(model.RouteFile, _account);

                    foreach (var item in _documents)
                    {
                        var _fileDocuments = new FileDocument
                        {
                            NameFile = model.Name + DateTime.Now.ToString(),
                            RouteFile = item,
                            DocumentoId = _model.ID
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
        public async Task Edit(int id, DocumentEditDTO model)
        {

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var _dateUpdate = DateTime.Now;
                var _urlName = _formatStringUrl.FormatString(model.Name);
                var _id = id;

                //Eliminar los documentos existentes
                var _fileDocuments = _context.Documents
                                    .Include(x => x.FileDocument)
                                    .Where(x => x.ID == _id)
                                    .ToListAsync();

                if (_fileDocuments != null)
                {
                    foreach (var files in await _fileDocuments)
                    {

                        if (files.FileDocument !=  null)
                        {
                            foreach (var item in files.FileDocument)
                            {
                                _uploadedFileIIS.DeleteConfirmed(item.RouteFile, _account);

                                _context.Remove(new FileDocument
                                {
                                    ID = item.ID
                                });

                                await _context.SaveChangesAsync();
                            }
                        }

                    }
                }

                //Cargar los documentos
                if (model.FileDocument != null)
                {
                    ICollection<string> _documents = _uploadedFileIIS.UploadedMultipleFileImage(model.RouteFile, _account);

                    foreach (var item in _documents)
                    {
                        var _fileDoc = new FileDocument
                        {
                            NameFile = model.Name + DateTime.Now.ToString(),
                            RouteFile = item,
                            DocumentoId = model.ID
                        };

                        await _context.AddAsync(_fileDoc);
                        await _context.SaveChangesAsync();
                    }

                }

                //Actualizar Datos
                var _save = await _context.Documents.SingleAsync(x => x.ID == _id);

                _save.Name = model.Name;
                _save.NameProyect = model.NameProyect;
                _save.Description = model.Description;
                _save.DateUpdate = _dateUpdate;

                await _context.SaveChangesAsync();

                transaction.Commit();

            }
            catch (Exception ex)
            {
                transaction.Rollback();
            }

        }

        public async Task DeleteConfirmed(int _id)
        {

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                //Eliminar los documentos existentes

                var _fileDocuments = _context.FileDocuments
                    .AsNoTracking()
                    .Where(x => x.ID == _id).ToListAsync();

                if (await _fileDocuments != null)
                {
                    foreach (var files in await _fileDocuments)
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
