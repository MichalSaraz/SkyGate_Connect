using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api;

[ApiController]
[Route("buggy")]
public class BuggyController : ControllerBase
{
    private readonly AppDbContext _context;
    
    public BuggyController(AppDbContext context)
    {
        _context = context;
    }
    
    [HttpGet("testauth")]
    [Authorize]
    public ActionResult<string> TestAuth()
    {
        return "You are authorized";
    }
}