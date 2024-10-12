using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BO200360_PD200491_Desafio2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BO200360_PD200491_Desafio2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CandidatosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public CandidatosController(AppDbContext context, UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        // GET: api/Candidatos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Candidato>>> GetCandidatos()
        {
            return await _context.Candidatos.ToListAsync();
        }

        // GET: api/Candidatos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Candidato>> GetCandidato(int id)
        {
            var candidato = await _context.Candidatos.FindAsync(id);

            if (candidato == null)
            {
                return NotFound();
            }

            return candidato;
        }

        // PUT: api/Candidatos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCandidato(int id, Candidato candidato)
        {
            if (id != candidato.Id)
            {
                return BadRequest();
            }

            _context.Entry(candidato).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CandidatoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Candidatos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<Candidato>> PostCandidato(Candidato candidato)
        {
            // crear usuario en Identity
            var user = new IdentityUser
            {
                UserName = candidato.Email,
                Email = candidato.Email
            };
            var result = await _userManager.CreateAsync(user, candidato.Contrasena);

            if (result.Succeeded)
            {
                // Generando código único para el candidato
                var codigoUnico = GenerarCodigoUnico(candidato.Apellidos);
                candidato.Codigo = codigoUnico;
                candidato.IdUser = user.Id;

                // Hashing de la contraseña
                candidato.Contrasena = BCrypt.Net.BCrypt.HashPassword(candidato.Contrasena);

                _context.Candidatos.Add(candidato);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetCandidato", new { id = candidato.Id }, candidato);
            }
            else
            {
                // Manejar los errores
                return BadRequest(result.Errors);
            }
        }

        // DELETE: api/Candidatos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCandidato(int id)
        {
            var candidato = await _context.Candidatos.FindAsync(id);
            if (candidato == null)
            {
                return NotFound();
            }

            _context.Candidatos.Remove(candidato);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CandidatoExists(int id)
        {
            return _context.Candidatos.Any(e => e.Id == id);
        }

        private string GenerarCodigoUnico(string apellidos)
        {
            var apellidosArr = apellidos.Split(' ');

            string iniciales;
            if (apellidosArr.Length >= 2)
            {
                iniciales = apellidosArr[0][0].ToString().ToUpper() + apellidosArr[1][0].ToString().ToUpper();
            }
            else
            {
                iniciales = apellidosArr[0][0].ToString().ToUpper() + apellidosArr[0][1].ToString().ToUpper();
            }

            // Generando número aleatorio de 8 dígitos
            var random = new Random();
            var numeroAleatorio = random.Next(10000000, 99999999); // Genera un número aleatorio de 8 dígitos

            // Paso 3: Combinar iniciales y número aleatorio
            var codigoUnico = iniciales + numeroAleatorio;

            // Paso 4: Verificar si el código es único en la base de datos
            while (_context.Candidatos.Any(c => c.Codigo == codigoUnico))
            {
                // Si ya existe un candidato con el mismo código, generar uno nuevo
                numeroAleatorio = random.Next(10000000, 99999999);
                codigoUnico = iniciales + numeroAleatorio;
            }

            return codigoUnico;
        }
    }
}
