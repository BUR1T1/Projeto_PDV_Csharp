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
        public ProdutosController(AplicacaoDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> ObterTodos()
        {
            return Ok(await _context.Produtos.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Produto>> ObterPorId(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null) return NotFound($"Produto com ID {id} não foi encontrado.");
            return Ok(produto);
        }

        [HttpPost]
        public async Task<ActionResult> Criar(Produto produto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(ObterPorId), new { id = produto.Id }, new { message = "Produto cadastrado com sucesso", data = produto });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Atualizar(int id, Produto produto)
        {
            if (id != produto.Id) return BadRequest("O ID na URL não corresponde ao ID do produto fornecido.");
            _context.Entry(produto).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProdutoExiste(id)) return NotFound($"Produto com ID {id} não foi encontrado para atualização.");
                throw;
            }
            return Ok("Produto editado com sucesso");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Deletar(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null) return NotFound($"Produto com ID {id} não foi encontrado para exclusão.");
            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();
            return Ok("Produto deletado com sucesso");
        }

        private bool ProdutoExiste(int id)
        {
            return _context.Produtos.Any(e => e.Id == id);
        }
    }
}