using AutoMapper;
using model;
using modelDTOs;
using persistenDatabase;
using services.Commons;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace services
{
    public interface IPQRSDService
    {
        Task<IEnumerable<PQRSD>> GetAll();
        Task<PQRSDDto> Details(Guid? id);
        Task<PQRSDDto> Create(PQRSDCreateDto model);
        Task<PQRSD> GetById(Guid? id);
        Task DeleteConfirmed(Guid? id);
        bool PQRSDExists(Guid id);
    }

    public class PQRSDService : IPQRSDService
    {
        //Variables
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFormatStringUrl _formatStringUrl;
        private readonly IUploadedFileIIS _uploadedFileIIS;
        private readonly string _account = "PQRSD";

        public PQRSDService(ApplicationDbContext context,
                                IMapper mapper,
                                IFormatStringUrl formatStringUrl,
                                IUploadedFileIIS uploadedFileIIS)
        {
            _context = context;
            _mapper = mapper;
            _formatStringUrl = formatStringUrl;
            _uploadedFileIIS = uploadedFileIIS;
        }

        public async Task<IEnumerable<PQRSD>> GetAll()
        {
            var _pqrsd = await _context.PQRSDs
                .AsNoTracking()
                .ToListAsync();
            return (_pqrsd);
        }

        public async Task<PQRSDDto> Details(Guid? id)
        {
            var _pqrsd = _mapper.Map<PQRSDDto>(
                    await _context.PQRSDs
                    .FirstOrDefaultAsync(m => m.PQRSDID == id)
                );

            return _mapper.Map<PQRSDDto>(_pqrsd);
        }

        public async Task<PQRSDDto> Create(PQRSDCreateDto model)
        {
            var _dateCreate = DateTime.Now;
            var _file = _uploadedFileIIS.UploadedFileImage(model.File, _account);

            var _pqrsd = new PQRSD
            {
                PQRSDID = model.PQRSDID,
                NamePerson = model.NamePerson,
                Email = model.Email,
                PQRSDName = model.PQRSDName,
                Description = model.Description,
                NameSotypeOfRequest = model.NameSotypeOfRequest,
                DateCreate = _dateCreate,
                File = _file,
            };

            await _context.AddAsync(_pqrsd);
            await _context.SaveChangesAsync();

            return _mapper.Map<PQRSDDto>(_pqrsd);
        }

        public async Task<PQRSD> GetById(Guid? id)
        {
            return await _context.PQRSDs.FirstOrDefaultAsync(x => x.PQRSDID== id);

            //return _mapper.Map<CategoryDto>(
            //await _context.Categorys.FindAsync(id)
            //);
        }

        public async Task DeleteConfirmed(Guid? id)
        {
            var _shearchFile = await _context.PQRSDs
                .AsNoTracking()
                .SingleAsync(x => x.PQRSDID == id);

            var _id = Guid.Parse(id.ToString());

            _uploadedFileIIS.DeleteConfirmed(_shearchFile.File, _account);

            _context.Remove(new PQRSD
            {
                PQRSDID = _id
            }) ;

            await _context.SaveChangesAsync();
        }

        public bool PQRSDExists(Guid id)
        {
            return _context.PQRSDs.Any(e => e.PQRSDID == id);
        }
    }
}
