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
                .FirstOrDefaultAsync(v => v.NumeroDeSequencai == id);

            if (venda == null)
                return NotFound();

            return Ok(venda);
        }

        // GET: api/clientes/{clienteId}/vendas
      /*  [HttpGet("~/api/v1/clientes/{clienteId}/vendas")]
        public async Task<ActionResult<IEnumerable<Venda>>> ObterPorCliente(int IdCliente)
        {
            var vendas = await _context.Vendas
                .Where(v => v.IdCliente == IdCliente)
                .Include(v => v.ItensDaVenda)
                .ToListAsync();

            return Ok(vendas);
        }

        // GET: api/atendentes/{atendenteId}/vendas
        [HttpGet("~/api/v1/atendentes/{atendenteId}/vendas")]
        public async Task<ActionResult<IEnumerable<Venda>>> ObterPorAtendente(int IdVendedor)
        {
            var vendas = await _context.Vendas
                .Where(v => v.IdVendedor == IdVendedor)
                .Include(v => v.ItensDaVenda)
                .ToListAsync();

            return Ok(vendas);
        }*/

        // POST: api/vendas
        [HttpPost]
        public async Task<ActionResult<Venda>> Criar(Venda venda)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Vendas.Add(venda);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ObterPorId), new { id = venda.NumeroDeSequencai }, venda);
        }

        // PUT: api/vendas/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, Venda venda)
        {
            if (id != venda.NumeroDeSequencai)
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
            return _context.Vendas.Any(e => e.NumeroDeSequencai == id);
        }
    }
}
