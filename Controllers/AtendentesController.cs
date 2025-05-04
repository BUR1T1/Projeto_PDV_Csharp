using Microsoft.AspNetCore.Mvc;
using WebPDV.Data;
using WebPDV.Models;

namespace WebPDV.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AtendentesController : ControllerBase
    {
        private readonly AplicacaoDbContext _context;

        public AtendentesController(AplicacaoDbContext context)
        {
            _context = context;
        }

        // GET: api/atendentes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Atendente>>> ObterTodos()
        {
            return Ok(await _context.Atendentes.ToListAsync());
        }

        // GET: api/atendentes/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Atendente>> ObterPorId(int id)
        {
            var atendente = await _context.Atendentes.FindAsync(id);
            if (atendente == null) return NotFound();
            return Ok(atendente);
        }

        // POST: api/atendentes
        [HttpPost]
        public async Task<ActionResult<Atendente>> Criar(Atendente atendente)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _context.Atendentes.Add(atendente);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(ObterPorId), new { id = atendente.Id }, atendente);
        }

        // PUT: api/atendentes/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, Atendente atendente)
        {
            if (id != atendente.Id) return BadRequest();
            _context.Entry(atendente).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AtendenteExiste(id)) return NotFound();
                throw;
            }
            return NoContent();
        }

        // DELETE: api/atendentes/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var atendente = await _context.Atendentes.FindAsync(id);
            if (atendente == null) return NotFound();
            _context.Atendentes.Remove(atendente);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Método para verificar se o atendente existe.
        private bool AtendenteExiste(int id)
        {
            return _context.Atendentes.Any(e => e.Id == id);
        }
    }
}