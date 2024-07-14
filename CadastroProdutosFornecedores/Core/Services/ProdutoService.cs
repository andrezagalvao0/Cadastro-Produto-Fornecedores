using CadastroProdutosFornecedores.Core.Entities;
using CadastroProdutosFornecedores.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;


namespace CadastroProdutosFornecedores.Core.Services
{
    public class ProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoService(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        public async Task<IEnumerable<Produto>> ListarTodosProdutos()
        {
            return await _produtoRepository.GetAllAsync();
        }

        public async Task<Produto> ObterProdutoPorId(int id)
        {
            return await _produtoRepository.GetByIdAsync(id);
        }

        public async Task<Produto> AdicionarProduto(Produto produto)
        {
            if (await _produtoRepository.ExistsAsync(p => p.Descricao == produto.Descricao))
                throw new ArgumentException("Já existe um produto com esta descrição.");

            await _produtoRepository.AddAsync(produto);
            return produto;
        }

        public async Task<Produto> AtualizarProduto(Produto produto)
        {
            if (!await _produtoRepository.ExistsAsync(p => p.Id == produto.Id))
                throw new ArgumentException("Produto não encontrado.");

            await _produtoRepository.UpdateAsync(produto);
            return produto;
        }

        public async Task RemoverProduto(int id)
        {
            if (!await _produtoRepository.ExistsAsync(p => p.Id == id))
                throw new ArgumentException("Produto não encontrado.");

            await _produtoRepository.DeleteAsync(id);
        }
    }
}
