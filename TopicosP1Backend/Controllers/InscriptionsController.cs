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
            string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknownhost";
            if (ipAddress.StartsWith("::ffff:10.52.12")) return new OkResult();
            return _queue.Request(Function.GetInscriptions, [], "", "GetInscriptions", true, ip: ipAddress);
        }

        [HttpGet("{id}")]
        public object GetInscription(long id)
        {
            string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "0.0.0.0";
            return _queue.Request(Function.GetInscription, [$"{id}"], "", $"GetInscription {id}", true, ip: ipAddress);
        }

        [HttpPost]
        public object PostInscription(Inscription.InscriptionPost i)
        {
            string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "0.0.0.0";
            string b = JsonSerializer.Serialize(i);
            return _queue.Request(Function.PostInscription, [], b, $"PostInscription {b}", ip: ipAddress);
        }

        [HttpDelete("{id}")]
        public object DeleteInscription(long id)
        {
            string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "0.0.0.0";
            return _queue.Request(Function.DeleteInscription, [$"{id}"], "", $"DeleteInscription {id}", ip: ipAddress);
        }
    }
}
