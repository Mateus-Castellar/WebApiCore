using AppCore.Business.Interfaces;
using AppCore.Business.Models;
using AppCore.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Data.Repository;

public class EnderecoRepository : Repository<Endereco>, IEnderecoRepository
{
    public EnderecoRepository(AppCoreDbContext context) : base(context) { }

    public async Task<Endereco> ObterEnderecoPorFornecedor(Guid fornecedorId)
    {
        return await _context.Enderecos
            .AsNoTracking()
            .FirstOrDefaultAsync(lbda => lbda.FornecedorId == fornecedorId);
    }
}
