using Microsoft.Extensions.Options;

namespace DynDnsProxy;

public class DynDnsService(IOptionsMonitor<DynDnsConfiguration> dynDnsConfiguration)
{
    public string Update(string ip4, string ip6, string ip6LanPrefix, string domain)
        => dynDnsConfiguration.CurrentValue.UpdateUrl
            .Replace("<domain>", domain)
            .Replace("<ip4>", ip4)
            .Replace("<ip6>", Combine(ip6LanPrefix, ip6));

    private static string Combine(string network, string subAddress)
    {
        return "1::1";
    }
}
