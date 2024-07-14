using CadastroProdutosFornecedores.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace CadastroProdutosFornecedores.Core.Entities
{
    public class Produto
    {
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Descricao { get; set; }
        [Required, MaxLength(50)]
        public string Marca { get; set; }
        [Required]
        public UnidadeMedida UnidadeMedida { get; set; } 

        public ICollection<ProdutoFornecedor> ProdutosFornecedores { get; set; }
    }
}
