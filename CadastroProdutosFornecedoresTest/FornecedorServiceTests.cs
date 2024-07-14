using CadastroProdutosFornecedores.Core.Exceptions;
using CadastroProdutosFornecedores.Infrastructure.Data.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using CadastroProdutosFornecedores.Core.Entities;
using CadastroProdutosFornecedores.Core.Services;
using Moq;
using System.Linq.Expressions;
using System.Net;

namespace CadastroProdutosFornecedoresTest
{
    [TestFixture]
    public class FornecedorServiceTests
    {
        private Mock<IFornecedorRepository> _fornecedorRepositoryMock;
        private ViaCepService _viaCepService;
        private FornecedorService _fornecedorService;

        [SetUp]
        public void SetUp()
        {
            _fornecedorRepositoryMock = new Mock<IFornecedorRepository>();
            var httpClient = new HttpClient(new MockHttpMessageHandler())
            {
                BaseAddress = new Uri("https://viacep.com.br/ws/") 

            };
            _viaCepService = new ViaCepService(httpClient);
            _fornecedorService = new FornecedorService(_fornecedorRepositoryMock.Object, _viaCepService);
           
        }

        public class MockHttpMessageHandler : HttpMessageHandler
        {
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("{\"cep\":\"12345678\",\"logradouro\":\"Rua Teste\",\"bairro\":\"Bairro Teste\",\"localidade\":\"Cidade Teste\",\"uf\":\"UF\"}")
                };

                return Task.FromResult(response);
            }
        }

        [Test]
        public async Task ListarTodosFornecedores_DeveRetornarTodosFornecedores()
        {
            var fornecedores = new List<Fornecedor> { new Fornecedor { Id = 1, Nome = "Fornecedor 1" } };
            _fornecedorRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(fornecedores);

            var result = await _fornecedorService.ListarTodosFornecedores();

            Assert.AreEqual(fornecedores, result);
        }

        [Test]
        public void ObterFornecedorPorId_FornecedorNaoExiste_DeveLancarNotFoundException()
        {
            _fornecedorRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Fornecedor)null);

            Assert.ThrowsAsync<NotFoundException>(async () => await _fornecedorService.ObterFornecedorPorId(1));
        }

        [Test]
        public async Task ObterFornecedorPorId_FornecedorExiste_DeveRetornarFornecedor()
        {
            var fornecedor = new Fornecedor { Id = 1, Nome = "Fornecedor 1" };
            _fornecedorRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(fornecedor);

            var result = await _fornecedorService.ObterFornecedorPorId(1);

            Assert.AreEqual(fornecedor, result);
        }

        [Test]
        public void AdicionarFornecedor_CNPJJaExiste_DeveLancarArgumentException()
        {
            var fornecedor = new Fornecedor { CNPJ = "12345678901234" };
            _fornecedorRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Fornecedor, bool>>>())).ReturnsAsync(true);

            Assert.ThrowsAsync<ArgumentException>(async () => await _fornecedorService.AdicionarFornecedor(fornecedor));
        }

        [Test]
        public async Task AdicionarFornecedor_CEPInvalido_DeveLancarArgumentException()
        {
            var fornecedor = new Fornecedor { CNPJ = "12345678901234", Endereco = new Endereco { Cep = "12345678" } };
            _fornecedorRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Fornecedor, bool>>>())).ReturnsAsync(true);

            Assert.ThrowsAsync<ArgumentException>(async () => await _fornecedorService.AdicionarFornecedor(fornecedor));
        }

        [Test]
        public async Task AdicionarFornecedor_DadosValidos_DeveAdicionarFornecedor()
        {
            var fornecedor = new Fornecedor { CNPJ = "12345678901234", Endereco = new Endereco { Cep = "12345678" } };

            _fornecedorRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Fornecedor, bool>>>()))
                                    .ReturnsAsync(false); 

            var result = await _fornecedorService.AdicionarFornecedor(fornecedor);

            _fornecedorRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Fornecedor>()), Times.Once);
            Assert.AreEqual("12345678", result.Endereco.Cep);
        }


    }
}
