using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebPDV.Data;
using WebPDV.Models;

namespace WebPDV.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class clientesController : ControllerBase
    {
        private readonly AplicacaoDbContext _context;

        public clientesController(AplicacaoDbContext context)
        {
            _context = context;
        }

        // GET: api/clientes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> ObterTodos()
        {
            return Ok(await _context.clientes.ToListAsync());
        }

        // GET: api/clientes/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> ObterPorId(int id)
        {
            var cliente = await _context.clientes.FindAsync(id);
            if (cliente == null) return NotFound();
            return Ok(cliente);
        }

        // POST: api/clientes
        [HttpPost]
        public async Task<ActionResult<Cliente>> Criar(Cliente cliente)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _context.clientes.Add(cliente);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(ObterPorId), new { id = cliente.IdCliente }, cliente);
        }

        // PUT: api/clientes/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, Cliente cliente)
        {
            if (id != cliente.IdCliente) return BadRequest();
            _context.Entry(cliente).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteExiste(id)) return NotFound();
                throw;
            }
            return NoContent();
        }

        // DELETE: api/clientes/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var cliente = await _context.clientes.FindAsync(id);
            if (cliente == null) return NotFound();
            _context.clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Método para verificar se o atendente existe.
        private bool ClienteExiste(int id)
        {
            return _context.clientes.Any(e => e.IdCliente == id);
        }
    }
}