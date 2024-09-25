using BO200360_PD200491_Desafio2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BO200360_PD200491_Desafio2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<ActionResult<Candidato>> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Buscar al candidato por codigo
            var candidato = await _context.Candidatos.SingleOrDefaultAsync(c => c.Codigo == request.Codigo);
            if (candidato == null)
                return Unauthorized("Credenciales inválidas");

            // Verificar la contraseña
            if (!BCrypt.Net.BCrypt.Verify(request.Contrasena, candidato.Contrasena))
                return Unauthorized("Credenciales inválidas");

            return Ok(candidato);
        }
    }
}
