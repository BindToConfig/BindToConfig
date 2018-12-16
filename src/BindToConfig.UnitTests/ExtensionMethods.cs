using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BindToConfig.UnitTests
{
  public static class ExtensionMethods
  {
    public static TesTConfigClassProvider SetSection(
      this TesTConfigClassProvider provider,
      int intValue,
      string stringValue)
    {
      provider.Set(CreateKeyName<SampleConfiguration>(x => x.IntValue), intValue.ToString());
      provider.Set(CreateKeyName<SampleConfiguration>(x => x.StringValue), stringValue);
      return provider;
    }

    public static KeyValuePair<string, string> CreateKeyValuePair<E>(Expression<Func<object>> expression)
    {
      if (!(expression.Body is MemberExpression member))
      {
        member = (expression.Body as UnaryExpression)?.Operand as MemberExpression;
      }
      if (member == null)
      {
        throw new ArgumentException("Action must be a member expression.");
      }
      return new KeyValuePair<string, string>(
        $"{typeof(E).Name}:{member.Member.Name}",
        expression.Compile().Invoke().ToString());
    }

    internal static string CreateKeyName<T>(Expression<Func<T, object>> expression)
    {
      if (!(expression.Body is MemberExpression member))
      {
        member = (expression.Body as UnaryExpression)?.Operand as MemberExpression;
      }
      if (member == null)
      {
        throw new ArgumentException("Action must be a member expression.");
      }
      return $"{typeof(T).Name}:{member.Member.Name}";
    }
  }
}
