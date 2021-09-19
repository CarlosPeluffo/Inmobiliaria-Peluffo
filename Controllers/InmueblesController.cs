using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inmobiliaria_Peluffo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Inmobiliaria_Peluffo.Controllers
{
    [Authorize]
    public class InmueblesController : Controller
    {
        private readonly IRepositorioInmueble repositorio;
        private readonly IRepositorioPropietario repProp;
        private readonly IConfiguration configuration;
        public InmueblesController(IConfiguration configuration, IRepositorioInmueble repositorio, IRepositorioPropietario repProp)
        {
            this.configuration = configuration;
            this.repositorio = repositorio;
            this.repProp = repProp;
        }
        // GET: Inmuebles
        public ActionResult Index(int id)
        {
            try
            {
                ViewBag.Propietario = id;
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
                if(id != 0){
                    var listado = repositorio.ObtenerPorPropietario(id);
                    TempData["Propietario"] = id;
                    return View(listado);
                }
                TempData["Propietario"] = id;
                var lista = repositorio.ObtenerTodos();
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
                if(TempData.ContainsKey("Propietario")){
                    ViewBag.Propietario = TempData["Propietario"];
                }
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
                if(TempData.ContainsKey("Propietario")){
                    ViewBag.Propietario = TempData["Propietario"];
                }
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
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            try
            {
                if(TempData.ContainsKey("Propietario")){
                    ViewBag.Propietario = TempData["Propietario"];
                }
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
        [Authorize(Policy = "Administrador")]
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
        public ActionResult Disponibles(IFormCollection collection){
            try
            {
                var fechaIn = collection["FechaIn"];
                var fechaF = collection["FechaF"];
                var lista = repositorio.ObtenerLibres(fechaIn, fechaF);
                ViewBag.Disponibles = 1;
                return View("Index", lista);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                TempData["StackTrate"] = ex.StackTrace;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}