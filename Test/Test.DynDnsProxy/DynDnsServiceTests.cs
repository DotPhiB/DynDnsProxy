using DynDnsProxy;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace Test.DynDnsProxy;

public class DynDnsServiceTests
{
    private IOptionsMonitor<DynDnsConfiguration> _optionsMonitor;
    private DynDnsConfiguration _testSettings;

    [SetUp]
    public void Setup()
    {
        _testSettings = new DynDnsConfiguration()
        {
            UpdateUrl = "https://example.com/update?hostname=<domain>&myip=<ip4>,<ip6>",
            UserName = "myUser",
            Password = "myPassword",
        };
        _optionsMonitor = Substitute.For<IOptionsMonitor<DynDnsConfiguration>>();
        _optionsMonitor.CurrentValue.Returns(_testSettings);
    }

    [Test]
    public void UpdateShouldReturnUpdateUrl()
    {
        var sut = new DynDnsService(_optionsMonitor);
        var expected = _testSettings.UpdateUrl;
        var actual = sut.Update("","","","");
        Assert.That(actual, Is.EqualTo(expected));
    }
}
