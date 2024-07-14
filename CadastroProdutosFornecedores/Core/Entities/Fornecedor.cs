using System.ComponentModel.DataAnnotations;

namespace CadastroProdutosFornecedores.Core.Entities
{
    public class Fornecedor
    {
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Nome { get; set; }
        [Required, MaxLength(14)]
        public string CNPJ { get; set; }
        [Required]
        public Endereco Endereco { get; set; }
        [Required, MaxLength(11)]
        public string Telefone { get; set; }

        public ICollection<ProdutoFornecedor> ProdutosFornecedores { get; set; }
    }
}
