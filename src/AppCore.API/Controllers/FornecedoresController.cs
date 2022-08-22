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
        private readonly IMapper _mapper;

        public FornecedoresController(IFornecedorRepository fornecedorRepository, IFornecedorService fornecedorService,
            IMapper mapper)
        {
            _fornecedorRepository = fornecedorRepository;
            _fornecedorService = fornecedorService;
            _mapper = mapper;
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
            if (ModelState.IsValid is false) return BadRequest();

            var fornecedor = _mapper.Map<Fornecedor>(fornecedorDTO);

            var result = await _fornecedorService.Adicionar(fornecedor);

            if (result is false) return BadRequest();

            return Ok(fornecedor);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Atualizar(Guid id, FornecedorDTO fornecedorDTO)
        {
            if (id != fornecedorDTO.Id) return BadRequest();

            if (ModelState.IsValid is false) return BadRequest();

            var fornecedor = _mapper.Map<Fornecedor>(fornecedorDTO);

            var result = await _fornecedorService.Atualizar(fornecedor);

            if (result is false) return BadRequest();

            return Ok(fornecedor);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<FornecedorDTO>> Excluir(Guid id)
        {
            var fornecedor = _mapper
                .Map<Fornecedor>(await _fornecedorRepository.ObterFornecedorEndereco(id));

            if (fornecedor is null) return NotFound();

            var result = await _fornecedorService.Remover(fornecedor.Id);

            if (result is false) return BadRequest();

            return Ok(fornecedor);
        }
    }
}