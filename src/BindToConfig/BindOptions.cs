using System.Collections.Generic;
using System.Linq;

namespace BindToConfig
{
  public class BindOptions
  {
    public static readonly BindOptions DontThrowIfSectionIsMissingOrEmpty = new BindOptions();
    public static readonly BindOptions ThrowIfSectionIsMissingOrEmpty = new BindOptions();

    private BindOptions()
    {
    }

    internal static bool IsDontThrowIfSectionIsMissingOrEmptySet(IEnumerable<BindOptions> options)
    {
      var lastSetOfOption = options.LastOrDefault(
        option =>
          option == DontThrowIfSectionIsMissingOrEmpty || option == ThrowIfSectionIsMissingOrEmpty);
      return lastSetOfOption == null || lastSetOfOption == ThrowIfSectionIsMissingOrEmpty;
    }
  }
}
