using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BindToConfig.UnitTests.AddBoundToConfig
{
  public class AddBoundToConfig_When_FastFail_Is_Off
  {
    private const string AnyKey = "_";
    private static readonly IConfiguration EmptyConfiguration = new ConfigurationBuilder().Build();
    private readonly IServiceCollection _serviceCollection = new ServiceCollection();

    [Fact]
    public void
      Do_Not_Throws_Any_Exception_When_Section_With_Given_Key_Does_Not_Exist_And_DontThrowIfSectionIsMissingOrEmpty_Is_Set() =>
      _serviceCollection.AddBoundToConfig<SampleConfiguration>(
        EmptyConfiguration,
        "NonExistentKey",
        BindOptions.DontThrowIfSectionIsMissingOrEmpty);
  }
}
