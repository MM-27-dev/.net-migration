namespace John.SocialClub.Web.Services;

public sealed class AuthState
{
    public bool IsAuthenticated { get; private set; }
    public string? Username { get; private set; }

    private readonly string _configuredUser;
    private readonly string _configuredPassword;

    public AuthState(IConfiguration config)
    {
        // Defaults to demo/demo123 if not set in appsettings
        _configuredUser = config["Auth:Username"] ?? "demo";
        _configuredPassword = config["Auth:Password"] ?? "demo123";
    }

    public bool Login(string username, string password)
    {
        if (string.Equals(username?.Trim(), _configuredUser, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(password, _configuredPassword))
        {
            IsAuthenticated = true;
            Username = username;
            return true;
        }
        IsAuthenticated = false;
        Username = null;
        return false;
    }

    public void Logout()
    {
        IsAuthenticated = false;
        Username = null;
    }
}
