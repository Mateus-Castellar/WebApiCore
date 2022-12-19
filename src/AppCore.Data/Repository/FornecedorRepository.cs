using AppCore.Business.Interfaces;
using AppCore.Business.Models;
using AppCore.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Data.Repository;

public class FornecedorRepository : Repository<Fornecedor>, IFornecedorRepository
{
    public FornecedorRepository(AppCoreDbContext context) : base(context) { }

    public async Task<Fornecedor> ObterFornecedorEndereco(Guid id)
    {
        return await _context.Fornecedores
            .AsNoTracking()
            .Include(lbda => lbda.Endereco)
            .FirstOrDefaultAsync(lbda => lbda.Id == id);
    }

    public async Task<Fornecedor> ObterFornecedorProdutosEndereco(Guid id)
    {
        return await _context.Fornecedores
            .AsNoTracking()
            .Include(lbda => lbda.Produtos)
            .Include(lbda => lbda.Endereco)
            .FirstOrDefaultAsync(lbda => lbda.Id == id);
    }
}
