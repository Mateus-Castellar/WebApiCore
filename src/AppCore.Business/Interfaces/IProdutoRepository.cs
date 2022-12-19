using AppCore.Business.Models;

namespace AppCore.Business.Interfaces;

public interface IProdutoRepository : IRepository<Produto>
{
    Task<IEnumerable<Produto>> ObterProdutosPorFornecedor(Guid fornecedorId);
    Task<IEnumerable<Produto>> ObterProdutosFornecedores();
    Task<Produto> ObterProdutoFornecedor(Guid id);
}
