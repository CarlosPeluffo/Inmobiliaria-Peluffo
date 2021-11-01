using System.Diagnostics.Contracts;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Inmobiliaria_Peluffo.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
namespace Inmobiliaria_Peluffo.API
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class PropietariosController : ControllerBase
    {
        private readonly DataContext contexto;
        private readonly IConfiguration configuration;

        public PropietariosController(DataContext contexto, IConfiguration configuration)
        {
            this.contexto = contexto;
            this.configuration = configuration;
        }
        [HttpGet]
        public async Task<ActionResult<Propietario>> Get(){
            try
            {
                var usuario = User.Identity.Name;
                return await contexto.propietarios.SingleOrDefaultAsync(x => x.Mail == usuario);
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
                var entidad = await contexto.propietarios.SingleOrDefaultAsync(x => x.Id == id);
                return entidad != null ? Ok(entidad) : NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(){
            try
            {
                return Ok(await contexto.propietarios.ToListAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login ([FromForm] LoginView loginView){
            try
            {
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: loginView.Clave,
                    salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 1000,
                    numBytesRequested: 256 / 8));
                var propietario = await contexto.propietarios.FirstOrDefaultAsync(x => x.Mail == loginView.Usuario);
                if( propietario == null || propietario.Clave != hashed)
                {
                    return BadRequest("Usuario o clave incorrectos");
                }
                else
                {
                    var key = new SymmetricSecurityKey(
                        System.Text.Encoding.ASCII.GetBytes(configuration["TokenAuthentication:SecretKey"]));
                    var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, propietario.Mail),
                        new Claim("FullName", propietario.Nombre + " " + propietario.Apellido),
                        new Claim(ClaimTypes.Role, "Propietario"),
                    };
                    var token = new JwtSecurityToken(
                        issuer: configuration["TokenAuthentication:Issuer"],
                        audience: configuration["TokenAuthentication:Audience"],
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(60),
                        signingCredentials: credenciales
                    );
                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm]Propietario entidad){
            try
            {
                if (ModelState.IsValid)
                {
                    await contexto.propietarios.AddAsync(entidad);
                    contexto.SaveChanges();
                    return CreatedAtAction(nameof(Get), new { id = entidad.Id }, entidad);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Propietario entidad){
            try
            {
                var usuario = User.Identity.Name;
                var prop = contexto.propietarios.AsNoTracking().Single(x => x.Mail == usuario);
                if (entidad.Mail == usuario)
                {
                    entidad.Id = prop.Id;
                    contexto.propietarios.Update(entidad);
                    await contexto.SaveChangesAsync();
                    return Ok(entidad);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var p = await contexto.propietarios.FindAsync(id);
                    if (p == null)
                        return NotFound();
                    contexto.propietarios.Remove(p);
                    contexto.SaveChanges();
                    return Ok(p);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("test")]
        [AllowAnonymous]
        public IActionResult Test()
        {
            try
            {
                return Ok("anduvo");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("test/{codigo}")]
        [AllowAnonymous]
        public IActionResult Code(int codigo)
        {
            try
            {
                return StatusCode(codigo, new { Mensaje = "Anduvo", Error = false });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}