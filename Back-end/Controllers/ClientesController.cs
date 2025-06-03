using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebPDV.Data;
using WebPDV.Models;

namespace WebPDV.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly AplicacaoDbContext _context;

        public ClientesController(AplicacaoDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> ObterTodos()
        {
            return Ok(await _context.clientes.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> ObterPorId(int id)
        {
            var cliente = await _context.clientes.FindAsync(id);
            if (cliente == null) return NotFound();
            return Ok(cliente);
        }

        [HttpPost]
        public async Task<ActionResult<Cliente>> Criar(Cliente cliente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.clientes.Add(cliente);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(ObterPorId), new { id = cliente.Id }, cliente);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return BadRequest("O ID na URL não corresponde ao ID do cliente fornecido.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(cliente).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteExiste(id))
                {
                    return NotFound("CLIENTE NÃO ENCOONTRADO");
                }
                else
                {
                    throw; 
                }
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var cliente = await _context.clientes.FindAsync(id);
            if (cliente == null) return NotFound("Cliente não encontrado para exclusão.");
            
            _context.clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool ClienteExiste(int id)
        {
            return _context.clientes.Any(e => e.Id == id);
        }
    }
}