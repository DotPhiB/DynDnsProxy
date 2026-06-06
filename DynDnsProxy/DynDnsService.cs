using Flurl.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DynDnsProxy;

public class DynDnsService(IOptionsMonitor<DynDnsConfiguration> dynDnsConfiguration)
{
    public async Task<ObjectResult> Update(string ip4, string ip6, string? ip6LanPrefix, string domain)
    {
        var response = await dynDnsConfiguration.CurrentValue.UpdateUrl
            .Replace("<domain>", Uri.EscapeDataString(domain))
            .Replace("<ip4>", Uri.EscapeDataString(ip4))
            .Replace("<ip6>", Uri.EscapeDataString(IpHelper.Combine(ip6LanPrefix, ip6)))
            .WithHeader("User-Agent", "DynDnsProxy")
            .WithBasicAuth(dynDnsConfiguration.CurrentValue.UserName, dynDnsConfiguration.CurrentValue.Password)
            .AllowAnyHttpStatus()
            .GetAsync();

        return new ObjectResult(await response.ResponseMessage.Content.ReadAsStringAsync()) { StatusCode = response.StatusCode };
    }
}
