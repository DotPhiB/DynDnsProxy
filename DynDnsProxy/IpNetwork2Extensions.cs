using System.Net;

namespace DynDnsProxy;

public static class IpNetwork2Extensions
{
    public static IPAddress GetAddress(this IPNetwork2 @this, IPAddress subAddress)
    {
        if (@this.AddressFamily != subAddress.AddressFamily)
            throw new ArgumentException("AddressFamily must match between Network and partialAddress.");

        var maskedSubAddressBytes = @this.WildcardMask.GetAddressBytes()
            .Zip(subAddress.GetAddressBytes())
            .Select(x => (byte)(x.First & x.Second));

        var targetAddressBytes = @this.Network.GetAddressBytes()
            .Zip(maskedSubAddressBytes)
            .Select(x => (byte)(x.First + x.Second))
            .ToArray();

        return new IPAddress(targetAddressBytes);
    }
}
