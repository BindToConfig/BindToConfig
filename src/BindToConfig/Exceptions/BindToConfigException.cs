using System;

namespace BindToConfig.Exceptions
{
  public abstract class AddBoundToConfigException : Exception
  {
    protected AddBoundToConfigException(Type correspondingType, string message, Exception innerException) : base(
      message,
      innerException) => CorrespondingType = correspondingType;

    protected AddBoundToConfigException(Type correspondingType, string message) : base(message) =>
      CorrespondingType = correspondingType;

    public Type CorrespondingType { get; }
  }
}
