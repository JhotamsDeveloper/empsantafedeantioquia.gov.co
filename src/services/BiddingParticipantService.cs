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
    public interface IBiddingParticipantService
    {
        Task<IEnumerable<BiddingParticipant>> GetAll();
        Task<BiddingParticipantDTO> Details(int? id);
        Task<BiddingParticipantDTO> Create(BiddingParticipantCreateDTO model);
        Task<BiddingParticipant> GetById(int? id);
        Task DeleteConfirmed(int id);
        Boolean DuplicaIdentificationOrNit(string identificationOrNit);
    }

    public class BiddingParticipantService : IBiddingParticipantService
    {
        //Variables
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUploadedFileIIS _uploadedFileIIS;
        private readonly string _account = "filesBiddingParticipant";

        public BiddingParticipantService(ApplicationDbContext context,
                                        IMapper mapper,
                                        IUploadedFileIIS uploadedFileIIS)
        {
            _context = context;
            _mapper = mapper;
            _uploadedFileIIS = uploadedFileIIS;
        }

        public async Task<IEnumerable<BiddingParticipant>> GetAll()
        {
            return (await _context.BiddingParticipants
                          .Include(x=>x.Master)
                          .ToListAsync());
        }

        public async Task<BiddingParticipantDTO> Details(int? id)
        {
            var _biddingParticipantDTO = _mapper.Map<BiddingParticipantDTO>(
                    await _context.BiddingParticipants
                    .FirstOrDefaultAsync(m => m.Id == id)
                );

            //return (category);
            return _mapper.Map<BiddingParticipantDTO>(_biddingParticipantDTO);
        }

        public async Task<BiddingParticipantDTO> Create(BiddingParticipantCreateDTO model)
        {
            var _dateCreate = DateTime.Now;
            var _proposals = _uploadedFileIIS.UploadedFileImage(model.Proposals, _account);

            var _model = new BiddingParticipant
            {
                Id = model.Id,
                NaturalPerson = model.NaturalPerson,
                Name = model.Name,
                IdentificationOrNit = model.IdentificationOrNit.Trim(),
                Phone = model.Phone,
                Cellular = model.Cellular,
                Address = model.Address,
                City = model.City,
                Email = model.Email,
                DateCreate = _dateCreate,
                Proposals = _proposals,
                MasterId = model.MasterId
            };

            await _context.AddAsync(_model);
            await _context.SaveChangesAsync();

            return _mapper.Map<BiddingParticipantDTO>(_model);
        }

        public async Task<BiddingParticipant> GetById(int? id)
        {
            return await _context.BiddingParticipants.FirstOrDefaultAsync(x => x.Id == id);

            //return _mapper.Map<CategoryDto>(
            //await _context.Categorys.FindAsync(id)
            //);
        }

        public async Task DeleteConfirmed(int id)
        {
            var _shearchCoverPage = await _context.BiddingParticipants
                                .AsNoTracking()
                                .SingleAsync(x => x.Id == id);

            _uploadedFileIIS.DeleteConfirmed(_shearchCoverPage.Proposals, _account);

            _context.Remove(new BiddingParticipant
            {
                Id = id
            });

            await _context.SaveChangesAsync();

        }

        public Boolean DuplicaIdentificationOrNit(string identificationOrNit)
        {
            return _context.BiddingParticipants.Any(x => x.IdentificationOrNit == identificationOrNit);

        }

    }
}
