using Microsoft.Extensions.Configuration;

namespace BindToConfig.UnitTests
{
  public class TesTConfigClassProvider : ConfigurationProvider
  {
    public void Reload() => OnReload();

    internal void Clear()
    {
      Data.Clear();
      Reload();
    }
  }
}
