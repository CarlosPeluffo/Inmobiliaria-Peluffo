using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Inmobiliaria_Peluffo.Models;

namespace Inmobiliaria_Peluffo.Controllers
{
    [Authorize]
    public class PagosController : Controller
    {
        private readonly IRepositorioPago repositorio;
        private readonly IRepositorioContrato repContr;
        private readonly IConfiguration configuration;
        public PagosController(IConfiguration configuration, IRepositorioPago repositorio, IRepositorioContrato repContr)
        {
            this.configuration = configuration;
            this.repositorio = repositorio;
            this.repContr = repContr;
        }

        // GET: Pagos
        public ActionResult Index(int id)
        {
            try
            {
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
                    IList<Pago> lista = repositorio.ObtenerTodosPorContrato(id);
                    var entidad = repContr.ObtenerPorId(id);
                    ViewBag.Contrato = id;
                    ViewBag.Dato = entidad.Inquilino.Dni + " - " + entidad.Inmueble.Direccion + "\n" + "Monto: $" + entidad.Monto;
                    TempData["Contrato"] = id;
                    ViewBag.Nro =  lista.Count() + 1 ;
                    return View(lista);
                }
                else{
                    IList<Pago> lista2 = repositorio.ObtenerTodos();
                    ViewBag.Contrato = id;
                    return View(lista2);
                }
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
                ViewBag.Contrato = TempData["Contrato"];
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
                var idC = p.ContratoId;
                if(ModelState.IsValid){
                    repositorio.Alta(p);
                    TempData["Id"] = p.Id;
                    return RedirectToAction("Index", new { id = idC});
                }
                else{
                    TempData["Error"] = "No se pudo cargar. Ingrese un monto Válido";
                    return RedirectToAction("Index", new { id = idC});
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
                 var entidad = repositorio.ObtenerPorId(id);
                 ViewBag.Contrato = TempData["Contrato"];
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
                    var idC = p.ContratoId;
                    TempData["Mensaje"] = "El Pago se modificó con éxito";
                    return RedirectToAction("Index", new { id = idC});
                }
                else{
                    ViewBag.Mensaje = "No se pudo Editar";
                    ViewBag.Contrato = p.ContratoId;
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
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            try
            {
                var entidad = repositorio.ObtenerPorId(id);
                ViewBag.Contrato = TempData["Contrato"];
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
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Pago p)
        {
            try
            {
                var idC = repositorio.ObtenerPorId(id);
                repositorio.Baja(p);
                TempData["Mensaje"] = "El Pago se eliminó con éxito";
                return RedirectToAction("Index" , new { id = idC.ContratoId});
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