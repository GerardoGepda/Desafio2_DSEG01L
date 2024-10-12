using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BO200360_PD200491_Desafio2.Models;
using Microsoft.AspNetCore.Authorization;

namespace BO200360_PD200491_Desafio2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CvsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CvsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Cvs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cv>>> GetCvs()
        {
            return await _context.Cvs.ToListAsync();
        }

        // GET: api/Cvs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cv>> GetCv(int id)
        {
            var cv = await _context.Cvs.FindAsync(id);

            if (cv == null)
            {
                return NotFound();
            }

            return cv;
        }

        // GET: api/Cvs/Candidato/5
        [HttpGet("Candidato/{candidatoId}")]
        public async Task<ActionResult<IEnumerable<Cv>>> GetCvsByCandidatoId(int candidatoId)
        {
            var cvs = await _context.Cvs.Where(cv => cv.CandidatoId == candidatoId).ToListAsync();

            if (cvs == null || cvs.Count == 0)
            {
                return NotFound();
            }

            return cvs;
        }

        // PUT: api/Cvs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCv(int id, Cv cv)
        {
            if (id != cv.Id)
            {
                return BadRequest();
            }

            _context.Entry(cv).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CvExists(id))
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

        // POST: api/Cvs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Cv>> PostCv(Cv cv)
        {
            var candidatoExists = await _context.Candidatos.AnyAsync(c => c.Id == cv.CandidatoId);
            if (!candidatoExists)
            {
                return BadRequest("CandidatoId no existe.");
            }

            _context.Cvs.Add(cv);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCv", new { id = cv.Id }, cv);
        }

        // DELETE: api/Cvs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCv(int id)
        {
            var cv = await _context.Cvs.FindAsync(id);
            if (cv == null)
            {
                return NotFound();
            }

            _context.Cvs.Remove(cv);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CvExists(int id)
        {
            return _context.Cvs.Any(e => e.Id == id);
        }
    }
}
