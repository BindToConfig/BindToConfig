using System;
using BindToConfig.Exceptions;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BindToConfig.UnitTests.AddBoundToConfig
{
  public class AddBoundToConfig_FastFails
  {
    private const string AnyKey = "_";
    private static readonly IConfiguration EmptyConfiguration = new ConfigurationBuilder().Build();
    private readonly IServiceCollection _serviceCollection = new ServiceCollection();

    [Fact]
    public void By_Throwing_ConfigurationObjectCreationFailed_If_Instantiation_Of_Object_Is_Not_Possible()
    {
      // act
      var exception = Assert.Throws<ObjectCreationFailed>(
        () =>
          new ServiceCollection().AddBoundToConfig<NotInstantiationClass>(
            EmptyConfiguration,
            AnyKey,
            BindOptions.DontThrowIfSectionIsMissingOrEmpty));
      // assert
      exception.CorrespondingType.Should().Be<NotInstantiationClass>();
      exception.InnerException.Should().NotBeNull("Runtime exception was catched.");
    }

    [Fact]
    public void
      By_Throwing_ConfigurationSectionIsMissingOrEmpty_When_Section_With_Given_Key_Does_Not_Exist()
    {
      // arrange
      const string expectedNotExistingSectionKey = "NonExistingSectionKey";
      // act
      var exception = Assert.Throws<SectionIsMissingOrEmpty>(
        () =>
          new ServiceCollection().AddBoundToConfig<NotInstantiationClass>(
            EmptyConfiguration,
            expectedNotExistingSectionKey));
      // assert
      Assert.Equal(expectedNotExistingSectionKey, exception.Key);
      exception.CorrespondingType.Should().Be<NotInstantiationClass>();
    }

    [Fact]
    public void
      Throws_When_Passed_Configuration_Is_Null()
    {
      // act && assert
      Assert.Throws<ArgumentNullException>(
        () => new ServiceCollection().AddBoundToConfig<NotInstantiationClass>(null));
    }
    public class NotInstantiationClass
    {
      public NotInstantiationClass(int i)
      {
      }
    }
  }
}
