using AppCore.API.DTO;
using AppCore.Business.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AppCore.API.Controllers
{
    [Route("api/[controller]")]
    public class FornecedoresController : BaseController
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IMapper _mapper;

        public FornecedoresController(IFornecedorRepository fornecedorRepository, IMapper mapper)
        {
            _mapper = mapper;
            _fornecedorRepository = fornecedorRepository;
        }

        [HttpGet("obter-todos")]
        public async Task<IEnumerable<FornecedorDTO>> ObterTodos()
        {
            var fornecedores = _mapper
                .Map<IEnumerable<FornecedorDTO>>(await _fornecedorRepository.ObterTodos());

            return fornecedores;
        }
    }
}