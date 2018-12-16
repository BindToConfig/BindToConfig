using BindToConfig.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BindToConfig
{
  public static class AddFromToConfigExtensions
  {

    public static IServiceCollection AddFromConfig<TConfigClass>(
        this IServiceCollection serviceCollection,
        IConfiguration configuration,
        params BindOptions[] options)
        where TConfigClass : class
      {
        Check.NotNull(configuration, nameof(configuration));
        var create = ConfigFactoryMethods.BuildFactory<TConfigClass>(configuration,
          BindOptions.IsDontThrowIfSectionIsMissingOrEmptySet(options));
        return serviceCollection.AddSingleton(create());
      }

    public static IServiceCollection AddFromConfig<TConfigClass>(
      this IServiceCollection serviceCollection,
      IConfiguration configuration,
      string key,
      params BindOptions[] options)
      where TConfigClass : class =>
        serviceCollection.AddFromConfig<TConfigClass>(configuration.GetSection(key), options);
  }
}
