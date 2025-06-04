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

        public VendasController(AplicacaoDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Venda>>> ObterTodas()
        {
            var vendas = await _context.Vendas
                .Include(v => v.ItensDaVenda)
                    .ThenInclude(i => i.Produto)
                .ToListAsync();
            return Ok(vendas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Venda>> ObterPorId(int id)
        {
            var venda = await _context.Vendas
                .Include(v => v.ItensDaVenda)
                    .ThenInclude(i => i.Produto)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (venda == null)
                return NotFound($"Venda com ID {id} não foi encontrado.");

            return Ok(venda);
        }

        [HttpPost]
        public async Task<ActionResult> Criar(Venda venda)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var itensParaAdicionar = new List<Venda.ItemDaVenda>();
            decimal valorTotalDaVenda = 0;

            foreach (var itemRecebido in venda.ItensDaVenda)
            {
                var produto = await _context.Produtos.FindAsync(itemRecebido.ProdutoId);

                if (produto == null)
                    return BadRequest($"Produto com ID {itemRecebido.ProdutoId} não encontrado.");

                if (produto.QuantidedeDeEstoque < itemRecebido.Quantidade)
                    return BadRequest($"Estoque insuficiente para o produto {produto.NomeProduto}");

                produto.QuantidedeDeEstoque -= itemRecebido.Quantidade;

                var novoItemVenda = new Venda.ItemDaVenda
                {
                    ProdutoId = produto.Id,
                    Produto = produto,
                    Quantidade = itemRecebido.Quantidade,
                    NomeProduto = produto.NomeProduto,
                    ValorUnitario = produto.ValorDeVenda
                };
                itensParaAdicionar.Add(novoItemVenda);
                valorTotalDaVenda += novoItemVenda.Subtotal;
            }

            venda.ItensDaVenda.Clear();
            venda.ItensDaVenda.AddRange(itensParaAdicionar);
            venda.ValorDaVenda = valorTotalDaVenda;

            _context.Vendas.Add(venda);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ObterPorId), new { id = venda.Id }, new { message = "Venda cadastrada com sucesso", data = venda });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Atualizar(int id, Venda venda)
        {
            if (id != venda.Id)
                return BadRequest("O ID na URL não corresponde ao ID da venda fornecida.");

            _context.Entry(venda).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VendaExiste(id))
                    return NotFound($"Venda com ID {id} não foi encontrado para atualização.");
                throw;
            }

            return Ok("Venda editada com sucesso");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Deletar(int id)
        {
            var venda = await _context.Vendas.FindAsync(id);
            if (venda == null)
                return NotFound($"Venda com ID {id} não foi encontrado para exclusão.");

            _context.Vendas.Remove(venda);
            await _context.SaveChangesAsync();

            return Ok("Venda deletada com sucesso");
        }

        private bool VendaExiste(int id)
        {
            return _context.Vendas.Any(e => e.Id == id);
        }
    }
}