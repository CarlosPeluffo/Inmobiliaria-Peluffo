using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication.Cookies;
using Inmobiliaria_Peluffo.Models;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Inmobiliaria_Peluffo
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
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(option => {
                option.LoginPath = "/Usuarios/Login";
                option.LogoutPath = "/Usuarios/Logout";
                option.AccessDeniedPath = "/Home/Restringido";
            });
            services.AddAuthorization(option =>{
                //option.AddPolicy("Empleado", policy => policy.RequireRole("Empleado", "Administrador"));
                option.AddPolicy("Administrador", policy => policy.RequireRole("Administrador"));
            });
            services.AddControllersWithViews();

            services.AddTransient<IRepositorio<Propietario>, RepositorioPropietario>();
            services.AddTransient<IRepositorioPropietario, RepositorioPropietario>();
            services.AddTransient<IRepositorio<Contrato>, RepositorioContrato>();
            services.AddTransient<IRepositorioContrato, RepositorioContrato>();
            services.AddTransient<IRepositorio<Inmueble>, RepositorioInmueble>();
            services.AddTransient<IRepositorioInmueble, RepositorioInmueble>();
            services.AddTransient<IRepositorio<Pago>, RepositorioPago>();
            services.AddTransient<IRepositorioPago, RepositorioPago>();
            services.AddTransient<IRepositorio<Inquilino>, RepositorioInquilino>();
            services.AddTransient<IRepositorioInquilino, RepositorioInquilino>();
            services.AddTransient<IRepositorio<Usuario>, RepositorioUsuario>();
            services.AddTransient<IRepositorioUsuario, RepositorioUsuario>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
