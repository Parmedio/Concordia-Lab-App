using BusinessLogic.DataTransferLogic.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Syncer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SyncerController : ControllerBase
    {
        private readonly IDataSyncer _dataSyncer;
        private readonly ILogger<SyncerController> _logger;

        public SyncerController(ILogger<SyncerController> logger, IDataSyncer dataSyncer)
        {
            _logger = logger;
            _dataSyncer = dataSyncer;
        }

        [HttpPut]
        public async Task<IActionResult> Sync()
        {
            try
            {
                await _dataSyncer.SynchronizeAsync();
                return Ok();
            } catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}