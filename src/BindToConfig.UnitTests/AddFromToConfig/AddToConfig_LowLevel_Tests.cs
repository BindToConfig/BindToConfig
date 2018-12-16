using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BindToConfig.UnitTests.AddFromToConfig
{
  public class AddFromConfig_Should_Add_To_ServiceCollection
  {
    private const string AnyKey = "_";
    private static readonly IConfiguration EmptyConfiguration = new ConfigurationBuilder().Build();
    private readonly IServiceCollection _serviceCollection = new ServiceCollection();

    [Fact]
    public void
      Descriptor_With_Given_ConfigClassType_And_Singleton_LifeTime_And_ImplementationInstance()
    {
      // act
      _serviceCollection.AddFromConfig<SampleConfiguration>(
        EmptyConfiguration,
        AnyKey,
        BindOptions.DontThrowIfSectionIsMissingOrEmpty);
      // assert
      _serviceCollection.Should().NotBeEmpty();
      var descriptor = _serviceCollection.Should().Contain(x => x.ServiceType == typeof(SampleConfiguration)).Subject;
      descriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);
      descriptor.ImplementationInstance.Should().NotBeNull();
    }
  }
}
