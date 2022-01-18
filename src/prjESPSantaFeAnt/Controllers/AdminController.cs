using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using prjESPSantaFeAnt.Models;
using services.Commons;

namespace prjESPSantaFeAnt.Controllers
{

    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUploadedFileIIS _uploadedFileIIS;

        public AdminController(RoleManager<IdentityRole> roleManager, IUploadedFileIIS uploadedFileIIS)
        {
            _roleManager = roleManager;
            _uploadedFileIIS = uploadedFileIIS;
        }

        [Authorize(Roles = "SuperAdmin")]
        public IActionResult Index()
        {
            var _rol = from a in _roleManager.Roles
                       select new ModelViewRoles
                       {
                           RoleName = a.Name
                       };

            return View(_rol.ToList());
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Create(ModelViewRoles model)
        {
            if (ModelState.IsValid)
            {
                // Solo necesitamos especificar un nombre de rol único para crear un nuevo rol
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };

                // Guarda el rol en la tabla AspNetRoles subyacente
                IdentityResult result = await _roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("index", "home");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Admin,UserApp")]
        public IActionResult Profile()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public IActionResult AddCoverpage()
        {
            ViewBag.error = string.Empty;
            ViewBag.success = string.Empty;

            ModelViewCoverage model = new ModelViewCoverage();
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public IActionResult AddCoverpage(ModelViewCoverage model)
        {
            string error = string.Empty;
            string success = string.Empty;

            if (model.File != null)
            {
                success = _uploadedFileIIS.UploadedFileImage("Coverpague", model.File, string.Empty, true);
            }
            else
            {
                error = "Debe de seleccionar una imagen .jpg";
            }

            ViewBag.error = error;
            ViewBag.success = success;

            return View();
        }

    }
}