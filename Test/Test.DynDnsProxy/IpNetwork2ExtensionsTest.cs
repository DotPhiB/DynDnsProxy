using System.Net;
using DynDnsProxy;

namespace Test.DynDnsProxy;

[TestFixture]
[TestOf(typeof(IpNetwork2Extensions))]
public class IpNetwork2ExtensionsTest
{

    [TestCase("1::/24", "::1", "1::1")]
    [TestCase("2::/24", "::2", "2::2")]
    [TestCase("1.1.0.0/16", "0.0.1.1", "1.1.1.1")]
    [TestCase("1.1.1.1/16", "1.1.1.1", "1.1.1.1")]
    public void GetAddressShouldSucceed(string network, string subAddress, string expected)
    {
        var sut = IPNetwork2.Parse(network);
        var subIpAddress = IPAddress.Parse(subAddress);
        var expectedIpAddress = IPAddress.Parse(expected);

        Assert.That(sut.GetAddress(subIpAddress), Is.EqualTo(expectedIpAddress));
    }
}
