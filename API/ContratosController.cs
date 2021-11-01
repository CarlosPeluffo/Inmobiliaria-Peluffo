using System;
using System.Linq;
using System.Collections;
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
    public class ContratosController : ControllerBase
    {
        private readonly DataContext context;
        private readonly IConfiguration configuration;
        public ContratosController(DataContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }
        [HttpGet]
        public async Task<IActionResult> Get(){
            try
            {
                var usuario = User.Identity.Name;
                var lista = await context.contratos
                                .Include(x => x.Inquilino)
                                .Include(x => x.Inmueble)
                                .Where(x => x.Inmueble.Propietario.Mail == usuario
                                        && x.Cancelado == false && x.FechaFin > DateTime.Now).ToListAsync();
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
                var contrato = await context.contratos.Include(x => x.Inquilino)
                                    .Include(x => x.Inmueble)
                                    .Where(x => x.Inmueble.Propietario.Mail == usuario)
                                    .SingleOrDefaultAsync(x => x.Id == id);
                return contrato != null ? Ok(contrato) : NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}