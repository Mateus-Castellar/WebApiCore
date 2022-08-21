using AppCore.Business.Models;

namespace AppCore.Business.Interfaces
{
    public interface IProdutoService : IDisposable
    {
        Task<bool> Adicionar(Produto produto);
        Task<bool> Atualizar(Produto produto);
        Task<bool> Remover(Guid id);
    }
}
