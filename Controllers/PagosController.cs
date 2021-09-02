using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Inmobiliaria_Peluffo.Models;

namespace Inmobiliaria_Peluffo.Controllers
{
    public class PagosController : Controller
    {
        private readonly RepositorioPago repositorio;
        private readonly RepositorioContrato repContr;
        private readonly IConfiguration configuration;
        public PagosController(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.repositorio = new RepositorioPago(configuration);
            this.repContr = new RepositorioContrato(configuration);
        }

        // GET: Pagos
        public ActionResult Index()
        {
            try
            {
                IList<Pago> lista = repositorio.ObtenerTodos();
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

        // GET: Pagos/Details/5
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

        // GET: Pagos/Create
        public ActionResult Create()
        {
            try
            {
                 ViewBag.Contratos = repContr.ObtenerTodos();
                 return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                TempData["StackTrate"] = ex.StackTrace;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Pagos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Pago p)
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
                    ViewBag.Contratos = repContr.ObtenerTodos();
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

        // GET: Pagos/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                 ViewBag.Contratos = repContr.ObtenerTodos();
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

        // POST: Pagos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Pago p)
        {
            try
            {
                if(ModelState.IsValid){
                    repositorio.Modificacion(p);
                    TempData["Mensaje"] = "El Pago se modificó con éxito";
                    return RedirectToAction(nameof(Index));
                }
                else{
                    ViewBag.Mensaje = "No se pudo Editar";
                    ViewBag.Contratos = repContr.ObtenerTodos();
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

        // GET: Pagos/Delete/5
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

        // POST: Pagos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Pago p)
        {
            try
            {
                repositorio.Baja(p);
                TempData["Mensaje"] = "El Pago se eliminó con éxito";
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