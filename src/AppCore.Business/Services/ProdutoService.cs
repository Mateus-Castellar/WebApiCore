using AppCore.Business.Interfaces;
using AppCore.Business.Models;
using AppCore.Business.Validations;

namespace AppCore.Business.Services;

public class ProdutoService : BaseService, IProdutoService
{
    private readonly IProdutoRepository _produtoRepository;

    public ProdutoService(IProdutoRepository produtoRepository, INotificador notificador) : base(notificador)
    {
        _produtoRepository = produtoRepository;
    }

    public async Task<bool> Adicionar(Produto produto)
    {
        if (ExecutarValidacao(new ProdutoValidation(), produto) is false) return false;

        await _produtoRepository.Adicionar(produto);
        return true;
    }

    public async Task<bool> Atualizar(Produto produto)
    {
        if (ExecutarValidacao(new ProdutoValidation(), produto) is false) return false;

        await _produtoRepository.Atualizar(produto);
        return true;
    }

    public async Task<bool> Remover(Guid id)
    {
        await _produtoRepository.Remover(id);
        return true;
    }

    public void Dispose()
    {
        _produtoRepository?.Dispose();
    }
}
