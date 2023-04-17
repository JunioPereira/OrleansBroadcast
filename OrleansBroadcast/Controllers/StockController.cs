using GransInterface.Interfaces;
using GransInterface.States;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;

namespace OrleansBroadcast.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        IClusterClient ClusterClient { get; }

        private readonly ILogger<StockController> _logger;

        public StockController(ILogger<StockController> logger, IClusterClient iClusterClient)
        {
            _logger = logger;
            ClusterClient = iClusterClient;
        }

        
        [HttpGet("{symbol}")]
        public async Task<IActionResult> GetStock(string symbol)
        {
            try
            {
                var igrain = ClusterClient.GetGrain<IStockGrain>(symbol);
                
                var result = await igrain.Get();

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(0, "GetStock"), ex, "GetStock");
                return BadRequest(ex.ToString());
            }
        }

        [HttpPut("{symbol}")]
        public async Task<IActionResult> SetStock(string symbol, [FromBody]decimal price)
        {
            try
            {
                var igrain = ClusterClient.GetGrain<IStockGrain>(symbol);
                
                await igrain.Set(price);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(0, "SetStock"), ex, "SetStock");
                return BadRequest(ex.ToString());
            }
        }
    }
}
