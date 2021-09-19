using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Inmobiliaria_Peluffo.Models;
using Microsoft.AspNetCore.Authorization;

namespace Inmobiliaria_Peluffo.Controllers
{
    [Authorize]
    public class ContratosController : Controller
    {
        private readonly IRepositorioContrato repositorio;
        private readonly IRepositorioInmueble repInm;
        private readonly IRepositorioInquilino repInq;
        private readonly IConfiguration configuration;
        public ContratosController(IConfiguration configuration, IRepositorioContrato repositorio, IRepositorioInmueble repInm, IRepositorioInquilino repInq)
        {
            this.configuration = configuration;
            this.repositorio = repositorio;
            this.repInm = repInm;
            this.repInq = repInq;
        } 
        // GET: Contratos
        public ActionResult Index(int id)
        {
            try
            {
                ViewBag.Inmueble = id;
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
                    var listado = repositorio.ObtenerPorInmueble(id);
                    TempData["Inmueble"] = id;
                    return View(listado);
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

        // GET: Contratos/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                if(TempData.ContainsKey("Inmueble")){
                    ViewBag.Inmueble = TempData["Inmueble"];
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

        // GET: Contratos/Create
        public ActionResult Create()
        {
            try
            {
                ViewBag.Inquilinos = repInq.ObtenerTodos();
                ViewBag.Inmuebles = repInm.ObtenerTodosActivos();
                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                TempData["StackTrate"] = ex.StackTrace;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Contratos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Contrato c)
        {
            try
            {
                if(ModelState.IsValid){
                    int i = DateTime.Compare(c.FechaFin, c.FechaInicio);
                    if(i > 0){
                        repositorio.Alta(c);
                        TempData["Id"] = c.Id;
                        return RedirectToAction(nameof(Index));
                    }
                    else{
                        ViewBag.Mensaje = "No se pudo cargar. Fecha de Fin debe ser mayor a la Fecha de Inicio";
                        ViewBag.Inquilinos = repInq.ObtenerTodos();
                        ViewBag.Inmuebles = repInm.ObtenerTodosActivos();
                        return View(c);
                    }
                }
                else{
                    ViewBag.Mensaje = "No se pudo cargar";
                    ViewBag.Inquilinos = repInq.ObtenerTodos();
                    ViewBag.Inmuebles = repInm.ObtenerTodosActivos();
                    return View(c);
                }
            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message;
                TempData["StackTrate"] = ex.StackTrace;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Contratos/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                if(TempData.ContainsKey("Inmueble")){
                    ViewBag.Inmueble = TempData["Inmueble"];
                }
                ViewBag.Inquilinos = repInq.ObtenerTodos();
                ViewBag.Inmuebles = repInm.ObtenerTodosActivos();
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

        // POST: Contratos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Contrato contrato)
        {
            try
            {
                if(ModelState.IsValid){
                    int i = DateTime.Compare(contrato.FechaFin, contrato.FechaInicio);
                    if(i > 0){
                        repositorio.Modificacion(contrato);
                        TempData["Mensaje"] = "El Contrato se modificó con éxito";
                        return RedirectToAction(nameof(Index));
                    }
                    else{
                        ViewBag.Mensaje = "No se pudo cargar. Fecha de Fin debe ser mayor a la Fecha de Inicio";
                        ViewBag.Inquilinos = repInq.ObtenerTodos();
                        ViewBag.Inmuebles = repInm.ObtenerTodosActivos();
                        return View(contrato);
                    }
                }
                else{
                    ViewBag.Mensaje = "No se pudo Editar";
                    ViewBag.Inquilinos = repInq.ObtenerTodos();
                    ViewBag.Inmuebles = repInm.ObtenerTodosActivos();
                    return View(contrato);
                }
            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message;
                TempData["StackTrate"] = ex.StackTrace;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Contratos/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            try
            {
                if(TempData.ContainsKey("Inmueble")){
                    ViewBag.Inmueble = TempData["Inmueble"];
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

        // POST: Contratos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Contrato contrato )
        {
            try
            {
                repositorio.Baja(contrato);
                TempData["Mensaje"] = "El Contrato se eliminó con éxito";
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message;
                TempData["StackTrate"] = ex.StackTrace;
                return RedirectToAction(nameof(Index));
            }
        }
        
        public ActionResult Cancel(int id){
            try
            {
                if(TempData.ContainsKey("Inmueble")){
                    ViewBag.Inmueble = TempData["Inmueble"];
                }
                var contrato = repositorio.ObtenerPorId(id);
                return View(contrato);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                TempData["StackTrate"] = ex.StackTrace;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cancel(int id, Contrato contrato){
            try
            {
                repositorio.CancelarContrato(contrato);
                TempData["Mensaje"] = "El contrato se canceló con éxito";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                TempData["StackTrate"] = ex.StackTrace;
                return RedirectToAction(nameof(Index));
                
            }
        }
        
        [Route("[controller]/Buscar/{fechaIn}/{fechaF}", Name= "Buscar")]
        public ActionResult Buscar(string fechaIn, string fechaF){
            try
            {
                var res = repInm.ObtenerLibres(fechaIn, fechaF);
                return Json(new { Datos = res });
            }
            catch (Exception ex)
            {
                return Json(new { Error = ex.Message });
            }
        }

        //[Route("[controller]/Vigentes/{fechaIn}/{fechaF}", Name= "Vigentes")]
        public ActionResult Vigentes(IFormCollection collection){
            try{
                var fechaIn = collection["FechaIn"];
                var fechaF = collection["FechaF"];
                var lista = repositorio.ObtenerVigentesFecha(fechaIn, fechaF);
                ViewBag.Vigente = 1;
                return View("Index", lista);
            }catch(Exception ex){
                TempData["Error"] = ex.Message;
                TempData["StackTrate"] = ex.StackTrace;
                return RedirectToAction(nameof(Index));
            }
        }
        public ActionResult Informes(int id){
            try{
                var contrato = repositorio.ObtenerPorId(id);
                var accion = repositorio.Informe(id);
                if(accion == 1){
                    ViewBag.Saldo = (contrato.Monto);
                }else{
                    ViewBag.Saldo = (contrato.Monto * 2);
                }
                return View();
            }catch(Exception ex){
                TempData["Error"] = ex.Message;
                TempData["StackTrate"] = ex.StackTrace;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}