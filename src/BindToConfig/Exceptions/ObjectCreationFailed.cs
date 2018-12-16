using System;

namespace BindToConfig.Exceptions
{
  public class ObjectCreationFailed : AddBoundToConfigException
  {
    internal ObjectCreationFailed(Type correspondingType, Exception innerException) : base(
      correspondingType,
      $"Could create a new instance of '{correspondingType.Name}' due to : '{innerException.Message}'",
      innerException)
    {
    }
  }
}
