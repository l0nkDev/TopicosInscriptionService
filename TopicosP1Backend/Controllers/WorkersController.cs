using Microsoft.AspNetCore.Mvc;
using TopicosP1Backend.Scripts;

namespace TopicosP1Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkersController(APIQueue queue, WorkerManager manager) : ControllerBase
    {
        private readonly APIQueue _queue = queue;
        private readonly WorkerManager _manager = manager;

        [HttpGet("")]
        public async Task<object> GetWorkers()
        {
            return _manager.GetWorkers();
        }

        [HttpGet("AddToQueue/{id}")]
        public async Task<object> AddWorker(int id)
        {
            _manager.AddWorker(id);
            return new OkResult();
        }

        [HttpGet("{id}")]
        public async Task<object> GetWorker(int id)
        {
            return _manager.GetWorker(id);
        }

        [HttpDelete("{id}")]
        public async Task<object> DeleteWorker(int id)
        {
            _manager.RemoveWorker(id);
            return new OkResult();
        }

        [HttpGet("{id}/SetToQueue/{q}")]
        public async Task<object> SetWorkerQueue(int id, int q)
        {
            _manager.SetWorkerQueue(id, q);
            return new OkResult();
        }

        [HttpGet("{id}/SetTake/{take}")]
        public async Task<object> SetWorkerTake(int id, int take)
        {
            _manager.SetWorkerTake(id, take);
            return new OkResult();
        }

        [HttpGet("{id}/Start")]
        public async Task<object> StartWorker(int id)
        {
            _manager.Start(id-1);
            return new OkResult();
        }

        [HttpGet("{id}/Stop")]
        public async Task<object> StopWorker(int id)
        {
            _manager.Stop(id-1);
            return new OkResult();
        }
    }
}
