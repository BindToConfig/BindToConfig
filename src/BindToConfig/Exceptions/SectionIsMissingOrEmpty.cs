using System;

namespace BindToConfig.Exceptions
{
  public class SectionIsMissingOrEmpty : AddBoundToConfigException
  {
    internal SectionIsMissingOrEmpty(Type correspondingType, string key) : base(
      correspondingType,
      $"Section '{key}' to which '{correspondingType.Name}' should be bound, is missing or is empty. ") => Key = key;

    public string Key { get; }
  }
}
