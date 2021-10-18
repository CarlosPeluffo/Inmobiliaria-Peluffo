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
    public class PagosController : ControllerBase
    {
        private readonly DataContext context;
        private readonly IConfiguration configuration;
        public PagosController(DataContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id){
            try
            {
                var lista = await context.pagos.Include(x => x.Contrato)
                                    .Where(x => x.Contrato.Id == id).ToListAsync();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}