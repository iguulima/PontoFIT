using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PontoFIT.Api.Data;
using PontoFIT.Api.Models;

namespace PontoFIT.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MarcacoesPontoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MarcacoesPontoController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/marcacoesponto
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MarcacaoPonto>>> GetMarcacoesPonto()
        {
            // Incluindo os dados do usuário para cada marcação (join)
            return await _context.MarcacoesPonto.Include(m => m.Usuario).ToListAsync();
        }

        // GET: api/marcacoesponto/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MarcacaoPonto>> GetMarcacaoPonto(int id)
        {
            var marcacao = await _context.MarcacoesPonto.Include(m => m.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (marcacao == null)
                return NotFound();

            return marcacao;
        }

        // POST: api/marcacoesponto
        [HttpPost]
        public async Task<ActionResult<MarcacaoPonto>> CreateMarcacaoPonto(MarcacaoPonto marcacao)
        {
            _context.MarcacoesPonto.Add(marcacao);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMarcacaoPonto), new { id = marcacao.Id }, marcacao);
        }

        // PUT: api/marcacoesponto/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMarcacaoPonto(int id, MarcacaoPonto marcacao)
        {
            if (id != marcacao.Id)
                return BadRequest();

            _context.Entry(marcacao).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.MarcacoesPonto.Any(m => m.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/marcacoesponto/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMarcacaoPonto(int id)
        {
            var marcacao = await _context.MarcacoesPonto.FindAsync(id);
            if (marcacao == null)
                return NotFound();

            _context.MarcacoesPonto.Remove(marcacao);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
