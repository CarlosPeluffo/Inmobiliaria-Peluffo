using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Inmobiliaria_Peluffo.Models;

namespace Inmobiliaria_Peluffo.Controllers
{
    public class ContratosController : Controller
    {
        private readonly RepositorioContrato repositorio;
        private readonly RepositorioInmueble repInm;
        private readonly RepositorioInquilino repInq;
        private readonly IConfiguration configuration;
        public ContratosController(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.repositorio = new RepositorioContrato(configuration);
            this.repInm = new RepositorioInmueble(configuration);
            this.repInq = new RepositorioInquilino(configuration);
        } 
        // GET: Contratos
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
                ViewBag.StackTrate = ex.StackTrace;
                return View();
            }
            
        }

        // GET: Contratos/Details/5
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
                    repositorio.Alta(c);
                    TempData["Id"] = c.Id;
                    return RedirectToAction(nameof(Index));
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
                    repositorio.Modificacion(contrato);
                    TempData["Mensaje"] = "El Contrato se modificó con éxito";
                    return RedirectToAction(nameof(Index));
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

        // POST: Contratos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
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
    }
}