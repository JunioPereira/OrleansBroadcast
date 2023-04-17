using GransInterface.Interfaces;
using GransInterface.States;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace OrleansBroadcast.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        IClusterClient ClusterClient { get; }

        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ILogger<CustomerController> logger, IClusterClient iClusterClient)
        {
            _logger = logger;
            ClusterClient = iClusterClient;
        }

        [HttpGet("{account}")]
        public async Task<IActionResult> GetCustomer(int account)
        {
            try
            {
                var igrain = ClusterClient.GetGrain<ICustomerGrain>(account);
                
                var result = await igrain.Get();

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(0, "GetCustomer"), ex, "GetCustomer");
                return BadRequest(ex.ToString());
            }
        }

        [HttpPut("{account}")]
        public async Task<IActionResult> SetCustomer(int account, [FromBody]CustomerState state)
        {
            try
            {
                var igrain = ClusterClient.GetGrain<ICustomerGrain>(account);
                await igrain.Set(state);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(0, "SetCustomer"), ex, "SetCustomer");
                return BadRequest(ex.ToString());
            }
        }

    }
}