using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inmobiliaria_Peluffo.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inmobiliaria_Peluffo.Controllers
{
    [Authorize]
    public class PropietariosController : Controller
    {
        private readonly IRepositorioPropietario repositorio;
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;

        public PropietariosController(IConfiguration configuration, IWebHostEnvironment environment,IRepositorioPropietario repositorio)
        {
            this.configuration = configuration;
            this.environment = environment;
            this.repositorio = repositorio;
        }
        // GET: Propietarios
        public ActionResult Index()
        {
            try
            {
                var lista = repositorio.ObtenerTodos();
                ViewBag.Id = TempData["Id"];
                if(TempData.ContainsKey("Mensaje")){
                    ViewBag.Mensaje = TempData["Mensaje"];
                }
                if(TempData.ContainsKey("Error")){
                    ViewBag.Mensaje = TempData["Error"];
                }
                if(TempData.ContainsKey("StackTrate")){
                    ViewBag.StackTrate = TempData["StackTrate"];
                }
                return View(lista);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StrackTrate = ex.StackTrace;
                return View();
            }
           
        }

        // GET: Propietarios/Details/5
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

        // GET: Propietarios/Create
        public ActionResult Create()
        {
           try
           {
               return View();
           }
           catch (Exception ex)
           {
                TempData["Error"] = ex.Message;
                TempData["StackTrate"] = ex.StackTrace;
                return RedirectToAction(nameof(Index));
           } 
        }

        // POST: Propietarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Propietario p)
        {
            try
            {
                if(ModelState.IsValid){
                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: p.Clave,
                        salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256/8));
                    p.Clave = hashed;
                    int res = repositorio.Alta(p);
                    if(p.AvatarFile != null && p.Id > 0){
                        string wwwPath = environment.WebRootPath;
                        string path = Path.Combine(wwwPath, "UsersFiles");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        string fileName = "photo_" + p.Id + Path.GetExtension(p.AvatarFile.FileName);
                        string pathCompleto = Path.Combine(path, fileName);
                        p.Avatar = Path.Combine("/UsersFiles", fileName);
                        using (FileStream stream = new FileStream(pathCompleto, FileMode.Create)){
                        p.AvatarFile.CopyTo(stream);
                        }
                        repositorio.Modificacion(p);
                    }
                    TempData["Id"] = p.Id;
                    return RedirectToAction(nameof(Index));
                }
                else{
                    ViewBag.Mensaje = "No se pudo cargar";
                    return View(p);
                }
                
            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message;
                TempData["StackTrate"] = ex.StackTrace;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Propietarios/Edit/5
        public ActionResult Edit(int id)
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

        // POST: Propietarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Propietario propietario)
        {
            try
            {
                var OldUser = repositorio.ObtenerPorId(id);
                if(propietario.Clave == null){
                    propietario.Clave = OldUser.Clave;
                }
                if(propietario.Clave != OldUser.Clave){
                        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                            password: propietario.Clave,
                            salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                            prf: KeyDerivationPrf.HMACSHA1,
                            iterationCount: 1000,
                            numBytesRequested: 256/8));
                        propietario.Clave = hashed;
                    }
                    if(propietario.AvatarFile != null){
                        string wwwPath = environment.WebRootPath;
                        string path = Path.Combine(wwwPath, "UsersFiles");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        string fileName = "photo_" + propietario.Id + Path.GetExtension(propietario.AvatarFile.FileName);
                        string pathCompleto = Path.Combine(path, fileName);
                        propietario.Avatar = Path.Combine("/UsersFiles", fileName);
                        using (FileStream stream = new FileStream(pathCompleto, FileMode.Create)){
                        propietario.AvatarFile.CopyTo(stream);
                        }
                    }else{
                        propietario.Avatar = OldUser.Avatar;
                    }
                    repositorio.Modificacion(propietario);
                    TempData["Mensaje"] = "El Propietario se modificó Correctamente";
                    return RedirectToAction("Index", "Home");
                /*if(ModelState.IsValid){
                repositorio.Modificacion(propietario);
                TempData["Mensaje"] = "El Propietario se modificó Correctamente";
                return RedirectToAction(nameof(Index));
                }
                else{
                    ViewBag.Mensaje = "No se pudo cargar. Propietario inválido";
                    return View(propietario);
                }*/
            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message;
                TempData["StackTrate"] = ex.StackTrace;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Propietarios/Delete/5
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

        // POST: Propietarios/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(Propietario propietario)
        {
            try
            {
                repositorio.Baja(propietario);
                TempData["Mensaje"] = "El Propietario se elimino con éxito";
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                TempData["Mensaje"] = "El Propietario está asociado a un Inmueble. Imposible Eliminar";
                //TempData["Error"] = ex.Message;
                TempData["StackTrate"] = ex.StackTrace;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}