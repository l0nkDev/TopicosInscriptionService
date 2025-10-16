using Microsoft.AspNetCore.Mvc;
using CareerApi.Models;
using TopicosP1Backend.Scripts;
using System.Text.Json;

namespace TopicosP1Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InscriptionsController(APIQueue queue) : ControllerBase
    {
        private readonly APIQueue _queue = queue;

        [HttpGet("status/{tranid}")]
        public object GetInscriptionState(string tranid)
        {
            return _queue.getTranStatus(tranid);
        }

        [HttpGet]
        public object GetInscriptions()
        {
            return _queue.Request(Function.GetInscriptions, [], "", "GetInscriptions", true);
        }

        [HttpGet("{id}")]
        public object GetInscription(long id)
        {
            return _queue.Request(Function.GetInscription, [$"{id}"], "", $"GetInscription {id}", true);
        }

        [HttpPost]
        public object PostInscription(Inscription.InscriptionPost i)
        {
            string b = JsonSerializer.Serialize(i);
            return _queue.Request(Function.PostInscription, [], b, $"PostInscription {b}");
        }

        [HttpDelete("{id}")]
        public object DeleteInscription(long id)
        {
            return _queue.Request(Function.DeleteInscription, [$"{id}"], "", $"DeleteInscription {id}");
        }
    }
}
