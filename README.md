[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://raw.githubusercontent.com/BindToConfig/BindToConfig/master/LICENSE)
[![Documentation Status](https://readthedocs.org/projects/bindtoconfig/badge/?version=latest)](https://bindtoconfig.readthedocs.io/en/latest/?badge=latest)
[![Coverage Status](https://coveralls.io/repos/github/BindToConfig/BindToConfig/badge.svg?branch=)](https://coveralls.io/github/BindToConfig/BindToConfig?branch=master)
[![Build Status](https://dev.azure.com/BindToConfig/BindToConfig/_apis/build/status/BindToConfig.BindToConfig?branchName=master)](https://dev.azure.com/BindToConfig/BindToConfig/_build/latest?definitionId=1?branchName=master)

# BindToConfig .Net Core
***BindToConfig*** is a simple and lightweight alternative to ***.NET Core Options***, that doesn't require any additional interface implemented by classes.
Focused on simplicity and good practices, promotes creating many small, clean, immutable classes that are a representation of application's configuration which lifetime scope and binding to the configuration are set in composition root and not directly by dependent classes. 

***BindToConfig*** fills the gap between .NET Core's Configuration and DI, by taking care of mapping and binding objects to the configuration, validating and registering in .NET Core DI. 
With just a single method call it provides .NET Core applications with an easy way of adding classes representing parts of Configuration. 

Built with the simplicity and promotion of best practices in mind
* Interface Segregation Principle - no additional  interface is required
* Immutability
* Fail-fast
* Composition root 
* Base what is need: ``services.AddBoundToConfig<ConfigClass>(..)``

*small, lightweight, free and open-source.*

## Code example
In ConfigureServices method, just call AddFromConfig or AddBoundToConfig on services.  
Startup.cs:

```csharp
public void ConfigureServices(IServiceCollection services)
{
  services.AddMvc();
  // adds SampleConfig1 as Singleton
  services.AddFromConfig<SampleConfig1>(Configuration, "SectionKey1");
  // adds Sample Config2 as Scoped, 
  // new scopes return updated Config2 object when configuration changes
  // use it only when You really use live-updates of configuration
  services.AddBoundToConfig<SampleConfig2>(Configuration, "SectionKey2");
  services.AddBoundToConfig<SampleConfig3>(
      Configuration,
      "NonExisting",
      BindOptions.DontThrowIfSectionIsMissingOrEmpty);
}
```
From now on `SampleConfig1` and `SmapleConfig2` are filled with proper values from the configuration and registered in DI, so that You can simply declare it as constructor parameters of any class:

```csharp
public class ValuesController : ControllerBase
{
  ...
  public ValuesController(SampleConfig1 config1, SampleConfig2 config2, SampleConfig3 config3)
  {
    _config1 = config1;
    _config2 = config2;
    _config3 = config3;  
  }
}
```

## Key features
- Pure configuration classes without any additional interface like IOptions or IOptionsSnapshot
- Automatic registration in .NET Core DI
- Automatic binding to Your configuration
  - detects and updates object automatically when configuration will be changed - if reloadOnChange is set to true for file configurations. 
  Each new request will use config with updated values. BindToConfig uses Scoped lifetime for registration.
- Immutable classes
    - allows to use private setters for properties.
    - works with a non-public private constructor. The only requirement is that class must have implicit or a parameterless constructor(public/private/protected).
- Fast fails
    - on application's startup detects that given configuration is not valid for a bound  object and causes exception so feedback is immediately instead of getting it late on runtime.
- Smart live configuration updates errors handling
    - when reloadOnChange flag is set for file-based configurations it is possible to update it without the need of application restart. But it's also possible to introduce some errors in configuration so that mapping to config class will be impossible. In that case, BindToConfig won't throw an error when getting object by DI(which could cause an error on some of request), but instead returns the last one, and pass information to Your custom handler.


