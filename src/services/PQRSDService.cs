using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using model;
using modelDTOs;
using persistenDatabase;
using services.Commons;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace services
{
    public interface IPQRSDService
    {
        IEnumerable<PQRSD> GetAll();
        PQRSDDto Details(Guid? id);
        Task<PQRSDDto> Create(PQRSDCreateDto model);
        PQRSD GetById(Guid? id);
        Task DeleteConfirmed(Guid id);
        Boolean Review(Guid id, ReviewCreateDto model);
    }

    public class PQRSDService : IPQRSDService
    {
        //Variables
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFormatStringUrl _formatStringUrl;
        private readonly IUploadedFileIIS _uploadedFileIIS;
        private readonly IEmailSendGrid _emailSendGrid;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly string _account = "PQRSD";

        public PQRSDService(ApplicationDbContext context,
                                IMapper mapper,
                                IHostingEnvironment hostingEnvironment,
                                IFormatStringUrl formatStringUrl,
                                IEmailSendGrid emailSendGrid,
                                IUploadedFileIIS uploadedFileIIS)
        {
            _context = context;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
            _formatStringUrl = formatStringUrl;
            _emailSendGrid = emailSendGrid;
            _uploadedFileIIS = uploadedFileIIS;
        }

        public IEnumerable<PQRSD> GetAll()
        {
            var _getAll = _context
                .PQRSDs;

            return _getAll.ToList();
        }

        public PQRSDDto Details(Guid? id)
        {
            var _pqrsd = _mapper.Map<PQRSDDto>(
                    _context.PQRSDs
                    .FirstOrDefault(m => m.PQRSDID == id)
                );

            return _mapper.Map<PQRSDDto>(_pqrsd);
        }

        public async Task<PQRSDDto> Create(PQRSDCreateDto model)
        {
            var _dateCreate = DateTime.Now;
            //var _file = _uploadedFileIIS.UploadedFileImage(model.File, _account);
            var _pqrsdID = Guid.NewGuid();

            var _pqrsd = new PQRSD
            {
                PQRSDID = _pqrsdID,
                NamePerson = model.NamePerson,
                Email = model.Email,
                PQRSDName = model.PQRSDName,
                Description = model.Description,
                NameSotypeOfRequest = model.NameSotypeOfRequest,
                DateCreate = _dateCreate
            };

            await _context.AddAsync(_pqrsd);
            await _context.SaveChangesAsync();

            var _body = CreateBody(model.NameSotypeOfRequest, model.NamePerson, model.Email, model.PQRSDName, model.Description) ;

            await _emailSendGrid.Execute(model.NameSotypeOfRequest, _body, model.Email);
            await _emailSendGrid.Execute(model.NameSotypeOfRequest, _body, "asesoria@espsantafedeantioquia.co");
            await _emailSendGrid.Execute(model.NameSotypeOfRequest, _body, "admin@espsantafedeantioquia.co");

            return _mapper.Map<PQRSDDto>(_pqrsd);
        }

        public Boolean Review(Guid id, ReviewCreateDto model)
        {
            if (id != null)
            {
                var _review = _context.PQRSDs.Single(x => x.PQRSDID == id);

                _review.Reply = model.Reply;
                _review.IsAnswered = true;
                _review.AnswerDate = DateTime.Now;

                _context.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }

        public PQRSD GetById(Guid? id)
        {
            return _context.PQRSDs.FirstOrDefault(x => x.PQRSDID== id);

            //return _mapper.Map<CategoryDto>(
            //await _context.Categorys.FindAsync(id)
            //);
        }

        public async Task DeleteConfirmed(Guid id)
        {
            var _shearchFile = await _context.PQRSDs
                .AsNoTracking()
                .SingleAsync(x => x.PQRSDID == id);

            _context.Remove(new PQRSD
            {
                PQRSDID = id
            }) ;

            await _context.SaveChangesAsync();
        }

        private string CreateBody(string _pqrsd, string _name,
            string _email, string _asunto, string _description)
        {
            var _body = string.Empty;

            //using StreamReader reader = new StreamReader()

            using (var _reader = new StreamReader(Path.Combine(_hostingEnvironment.WebRootPath, "emailTemplete", "PQRSDEmail.html")))
            {
                _body = _reader.ReadToEnd();
            };

            _body = _body.Replace("{PQRSD}", _pqrsd); //replacing Parameters

            _body = _body.Replace("{Name}", _name);
            _body = _body.Replace("{Email}", _email);
            _body = _body.Replace("{Asunto}", _asunto);
            _body = _body.Replace("{Description}", _description);

            return _body;
        }

    }
}
