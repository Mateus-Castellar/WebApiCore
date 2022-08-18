using AppCore.Business.Interfaces;
using AppCore.Business.Models;
using AppCore.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Data.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppCoreDbContext context) : base(context) { }

        public async Task<Produto> ObterProdutoFornecedor(Guid id)
        {
            return await _context.Produtos
                .AsNoTracking()
                .Include(lbda => lbda.Fornecedor)
                .FirstOrDefaultAsync(lbda => lbda.Id == id);
        }

        public async Task<IEnumerable<Produto>> ObterProdutosFornecedores()
        {
            return await _context.Produtos
                .AsNoTracking()
                .Include(lbda => lbda.Fornecedor)
                .OrderBy(lbda => lbda.Nome).ToListAsync();
        }

        public async Task<IEnumerable<Produto>> ObterProdutosPorFornecedor(Guid fornecedorId)
        {
            return await Buscar(p => p.FornecedorId == fornecedorId);
        }
    }
}