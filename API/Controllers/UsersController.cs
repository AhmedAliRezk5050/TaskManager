using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Data;
using Infrastructure.DTOs.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers;

[Route("api/auth")]
public class UsersController : ApiController
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;

    public UsersController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration
    )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
    {
        var user = new ApplicationUser
        {
            UserName = registerDTO.UserName
        };

        var result = await _userManager.CreateAsync(user, registerDTO.Password);

        if (result.Succeeded)
        {
            return Ok();
        }

        throw new Exception("Register failed");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
    {
        var result = await _signInManager
            .PasswordSignInAsync(loginDTO.UserName,
                loginDTO.Password,
                isPersistent: false,
                lockoutOnFailure: false);

        if (result.Succeeded)
        {
            var token = await GenerateJwtToken(loginDTO.UserName);

            return Ok(new { Token = token });
        }

        throw new Exception("Login failed");
    }

    private async Task<string> GenerateJwtToken(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var userClaims = await _userManager.GetClaimsAsync(user);
        claims.AddRange(userClaims);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordDTO changePasswordDTO)
    {
        var applicationUser = await FetchUser(_userManager.GetUserId(User));

        if (applicationUser == null)
        {
            return Unauthorized();
        }

        var changePasswordResult = await _userManager.ChangePasswordAsync(
            applicationUser,
            changePasswordDTO.OldPassword,
            changePasswordDTO.NewPassword);

        if (changePasswordResult.Succeeded)
        {
            return Ok();
        }
        
        return BadRequest();
    }

    private async Task<ApplicationUser?> FetchUser(string userId)
    {
        var user = await _userManager
            .Users
            .FirstOrDefaultAsync(x => x.Id == userId);

        return user;
    }
}