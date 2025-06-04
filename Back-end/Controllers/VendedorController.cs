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
        private readonly AplicacaoDbContext _context; public VendedorControllers(AplicacaoDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vendedor>>> ObterTodos()
        {
            var vendedores = await _context.vendedores.ToListAsync();
            return Ok(vendedores);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Vendedor>> ObterPorId(int id)
        {
            var vendedor = await _context.vendedores.FindAsync(id);
            if (vendedor == null)
                return NotFound($"Vendedor com ID {id} não foi encontrado.");
            return Ok(vendedor);
        }

        [HttpPost]
        public async Task<ActionResult> Criar(Vendedor vendedor)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            _context.vendedores.Add(vendedor);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(ObterPorId), new { id = vendedor.Id }, new { message = "Vendedor cadastrado com sucesso", data = vendedor });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Atualizar(int id, Vendedor vendedor)
        {
            if (id != vendedor.Id)
                return BadRequest("O ID na URL não corresponde ao ID do vendedor fornecido.");
            _context.Entry(vendedor).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VendedorExiste(id))
                    return NotFound($"Vendedor com ID {id} não foi encontrado para atualização.");
                throw;
            }
            return Ok("Vendedor atualizado com sucesso");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Deletar(int id)
        {
            var vendedor = await _context.vendedores.FindAsync(id);
            if (vendedor == null)
                return NotFound($"Vendedor com ID {id} não foi encontrado para exclusão.");
            _context.vendedores.Remove(vendedor);
            await _context.SaveChangesAsync();
            return Ok("Vendedor deletado com sucesso");
        }

        private bool VendedorExiste(int id)
        {
            return _context.vendedores.Any(e => e.Id == id);
        }
    }
}