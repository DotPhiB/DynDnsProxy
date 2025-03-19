using Flurl.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DynDnsProxy;

public class DynDnsService(IOptionsMonitor<DynDnsConfiguration> dynDnsConfiguration)
{
    public async Task<ObjectResult> Update(string ip4, string ip6, string? ip6LanPrefix, string domain)
    {
        var response = await dynDnsConfiguration.CurrentValue.UpdateUrl
            .Replace("<domain>", domain)
            .Replace("<ip4>", ip4)
            .Replace("<ip6>", IpHelper.Combine(ip6LanPrefix, ip6))
            .WithHeader("User_Agent", "DynDnsProxy")
            .WithBasicAuth(dynDnsConfiguration.CurrentValue.UserName, dynDnsConfiguration.CurrentValue.Password)
            .GetAsync();

        return new ObjectResult(await response.ResponseMessage.Content.ReadAsStringAsync()) { StatusCode = response.StatusCode };
    }
}
