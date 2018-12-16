using System;

namespace BindToConfig.Internal
{
  internal static class Check
  {
    internal static void NotNull<T>(T any, string paramName) where T : class
    {
      if (any is null)
      {
        throw new ArgumentNullException(paramName);
      }
    }
  }
}
