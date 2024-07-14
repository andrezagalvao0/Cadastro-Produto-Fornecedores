using CadastroProdutosFornecedores.Core.Entities;
using CadastroProdutosFornecedores.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace CadastroProdutosFornecedores.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly ProdutoService _produtoService;

        public ProdutosController(ProdutoService produtoService)
        {
            _produtoService = produtoService;
        }

        [HttpGet("ListarTodos")]
        public async Task<ActionResult<IEnumerable<Produto>>> ListarTodosProdutos()
        {
            var produtos = await _produtoService.ListarTodosProdutos();
            return Ok(produtos);
        }

        [HttpGet("Obter/{id}")]
        public async Task<ActionResult<Produto>> ObterProdutoPorId(int id)
        {
            var produto = await _produtoService.ObterProdutoPorId(id);
            if (produto == null)
                return NotFound();

            return Ok(produto);
        }

        /// <summary>
        /// EndPoint para cadastro de produto -  passar null no campo fornecedorId, pois não há necessidade de passar 
        /// o ID do fornecedor no cadstro do produto
        /// </summary>
        /// <param name="produto"></param>
        /// <returns></returns>
        [HttpPost("Adicionar")]
        public async Task<ActionResult<Produto>> AdicionarProduto(Produto produto)
        {
            try
            {
                var produtoCriado = await _produtoService.AdicionarProduto(produto);
                return CreatedAtAction(nameof(ObterProdutoPorId), new { id = produtoCriado.Id }, produtoCriado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Atualizar/{id}")]
        public async Task<IActionResult> AtualizarProduto(int id, Produto produto)
        {
            if (id != produto.Id)
                return BadRequest();

            try
            {
                await _produtoService.AtualizarProduto(produto);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Remover/{id}")]
        public async Task<IActionResult> RemoverProduto(int id)
        {
            try
            {
                await _produtoService.RemoverProduto(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
