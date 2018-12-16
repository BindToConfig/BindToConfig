using System.Collections.Generic;
using BindToConfig.Internal;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace BindToConfig.UnitTests.Internal
{
  public class CachedFactoryTests
  {
    [Fact]
    public void
      Create_Should_Return_Previous_Object_If_Configuration_Had_Been_Changed_And_Is_Invalid_To_Create_New_One()
    {
      var tesTConfigClassProvider = new TesTConfigClassProvider();
      tesTConfigClassProvider.Set(ExtensionMethods.CreateKeyName<SampleConfiguration>(x => x.IntValue), "10");
      var configuration = new ConfigurationRoot(new List<IConfigurationProvider> {tesTConfigClassProvider});
      var factory = new TrackingConfigurationChangesFactory<SampleConfiguration>(
        configuration.GetSection(nameof(SampleConfiguration)),
        BindPolicy.Default);
      var lastValidConfig = factory.Create();
      tesTConfigClassProvider.Clear();
      // act && assert
      var actualConfig = factory.Create();
      Assert.Same(lastValidConfig, actualConfig);
    }

    [Fact]
    public void Test()
    {
      var configurationBuilder = new ConfigurationBuilder();
      var expectedSampleConfiguration = new SampleConfiguration(10, "Value");
      configurationBuilder.AddInMemoryCollection(
        new List<KeyValuePair<string, string>>
        {
          ExtensionMethods.CreateKeyValuePair<SampleConfiguration>(() => expectedSampleConfiguration.IntValue),
          ExtensionMethods.CreateKeyValuePair<SampleConfiguration>(() => expectedSampleConfiguration.StringValue)
        });
      var configuration = configurationBuilder.Build();
      var factory = new TrackingConfigurationChangesFactory<SampleConfiguration>(
        configuration.GetSection(nameof(SampleConfiguration)),
        BindPolicy.Default);
      var obje = factory.Create();
      obje.Should().BeEquivalentTo(expectedSampleConfiguration);
    }
  }
}
