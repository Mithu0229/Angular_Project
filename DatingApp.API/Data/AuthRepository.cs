using System;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        public AuthRepository(DataContext context)
        {
            _context=context;
        }
        public async Task<User> Login(string userName, string password)
        {
           var user=await _context.Users.FirstOrDefaultAsync(a=>a.UserName==userName);
           if(user==null)
              return null;

            if(!VarifiedPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;  
           return user;
        }

        private bool VarifiedPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using(var hmac=new System.Security.Cryptography.HMACSHA512(passwordSalt))
           {
             var   computeHask= hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
             for (int i = 0; i < computeHask.Length; i++)
             {
                 if(computeHask[i]!=passwordHash[i]) return false;
             }
             return true;
           }
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreateHashPassword(password,out passwordHash, out passwordSalt);
           user.PasswordHash=passwordHash;
           user.PasswordSalt=passwordSalt;
           await _context.Users.AddAsync(user);
           await _context.SaveChangesAsync();
           return user;

        }

        private void CreateHashPassword(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
           using(var hmac=new System.Security.Cryptography.HMACSHA512())
           {
               passwordSalt=hmac.Key;
               passwordHash= hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
           }
        }

        public async Task<bool> UserExists(string userName)
        {
            if(await _context.Users.AnyAsync(a=>a.UserName==userName))
                return true;

            return false;    
        }
    }
}