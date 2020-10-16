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
        Task<NacionLicitanteDto> Create(NacionLicitanteCreateDto model);
        Task<NacionLicitanteDto> Edit(int? id);
        Task Edit(int id, NacionLicitanteEditDto model);
        Task<Master> GetById(int? id);
        Task<Master> GetById(string nacionLicitante);
        Task DeleteConfirmed(int id);
        bool NacionLicitantegExists(int id);
        bool DuplicaName(string _stringName);
    }

    public class NacionLicitanteService: INacionLicitanteService
    {
        //Variables
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFormatStringUrl _formatStringUrl;
        private readonly IUploadedFileIIS _uploadedFileIIS;
        private readonly string _account = "nacionLicitante";

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
            return (await _context.Masters.ToListAsync());
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

        public async Task<NacionLicitanteDto> Create(NacionLicitanteCreateDto model)
        {
            var _dateCreate = DateTime.Now;
            var _urlCategory = _formatStringUrl.FormatString(model.NameMaster);
            var _coverPage = _uploadedFileIIS.UploadedFileImage(model.CoverPage, _account);
            var _file = _uploadedFileIIS.UploadedFileImage(model.NacionLicitantegFile, _account);

            var _master = new Master
            {
                Id = model.Id,
                NameMaster = model.NameMaster.Trim(),
                UrlMaster = _urlCategory,
                Description = model.Description.Trim(),
                CoverPage = _coverPage,
                Statud = model.Statud,
                NacionLicitante = true,
                NacionLicitantegStartDate = model.NacionLicitantegStartDate,
                NacionLicitanteEndDate = model.NacionLicitanteEndDate,
                NacionLicitantegFile = _file,
                DateCreate = _dateCreate
            };

            await _context.AddAsync(_master);
            await _context.SaveChangesAsync();

            return _mapper.Map<NacionLicitanteDto>(_master);
        }

        public async Task<NacionLicitanteDto> Edit(int? id)
        {
            return _mapper.Map<NacionLicitanteDto>(
                await _context.Masters.FindAsync(id));
        }

        public async Task Edit(int id, NacionLicitanteEditDto model)
        {
            var _coverPage = "";
            var _nacionLicitantegFile = "";
            var _dateUpdate = DateTime.Now;

            var _master = await _context.Masters.SingleAsync(x => x.Id == id);

            if (model.CoverPage != null)
            {
                _coverPage = _uploadedFileIIS.UploadedFileImage(_master.CoverPage, model.CoverPage, _account);
            }
            else
            {
                _coverPage = _master.CoverPage;
            }

            if (model.NacionLicitantegFile != null)
            {
                _nacionLicitantegFile = _uploadedFileIIS.UploadedFileImage(_master.NacionLicitantegFile, model.NacionLicitantegFile, _account);
            }
            else
            {
                _nacionLicitantegFile = _master.NacionLicitantegFile;
            }

            _master.Description = model.Description;
            _master.CoverPage = _coverPage;
            _master.Statud = model.Statud;
            _master.NacionLicitante = true;
            _master.NacionLicitantegStartDate = model.NacionLicitantegStartDate;
            _master.NacionLicitanteEndDate = model.NacionLicitanteEndDate;
            _master.NacionLicitantegFile = _nacionLicitantegFile;
            _master.DateUpdate = _dateUpdate;

            await _context.SaveChangesAsync();
        }

        public async Task<Master> GetById(int? id)
        {
            return await _context.Masters.FirstOrDefaultAsync(x => x.Id == id);

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
            var _shearch = await _context.Masters
                .AsNoTracking()
                .SingleAsync(x => x.Id == id);

            _uploadedFileIIS.DeleteConfirmed(_shearch.CoverPage, _account);
            _uploadedFileIIS.DeleteConfirmed(_shearch.NacionLicitantegFile, _account);

            _context.Remove(new Master
            {
                Id = id
            });

            await _context.SaveChangesAsync();

        }

        public bool NacionLicitantegExists(int id)
        {
            return _context.Masters.Any(e => e.Id == id);
        }

        public bool DuplicaName(string _stringName)
        {
            return _context.Masters.Any(e => e.NameMaster == _stringName);
        }
    }
}
