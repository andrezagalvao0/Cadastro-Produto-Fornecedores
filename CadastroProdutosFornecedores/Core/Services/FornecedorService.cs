using CadastroProdutosFornecedores.Core.Entities;
using CadastroProdutosFornecedores.Core.Exceptions;
using CadastroProdutosFornecedores.Infrastructure.Data.Repositories;

namespace CadastroProdutosFornecedores.Core.Services
{
    public class FornecedorService
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly ViaCepService _viaCepService;

        public FornecedorService(IFornecedorRepository fornecedorRepository, ViaCepService viaCepService)
        {
            _fornecedorRepository = fornecedorRepository;
            _viaCepService = viaCepService;
        }

        public async Task<IEnumerable<Fornecedor>> ListarTodosFornecedores()
        {
            return await _fornecedorRepository.GetAllAsync();
        }

        public async Task<Fornecedor> ObterFornecedorPorId(int id)
        {
            var fornecedor = await _fornecedorRepository.GetByIdAsync(id);
            if (fornecedor == null)
                throw new NotFoundException("Fornecedor não encontrado.");
            return fornecedor;
        }

        public async Task<Fornecedor> AdicionarFornecedor(Fornecedor fornecedor)
        {
            if (await _fornecedorRepository.ExistsAsync(p => p.CNPJ == fornecedor.CNPJ))
                throw new ArgumentException("Já existe um fornecedor com este CNPJ.");

            try
            {
                var enderecoDTO = await _viaCepService.ObterEnderecoPorCep(fornecedor.Endereco.Cep);
                fornecedor.Endereco = new Endereco
                {
                    Cep = enderecoDTO.Cep,
                    Logradouro = enderecoDTO.Logradouro,
                    Complemento = enderecoDTO.Complemento,
                    Bairro = enderecoDTO.Bairro,
                    Localidade = enderecoDTO.Localidade,
                    Uf = enderecoDTO.Uf
                };
            }
            catch (CepNotFoundException ex)
            {
                throw new ArgumentException("O CEP informado não foi encontrado.", ex);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Erro ao consultar o CEP.", ex);
            }

            await _fornecedorRepository.AddAsync(fornecedor);
            return fornecedor;
        }

        public async Task<Fornecedor> AtualizarFornecedor(Fornecedor fornecedor)
        {
            if (!await _fornecedorRepository.ExistsAsync(f => f.Id == fornecedor.Id))
                throw new ArgumentException("Fornecedor não encontrado.");

            await _fornecedorRepository.UpdateAsync(fornecedor);
            return fornecedor;
        }

        public async Task RemoverFornecedor(int id)
        {
            if (!await _fornecedorRepository.ExistsAsync(f => f.Id == id))
                throw new ArgumentException("Fornecedor não encontrado.");

            await _fornecedorRepository.DeleteAsync(id);
        }


    }
}
