using System;
using BindToConfig.Exceptions;
using Microsoft.Extensions.Configuration;

namespace BindToConfig.Internal
{
  internal static class ConfigFactoryMethods
  {
    internal static TConfigClass CreateConfig<TConfigClass>(IConfiguration configuration)
      where TConfigClass : class
    {
      try
      {
        var config = (TConfigClass)Activator.CreateInstance(typeof(TConfigClass), true);
        configuration.Bind(config, x => x.BindNonPublicProperties = true);
        return config;
      }
      catch (Exception ex)
      {
        throw Throw.CouldNotInstantiateClass<TConfigClass>(ex);
      }
    }

    internal static Func<TConfigClass> BuildFactory<TConfigClass>(
      IConfiguration configuration,
      bool throwIfSectionNotExists)
      where TConfigClass : class
    {
      if (throwIfSectionNotExists == false)
      {
        return () => CreateConfig<TConfigClass>(configuration);
      }
      return () =>
      {
        if (configuration is IConfigurationSection section && !section.Exists())
        {
          throw Throw.SectionIsMissingOrEmpty<TConfigClass>(section.Key);
        }

        return CreateConfig<TConfigClass>(configuration);
      };
    }
  }
}
