using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApplication.Configs;

namespace WebApplication.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ValuesController : ControllerBase
  {
    private readonly ConfigSampleWithPrivateCtorA _config1;
    private readonly ConfigSampleWithPrivateCtorB _config2;
    private readonly ConfigSampleWithPrivateSetters _config3;
    private readonly ConfigSample _config4;

    public ValuesController(
      ConfigSampleWithPrivateCtorA config1,
      ConfigSampleWithPrivateCtorB config2,
      ConfigSampleWithPrivateSetters config3,
      ConfigSample config4)
    {
      _config1 = config1;
      _config2 = config2;
      _config3 = config3;
      _config4 = config4;
    }
    // GET api/values
    [HttpGet]
    public ActionResult<string> Get()
    {
      return $@"Config values:
{_config1.GetType().Name} : {JsonConvert.SerializeObject(_config1)}
{_config2.GetType().Name} : {JsonConvert.SerializeObject(_config2)}
{_config3.GetType().Name} : {JsonConvert.SerializeObject(_config3)}
{_config4.GetType().Name} : {JsonConvert.SerializeObject(_config4)}
";
    }
  }
}
