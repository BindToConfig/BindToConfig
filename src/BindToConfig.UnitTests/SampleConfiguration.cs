namespace BindToConfig.UnitTests
{
  public sealed class SampleConfiguration
  {
    private SampleConfiguration()
    {
    }

    public SampleConfiguration(int intValue, string stringValue)
    {
      IntValue = intValue;
      StringValue = stringValue;
    }

    public int IntValue { get; private set; }
    public string StringValue { get; private set;}
  }
}
