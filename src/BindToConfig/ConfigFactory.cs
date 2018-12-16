namespace BindToConfig
{
  public interface ConfigFactory<out TConfigClass> where TConfigClass : class
  {
    TConfigClass Create();
  }
}
