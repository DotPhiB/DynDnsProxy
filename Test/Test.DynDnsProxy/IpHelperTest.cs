using DynDnsProxy;

namespace Test.DynDnsProxy;

[TestFixture]
[TestOf(typeof(IpHelper))]
public class IpHelperTest
{

    [TestCase("1::/24", "::1", "1::1")]
    [TestCase("2::/24", "::2", "2::2")]
    [TestCase(null, "::2", "::2")]
    [TestCase("", "::2", "::2")]
    public void CombineShouldSucceed(string? network, string subAddress, string expected)
        => Assert.That(IpHelper.Combine(network, subAddress), Is.EqualTo(expected));
}
