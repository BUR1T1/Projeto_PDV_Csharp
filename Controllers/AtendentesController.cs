using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebPDV.Data;
using WebPDV.Models;

namespace WebPDV.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VendedorControllers : ControllerBase
    {
        private readonly AplicacaoDbContext _context;

        public VendedorControllers(AplicacaoDbContext context)
        {
            _context = context;
        }

        // GET: api/Vendedors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vendedor>>> ObterTodos()
        {
            return Ok(await _context.vendedores.ToListAsync());
        }

        // GET: api/Vendedors/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Vendedor>> ObterPorId(int id)
        {
            var Vendedor = await _context.vendedores.FindAsync(id);
            if (Vendedor == null) return NotFound();
            return Ok(Vendedor);
        }

        // POST: api/Vendedors
        [HttpPost]
        public async Task<ActionResult<Vendedor>> Criar(Vendedor Vendedor)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _context.vendedores.Add(Vendedor);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(ObterPorId), new { id = Vendedor.IdVendedor }, Vendedor);
        }

        // PUT: api/Vendedors/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, Vendedor Vendedor)
        {
            if (id != Vendedor.IdVendedor) return BadRequest();
            _context.Entry(Vendedor).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VendedorExiste(id)) return NotFound();
                throw;
            }
            return NoContent();
        }

        // DELETE: api/Vendedors/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var Vendedor = await _context.vendedores.FindAsync(id);
            if (Vendedor == null) return NotFound();
            _context.vendedores.Remove(Vendedor);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Método para verificar se o Vendedor existe.
        private bool VendedorExiste(int id)
        {
            return _context.vendedores.Any(e => e.IdVendedor == id);
        }
    }
}