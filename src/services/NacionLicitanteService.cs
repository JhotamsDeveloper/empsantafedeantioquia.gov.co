using AutoMapper;
using Microsoft.EntityFrameworkCore;
using model;
using modelDTOs;
using persistenDatabase;
using services.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace services
{
    public interface INacionLicitanteService
    {
        Task<IEnumerable<Master>> GetAll();
        Task<NacionLicitanteDto> Details(int? id);
        Task Create(NacionLicitanteCreateDto model);
        Task<Master> GetById(int? id);
        Task<Master> GetById(string nacionLicitante);
        Task DeleteConfirmed(int id);
        bool NacionLicitantegExists(int id);
        bool DuplicaName(string _stringName);
        Task<IEnumerable<FileDocument>> FilesDocuments();
        Task<IEnumerable<Document>> DelatedDocuments(int id);
    }

    public class NacionLicitanteService: INacionLicitanteService
    {
        //Variables
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFormatStringUrl _formatStringUrl;
        private readonly IUploadedFileIIS _uploadedFileIIS;
        private readonly string _account = "nacionLicitante";
        private readonly string _filesDocuments = "filesDocuments";

        public NacionLicitanteService(ApplicationDbContext context,
                                IMapper mapper,
                                IFormatStringUrl formatStringUrl,
                                IUploadedFileIIS uploadedFileIIS)
        {
            _context = context;
            _mapper = mapper;
            _formatStringUrl = formatStringUrl;
            _uploadedFileIIS = uploadedFileIIS;
        }

        public async Task<IEnumerable<Master>> GetAll()
        {
            return (await _context.Masters
                          .AsNoTracking()
                         .Where(x=>x.NacionLicitante == true)
                         .ToListAsync());
        }

        public async Task<NacionLicitanteDto> Details(int? id)
        {
            var _nacionLicitante = _mapper.Map<NacionLicitanteDto>(
                    await _context.Masters
                    .FirstOrDefaultAsync(m => m.Id == id)
                );

            //return (category);
            return _mapper.Map<NacionLicitanteDto>(_nacionLicitante);
        }

        public async Task Create(NacionLicitanteCreateDto model)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var _dateCreate = DateTime.Now;
                var _urlCategory = _formatStringUrl.FormatString(model.NameMaster);
                var _coverPage = _uploadedFileIIS.UploadedFileImage(model.CoverPage, _account);

                var _master = new Master
                {
                    Id = model.Id,
                    NameMaster = model.NameMaster.Trim(),
                    UrlMaster = _urlCategory,
                    Description = model.Description.Trim(),
                    CoverPage = _coverPage,
                    Statud = true,
                    NacionLicitante = true,
                    NacionLicitantegStartDate = model.NacionLicitantegStartDate,
                    NacionLicitanteEndDate = model.NacionLicitanteEndDate,
                    DateCreate = _dateCreate
                };

                await _context.AddAsync(_master);
                await _context.SaveChangesAsync();

                _mapper.Map<NacionLicitanteDto>(_master);

                if (model.NacionLicitantegFile != null)
                {
                    foreach (var item in model.NacionLicitantegFile)
                    {
                        string _nameFile = System.IO.Path.GetFileNameWithoutExtension(item.FileName);
                        string _nameFileFormat = _formatStringUrl.FormatString(_nameFile);

                        var _urlNameFile = _formatStringUrl.FormatString(_nameFileFormat);
                        var _file = _uploadedFileIIS.UploadedFileImage(item, _urlNameFile, _filesDocuments);

                        var _fileDocuments = new FileDocument
                        {
                            NameFile = model.NameMaster,
                            RouteFile = _file,
                            MasterId = _master.Id
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

        public async Task<Master> GetById(int? id)
        {
            return await _context.Masters
                .Include(x=>x.FileDocument)
                .FirstOrDefaultAsync(x => x.Id == id);

            //return _mapper.Map<CategoryDto>(
            //await _context.Categorys.FindAsync(id)
            //);
        }

        public async Task<Master> GetById(string nacionLicitante)
        {
            return await _context.Masters.FirstOrDefaultAsync(x => x.UrlMaster == nacionLicitante);

            //return _mapper.Map<CategoryDto>(
            //await _context.Categorys.FindAsync(id)
            //);
        }

        public async Task DeleteConfirmed(int id)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {

                var _shearch = await _context.Masters
                .Include(a=>a.Documents)
                .Include(b=>b.FileDocument)
                .AsNoTracking()
                .SingleAsync(c => c.Id == id);

                var _deleteDocumet = await _context.Documents
                    .AsNoTracking()
                    .Where(x => x.MasterId == _shearch.Id)
                    .ToListAsync();

                var _fileDocument = await _context.FileDocuments
                    .AsNoTracking()
                    .Where(x=>x.MasterId == _shearch.Id)
                    .ToListAsync();

                if (_shearch.CoverPage != null)
                {
                    _uploadedFileIIS.DeleteConfirmed(_shearch.CoverPage, _account);
                }

                if (_deleteDocumet != null)
                {
                    foreach (var item in _deleteDocumet)
                    {
                        var _deleteFileDocument = await _context.FileDocuments
                            .AsNoTracking()
                            .Where(x => x.DocumentoId == item.ID)
                            .ToListAsync();

                        foreach (var _deleteFiles in _deleteFileDocument)
                        {
                            _uploadedFileIIS.DeleteConfirmed(_deleteFiles.RouteFile, _filesDocuments);

                            _context.Remove(new FileDocument
                            {
                                ID = _deleteFiles.ID
                            });
                            await _context.SaveChangesAsync();
                        }

                        _context.Remove(new Document
                        {
                            ID = item.ID
                        });
                        await _context.SaveChangesAsync();
                    }
                }

                if (_fileDocument != null)
                {
                    foreach (var item in _fileDocument)
                    {
                        _uploadedFileIIS.DeleteConfirmed(item.RouteFile, _filesDocuments);

                        _context.Remove(new FileDocument
                        {
                            ID = item.ID
                        });
                        await _context.SaveChangesAsync();
                    }
                }

                _context.Remove(new Master
                {
                    Id = id
                });
                await _context.SaveChangesAsync();


                transaction.Commit();

            }
            catch (Exception ex)
            {
                transaction.Rollback();
            }
        }

        public bool NacionLicitantegExists(int id)
        {
            return _context.Masters.Any(e => e.Id == id);
        }

        public bool DuplicaName(string _stringName)
        {
            return _context.Masters.Any(e => e.NameMaster == _stringName);
        }

        public async Task<IEnumerable<FileDocument>> FilesDocuments()
        {
            return (await _context.FileDocuments
                          .AsNoTracking()
                          .ToListAsync());
        }

        public async Task<IEnumerable<Document>> DelatedDocuments(int id)
        {
            return (await _context.Documents
                          .AsNoTracking()
                          .Where(x=>x.MasterId == id)
                          .ToListAsync());
        }
    }
}
