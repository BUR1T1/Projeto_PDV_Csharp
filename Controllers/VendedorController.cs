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

        // GET: api/VendedorControllers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vendedor>>> ObterTodos()
        {
            var vendedores = await _context.vendedores.ToListAsync();
            return Ok(vendedores);
        }

        // GET: api/VendedorControllers/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Vendedor>> ObterPorId(int id)
        {
            var vendedor = await _context.vendedores.FindAsync(id);
            if (vendedor == null)
                return NotFound();

            return Ok(vendedor);
        }

        // POST: api/VendedorControllers
        [HttpPost]
        public async Task<ActionResult<Vendedor>> Criar(Vendedor vendedor)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.vendedores.Add(vendedor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ObterPorId), new { id = vendedor.Id }, vendedor);
        }

        // PUT: api/VendedorControllers/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, Vendedor vendedor)
        {
            if (id != vendedor.Id)
                return BadRequest();

            _context.Entry(vendedor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VendedorExiste(id))
                    return NotFound();

                throw;
            }

            return NoContent();
        }

        // DELETE: api/VendedorControllers/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var vendedor = await _context.vendedores.FindAsync(id);
            if (vendedor == null)
                return NotFound();

            _context.vendedores.Remove(vendedor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Verifica se o vendedor existe no banco
        private bool VendedorExiste(int id)
        {
            return _context.vendedores.Any(e => e.Id == id);
        }
    }
}
