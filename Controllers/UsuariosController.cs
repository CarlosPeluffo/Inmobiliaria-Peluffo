using System;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Inmobiliaria_Peluffo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inmobiliaria_Peluffo.Controllers
{
    [Authorize]
    public class UsuariosController : Controller
    {
        private readonly IRepositorioUsuario repositorio;
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;
        public UsuariosController(IConfiguration configuration, IWebHostEnvironment environment, IRepositorioUsuario repo)
        {
            this.configuration = configuration;
            this.environment = environment;
            this.repositorio = repo;
        }
        // GET: Usuarios
        [Authorize(Policy = "Administrador")]
        public ActionResult Index()
        {
            try
            {
                if(TempData.ContainsKey("Mensaje")){
                    ViewBag.Mensaje = TempData["Mensaje"];
                }
                if(TempData.ContainsKey("Error")){
                    ViewBag.Mensaje = TempData["Error"];
                }
                if(TempData.ContainsKey("StackTrate")){
                    ViewBag.StackTrate = TempData["StackTrate"];
                }
                var lista = repositorio.ObtenerTodos();
                return View(lista);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View();
            }
            
        }

        // GET: Usuarios/Details/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Details(int id)
        {
            try
            {
                var entidad = repositorio.ObtenerPorId(id);
                return View(entidad);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                TempData["StackTrate"] = ex.StackTrace;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Usuarios/Create
        [Authorize(Policy = "Administrador")]
        public ActionResult Create()
        {
            try
            {
                ViewBag.Roles = Usuario.ObtenerRoles();
                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                TempData["StackTrate"] = ex.StackTrace;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Usuarios/Create
        [Authorize(Policy = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Usuario user)
        {
            try
            {
                if(ModelState.IsValid){
                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: user.Clave,
                        salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256/8));
                    user.Clave = hashed;
                    int res = repositorio.Alta(user);
                    if(user.AvatarFile != null && user.Id > 0){
                        string wwwPath = environment.WebRootPath;
                        string path = Path.Combine(wwwPath, "UsersFiles");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        string fileName = "photo_" + user.Id + Path.GetExtension(user.AvatarFile.FileName);
                        string pathCompleto = Path.Combine(path, fileName);
                        user.Avatar = Path.Combine("/UsersFiles", fileName);
                        using (FileStream stream = new FileStream(pathCompleto, FileMode.Create)){
                        user.AvatarFile.CopyTo(stream);
                        }
                        repositorio.Modificacion(user);
                    }
                    return RedirectToAction(nameof(Index));
                }
                else{
                    ViewBag.Mensaje = "No se pudo cargar";
                    ViewBag.Roles = Usuario.ObtenerRoles();
                    return View(user);
                }
            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message;
                TempData["StackTrate"] = ex.StackTrace;
                return RedirectToAction(nameof(Index));
            }
        }

        public ActionResult Perfil()
        {
            ViewData["Title"] = "Mi perfil";
            var user = repositorio.ObtenerPorMail(User.Identity.Name);
            ViewBag.Roles = Usuario.ObtenerRoles();
            return View("Edit",user);
        }
        // GET: Usuarios/Edit/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Edit(int id)
        {
            try
            {
                ViewData["Title"] = "Editar usuario";
                var user = repositorio.ObtenerPorId(id);
                ViewBag.Roles = Usuario.ObtenerRoles();
                return View(user);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                TempData["StackTrate"] = ex.StackTrace;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Usuario user)
        {
            try
            {
                var OldUser = repositorio.ObtenerPorId(id);
                if(user.Clave == null){
                    user.Clave = OldUser.Clave;
                }
                //if(ModelState.IsValid){
                    if (!User.IsInRole("Administrador")){
                        var usuarioActual = repositorio.ObtenerPorMail(User.Identity.Name);
                        if (usuarioActual.Id != id){
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    if(user.Clave != OldUser.Clave){
                        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                            password: user.Clave,
                            salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                            prf: KeyDerivationPrf.HMACSHA1,
                            iterationCount: 1000,
                            numBytesRequested: 256/8));
                        user.Clave = hashed;
                    }
                    if(user.AvatarFile != null){
                        string wwwPath = environment.WebRootPath;
                        string path = Path.Combine(wwwPath, "UsersFiles");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        string fileName = "photo_" + user.Id + Path.GetExtension(user.AvatarFile.FileName);
                        string pathCompleto = Path.Combine(path, fileName);
                        user.Avatar = Path.Combine("/UsersFiles", fileName);
                        using (FileStream stream = new FileStream(pathCompleto, FileMode.Create)){
                        user.AvatarFile.CopyTo(stream);
                        }
                    }else{
                        user.Avatar = OldUser.Avatar;
                    }
                    repositorio.Modificacion(user);
                    TempData["Mensaje"] = "El Usuario se modificó Correctamente";
                    return RedirectToAction("Index", "Home");
                /*}
                else{
                    ViewBag.Mensaje = "No se pudo cargar";
                    ViewBag.Roles = Usuario.ObtenerRoles();
                    return View(user);
                }*/
            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message;
                TempData["StackTrate"] = ex.StackTrace;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Usuarios/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            try
            {
                var entidad = repositorio.ObtenerPorId(id);
                return View(entidad);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                TempData["StackTrate"] = ex.StackTrace;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Usuarios/Delete/5
        [Authorize(Policy = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Usuario user)
        {
            try
            {
                repositorio.Baja(user);
                TempData["Mensaje"] = "El Usuario se eliminó con éxito";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                TempData["StackTrate"] = ex.StackTrace;
                return RedirectToAction(nameof(Index));
            }
        }
        [AllowAnonymous]
        public ActionResult Login(string returnUrl){
            try
            {
                TempData["returnUrl"] = returnUrl;
                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                TempData["StackTrate"] = ex.StackTrace;
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login (LoginView login){
            try
            {
                var returnUrl = String.IsNullOrEmpty(TempData["returnUrl"] as string)? "/Home" : TempData["returnUrl"].ToString();
                if(ModelState.IsValid){
                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: login.Clave,
                        salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));
                    var user = repositorio.ObtenerPorMail(login.Usuario);
                    if (user == null || user.Clave != hashed){
                        ModelState.AddModelError("", "El email o la clave no son correctos");
                        TempData["returnUrl"] = returnUrl;
                        return View();
                    }
                    var claims = new List<Claim>{
                        new Claim(ClaimTypes.Name, user.Mail),
                        new Claim("FullName", user.Nombre + " " + user.Apellido),
                        new Claim(ClaimTypes.Role, user.RolNombre),
                    };
                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity));
                    TempData.Remove("returnUrl");
                    return Redirect(returnUrl);
                }
                TempData["returnUrl"] = returnUrl;
                return View();
            }
            catch (Exception ex)
            {
                
                TempData["Error"] = ex.Message;
                TempData["StackTrate"] = ex.StackTrace;
                return RedirectToAction("Index", "Home");;
            }
        }
        
        [Route("salir", Name = "logout")]
        public async Task<ActionResult> Logout(){
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["Mensaje"] = "Sesión finalizada con éxito";
            return RedirectToAction("Index", "Home");
        }
    }
}