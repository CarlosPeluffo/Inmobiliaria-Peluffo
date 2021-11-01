using System.Net.Http;
using System.Dynamic;
using System;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Inmobiliaria_Peluffo.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;

namespace Inmobiliaria_Peluffo.API
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class InmueblesController : ControllerBase
    {
        private readonly DataContext context;
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;
        public InmueblesController(DataContext context, IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.context = context;
            this.configuration = configuration;
            this.environment = environment;
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
        public async Task<IActionResult> Post([FromBody] Inmueble inmueble){
            try
            {
                    var usuario = User.Identity.Name;
                    if(inmueble.AvatarFile != null){
                        var stream = new MemoryStream(Convert.FromBase64String(inmueble.AvatarFile));
                        IFormFile imagen = new FormFile(stream, 0, stream.Length, "inmueble", ".jpg");
                        string wwwPath = environment.WebRootPath;
                        string path = Path.Combine(wwwPath, "UsersFiles");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                            string fileName = "photo_" + inmueble.Direccion + Path.GetExtension(imagen.FileName);
                            string pathCompleto = Path.Combine(path, fileName);
                            inmueble.Avatar = Path.Combine("/UsersFiles", fileName);
                            using (FileStream streamF = new FileStream(pathCompleto, FileMode.Create)){
                            imagen.CopyTo(streamF);
                        }
                        inmueble.PropietarioId = context.propietarios.Single(x => x.Mail == usuario).Id;
                        context.inmuebles.Add(inmueble);
                        await context.SaveChangesAsync();
                        return Ok(inmueble);
                        //return CreatedAtAction(nameof(Get), new {id = inmueble.Id}, inmueble);
                    }
                    else
                    {
                        return BadRequest("No entra al if");
                    }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        /*[HttpPut("{id}")]
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
        }*/
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id){
            try
            {
                var usuario = User.Identity.Name;
                var inmueble = context.inmuebles.Include(x => x.Propietario).FirstOrDefault(x => x.Id == id && x.Propietario.Mail == usuario);
                if(inmueble != null){
                    inmueble.Estado = !inmueble.Estado;
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