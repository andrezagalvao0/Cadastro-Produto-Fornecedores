using CadastroProdutosFornecedores.Core.Entities;
using CadastroProdutosFornecedores.Core.Exceptions;
using Newtonsoft.Json;

namespace CadastroProdutosFornecedores.Core.Services
{
    public class ViaCepService
    {
        private readonly HttpClient _httpClient;

        public ViaCepService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Endereco> ObterEnderecoPorCep(string cep)
        {
            cep = new string(cep.Where(char.IsDigit).ToArray());

            if (cep.Length != 8)
                throw new ArgumentException("CEP inválido.");

            var response = await _httpClient.GetAsync($"{cep}/json/");

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException("Erro ao consultar a API do ViaCEP.");

            var content = await response.Content.ReadAsStringAsync();
            var enderecoDTO = JsonConvert.DeserializeObject<Endereco>(content);

            if (enderecoDTO == null || string.IsNullOrEmpty(enderecoDTO.Cep))
            {
                throw new CepNotFoundException();
            }

            return enderecoDTO;
        }
    }
}
