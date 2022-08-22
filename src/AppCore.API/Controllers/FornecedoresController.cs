using AppCore.API.DTO;
using AppCore.Business.Interfaces;
using AppCore.Business.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AppCore.API.Controllers
{
    [Route("api/[controller]")]
    public class FornecedoresController : BaseController
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IFornecedorService _fornecedorService;
        private readonly IEnderecoRepository _enderecoRepository;
        private readonly IMapper _mapper;

        public FornecedoresController(IFornecedorRepository fornecedorRepository, IFornecedorService fornecedorService,
            IMapper mapper, IEnderecoRepository enderecoRepository, INotificador notificador) : base(notificador)
        {
            _mapper = mapper;
            _fornecedorRepository = fornecedorRepository;
            _enderecoRepository = enderecoRepository;
            _fornecedorService = fornecedorService;
        }

        [HttpGet]
        public async Task<IEnumerable<FornecedorDTO>> ObterTodos()
        {
            var fornecedores = _mapper
                .Map<IEnumerable<FornecedorDTO>>(await _fornecedorRepository.ObterTodos());

            return fornecedores;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<FornecedorDTO>> ObterPorId(Guid id)
        {
            var fornecedor = _mapper
                .Map<FornecedorDTO>(await _fornecedorRepository.ObterFornecedorProdutosEndereco(id));

            if (fornecedor is null) return NotFound();

            return fornecedor;
        }

        [HttpPost]
        public async Task<ActionResult> Adicionar(FornecedorDTO fornecedorDTO)
        {
            if (ModelState.IsValid is false) return CustomResponse(ModelState);

            await _fornecedorService.Adicionar(_mapper.Map<Fornecedor>(fornecedorDTO));

            return CustomResponse(fornecedorDTO);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Atualizar(Guid id, FornecedorDTO fornecedorDTO)
        {
            if (id != fornecedorDTO.Id) return BadRequest();

            if (ModelState.IsValid is false) return CustomResponse(ModelState);

            await _fornecedorService.Atualizar(_mapper.Map<Fornecedor>(fornecedorDTO));

            return CustomResponse(fornecedorDTO);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<FornecedorDTO>> Excluir(Guid id)
        {
            var fornecedorDTO = _mapper
                .Map<Fornecedor>(await _fornecedorRepository.ObterFornecedorEndereco(id));

            if (fornecedorDTO is null) return NotFound();

            await _fornecedorService.Remover(fornecedorDTO.Id);

            return CustomResponse();
        }

        [HttpGet("endereco/{id:guid}")]
        public async Task<EnderecoDTO> ObterEnderecoPorId(Guid id)
        {
            return _mapper.Map<EnderecoDTO>(await _enderecoRepository.ObterPorId(id));
        }

        [HttpPut("endereco/{id:guid}")]
        public async Task<IActionResult> AtualizarEndereco(Guid id, EnderecoDTO enderecoDTO)
        {
            if (id != enderecoDTO.Id)
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(enderecoDTO);
            }

            if (ModelState.IsValid is false) return CustomResponse(ModelState);

            await _fornecedorService.AtualizarEndereco(_mapper.Map<Endereco>(enderecoDTO));

            return CustomResponse(enderecoDTO);
        }
    }
}