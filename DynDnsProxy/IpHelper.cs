using System.Net;

namespace DynDnsProxy;

public static class IpHelper
{
    public static string Combine(string? network, string subAddress)
        => string.IsNullOrWhiteSpace(network)
            ? subAddress
            : IPNetwork2.Parse(network).GetAddress(IPAddress.Parse(subAddress)).ToString();
}
