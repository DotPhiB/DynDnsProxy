using DynDnsProxy;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace Test.DynDnsProxy;

public class DynDnsServiceTests : TestsFor<DynDnsService>
{
    [SetUp]
    public void Setup()
    {
        Set.SubstituteFor<DynDnsConfiguration>().To(new DynDnsConfiguration()
        {
            UpdateUrl = "https://example.com/update?hostname=<domain>&myip=<ip4>,<ip6>",
            UserName = "myUser",
            Password = "myPassword",
        });
        Set.SubstituteFor<IOptionsMonitor<DynDnsConfiguration>>()
            .Configure(monitor => monitor.CurrentValue.Returns(SubstituteFor<DynDnsConfiguration>()));
    }

    [TestCase("1.1.1.1", "example.com", "::1", "1::/24", "1::1")]
    [TestCase("2.2.2.2", "my.example.com","::2", "2::/24", "2::2")]
    public void UpdateShouldReturnReplacedUrl(string ipv, string domain, string ip6, string ip6LanPrefix, string? expectedIp6)
    {
        var expected = SubstituteFor<DynDnsConfiguration>().UpdateUrl
            .Replace("<domain>", domain)
            .Replace("<ip4>", ipv)
            .Replace("<ip6>", expectedIp6);
        var actual = Subject.Update(ipv, ip6, ip6LanPrefix, domain);
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void ShouldThrow()
    {
        Set.SubstituteFor<DynDnsConfiguration>().To((DynDnsConfiguration)null!);
        Assert.Throws<NullReferenceException>(() => Subject.Update("", "", "", ""));
    }
}
