using AppCore.Business.Models;

namespace AppCore.Business.Interfaces;

public interface IEnderecoRepository : IRepository<Endereco>
{
    Task<Endereco> ObterEnderecoPorFornecedor(Guid fornecedorId);
}