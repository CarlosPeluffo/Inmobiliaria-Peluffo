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
    public class InmueblesController : Controller
    {
        private readonly RepositorioInmueble repositorio;
        private readonly RepositorioPropietario repProp;
        private readonly IConfiguration configuration;
        public InmueblesController(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.repositorio = new RepositorioInmueble(configuration);
            this.repProp = new RepositorioPropietario(configuration);
        }
        // GET: Inmuebles
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

        // GET: Inmuebles/Details/5
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

        // GET: Inmuebles/Create
        public ActionResult Create()
        {
            try
            {
                ViewBag.Propietarios = repProp.ObtenerTodos();
                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                TempData["StackTrate"] = ex.StackTrace;
                return RedirectToAction(nameof(Index));
            }
            
        }

        // POST: Inmuebles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inmueble i)
        {
            try
            {
                if(ModelState.IsValid){
                    repositorio.Alta(i);
                    TempData["Id"] = i.Id;
                    return RedirectToAction(nameof(Index));
                }
                else{
                    ViewBag.Mensaje = "No se pudo cargar";
                    ViewBag.Propietarios = repProp.ObtenerTodos();
                    return View(i);
                }
            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message;
                TempData["StackTrate"] = ex.StackTrace;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Inmuebles/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var entidad = repositorio.ObtenerPorId(id);
                ViewBag.Propietarios = repProp.ObtenerTodos();
                return View(entidad);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                TempData["StackTrate"] = ex.StackTrace;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Inmuebles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Inmueble inmueble)
        {
            try
            {
                if(ModelState.IsValid){
                    repositorio.Modificacion(inmueble);
                    TempData["Mensaje"] = "El Inmueble se modificó con éxito";
                    return RedirectToAction(nameof(Index));
                }
                else{
                    ViewBag.Mensaje = "No se pudo Editar";
                    ViewBag.Propietarios = repProp.ObtenerTodos();
                    return View(inmueble);
                }
            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message;
                TempData["StackTrate"] = ex.StackTrace;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Inmuebles/Delete/5
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

        // POST: Inmuebles/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Inmueble inmueble)
        {
            try
            {
                repositorio.Baja(inmueble);
                TempData["Mensaje"] = "El Inmueble se eliminó con éxito";
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                TempData["Mensaje"] = "El Inmueble está asociado a un Contrato. Imposible Eliminar";
                //TempData["Error"] = ex.Message;
                TempData["StackTrate"] = ex.StackTrace;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}