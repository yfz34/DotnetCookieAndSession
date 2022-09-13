using Demo.Api;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly ILogger<TestController> _logger;
    private readonly ISessionWrapper _sessionWrapper;

    public TestController(ILogger<TestController> logger, ISessionWrapper sessionWrapper)
    {
        _logger = logger;
        _sessionWrapper = sessionWrapper;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var user = _sessionWrapper.User;
        return Ok(new {
            HttpContext.Request.Cookies,
            user
        });
    }

    [HttpPost]
    public IActionResult Post()
    {
        SetCookie("cookieTest", "123", new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(7),
            Secure = true,
        });

        _sessionWrapper.User = new UserModel
        {
            Name = "Allen",
            Phone = "0912345678"
        };

        return Ok();
    }

    [HttpDelete]
    public IActionResult Delete()
    {
        DeleteCookie("cookieTest");
        return Ok();
    }

    private string? GetCookie(string key)
    {
        if (HttpContext.Request.Cookies.TryGetValue(key, out var message))
        {
            _logger.LogInformation($"has no {key} cookie");
            return null;
        }

        return message;
    }

    private bool SetCookie(string key, string value, CookieOptions options = null!)
    {
        try
        {
            options.HttpOnly = true;
            HttpContext.Response.Cookies.Append(key, value, options);
            return true;
        }
        catch(Exception ex)
        {
            _logger.LogError(ex.Message.ToString());
            return false;
        }
    }

    private bool DeleteCookie(string key)
    {
        try
        {
            HttpContext.Response.Cookies.Delete(key);
            return true;
        }
        catch
        {
            return false;
        }
    }
}