using System.Text.Json;

namespace Demo.Api;

public class UserModel
{
    public string Name { get; set; } = "";
    public string Phone { get; set; } = "";
}

public interface ISessionWrapper
{
    UserModel User { get; set; }
}

public class SessionWrapper : ISessionWrapper
{
    private static readonly string _userKey = "session.user";
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SessionWrapper(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ISession Session
    {
        get
        {
            return _httpContextAccessor.HttpContext.Session;
        }
    }

    public UserModel User
    {
        get
        {
            var userValue = Session.GetString(_userKey);

            if (userValue == null)
            {
                return default!;
            }
            else
            {
                return JsonSerializer.Deserialize<UserModel>(userValue) ?? default!;
            }
        }
        set
        {
            var json = JsonSerializer.Serialize<UserModel>(value);
            Session.SetString(_userKey, json);
        }
    }
}