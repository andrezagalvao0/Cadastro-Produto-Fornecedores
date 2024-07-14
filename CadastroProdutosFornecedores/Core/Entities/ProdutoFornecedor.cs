using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CadastroProdutosFornecedores.Core.Entities
{
    public class ProdutoFornecedor
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public int? FornecedorId { get; set; }
        public decimal ValorCompra { get; set; }
    }
}
