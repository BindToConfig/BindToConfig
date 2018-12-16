using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BindToConfig.UnitTests.AddFromToConfig
{
  public class AddFromConfig_Registers_Class_So_That
  {
    private const string AnyKey = "_";
    private static readonly IConfiguration EmptyConfiguration = new ConfigurationBuilder().Build();
    private readonly IServiceCollection _serviceCollection = new ServiceCollection();

    [Fact]
    public void When_Configuration_Has_Been_Changed_It_Does_Not_Affect_New_Scopes_And_The_Same_Object_Is_Always_Returned_For_Each_Scope()
    {
      // arrange
      var configurationProvider = new TesTConfigClassProvider();
      var configuration = new ConfigurationRoot(new List<IConfigurationProvider> { configurationProvider });
      _serviceCollection.AddFromConfig<SampleConfiguration>(
        configuration,
        AnyKey,
        BindOptions.DontThrowIfSectionIsMissingOrEmpty);
      var provider = _serviceCollection.BuildServiceProvider(true);
      var myOptions1 = provider.CreateScope().ServiceProvider.GetRequiredService<SampleConfiguration>();
      Func<SampleConfiguration> GetObjectFromNewScope = () =>
        provider.CreateScope().ServiceProvider.GetRequiredService<SampleConfiguration>();
      var expected = GetObjectFromNewScope();
      // act
      configurationProvider.Reload();
      // assert
      var list = new List<SampleConfiguration>();
      for (var i = 0; i < 10; i++)
      {
        list.Add(GetObjectFromNewScope());
      }
      list.TrueForAll(x => ReferenceEquals(expected, x)).Should().BeTrue();
    }

    [Fact]
    public void
      When_Configuration_Has_Been_Changed_It_Does_Not_Affect_Existing_Scope_And_The_Same_Object_Is_Always_Returned_For_The_Same_Scope()
    {
      // arrange
      var configurationProvider = new TesTConfigClassProvider();
      var configuration = new ConfigurationRoot(new List<IConfigurationProvider> {configurationProvider});
      _serviceCollection.AddFromConfig<SampleConfiguration>(
        configuration,
        AnyKey,
        BindOptions.DontThrowIfSectionIsMissingOrEmpty);
      var provider = _serviceCollection.BuildServiceProvider(true).CreateScope().ServiceProvider;
      var myOptions1 = provider.GetRequiredService<SampleConfiguration>();
      // act
      configurationProvider.Reload();
      // assert
      var myOptions2 = provider.GetRequiredService<SampleConfiguration>();
      Assert.Same(myOptions1, myOptions2);
    }

    [Fact]
    public void
      When_Configuration_Has_Not_Been_Changed_The_Same_Object_Is_Always_Returned_For_The_Same_Scope()
    {
      // act
      _serviceCollection.AddFromConfig<SampleConfiguration>(
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
      _serviceCollection.AddFromConfig<SampleConfiguration>(
        EmptyConfiguration,
        AnyKey,
        BindOptions.DontThrowIfSectionIsMissingOrEmpty);
      // assert
      var provider = _serviceCollection.BuildServiceProvider(true);
      Func<SampleConfiguration> GetObjectFromNewScope = () =>
        provider.CreateScope().ServiceProvider.GetRequiredService<SampleConfiguration>();
      var expected = GetObjectFromNewScope();
      var list = new List<SampleConfiguration>();
      for (var i = 0; i < 10; i++)
      {
        list.Add(GetObjectFromNewScope());
      }
      list.TrueForAll(x => ReferenceEquals(expected, x)).Should().BeTrue();
    }
  }
}
