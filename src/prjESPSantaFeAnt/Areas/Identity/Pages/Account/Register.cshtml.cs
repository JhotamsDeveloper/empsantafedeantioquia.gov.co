using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using services.Commons;

namespace prjESPSantaFeAnt.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSendGrid _emailSendGrid;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<RegisterModel> logger,
            IEmailSendGrid emailSendGrid)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _roleManager = roleManager;
            _emailSendGrid = emailSendGrid;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "El {0} debe tener al menos {2} y un máximo de {1} caracteres.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirmar password")]
            [Compare("Password", ErrorMessage = "La contraseña y la contraseña de confirmación no coinciden.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    if (!_roleManager.Roles.Any())
                        CreateRoles();

                    await AssignRoles(user);

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSendGrid.Execute("Confirme su email",
                         $"Confirme su cuenta por <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>haciendo click aqui</a>.", Input.Email);

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private void CreateRoles()
        {
            List<IdentityRole> identityRoles = new List<IdentityRole>();

            IdentityRole UserApp = new IdentityRole();
            UserApp.Id = Guid.NewGuid().ToString();
            UserApp.Name = "UserApp";
            UserApp.NormalizedName = "UserApp".ToUpper();
            UserApp.ConcurrencyStamp = Guid.NewGuid().ToString();
            identityRoles.Add(UserApp);

            IdentityRole Admin = new IdentityRole();
            Admin.Id = Guid.NewGuid().ToString();
            Admin.Name = "Admin";
            Admin.NormalizedName = "Admin".ToUpper();
            Admin.ConcurrencyStamp = Guid.NewGuid().ToString();
            identityRoles.Add(Admin);

            IdentityRole SuperAdmin = new IdentityRole();
            SuperAdmin.Id = Guid.NewGuid().ToString();
            SuperAdmin.Name = "SuperAdmin";
            SuperAdmin.NormalizedName = "SuperAdmin".ToUpper();
            SuperAdmin.ConcurrencyStamp = Guid.NewGuid().ToString();
            identityRoles.Add(SuperAdmin);

            identityRoles.ForEach(roles => { _roleManager.CreateAsync(roles);});

        }

        private async Task AssignRoles(IdentityUser user)
        {
            await _userManager.AddToRoleAsync(user, "UserApp");
            _logger.LogInformation("Se creo o se le asigno un rol");
        }
    }
}
