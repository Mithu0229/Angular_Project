using System.Threading.Tasks;
using DatingApp.API.Data;
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
         public async Task<IActionResult> Register(string userName, string password)
         {
             userName=userName.ToLower();
             if(await _authRepository.UserExists(userName))
                return BadRequest();
            
            var userPra=new User(){UserName=userName};
            var createUser= await _authRepository.Register(userPra,password);
            return StatusCode(201);
         }

    }
}