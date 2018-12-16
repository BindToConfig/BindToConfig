using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace BindToConfig.Internal
{
  internal class TrackingConfigurationChangesFactory<TConfigClass> : ConfigFactory<TConfigClass>, IDisposable
    where TConfigClass : class
  {
    private readonly Func<TConfigClass> _configObjectsFactory;
    private readonly BindPolicy _policy;
    private readonly IDisposable _subscription;
    private readonly object _syncObj = new object();
    private TConfigClass _lastConfig;

    internal TrackingConfigurationChangesFactory(IConfiguration configuration, BindPolicy policy)
    {
      Check.NotNull(configuration, nameof(configuration));
      Check.NotNull(policy, nameof(policy));
      _policy = policy;
      _configObjectsFactory = ConfigFactoryMethods.BuildFactory<TConfigClass>(configuration, _policy.ThrowIfSectionIsMissingOrEmpty);
      _lastConfig = _configObjectsFactory();
      _subscription = ChangeToken.OnChange(configuration.GetReloadToken, UpdateConfig);
    }

    public TConfigClass Create() => _lastConfig;

    public void Dispose() => _subscription.Dispose();

    private void UpdateConfig()
    {

      try
      {
        lock (_syncObj) // to guarantee that config have the most updated values.
        {
          _lastConfig = _configObjectsFactory();
        }

        AddBoundToConfigLogger.Instance.LogBindSuccess<TConfigClass>();
      }
      catch (Exception ex)
      {
        AddBoundToConfigLogger.Instance.LogBindingError<TConfigClass>(ex);
        if (_policy.WhenConfigurationChangeCausesError != null)
        {
          Task.Factory.StartNew(
            () => _policy.WhenConfigurationChangeCausesError.Invoke(ex, typeof(TConfigClass)));
        }
      }
    }
  }
}
