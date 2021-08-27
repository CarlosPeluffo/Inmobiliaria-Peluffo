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
    public class InquilinosController : Controller
    {
        private readonly RepositorioInquilino repositorio;
        private readonly IConfiguration configuration;
        public InquilinosController(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.repositorio = new RepositorioInquilino(configuration);
        }
        // GET: Inquilinos
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

        // GET: Inquilinos/Details/5
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

        // GET: Inquilinos/Create
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

        // POST: Inquilinos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inquilino i)
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

        // GET: Inquilinos/Edit/5
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

        // POST: Inquilinos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Inquilino i)
        {
            try
            {
                repositorio.Modificacion(i);
                TempData["Mensaje"] = "El Inquilino se modificó Correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message;
                TempData["StackTrate"] = ex.StackTrace;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Inquilinos/Delete/5
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

        // POST: Inquilinos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Inquilino i)
        {
            try
            {
                repositorio.Baja(i);
                TempData["Mensaje"] = "El Inquilino se elimino con éxito";
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                TempData["Mensaje"] = "El Inquilino posee contratos asociados. Imposible eliminar";
                //TempData["Error"] = ex.Message;
                TempData["StackTrate"] = ex.StackTrace;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}