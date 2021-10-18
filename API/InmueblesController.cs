using System;
using System.Linq;
using System.Threading.Tasks;
using Inmobiliaria_Peluffo.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
namespace Inmobiliaria_Peluffo.API
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class InmueblesController : ControllerBase
    {
        private readonly DataContext context;
        private readonly IConfiguration configuration;
        public InmueblesController(DataContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }
        [HttpGet] //ActionResult<Inmueble>
        public async Task<IActionResult> Get(){
            try
            {
                var usuario = User.Identity.Name;
                var lista = await context.inmuebles
                    .Include(x => x.Propietario)
                    .Where(x => x.Propietario.Mail== usuario).ToListAsync();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id){
            try
            {
                var usuario = User.Identity.Name;
                var entidad = await context.inmuebles.Include(x => x.Propietario).Where(x => x.Propietario.Mail == usuario).SingleAsync(x => x.Id == id);
                return entidad != null ? Ok(entidad) : NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] Inmueble inmueble){
            try
            {
                if(ModelState.IsValid){
                    var usuario = User.Identity.Name;
                    inmueble.PropietarioId = context.propietarios.Single(x => x.Mail == usuario).Id;
                    context.inmuebles.Add(inmueble);
                    await context.SaveChangesAsync();
                    return CreatedAtAction(nameof(Get), new {id = inmueble.Id}, inmueble);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromForm] Inmueble inmueble){
            try
            {
                var usuario = User.Identity.Name;
                if(ModelState.IsValid && context.inmuebles.AsNoTracking().Include(x => x.Propietario).FirstOrDefault(x => x.Id == id && x.Propietario.Mail == usuario ) !=null){
                    inmueble.Id = id;
                    inmueble.PropietarioId = context.propietarios.Single(x => x.Mail == usuario).Id;
                    context.inmuebles.Update(inmueble);
                    await context.SaveChangesAsync();
                    return Ok(inmueble);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id){
            try
            {
                var usuario = User.Identity.Name;
                var inmueble = context.inmuebles.Include(x => x.Propietario).FirstOrDefault(x => x.Id == id && x.Propietario.Mail == usuario);
                if(inmueble != null){
                    inmueble.Estado = false;
                    context.inmuebles.Update(inmueble);
                    await context.SaveChangesAsync();
                    return Ok(inmueble);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}