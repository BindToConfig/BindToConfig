using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace WebApplication
{
  public class Program
  {
    public static void Main(string[] args) => CreateWebHostBuilder(args).Build().Run();

    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
      WebHost.CreateDefaultBuilder(args)
        .ConfigureLogging(x=>x.AddConsole())
        .ConfigureAppConfiguration(x => x.AddJsonFile("appsettings.json", false, true))
        .UseStartup<Startup>();
  }
}
