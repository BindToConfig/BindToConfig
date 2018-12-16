using System;

namespace BindToConfig
{
  public class BindPolicy
  {
    public static readonly BindPolicy Default = new BindPolicy
    {
      ThrowIfSectionIsMissingOrEmpty = true,
      WhenConfigurationChangeCausesError = null
    };

    public bool ThrowIfSectionIsMissingOrEmpty { get; set; } = true;
    public Action<Exception, Type> WhenConfigurationChangeCausesError { get; set; }
  }
}
