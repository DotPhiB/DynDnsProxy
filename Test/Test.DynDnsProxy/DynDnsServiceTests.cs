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

    [Test]
    public void UpdateShouldReturnUpdateUrl()
    {
        var expected = SubstituteFor<DynDnsConfiguration>().UpdateUrl;
        var actual = Subject.Update("", "", "", "");
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void ShouldThrow()
    {
        Set.SubstituteFor<DynDnsConfiguration>().To((DynDnsConfiguration)null!);
        Assert.Throws<NullReferenceException>(() => Subject.Update("", "", "", ""));
    }
}
