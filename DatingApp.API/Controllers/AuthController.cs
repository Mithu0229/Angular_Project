using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        public AuthController(IAuthRepository authRepository)
        {
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

    }
}