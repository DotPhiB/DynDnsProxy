namespace DynDnsProxy;

public class DynDnsConfiguration()
{
    public required string UpdateUrl { get; init; }
    public required string UserName { get; init; }
    public required string Password { get; init; }
}
