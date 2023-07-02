using System.Security.Cryptography;
using System.Text;
using BasicAPIwithJWT.Data;
using BasicAPIwithJWT.DTOs;
using BasicAPIwithJWT.Entities;
using BasicAPIwithJWT.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BasicAPIwithJWT.Controllers;

public class AccountController : BaseApiController
{
    private readonly ITokenService _tokenService;
    public AccountController(DataContext context, ITokenService tokenService) : base(context)
    {
        _tokenService = tokenService;
    }
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        try
        {
            if (await UserExist(registerDto.Username))
                return BadRequest("Username is taken");


            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                UserName = registerDto.Username,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }
        catch (System.Exception)
        {

            throw;
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        try
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.Username);

            if (user == null)
                return Unauthorized("Invalid username");

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            }

            return new UserDto
            {
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }
        catch (System.Exception)
        {

            throw;
        }
    }

    private async Task<bool> UserExist(string username)
    {
        try
        {
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
        catch (System.Exception)
        {

            throw;
        }
    }
}
