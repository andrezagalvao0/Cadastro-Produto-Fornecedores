using NUnit.Framework;
using Moq;
using CadastroProdutosFornecedores.Core.Services;
using CadastroProdutosFornecedores.Core.Entities;
using CadastroProdutosFornecedores.Infrastructure.Data.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace CadastroProdutosFornecedoresTest
{
    [TestFixture]
    public class ProdutoServiceTests
    {
        private Mock<IProdutoRepository> _produtoRepositoryMock;
        private ProdutoService _produtoService;

        [SetUp]
        public void SetUp()
        {
            _produtoRepositoryMock = new Mock<IProdutoRepository>();
            _produtoService = new ProdutoService(_produtoRepositoryMock.Object);
        }

        [Test]
        public async Task ListarTodosProdutos_DeveRetornarTodosProdutos()
        {
            var produtos = new List<Produto> { new Produto { Id = 1, Descricao = "Produto 1" } };
            _produtoRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(produtos);

            var result = await _produtoService.ListarTodosProdutos();

            Assert.AreEqual(produtos, result);
        }

        [Test]
        public async Task ObterProdutoPorId_DeveRetornarProduto()
        {
            var produto = new Produto { Id = 1, Descricao = "Produto 1" };
            _produtoRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(produto);

            var result = await _produtoService.ObterProdutoPorId(1);

            Assert.AreEqual(produto, result);
        }

        [Test]
        public void AdicionarProduto_DescricaoJaExiste_DeveLancarArgumentException()
        {
            var produto = new Produto { Descricao = "Produto 1" };
            _produtoRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Produto, bool>>>())).ReturnsAsync(true);

            Assert.ThrowsAsync<ArgumentException>(async () => await _produtoService.AdicionarProduto(produto));
        }

        [Test]
        public async Task AdicionarProduto_DadosValidos_DeveAdicionarProduto()
        {
            var produto = new Produto { Descricao = "Produto 1" };
            _produtoRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Produto, bool>>>())).ReturnsAsync(false);

            var result = await _produtoService.AdicionarProduto(produto);

            _produtoRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Produto>()), Times.Once);
            Assert.AreEqual(produto.Descricao, result.Descricao);
        }

        [Test]
        public void AtualizarProduto_ProdutoNaoExiste_DeveLancarArgumentException()
        {
            var produto = new Produto { Id = 1, Descricao = "Produto 1" };
            _produtoRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Produto, bool>>>())).ReturnsAsync(false);

            Assert.ThrowsAsync<ArgumentException>(async () => await _produtoService.AtualizarProduto(produto));
        }

        [Test]
        public async Task AtualizarProduto_ProdutoExiste_DeveAtualizarProduto()
        {
            var produto = new Produto { Id = 1, Descricao = "Produto 1" };
            _produtoRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Produto, bool>>>())).ReturnsAsync(true);

            var result = await _produtoService.AtualizarProduto(produto);

            _produtoRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Produto>()), Times.Once);
            Assert.AreEqual(produto.Descricao, result.Descricao);
        }

        [Test]
        public void RemoverProduto_ProdutoNaoExiste_DeveLancarArgumentException()
        {
            _produtoRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Produto, bool>>>())).ReturnsAsync(false);

            Assert.ThrowsAsync<ArgumentException>(async () => await _produtoService.RemoverProduto(1));
        }

        [Test]
        public async Task RemoverProduto_ProdutoExiste_DeveRemoverProduto()
        {
            _produtoRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Produto, bool>>>())).ReturnsAsync(true);

            await _produtoService.RemoverProduto(1);

            _produtoRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<int>()), Times.Once);
        }
    }
}
