using DynDnsProxy;
using Flurl.Http.Testing;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace Test.DynDnsProxy;


[TestFixture]
[TestOf(typeof(DynDnsService))]
public class DynDnsServiceTest : TestsFor<DynDnsService>
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
    public async Task UpdateShouldSucceed(string ipv, string domain, string ip6, string? ip6LanPrefix, string? expectedIp6)
    {
        using var httpTest = new HttpTest();

        var dynDnsConfiguration = SubstituteFor<DynDnsConfiguration>();

        var expectedUrl = dynDnsConfiguration.UpdateUrl
            .Replace("<domain>", domain)
            .Replace("<ip4>", ipv)
            .Replace("<ip6>", expectedIp6);

        await Subject.Update(ipv, ip6, ip6LanPrefix, domain);

        httpTest.ShouldHaveCalled(expectedUrl)
            .WithHeader("User_Agent", "DynDnsProxy")
            .WithBasicAuth(dynDnsConfiguration.UserName, dynDnsConfiguration.Password);
    }

    [Test]
    public void ShouldThrow()
    {
        Set.SubstituteFor<DynDnsConfiguration>().To((DynDnsConfiguration)null!);
        Assert.ThrowsAsync<NullReferenceException>(async () => await Subject.Update("", "", "", ""));
    }
}
