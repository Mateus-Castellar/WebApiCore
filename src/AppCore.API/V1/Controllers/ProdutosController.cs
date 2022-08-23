using AppCore.API.Controllers;
using AppCore.API.DTO;
using AppCore.Business.Interfaces;
using AppCore.Business.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppCore.API.V1.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProdutosController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IProdutoRepository _produtoRepository;
        private readonly IProdutoService _produtoService;

        public ProdutosController(INotificador notificador, IProdutoRepository produtoRepository,
            IProdutoService produtoService, IMapper mapper, IUser user) : base(notificador, user)
        {
            _mapper = mapper;
            _produtoRepository = produtoRepository;
            _produtoService = produtoService;
        }

        [HttpGet]
        public async Task<IEnumerable<ProdutoDTO>> ObterTodos()
        {
            return _mapper.Map<IEnumerable<ProdutoDTO>>(
                await _produtoRepository.ObterProdutosFornecedores());
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProdutoDTO>> ObterPorId(Guid id)
        {
            var produtoViewModel = _mapper.Map<ProdutoDTO>(await
                _produtoRepository.ObterProdutoFornecedor(id));

            if (produtoViewModel is null) return NotFound();

            return produtoViewModel;
        }

        [HttpPost]
        public async Task<ActionResult<ProdutoDTO>> Adicionar(ProdutoDTO produtoDTO)
        {
            if (ModelState.IsValid is false) return CustomResponse(ModelState);

            var imagemNome = Guid.NewGuid() + "_" + produtoDTO.Imagem;

            if (UploadArquivo(produtoDTO.ImagemUpload, imagemNome) is false)
                return CustomResponse();

            produtoDTO.Imagem = imagemNome;

            await _produtoService.Adicionar(_mapper.Map<Produto>(produtoDTO));

            return CustomResponse(produtoDTO);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Atualizar(Guid id, ProdutoDTO produtoDTO)
        {
            if (id != produtoDTO.Id)
            {
                NotificarErro("Os ids informados não são iguais!");
                return CustomResponse();
            }

            var produtoAtualizacao = await _produtoRepository.ObterProdutoFornecedor(id);

            if (string.IsNullOrEmpty(produtoDTO.Imagem))
                produtoDTO.Imagem = produtoAtualizacao.Imagem;

            if (ModelState.IsValid is false) return CustomResponse(ModelState);

            if (produtoDTO.ImagemUpload is not null)
            {
                var imagemNome = Guid.NewGuid() + "_" + produtoDTO.Imagem;

                if (UploadArquivo(produtoDTO.ImagemUpload, imagemNome) is false)
                    return CustomResponse(ModelState);

                produtoAtualizacao.Imagem = imagemNome;
            }

            produtoAtualizacao.FornecedorId = produtoDTO.FornecedorId;
            produtoAtualizacao.Nome = produtoDTO.Nome;
            produtoAtualizacao.Descricao = produtoDTO.Descricao;
            produtoAtualizacao.Valor = produtoDTO.Valor;
            produtoAtualizacao.Ativo = produtoDTO.Ativo;

            await _produtoService.Atualizar(_mapper.Map<Produto>(produtoAtualizacao));

            return CustomResponse(produtoDTO);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ProdutoDTO>> Excluir(Guid id)
        {
            var produto = await _produtoRepository.ObterProdutoFornecedor(id);

            if (produto is null) return NotFound();

            await _produtoService.Remover(id);

            return CustomResponse(produto);
        }

        private bool UploadArquivo(string arquivo, string imgNome)
        {
            if (string.IsNullOrEmpty(arquivo))
            {
                NotificarErro("Forneça uma imagem para este produto!");
                return false;
            }

            var imageDataByteArray = Convert.FromBase64String(arquivo);

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens", imgNome);

            if (System.IO.File.Exists(filePath))
            {
                NotificarErro("Já existe um arquivo com este nome!");
                return false;
            }

            System.IO.File.WriteAllBytes(filePath, imageDataByteArray);

            return true;
        }
    }
}