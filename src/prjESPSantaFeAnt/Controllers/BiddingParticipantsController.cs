using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using modelDTOs;
using persistenDatabase;
using prjESPSantaFeAnt.Models;
using services;

namespace prjESPSantaFeAnt.Controllers
{
    public class BiddingParticipantsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IBiddingParticipantService _biddingParticipantService;
        private readonly INacionLicitanteService _nacionLicitante;

        public BiddingParticipantsController(ApplicationDbContext context,
                                                IBiddingParticipantService biddingParticipantService,
                                                INacionLicitanteService nacionLicitante)
        {
            _context = context;
            _biddingParticipantService = biddingParticipantService;
            _nacionLicitante = nacionLicitante;
        }

        // GET: BiddingParticipants
        public async Task<IActionResult> Index()


        {

            var _biddingParticipantGetAll = await _biddingParticipantService.GetAll();
            var _nacionLicitanteGellGetAll = await _nacionLicitante.GetAll();

            var _model = from a in _biddingParticipantGetAll
                         from b in _nacionLicitanteGellGetAll
                         where a.MasterId == b.Id
                         select new ModelViewBiddingParticipant
                         {
                             Id = a.Id,
                             NaturalPerson = a.NaturalPerson,
                             Name = a.Name,
                             IdentificationOrNit = a.IdentificationOrNit,
                             Email = a.Email,
                             City = a.City,
                             Proposals = a.Proposals,
                             DateCreate = a.DateCreate.ToString("MMMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO")),
                             NameNocionLicitante = b.NameMaster
                         };

            return View(_model);

        }

        // GET: BiddingParticipants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var _getById = await _biddingParticipantService.GetById(id);
            var _nacionLicitanteGellGetById = await _nacionLicitante.GetById(_getById.MasterId);

            if (_getById == null)
            {
                return NotFound();
            }

            var _dateCreate = _getById.DateCreate.ToString("MMMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO"));

            var _model = new ModelViewBiddingParticipant
            {
                Id = _getById.Id,
                NaturalPerson = _getById.NaturalPerson,
                Name = _getById.Name,
                IdentificationOrNit = _getById.IdentificationOrNit,
                Phone = _getById.Phone,
                Cellular = _getById.Cellular,
                Address = _getById.Address,
                City = _getById.City,
                Email = _getById.Email,
                DateCreate = _dateCreate,
                Proposals = _getById.Proposals,
                NameNocionLicitante = _nacionLicitanteGellGetById.NameMaster
            };

            return View(_model);
        }

        // GET: BiddingParticipants/Create
        public async Task<IActionResult> Create(int idConvocatoria)
        {
            var _nameLicitante = await _nacionLicitante.GetById(idConvocatoria);

            ViewData["idConvocatoria"] = _nameLicitante.Id;
            ViewData["nameLicitante"] = _nameLicitante.NameMaster;
            return View();
        }

        // POST: BiddingParticipants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BiddingParticipantCreateDTO model)
        {
            var _identificationOrNit = _biddingParticipantService.DuplicaIdentificationOrNit(model.IdentificationOrNit);

            if (_identificationOrNit)
            {
                ViewData["DuplicaIdentificationOrNit"] = $"Este documento {model.IdentificationOrNit} ya ha sido utilizado, cambielo";
                return View(model);
            }

            if (ModelState.IsValid)
            {
                var result = await _biddingParticipantService.Create(model);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: BiddingParticipants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var _getById = await _biddingParticipantService.GetById(id);
            var _nacionLicitanteGellGetById = await _nacionLicitante.GetById(_getById.MasterId);

            if (_getById == null)
            {
                return NotFound();
            }

            var _dateCreate = _getById.DateCreate.ToString("MMMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO"));

            var _model = new ModelViewBiddingParticipant
            {
                Id = _getById.Id,
                NaturalPerson = _getById.NaturalPerson,
                Name = _getById.Name,
                IdentificationOrNit = _getById.IdentificationOrNit,
                Phone = _getById.Phone,
                Cellular = _getById.Cellular,
                Address = _getById.Address,
                City = _getById.City,
                Email = _getById.Email,
                DateCreate = _dateCreate,
                Proposals = _getById.Proposals,
                NameNocionLicitante = _nacionLicitanteGellGetById.NameMaster
            };

            ViewData["detail"] = true;
            return View(_model);
        }

        // POST: BiddingParticipants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _biddingParticipantService.DeleteConfirmed(id);

            return RedirectToAction(nameof(Index));
        }

        private bool DuplicaIdentificationOrNit(string identificationOrNit)
        {
            return _biddingParticipantService.DuplicaIdentificationOrNit(identificationOrNit);
        }

        [ActionName("Documento-participante")]
        public FileResult OfficialLicitante(string nameFile, string routeFile)
        {
            return File($"/images/filesBiddingParticipant/{routeFile}", "application/pdf", $"{nameFile}-{DateTime.Now.ToString()}.pdf");
        }
    }
}