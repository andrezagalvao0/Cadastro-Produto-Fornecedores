using CadastroProdutosFornecedores.Core.Entities;
using System.Linq.Expressions;

namespace CadastroProdutosFornecedores.Infrastructure.Data.Repositories
{
    public interface IFornecedorRepository
    {
        Task<IEnumerable<Fornecedor>> GetAllAsync();
        Task<Fornecedor> GetByIdAsync(int id);
        Task AddAsync(Fornecedor fornecedor);
        Task UpdateAsync(Fornecedor fornecedor);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(Expression<Func<Fornecedor, bool>> predicate);
    }
}
