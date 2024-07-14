using CadastroProdutosFornecedores.Core.Entities;
using System.Linq.Expressions;

namespace CadastroProdutosFornecedores.Infrastructure.Data.Repositories
{
    public interface IProdutoRepository
    {
        Task<IEnumerable<Produto>> GetAllAsync();
        Task<Produto> GetByIdAsync(int id);
        Task AddAsync(Produto produto);
        Task UpdateAsync(Produto produto);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(Expression<Func<Produto, bool>> predicate);
    }
}
