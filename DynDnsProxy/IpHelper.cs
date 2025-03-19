namespace DynDnsProxy;

public static class IpHelper
{
    public static string Combine(string? network, string subAddress)
        => string.IsNullOrWhiteSpace(network)
            ? subAddress
            : "1::1";
}
