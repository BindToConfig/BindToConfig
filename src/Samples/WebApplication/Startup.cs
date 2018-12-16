using BindToConfig;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApplication.Configs;

namespace WebApplication
{
  public class Startup
  {
    public Startup(IConfiguration configuration) => Configuration = configuration;

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
      services.AddBoundToConfig<ConfigSampleWithPrivateCtorA>(Configuration, "ConfigValues1");
      services.AddFromConfig<ConfigSampleWithPrivateCtorB>(Configuration, "ConfigValues1");
      services.AddBoundToConfig<ConfigSampleWithPrivateSetters>(Configuration, "ConfigValues2");
      services.AddBoundToConfig<ConfigSample>(Configuration, "NonExisting", BindOptions.DontThrowIfSectionIsMissingOrEmpty);
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      app.UseMvc();
    }
  }
}
