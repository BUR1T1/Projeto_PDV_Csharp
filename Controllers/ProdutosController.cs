using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebPDV.Data;
using WebPDV.Models;

namespace WebPDV.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly AplicacaoDbContext _context;

        public ProdutosController(AplicacaoDbContext context)
        {
            _context = context;
        }

        // GET: api/Produtos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> ObterTodos()
        {
            return Ok(await _context.Produtos.ToListAsync());
        }

        // GET: api/Produtos/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Produto>> ObterPorId(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null) return NotFound();
            return Ok(produto);
        }

        // POST: api/Produtos
        [HttpPost]
        public async Task<ActionResult<Produto>> Criar(Produto produto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(ObterPorId), new { id = produto.Id }, produto);
        }

        // PUT: api/Produtos/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, Produto produto)
        {
            if (id != produto.Id) return BadRequest();
            _context.Entry(produto).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProdutoExiste(id)) return NotFound();
                throw;
            }
            return NoContent();
        }

        // DELETE: api/Produtos/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null) return NotFound();
            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Método para verificar se o produto existe.
        private bool ProdutoExiste(int id)
        {
            return _context.Produtos.Any(e => e.Id == id);
        }
    }
}