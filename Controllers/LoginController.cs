using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private readonly ILogger<LoginController> _logger;

    public LoginController(ILogger<LoginController> logger)
    {
        _logger = logger;
    }

    [HttpGet("NoLogin")]
    public string noLogin()
    {
        return "未登入";
    }

    [Authorize]
    [HttpGet]
    public IActionResult Get()
    {
        var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;

        var results = new Dictionary<string, string>();
        // iterate all claims
        foreach (var claim in claimsIdentity.Claims)
        {
            results.Add(claim.Type, claim.Value);
            // System.Console.WriteLine(claim.Type + ":" + claim.Value);
        }

        // var nameClaim = claimsIdentity.FindFirst(ClaimTypes.Name);

        return Ok(results);
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginPost value)
    {
        _logger.LogTrace(value.Account);
        _logger.LogTrace(value.Password);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, value.Account),
            new Claim(ClaimTypes.Email, "123@xxx.com"),
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

        return Ok();
    }

    [HttpDelete]
    public void logout()
    {
        HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}