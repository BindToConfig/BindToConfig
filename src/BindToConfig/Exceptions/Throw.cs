using System;

namespace BindToConfig.Exceptions
{
  internal static class Throw
  {
    internal static Exception CouldNotInstantiateClass<TConfigClass>(Exception ex) where TConfigClass : class =>
      new ObjectCreationFailed(typeof(TConfigClass), ex);

    internal static Exception SectionIsMissingOrEmpty<TConfigClass>(string key) where TConfigClass : class =>
      new SectionIsMissingOrEmpty(typeof(TConfigClass), key);
  }
}
