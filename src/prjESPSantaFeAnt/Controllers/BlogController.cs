using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using model;
using modelDTOs;
using persistenDatabase;
using prjESPSantaFeAnt.Models;
using services;
using X.PagedList;

namespace prjESPSantaFeAnt.Controllers
{
    public class BlogController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IBlogService _blogService;

        public BlogController(ApplicationDbContext context,
            IBlogService blogService)
        {
            _context = context;
            _blogService = blogService;
        }

        // GET: Blog
        public async Task<IActionResult> Index()
        {
            var _model = from a in await _blogService.GetAll()
                         select new ModelViewBlog
                         {
                             Id = a.Id,
                             NameBlog = a.NameMaster,
                             UrlMaster = a.UrlMaster,
                             Author = a.Author,
                             DateCreate = a.DateCreate.ToString("MMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO")),
                             DateUpdate = a.DateUpdate.ToString("MMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO")),
                             Statud = a.Statud
                         };

            return View(_model);

        }

        // GET: Blog/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var _blog = await _blogService.GetById(id);

            var _dateCreate = _blog.DateCreate.ToString("MMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO"));
            var _dateUpdate = _blog.DateUpdate.ToString("MMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO"));

            if (_blog == null)
            {
                return NotFound();
            }

            var _model = new ModelViewBlog
            {
                Id = _blog.Id,
                NameBlog = _blog.NameMaster,
                UrlMaster = _blog.UrlMaster,
                Description = _blog.Description,
                Author = _blog.Author,
                CoverPage = _blog.CoverPage,
                Statud = _blog.Statud,
                DateCreate = _dateCreate,
                DateUpdate = _dateUpdate
            };

            ViewData["detail"] = true;
            return View(_model);
        }

        // GET: Blog/Details/5
        [Route("noticias/{urlBlog}")]
        public async Task<IActionResult> Details(string urlBlog)
        {
            if (urlBlog == "")
            {
                return NotFound();
            }

            var _blog = await _blogService.GetById(urlBlog);

            var _dateCreate = _blog.DateCreate.ToString("MMMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO"));
            var _dateUpdate = _blog.DateUpdate.ToString("MMMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO"));

            if (_blog == null)
            {
                return NotFound();
            }

            var _model = new ModelViewBlog
            {
                Id = _blog.Id,
                NameBlog = _blog.NameMaster,
                UrlMaster = _blog.UrlMaster,
                Description = _blog.Description,
                Author = _blog.Author,
                CoverPage = _blog.CoverPage,
                Statud = _blog.Statud,
                DateCreate = _dateCreate,
                DateUpdate = _dateUpdate
            };

            ViewData["detail"] = false;
            return View(_model);
        }

        // GET: Blog/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Blog/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogCreateDto model)
        {
            var _urlName = _blogService.DuplicaName(model.NameBlog);

            if (_urlName)
            {
                ViewData["DuplicaName"] = $"El Nombre {model.NameBlog} ya ha sido utilizado, cambielo";
                return View(model);
            }

            if (ModelState.IsValid)
            {
                var result = await _blogService.Create(model);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: Blog/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var _blog = await _blogService.GetById(id);

            if (_blog == null)
            {
                return NotFound();
            }

            var _model = new BlogEditDto
            {
                Id = _blog.Id,
                NameBlog = _blog.NameMaster,
                UrlMaster = _blog.UrlMaster,
                Description = _blog.Description,
                Author = _blog.Author,
                Statud = _blog.Statud,
            };

            return View(_model);
        }

        // POST: Blog/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BlogEditDto model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _blogService.Edit(id, model);
                }
                catch (DbUpdateConcurrencyException)
                {

                    if (!MasterExists(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: Blog/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var _blog = await _blogService.GetById(id);

            var _dateCreate = _blog.DateCreate.ToString("MMMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO"));
            var _dateUpdate = _blog.DateUpdate.ToString("MMMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO"));

            if (_blog == null)
            {
                return NotFound();
            }

            var _model = new ModelViewBlog
            {
                Id = _blog.Id,
                NameBlog = _blog.NameMaster,
                UrlMaster = _blog.UrlMaster,
                Description = _blog.Description,
                Author = _blog.Author,
                CoverPage = _blog.CoverPage,
                Statud = _blog.Statud,
                DateCreate = _dateCreate,
                DateUpdate = _dateUpdate
            };

            ViewData["detail"] = true;
            return View(_model);
        }

        // POST: Blog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _blogService.DeleteConfirmed(id);
            return RedirectToAction(nameof(Index));
        }

        private bool MasterExists(int id)
        {
            return _blogService.BlogExists(id);
        }

        // GET: Blog
        [Route("noticias")]
        public async Task<IActionResult> Notice(int? page)
        {

            var _model = from a in await _blogService.GetAll()
                         where a.Statud == true
                         select new ModelViewBlog
                         {
                             Id = a.Id,
                             NameBlog = a.NameMaster,
                             UrlMaster = a.UrlMaster,
                             CoverPage = a.CoverPage,
                             Author = a.Author,
                             DateCreate = a.DateCreate.ToString("MMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO")),
                             DateUpdate = a.DateUpdate.ToString("MMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO")),
                             Statud = a.Statud
                         };


            var pageNumber = page ?? 1; // if no page is specified, default to the first page (1)
            int pageSize = 9; // Get 25 students for each requested page.
            var onePageOfStudents = _model.ToPagedList(pageNumber, pageSize);
            return View(onePageOfStudents); // Send 25 students to the page.

            //return View(_model);

        }
    }
}
