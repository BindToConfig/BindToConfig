using System;
using BindToConfig.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BindToConfig
{
  public static class AddBoundToConfigExtensions
  {
    public static IServiceCollection AddBoundToConfig<TConfigClass>(
      this IServiceCollection serviceCollection,
      IConfiguration configuration,
      BindPolicy policy)
      where TConfigClass : class
    {
      Check.NotNull(configuration, nameof(configuration));
      var factory = new TrackingConfigurationChangesFactory<TConfigClass>(configuration, policy);
      serviceCollection.AddSingleton(factory as ConfigFactory<TConfigClass>);
      return serviceCollection.AddScoped(x =>
      {
        if (AddBoundToConfigLogger.Instance.Logger == null)
        {
          AddBoundToConfigLogger.Instance.TrySetLogger(x.GetService<ILogger<AddBoundToConfigLogger>>());
        }

        return factory.Create();
      });
    }

    public static IServiceCollection AddBoundToConfig<TConfigClass>(
      this IServiceCollection serviceCollection,
      IConfiguration configuration,
      Action<Exception, Type> whenConfigurationChangeCausesError = null,
      params BindOptions[] options)
      where TConfigClass : class =>
      serviceCollection.AddBoundToConfig<TConfigClass>(
        configuration,
        new BindPolicy
        {
          ThrowIfSectionIsMissingOrEmpty = BindOptions.IsDontThrowIfSectionIsMissingOrEmptySet(options),
          WhenConfigurationChangeCausesError = whenConfigurationChangeCausesError
        });

    public static IServiceCollection AddBoundToConfig<TConfigClass>(
      this IServiceCollection serviceCollection,
      IConfiguration configuration,
      params BindOptions[] options)
      where TConfigClass : class =>
      serviceCollection.AddBoundToConfig<TConfigClass>(configuration, (Action<Exception, Type>)null, options);

    public static IServiceCollection AddBoundToConfig<TConfigClass>(
      this IServiceCollection serviceCollection,
      IConfiguration configuration,
      string key,
      Action<Exception, Type> whenConfigurationChangeCausesError = null,
      params BindOptions[] options)
      where TConfigClass : class => serviceCollection.AddBoundToConfig<TConfigClass>(
      configuration.GetSection(key),
      whenConfigurationChangeCausesError,
      options);

    public static IServiceCollection AddBoundToConfig<TConfigClass>(
      this IServiceCollection serviceCollection,
      IConfiguration configuration,
      string key,
      params BindOptions[] options)
      where TConfigClass : class =>
      serviceCollection.AddBoundToConfig<TConfigClass>(configuration.GetSection(key), options);
  }
}
