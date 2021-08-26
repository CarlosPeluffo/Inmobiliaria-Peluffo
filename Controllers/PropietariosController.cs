using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inmobiliaria_Peluffo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inmobiliaria_Peluffo.Controllers
{
    public class PropietariosController : Controller
    {
        private readonly RepositorioPropietario repositorio;
        private readonly IConfiguration configuration;

        public PropietariosController(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.repositorio = new RepositorioPropietario(configuration);
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
                return View(lista);
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex;
                return View();
                throw;
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
                TempData["Mensaje"] = ex;
                return RedirectToAction(nameof(Index));
                throw;
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
                TempData["Mensaje"] = ex;
                return RedirectToAction(nameof(Index));
                throw;
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
                    repositorio.Alta(p);
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
                TempData["Mensaje"] = ex;
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
                TempData["Mensaje"] = ex;
                return RedirectToAction(nameof(Index));
                throw;
            }
            
        }

        // POST: Propietarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Propietario propietario)
        {
            try
            {
                // TODO: Add update logic here
                repositorio.Modificacion(propietario);
                TempData["Mensaje"] = "El Propietario se modificó Correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ViewBag.Mensaje = ex;
                return View();
            }
        }

        // GET: Propietarios/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var entidad = repositorio.ObtenerPorId(id);
                return View(entidad);
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = ex;
                return RedirectToAction(nameof(Index));
                throw;
            }
            
        }

        // POST: Propietarios/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Propietario propietario)
        {
            try
            {
                // TODO: Add delete logic here
                repositorio.Baja(propietario);
                TempData["Mensaje"] = "El propietario se elimino con éxito";
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ViewBag.Mensaje = ex;
                return View();
            }
        }
    }
}