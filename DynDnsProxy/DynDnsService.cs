using Microsoft.Extensions.Options;

namespace DynDnsProxy;

public class DynDnsService(IOptionsMonitor<DynDnsConfiguration> dynDnsConfiguration)
{
    public string Update(string ip4, string ip6, string ip6LanPrefix, string domain)
        => dynDnsConfiguration.CurrentValue.UpdateUrl;
}
