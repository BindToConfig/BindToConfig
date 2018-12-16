using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BindToConfig.UnitTests.AddBoundToConfig
{
  public class AddBoundToConfig_Binds
  {
    private const string AnyKey = "_";
    private static readonly IConfiguration EmptyConfiguration = new ConfigurationBuilder().Build();
    private readonly IServiceCollection _serviceCollection = new ServiceCollection();

    [Fact]
    public void Private_Setters()
    {
      // arrange
      const string key = nameof(SampleConfiguration);
      var expectedSampleConfiguration = new SampleConfiguration(100, "My string value");
      var configurationBuilder = new ConfigurationBuilder();
      configurationBuilder.AddInMemoryCollection(
        new List<KeyValuePair<string, string>>
        {
          new KeyValuePair<string, string>(
            key + ":" + nameof(expectedSampleConfiguration.IntValue),
            expectedSampleConfiguration.IntValue.ToString()),
          new KeyValuePair<string, string>(
            key + ":" + nameof(expectedSampleConfiguration.StringValue),
            expectedSampleConfiguration.StringValue)
        });
      var configuration = configurationBuilder.Build();
      // act
      _serviceCollection.AddBoundToConfig<SampleConfiguration>(configuration, key);
      // assert
      var provider = _serviceCollection.BuildServiceProvider(true);
      var myOptions1 = provider.CreateScope().ServiceProvider.GetRequiredService<SampleConfiguration>();
      Assert.Equal(expectedSampleConfiguration.IntValue, myOptions1.IntValue);
      Assert.Equal(expectedSampleConfiguration.StringValue, myOptions1.StringValue);
    }
  }
}
