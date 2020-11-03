using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using model;
using modelDTOs;
using persistenDatabase;
using services.Commons;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
        Task<BiddingParticipant> GetByRef(string reference);
        Task DeleteConfirmed(int id);
        Boolean DuplicaIdentificationOrNit(string identificationOrNit, int masterId);
    }

    public class BiddingParticipantService : IBiddingParticipantService
    {
        //Variables
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUploadedFileIIS _uploadedFileIIS;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IEmailSendGrid _emailSendGrid;
        private readonly string _account = "filesBiddingParticipant";

        public BiddingParticipantService(ApplicationDbContext context,
                                        IMapper mapper,
                                        IHostingEnvironment hostingEnvironment,
                                        IEmailSendGrid emailSendGrid,
                                        IUploadedFileIIS uploadedFileIIS)
        {
            _context = context;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
            _emailSendGrid = emailSendGrid;
            _uploadedFileIIS = uploadedFileIIS;
        }

        public async Task<IEnumerable<BiddingParticipant>> GetAll()
        {
            return (await _context.BiddingParticipants
                          .Include(x => x.Master)
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
            var _dateCreateFormat = DateTime.Now.ToString("MMMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO"));
            var _ref = Guid.NewGuid();
            var _proposals = _uploadedFileIIS.UploadedFileImage(model.Proposals, _account);

            var _model = new BiddingParticipant
            {
                Id = model.Id,
                Ref = _ref,
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

            var _body = CreateBody(model.Name, model.IdentificationOrNit, model.Phone,
                model.Cellular, model.Address, model.City, model.Email,
                _dateCreateFormat, _ref.ToString().ToUpper());

            await _emailSendGrid.Execute("Registro Satisfactorio", _body, model.Email);
            await _emailSendGrid.Execute("Registro Satisfactorio", _body, "asesoria@espsantafedeantioquia.co");
            await _emailSendGrid.Execute("Registro Satisfactorio", _body, "gerencia@espsantafedeantioquia.co");

            await _context.AddAsync(_model);
            await _context.SaveChangesAsync();

            return _mapper.Map<BiddingParticipantDTO>(_model);
        }

        public async Task<BiddingParticipant> GetById(int? id)
        {
            return await _context.BiddingParticipants
                .Include(x=>x.Master)
                .FirstOrDefaultAsync(x => x.Id == id);

            //return _mapper.Map<CategoryDto>(
            //await _context.Categorys.FindAsync(id)
            //);
        }

        public async Task<BiddingParticipant> GetByRef(string reference)
        {
            var _reference = new Guid(reference);

            return await _context.BiddingParticipants
                .Include(x => x.Master)
                .FirstOrDefaultAsync(x => x.Ref == _reference);

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

        public Boolean DuplicaIdentificationOrNit(string identificationOrNit, int masterId)
        {
            var _any = false;
            if (_context.BiddingParticipants.Any(x => x.MasterId == masterId))
            {
                _any = _context.BiddingParticipants.Any(x => x.IdentificationOrNit == identificationOrNit);
            }
            return _any;
        }

        private string CreateBody(string name, string id,
            string phone, string cellular, string address,
            string city, string email, string date,
            string reference)
        {
            var _body = string.Empty;

            //using StreamReader reader = new StreamReader()

            using (var _reader = new StreamReader(Path.Combine(_hostingEnvironment.WebRootPath, "emailTemplete", "ParticipantDataEmail.html"))){
                _body = _reader.ReadToEnd();
            };

            _body = _body.Replace("{name}", name); //replacing Parameters

            _body = _body.Replace("{ID}", id);
            _body = _body.Replace("{phone}", phone);
            _body = _body.Replace("{cellular}", cellular);
            _body = _body.Replace("{address}", address);
            _body = _body.Replace("{city}", city);
            _body = _body.Replace("{email}", email);
            _body = _body.Replace("{date}", date);
            _body = _body.Replace("{reference}", reference);

            return _body;
        }
    }
}
