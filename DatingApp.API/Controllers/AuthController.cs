using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("MyPolicy")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _config;
        public AuthController(IAuthRepository authRepository, IConfiguration config)
        {
            _config=config;
            _authRepository=authRepository;
        }
        [HttpPost("register")]
         public async Task<IActionResult> Register(UserForRegister userForRegister)
         {
             userForRegister.UserName=userForRegister.UserName.ToLower();
             if(await _authRepository.UserExists(userForRegister.UserName))
                return BadRequest();
            
            var userPra=new User(){UserName= userForRegister.UserName};
            var createUser= await _authRepository.Register(userPra, userForRegister.UserName);
            return StatusCode(201);
         }
          
          [HttpPost("login")]
         public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
         {
             var loginUser=await _authRepository.Login(userForLoginDto.UserName,userForLoginDto.Password);
             if(loginUser==null)
                return Unauthorized();

            var claims= new[]{
             new Claim(ClaimTypes.NameIdentifier,loginUser.Id.ToString()),
             new Claim(ClaimTypes.Name,loginUser.UserName)
             };
             var key=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
             var crds=new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);
             var tokenDescriptor=new SecurityTokenDescriptor(){
                 Subject=new ClaimsIdentity(claims),
                 Expires=DateTime.Now.AddDays(1),
                 SigningCredentials=crds

             };
             var tokenHendler=new JwtSecurityTokenHandler();
             var token=tokenHendler.CreateToken(tokenDescriptor);
            return Ok(new {token=tokenHendler.WriteToken(token)});
         }


    }
}