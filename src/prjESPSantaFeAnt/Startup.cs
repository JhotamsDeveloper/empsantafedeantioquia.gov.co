using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using persistenDatabase;
using AutoMapper;
using services;
using services.Commons;

namespace prjESPSantaFeAnt
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<IdentityUser, IdentityRole>(
                options => options.SignIn.RequireConfirmedAccount = true)
               .AddEntityFrameworkStores<ApplicationDbContext>()
               .AddDefaultTokenProviders();

            //Cambiar todas las duraciones del token de proteccion de datos
            services.Configure<DataProtectionTokenProviderOptions>(o =>
                o.TokenLifespan = TimeSpan.FromDays(7));

            //Para mas informacion visite https://docs.microsoft.com/es-es/dotnet/api/microsoft.aspnetcore.identity.identityoptions?view=aspnetcore-3.1
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 4;
                options.Password.RequiredUniqueChars = 0;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(180);

                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            services.AddAutoMapper(typeof(Startup));

            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<INacionLicitanteService, NacionLicitanteService>();
            services.AddTransient<IBiddingParticipantService, BiddingParticipantService>();
            services.AddTransient<IBlogService, BlogService>();
            services.AddTransient<IPQRSDService, PQRSDService>();

            services.AddTransient<IUploadedFileIIS, UploadedFileIIS>();
            services.AddTransient<IFormatStringUrl, FormatStringUrl>();
            services.AddTransient<IEmailSendGrid, EmailSendGrid>();

            services.AddControllersWithViews();
            services.AddRazorPages();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //Defaults
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                //Acerca de nosotros
                endpoints.MapControllerRoute(
                name: "Acerca-de-nosostros",
                pattern: "acerca-de-nosotros",
                defaults: new { Controller = "Home", action = "About"   });

                //Categorias
                endpoints.MapControllerRoute(
                name: "Servicios",
                pattern: "servicios/{nameCategory}",
                defaults: new { Controller = "Home", action = "Details" });

                //PQRSD
                endpoints.MapControllerRoute(
                name: "PQRSD",
                pattern: "pqrsd",
                defaults: new { Controller = "PQRSDs", action = "TypePQRSD" });

                endpoints.MapControllerRoute(
                name: "formular-pqrsd",
                pattern: "formular-pqrsd",
                defaults: new { Controller = "PQRSDs", action = "create" });

                //Licitaciones
                endpoints.MapControllerRoute(
                name: "Licitaciones",
                pattern: "convocatorias/{nameNacionLicitante}",
                defaults: new { Controller = "NacionLicitante", action = "Details" });

                endpoints.MapControllerRoute(
                name: "LicitacionesGrid",
                pattern: "convocatorias",
                defaults: new { Controller = "NacionLicitante", action = "ListGetAll" });

                //Participante
                endpoints.MapControllerRoute(
                name: "participante",
                pattern: "convocatorias/participacion/{idConvocatoria}",
                defaults: new { Controller = "BiddingParticipants", action = "Create"});

                endpoints.MapControllerRoute(
                name: "participante",
                pattern: "convocatorias/reference/{reference}",
                defaults: new { Controller = "BiddingParticipants", action = "Details" });

                //Noticias
                endpoints.MapControllerRoute(
                name: "noticia",
                pattern: "noticias",
                defaults: new { Controller = "Blog", action = "notice" });

                endpoints.MapControllerRoute(
                name: "detalleNoticia",
                pattern: "noticias/{urlBlog}",
                defaults: new { Controller = "Blog", action = "Details" });

                //Funcionarios
                endpoints.MapControllerRoute(
                name: "funcionarios",
                pattern: "funcionarios",
                defaults: new { Controller = "Home", action = "Employes" });

                endpoints.MapRazorPages();

                //Funcionarios
                endpoints.MapControllerRoute(
                name: "perfil",
                pattern: "perfil",
                defaults: new { Controller = "Admin", action = "Profile" });

                endpoints.MapRazorPages();
            });
        }
    }
}
