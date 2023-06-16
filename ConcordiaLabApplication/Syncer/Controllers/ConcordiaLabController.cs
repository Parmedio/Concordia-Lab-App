using BusinessLogic.DataTransferLogic.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Syncer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConcordiaLabController : ControllerBase
    {
        private readonly ILogger<ConcordiaLabController> _logger;
        private readonly IDataSyncer _dataSyncer;

        public ConcordiaLabController(ILogger<ConcordiaLabController> logger, IDataSyncer dataSyncer)
        {
            _logger = logger;
            _dataSyncer = dataSyncer;
        }

        [HttpPut(Name = "ExecuteAsync")]
        public async Task<IActionResult> ExecuteSynchronization()
        {
            try
            {
                await _dataSyncer.SynchronizeAsync();
                return Ok();

            }catch (Exception)
            {
                return StatusCode(500, "Error syncing data...");
            }
        }
    }
}