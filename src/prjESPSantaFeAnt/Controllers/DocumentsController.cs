using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using model;
using modelDTOs;
using persistenDatabase;
using prjESPSantaFeAnt.Models;
using services;

namespace prjESPSantaFeAnt.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly IDocumentService _documentService;
        private readonly ApplicationDbContext _context; 

        public DocumentsController(IDocumentService documentService,
            ApplicationDbContext context)
        {
            _documentService = documentService;
            _context = context;
        }

        // GET: Documents
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> Index()
        {
            var _documents = from a in await _documentService.GetAll()
                             select new ModelViewDocument
                             {
                                ID = a.ID,
                                Name = a.Name,
                                NameProyect = a.NameProyect,
                                Description = a.Description,
                                CreateDate = a.CreateDate.ToString("MMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO"))
                             };

            return View(_documents);
        }

        // GET: Documents/Details/5
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var _document = await _documentService.GetById(id);
            var _filesDocuments = from a in await _documentService.FilesDocuments()
                                  where a.DocumentoId == _document.ID
                                  select a;

            var _model = new ModelViewDocument
            {
                ID = _document.ID,
                Name = _document.Name,
                NameProyect = _document.NameProyect,
                Description = _document.Description,
                CreateDate = _document.CreateDate.ToString("MMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO")),
                DateUpdate = _document.DateUpdate.ToString("MMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO")),
                FileDocument = _filesDocuments
            };

            if (_model == null)
            {
                return NotFound();
            }

            ViewData["detail"] = true;

            return View(_model);
        }

        // GET: Documents/Details/5
        [Route("documentos-legales/{urlName}")]
        public async Task<IActionResult> Details(string urlName)
        {
            if (urlName == "")
            {
                return NotFound();
            }

            var _document = await _documentService.GetById(urlName);
            var _filesDocuments = from a in await _documentService.FilesDocuments()
                                  where a.DocumentoId == _document.ID
                                  select a;

            var _model = new ModelViewDocument
            {
                ID = _document.ID,
                Name = _document.Name,
                NameProyect = _document.NameProyect,
                Description = _document.Description,
                CreateDate = _document.CreateDate.ToString("MMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO")),
                DateUpdate = _document.DateUpdate.ToString("MMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO")),
                FileDocument = _filesDocuments
            };

            if (_model == null)
            {
                return NotFound();
            }

            ViewData["detail"] = false;

            return View(_model);
        }

        // GET: Documents/Create
        [Authorize(Roles = "SuperAdmin,Admin")]
        public IActionResult Create()
        {
            ViewData["NameLicitante"] = new SelectList(_context.Masters
                .Where(x=>x.NacionLicitante == true)
                .OrderByDescending(x=>x.DateCreate),"Id", "NameMaster");

            return View();
        }

        // POST: Documents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DocumentCreateDTO model)
        {
            if (_documentService.DuplicaName(model.Name))
            {
                ViewData["DuplicaName"] = "El nombre ya esta utilizada, favor cambiarlo";
                return View(model);
            }

            if (ModelState.IsValid)
            {
                await _documentService.Create(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Documents/Delete/5
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _documentService.GetById(id);

            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // POST: Documents/Delete/5
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _documentService.DeleteConfirmed(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Documents/Document
        [Route("documentos-legales")]
        public async Task<IActionResult> Document()
        {
            var _documents = from a in await _documentService.GetAll()
                             select new ModelViewDocument
                             {
                                 ID = a.ID,
                                 Name = a.Name,
                                 UrlName = a.UrlName,
                                 NameProyect = a.NameProyect,
                                 Description = a.Description,
                                 CreateDate = a.CreateDate.ToString("MMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO")),
                                 DateUpdate = a.DateUpdate.ToString("MMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO")),
                                 FileDocument = a.FileDocument
                             };

            return View(_documents);

        }

        private bool DocumentExists(int id)
        {
            return _documentService.DocumentExists(id);
        }

        [AllowAnonymous]
        [ActionName("documentos-publicos")]
        public FileResult PublicDocuments(string routeFile)
        {
            return File($"/images/filesDocuments/{routeFile}", "application/pdf", $"{routeFile}.pdf");
        }
    }
}
