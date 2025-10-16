using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using TopicosP1Backend.Scripts;

namespace TopicosP1Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController(APIQueue queue, WorkerManager manager) : ControllerBase
    {
        private readonly APIQueue _queue = queue;

        [HttpGet("Transaction/{id}")]
        public async Task<ActionResult<object>> GetTransaction(string id)
        {
            return _queue.getTranStatus(id);
        }

        [HttpGet("MakeAsync")]
        public async Task<ActionResult<object>> MakeAsync()
        {
            return _queue.isasync = true;
        }

        [HttpGet("MakeSync")]
        public async Task<ActionResult<object>> MakeSync()
        {
            return _queue.isasync = false;
        }

        [HttpGet("ThingsReceived")]
        public async Task<ActionResult<object>> GetThingsReceived()
        {
            int total = _queue.thingsreceived["Total"];
            _queue.thingsreceived["Total"] = _queue.thingsreceived.Values.Sum() - total;
            return _queue.thingsreceived;
        }

        [HttpGet("ThingsDone")]
        public async Task<ActionResult<object>> GetThingsDone()
        {
            int total = _queue.thingsdone["Total"];
            _queue.thingsdone["Total"] = _queue.thingsdone.Values.Sum() - total;
            return _queue.thingsdone;
        }
    }
}
