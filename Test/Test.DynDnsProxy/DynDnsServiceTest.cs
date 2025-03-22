using DynDnsProxy;
using Flurl.Http.Testing;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace Test.DynDnsProxy;


[TestFixture]
[TestOf(typeof(DynDnsService))]
public class DynDnsServiceTest : TestsFor<DynDnsService>
{

    protected override void SetUpSubstitutions()
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

    [TestCase("1.1.1.1", "example.com", "::1", "1::/24")]
    [TestCase("2.2.2.2", "my.example.com","::2", "2::/24")]
    public async Task UpdateShouldSucceed(string ip4, string domain, string ip6, string? ip6LanPrefix)
    {
        using var httpTest = new HttpTest();

        var dynDnsConfiguration = SubstituteFor<DynDnsConfiguration>();

        var expectedUrl = dynDnsConfiguration.UpdateUrl
            .Replace("<domain>", domain)
            .Replace("<ip4>", ip4)
            .Replace("<ip6>", IpHelper.Combine(ip6LanPrefix, ip6));

        const string expectedResultString = "Expected result.";
        const int expectedStatusCode = 204;

        httpTest.ForCallsTo(expectedUrl)
            .RespondWith(expectedResultString, expectedStatusCode);

        var result = await Subject.Update(ip4, ip6, ip6LanPrefix, domain);

        using (Assert.EnterMultipleScope())
        {
            Assert.DoesNotThrow(()
                => httpTest.ShouldHaveCalled(expectedUrl).Times(1));
            Assert.DoesNotThrow(()
                => httpTest.ShouldHaveCalled(expectedUrl).WithVerb(HttpMethod.Get));
            Assert.DoesNotThrow(()
                => httpTest.ShouldHaveCalled(expectedUrl).WithHeader("User_Agent", "DynDnsProxy"));
            Assert.DoesNotThrow(()
                => httpTest.ShouldHaveCalled(expectedUrl).WithBasicAuth(dynDnsConfiguration.UserName, dynDnsConfiguration.Password));

            Assert.That(result.Value, Is.EqualTo(expectedResultString));
            Assert.That(result.StatusCode, Is.EqualTo(expectedStatusCode));
        }
    }

    [Test]
    public void ShouldThrow()
    {
        Set.SubstituteFor<DynDnsConfiguration>().ToNull();
        Assert.ThrowsAsync<NullReferenceException>(async () => await Subject.Update("", "", "", ""));
    }
}
