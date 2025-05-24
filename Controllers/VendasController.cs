using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebPDV.Data;
using WebPDV.Models;

namespace WebPDV.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VendasController : ControllerBase
    {
        private readonly AplicacaoDbContext _context;

        public VendasController(AplicacaoDbContext context)
        {
            _context = context;
        }

        // GET: api/vendas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Venda>>> ObterTodas()
        {
            var vendas = await _context.Vendas
                .Include(v => v.ItensDaVenda)
                .ToListAsync();

            return Ok(vendas);
        }

        // GET: api/vendas/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Venda>> ObterPorId(int id)
        {
            var venda = await _context.Vendas
                .Include(v => v.ItensDaVenda)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (venda == null)
                return NotFound();

            return Ok(venda);
        }

        // POST: api/vendas
      [HttpPost]
public async Task<ActionResult<Venda>> Criar(Venda venda)
{
    if (!ModelState.IsValid)
        return BadRequest(ModelState);

    var itensValidados = new List<Venda.ItemDaVenda>();

    foreach (var item in venda.ItensDaVenda)
    {
        // Busca o produto pelo ProdutoId correto
        var produto = await _context.produtos.FindAsync(item.ProdutoId);

        if (produto == null)
            return BadRequest($"Produto com ID {item.ProdutoId} não encontrado.");

        if (produto.QuantidedeDeEstoque < item.Quantidade)
            return BadRequest($"Estoque insuficiente para o produto {produto.NomeProduto}");

        // Reduz estoque diretamente no produto
        produto.QuantidedeDeEstoque -= item.Quantidade;

        // Cria um novo item de venda com dados corretos
        var itemVenda = new Venda.ItemDaVenda
        {
            ProdutoId = produto.Id,
            NomeProduto = produto.NomeProduto,
            Quantidade = item.Quantidade
        };

        itensValidados.Add(itemVenda);
    }

    // Substitui a lista de itens da venda pelos validados
    venda.ItensDaVenda.Clear();
    venda.ItensDaVenda.AddRange(itensValidados);

    // Adiciona a venda ao contexto
    _context.Vendas.Add(venda);

    // Salva tudo (inclui atualização de estoque)
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(ObterPorId), new { id = venda.Id }, venda);
}


        // PUT: api/vendas/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, Venda venda)
        {
            if (id != venda.Id)
                return BadRequest();

            _context.Entry(venda).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VendaExiste(id))
                    return NotFound();

                throw;
            }

            return NoContent();
        }

        // DELETE: api/vendas/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var venda = await _context.Vendas.FindAsync(id);
            if (venda == null)
                return NotFound();

            _context.Vendas.Remove(venda);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Verifica se a venda existe
        private bool VendaExiste(int id)
        {
            return _context.Vendas.Any(e => e.Id == id);
        }
    }
}
