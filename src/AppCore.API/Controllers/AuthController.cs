using AppCore.API.DTO;
using AppCore.API.Extensions;
using AppCore.Business.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace AppCore.API.Controllers
{
    [Route("api")]
    public class AuthController : BaseController
    {
        private readonly SignInManager<IdentityUser> _signInManager;//gerencia a autenticacao
        private readonly UserManager<IdentityUser> _userManager;//gerencia o usuario
        private readonly AppSettings _appSettings;

        public AuthController(INotificador notificador, SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager, IOptions<AppSettings> appSettings) : base(notificador)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _appSettings = appSettings.Value;
        }

        [HttpPost("nova-conta")]
        public async Task<ActionResult> Registrar(RegisterUserDTO registerUser)
        {
            if (ModelState.IsValid is false) return CustomResponse(ModelState);

            var user = new IdentityUser
            {
                UserName = registerUser.Email,
                Email = registerUser.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, registerUser.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return CustomResponse(GerarJWT());
            }

            foreach (var erro in result.Errors)
                NotificarErro(erro.Description);

            return CustomResponse(registerUser);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginUserDTO loginUser)
        {
            if (ModelState.IsValid is false) return CustomResponse(ModelState);

            var result = await _signInManager.PasswordSignInAsync(loginUser.Email,
                loginUser.Password, false, true);

            if (result.Succeeded)
                return CustomResponse(GerarJWT());

            if (result.IsLockedOut)
            {
                NotificarErro("Bloqueado por tentativas inválidas");
                return CustomResponse(loginUser);
            }

            NotificarErro("Usuário ou senha incorreto");
            return CustomResponse(loginUser);
        }

        private string GerarJWT()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Emissor,
                Audience = _appSettings.ValidoEm,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            return tokenHandler.WriteToken(token);
        }
    }
}