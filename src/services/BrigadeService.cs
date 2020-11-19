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
    public interface IBrigadeService
    {
        Task<IEnumerable<Master>> GetAll();
        Task<BrigadeDto> Details(int? id);
        Task<BrigadeDto> Create(BrigadeCreateDto model);
        Task<Master> GetById(int? id);
        Task<Master> GetById(string urBrigade);
        Task DeleteConfirmed(int id);
        bool BrigadeExists(int id);
        bool DuplicaName(string _stringName);
    }

    public class BrigadeService : IBrigadeService
    {
        //Variables
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFormatStringUrl _formatStringUrl;
        private readonly IUploadedFileIIS _uploadedFileIIS;
        private readonly string _account = "brigade";

        public BrigadeService(ApplicationDbContext context,
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
            var _brigade = await _context.Masters
                .AsNoTracking()
                .Where(x => x.Brigade == true)
                .OrderByDescending(x => x.DateCreate)
                .ToListAsync();
            return (_brigade);
        }

        public async Task<BrigadeDto> Details(int? id)
        {
            var _bragade = _mapper.Map<BrigadeDto>(
                    await _context.Masters
                    .FirstOrDefaultAsync(m => m.Id == id)
                );

            return _mapper.Map<BrigadeDto>(_bragade);
        }

        public async Task<BrigadeDto> Create(BrigadeCreateDto model)
        {
            var _dateCreate = DateTime.Now;
            var _urlCategory = _formatStringUrl.FormatString(model.NameMaster);
            var _coverPage = _uploadedFileIIS.UploadedFileImage(model.CoverPage, _account);

            var _brigade = new Master
            {
                Id = model.Id,
                NameMaster = model.NameMaster.Trim(),
                UrlMaster = _urlCategory,
                Description = model.Description.Trim(),
                CoverPage = _coverPage,
                Statud = true,
                Brigade = true,
                Author = model.Author,
                DateBrigade = model.DateBrigade,
                DateCreate = _dateCreate,
            };

            await _context.AddAsync(_brigade);
            await _context.SaveChangesAsync();

            return _mapper.Map<BrigadeDto>(_brigade);
        }

        public async Task<Master> GetById(int? id)
        {
            return await _context.Masters.FirstOrDefaultAsync(x => x.Id == id);

            //return _mapper.Map<CategoryDto>(
            //await _context.Categorys.FindAsync(id)
            //);
        }

        public async Task<Master> GetById(string urBrigade)
        {
            return await _context.Masters.FirstOrDefaultAsync(x => x.UrlMaster == urBrigade);

            //return _mapper.Map<CategoryDto>(
            //await _context.Categorys.FindAsync(id)
            //);
        }

        public async Task DeleteConfirmed(int id)
        {
            var _shearchCoverPage = await _context.Masters
                .AsNoTracking()
                .SingleAsync(x => x.Id == id);

            _uploadedFileIIS.DeleteConfirmed(_shearchCoverPage.CoverPage, _account);

            _context.Remove(new Master
            {
                Id = id
            });

            await _context.SaveChangesAsync();

        }

        public bool BrigadeExists(int id)
        {
            return _context.Masters.Any(e => e.Id == id);
        }

        public bool DuplicaName(string _stringName)
        {
            return _context.Masters.Any(e => e.NameMaster == _stringName);
        }
    }
}
