using AppCore.API.Controllers;
using AppCore.API.DTO;
using AppCore.API.Extensions;
using AppCore.Business.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AppCore.API.V1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}")]
    public class AuthController : BaseController
    {
        private readonly SignInManager<IdentityUser> _signInManager;//gerencia a autenticacao
        private readonly UserManager<IdentityUser> _userManager;//gerencia o usuario
        private readonly AppSettings _appSettings;
        private readonly ILogger _logger;

        public AuthController(INotificador notificador, SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager, IOptions<AppSettings> appSettings, IUser user,
            ILogger<AuthController> logger) : base(notificador, user)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _appSettings = appSettings.Value;
            _logger = logger;
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
                _logger.LogInformation($"usuário {user.Email} cadastrado com sucesso");
                await _signInManager.SignInAsync(user, false);
                return CustomResponse(await GerarJWT(user.Email));
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
            {
                _logger.LogInformation($"usuário {loginUser.Email} cadastrado com sucesso");
                return CustomResponse(await GerarJWT(loginUser.Email));
            }

            if (result.IsLockedOut)
            {
                NotificarErro("Bloqueado por tentativas inválidas");
                return CustomResponse(loginUser);
            }

            NotificarErro("Usuário ou senha incorreto");
            return CustomResponse(loginUser);
        }

        private async Task<LoginResponseDTO> GerarJWT(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var claims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));

            foreach (var userRole in userRoles)
                claims.Add(new Claim("role", userRole));

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Emissor,
                Audience = _appSettings.ValidoEm,
                Subject = identityClaims, //passa a claims para o token
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            }); ;

            var encodedToken = tokenHandler.WriteToken(token);

            var response = new LoginResponseDTO
            {
                AccessToken = encodedToken,
                ExpiresIn = TimeSpan.FromHours(_appSettings.ExpiracaoHoras).TotalSeconds,
                UserToken = new UserTokenDTO
                {
                    Id = user.Id,
                    Email = user.Email,
                    Claims = claims.Select(lbda => new ClaimDTO { Type = lbda.Type, Value = lbda.Value })
                }
            };

            return response;
        }

        private static long ToUnixEpochDate(DateTime date)
        {
            return (long)Math.Round((date.ToUniversalTime() - new
                DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
        }

        //Tipos de retornos possíveis com log

        //_logger.LogTrace("Log de Trace");
        //_logger.LogDebug("Log de Debug");
        //_logger.LogInformation("Log de Informação");
        //_logger.LogWarning("Log de Aviso");
        //_logger.LogError("Log de Erro");
        //_logger.LogCritical("Log de Problema Critico");
    }
}