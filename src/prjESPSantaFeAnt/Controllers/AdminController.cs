﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using prjESPSantaFeAnt.Models;

namespace prjESPSantaFeAnt.Controllers
{

    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
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
    }
}