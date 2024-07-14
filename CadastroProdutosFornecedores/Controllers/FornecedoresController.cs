using CadastroProdutosFornecedores.Core.Entities;
using CadastroProdutosFornecedores.Core.Services;
using Microsoft.AspNetCore.Mvc;
namespace CadastroProdutosFornecedores.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FornecedoresController : ControllerBase
    {
        private readonly FornecedorService _fornecedorService;

        public FornecedoresController(FornecedorService fornecedorService)
        {
            _fornecedorService = fornecedorService;
        }

        [HttpGet("ListarTodos")]
        public async Task<ActionResult<IEnumerable<Fornecedor>>> ListarTodosFornecedores()
        {
            var fornecedores = await _fornecedorService.ListarTodosFornecedores();
            return Ok(fornecedores);
        }

        [HttpGet("Obter/{id}")]
        public async Task<ActionResult<Fornecedor>> ObterFornecedorPorId(int id)
        {
            var fornecedor = await _fornecedorService.ObterFornecedorPorId(id);
            if (fornecedor == null)
                return NotFound();

            return Ok(fornecedor);
        }

        [HttpPost("Adicionar")]
        public async Task<ActionResult<Fornecedor>> AdicionarFornecedor(Fornecedor fornecedor)
        {
            try
            {
                var fornecedorCriado = await _fornecedorService.AdicionarFornecedor(fornecedor);
                return CreatedAtAction(nameof(ObterFornecedorPorId), new { id = fornecedorCriado.Id }, fornecedorCriado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Atualizar/{id}")]
        public async Task<IActionResult> AtualizarFornecedor(int id, Fornecedor fornecedor)
        {
            if (id != fornecedor.Id)
                return BadRequest();

            try
            {
                await _fornecedorService.AtualizarFornecedor(fornecedor);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Remover/{id}")]
        public async Task<IActionResult> RemoverFornecedor(int id)
        {
            try
            {
                await _fornecedorService.RemoverFornecedor(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
