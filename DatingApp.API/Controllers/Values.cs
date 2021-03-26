using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
namespace DatingApp.API.Controllers
{
    [Authorize]
   // [EnableCors("MyPolicy")]
    [ApiController]
    [Route("api/[controller]")]
    public class ValuesController : ControllerBase
    {
        private readonly DataContext _context;

        public ValuesController(DataContext context)
        {
            _context=context;
        }
        
       [HttpGet]  
        [AllowAnonymous]
        public async Task<ActionResult> GetValues()
        {
            var result=await _context.Values.ToListAsync();
            return Ok(result);
        }
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetValues(int id)
        {
            var result=await _context.Values.FirstAsync(a=>a.Id==id);
            return Ok(result);
        }
    }
}