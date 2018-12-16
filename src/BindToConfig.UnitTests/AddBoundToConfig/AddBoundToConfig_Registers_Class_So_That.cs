using System;
using System.Collections.Generic;
using System.Threading;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BindToConfig.UnitTests.AddBoundToConfig
{
  public class AddBoundToConfig_Registers_Class_So_That
  {
    private const string AnyKey = "_";
    private static readonly IConfiguration EmptyConfiguration = new ConfigurationBuilder().Build();
    private readonly IServiceCollection _serviceCollection = new ServiceCollection();

    [Fact]
    public void When_Configuration_Has_Been_Changed_A_New_Object_Is_Returned_For_A_New_Scope()
    {
      // arrange
      var expected = new SampleConfiguration(1001, Guid.NewGuid().ToString());
      var configurationProvider = new TesTConfigClassProvider().SetSection(100, "any");
      var configuration = new ConfigurationRoot(new List<IConfigurationProvider> {configurationProvider});
      _serviceCollection.AddBoundToConfig<SampleConfiguration>(configuration, nameof(SampleConfiguration));
      var provider = _serviceCollection.BuildServiceProvider(true);
      var myOptions1 = provider.CreateScope().ServiceProvider.GetRequiredService<SampleConfiguration>();
      // act
      configurationProvider.SetSection(expected.IntValue, expected.StringValue);
      configurationProvider.Reload();
      // assert
      var myOptions2 = provider.CreateScope().ServiceProvider.GetRequiredService<SampleConfiguration>();
      Assert.NotSame(myOptions1, myOptions2);
      myOptions2.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void When_Configuration_Has_Been_Changed_And_Causes_Instatnion_Errors_Registered_Handler_Is_Called()
    {
      // arrange
      var configurationProvider = new TesTConfigClassProvider().SetSection(100, "any");
      var configuration = new ConfigurationRoot(new List<IConfigurationProvider> {configurationProvider});
      var resetEvent = new ManualResetEventSlim(false);
      _serviceCollection.AddBoundToConfig<SampleConfiguration>(
        configuration,
        nameof(SampleConfiguration),
        (ex, type) => { resetEvent.Set(); });
      var provider = _serviceCollection.BuildServiceProvider(true);
      // act
      configurationProvider.Clear();
      configurationProvider.Reload();
      provider.CreateScope().ServiceProvider.GetRequiredService<SampleConfiguration>();
      // assert
      resetEvent.Wait(TimeSpan.FromMilliseconds(1000)).Should().BeTrue("Error handler should be called.");
    }

    [Fact]
    public void
      When_Configuration_Has_Been_Changed_And_Now_Causes_Instatnion_Errors_The_Last_Object_Is_Returned_For_A_New_Scope()
    {
      // arrange
      var configurationProvider = new TesTConfigClassProvider().SetSection(100, "any");
      var configuration = new ConfigurationRoot(new List<IConfigurationProvider> {configurationProvider});
      _serviceCollection.AddBoundToConfig<SampleConfiguration>(configuration, nameof(SampleConfiguration));
      var provider = _serviceCollection.BuildServiceProvider(true);
      var myOptions1 = provider.CreateScope().ServiceProvider.GetRequiredService<SampleConfiguration>();
      // act
      configurationProvider.Clear();
      configurationProvider.Reload();
      var myOptions2 = provider.CreateScope().ServiceProvider.GetRequiredService<SampleConfiguration>();
      // assert
      Assert.Same(myOptions1, myOptions2);
    }

    [Fact]
    public void
      When_Configuration_Has_Been_Changed_It_Does_Not_Affect_Existing_Scope_And_The_Same_Object_Is_Always_Returned_For_The_Same_Scope()
    {
      // arrange
      var configurationProvider = new TesTConfigClassProvider();
      var configuration = new ConfigurationRoot(new List<IConfigurationProvider> {configurationProvider});
      _serviceCollection.AddBoundToConfig<SampleConfiguration>(
        configuration,
        AnyKey,
        BindOptions.DontThrowIfSectionIsMissingOrEmpty);
      var provider = _serviceCollection.BuildServiceProvider(true);
      var myOptions1 = provider.CreateScope().ServiceProvider.GetRequiredService<SampleConfiguration>();
      // act
      configurationProvider.Reload();
      // assert
      var myOptions2 = provider.CreateScope().ServiceProvider.GetRequiredService<SampleConfiguration>();
      Assert.NotSame(myOptions1, myOptions2);
    }

    [Fact]
    public void
      When_Configuration_Has_Not_Been_Changed_The_Same_Object_Is_Always_Returned_For_The_Same_Scope()
    {
      // act
      _serviceCollection.AddBoundToConfig<SampleConfiguration>(
        EmptyConfiguration,
        AnyKey,
        BindOptions.DontThrowIfSectionIsMissingOrEmpty);
      // assert
      var provider = _serviceCollection.BuildServiceProvider(true).CreateScope().ServiceProvider;
      var myOptions1 = provider.GetRequiredService<SampleConfiguration>();
      var myOptions2 = provider.GetRequiredService<SampleConfiguration>();
      Assert.Same(myOptions1, myOptions2);
    }

    [Fact]
    public void
      When_Configuration_Has_Not_Been_Changed_The_Same_Object_Is_Returned_For_Each_New_Scope()
    {
      // act
      _serviceCollection.AddBoundToConfig<SampleConfiguration>(
        EmptyConfiguration,
        AnyKey,
        BindOptions.DontThrowIfSectionIsMissingOrEmpty);
      var provider = _serviceCollection.BuildServiceProvider(true);
      Func<SampleConfiguration> GetObjectFromNewScope = () =>
        provider.CreateScope().ServiceProvider.GetRequiredService<SampleConfiguration>();
      var expected = GetObjectFromNewScope();
      var list = new List<SampleConfiguration>();
      // act
      for (var i = 0; i < 10; i++)
      {
        list.Add(GetObjectFromNewScope());
      }

      // assert
      list.TrueForAll(x => ReferenceEquals(expected, x)).Should().BeTrue();
    }
  }
}
