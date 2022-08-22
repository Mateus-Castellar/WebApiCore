using AppCore.API.DTO;
using AppCore.Business.Interfaces;
using AppCore.Business.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AppCore.API.Controllers
{
    [Route("api/[controller]")]
    public class ProdutosController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IProdutoRepository _produtoRepository;
        private readonly IProdutoService _produtoService;

        public ProdutosController(INotificador notificador,
                                  IProdutoRepository produtoRepository,
                                  IProdutoService produtoService,
                                  IMapper mapper) : base(notificador)
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