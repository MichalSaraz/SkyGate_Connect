using Core.Dtos;
using Core.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Api.AccountManagement.Models;
using Web.Errors;

namespace Web.Api.AccountManagement.Controllers;

[ApiController]
[Route("account-management")]
public class AccountController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ITokenService _tokenService;
    
    public AccountController(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        ITokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }
    
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginModel loginModel)
    {
        var user = await _userManager.FindByNameAsync(loginModel.UserName);
        
        if (user == null) return Unauthorized(new ApiResponse(401));
        
        var result = await _signInManager.CheckPasswordSignInAsync(user, loginModel.Password, false);
        
        if (!result.Succeeded) return Unauthorized(new ApiResponse(401));
        
        return new UserDto
        {
            Token = _tokenService.CreateToken(user),
            UserName = user.UserName,
            Role = user.Role.ToString(),
            Station = user.Station
        };
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterModel registerModel)
    {
        var existingUser = await _userManager.FindByNameAsync(registerModel.UserName);

        if (existingUser != null)
        {
            return BadRequest("Username already exists.");
        }
        
        var user = new AppUser
        {
            UserName = registerModel.UserName,
            Role =  registerModel.Role,
            Station = registerModel.Station,
        };
        
        var result = await _userManager.CreateAsync(user, registerModel.Password);
        
        if (!result.Succeeded) return BadRequest(new ApiResponse(400));
        
        return new UserDto
        {
            Token = "This will be a token",
            UserName = user.UserName,
            Role = user.Role.ToString(),
            Station = user.Station
        };
    }
    
    [HttpGet("username-exists")]
    public async Task<ActionResult<bool>> CheckUserNameExistsAsync([FromQuery] string userName)
    {
        return await _userManager.FindByNameAsync(userName) != null;
    }
}