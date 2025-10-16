using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.OpenApi.Extensions;
using System.Collections.Concurrent;
using TopicosP1Backend.Cache;

namespace TopicosP1Backend.Scripts
{

    public class Qcount
    {
        public long Id { get; set; }

        public List<int> Endpoints { get; set; } = [-1];
        public bool Deleting { get; set; }
    }

    public class CustomQueue: ConcurrentQueue<QueuedFunction>
    {
        public List<int> Endpoints { get; set; } = [-1];
        public bool Deleting { get; set; }
    }
    public class APIQueue
    {
        private readonly IServiceScopeFactory scopeFactory;
        public List<CustomQueue> queues = [];
        private ConcurrentDictionary<string, byte> queued = [];
        private ConcurrentDictionary<string, object> responses = [];
        public ConcurrentDictionary<string, int> thingsdone = [];
        public ConcurrentDictionary<string, int> thingsreceived = [];
        public bool isasync = true;

        public APIQueue(IServiceScopeFactory scopeFactory)
        {
            thingsdone.TryAdd("Total", 0);
            thingsreceived.TryAdd("Total", 0);
            this.scopeFactory = scopeFactory;
            IServiceScope? scope = scopeFactory.CreateScope();
            CacheContext cache = scope.ServiceProvider.GetService<CacheContext>();
            List<Qcount> qcs = cache.qcounts.ToList();
            foreach (Qcount qc in qcs) queues.Add(new() { Endpoints = qc.Endpoints, Deleting=qc.Deleting });
            if (queues.Count == 0) AddQueue([-1]);
            List<QueuedFunction.DBItem> saved = cache.QueuedFunctions.ToList();
            foreach (QueuedFunction.DBItem item in saved) queues[item.Queue].Enqueue(item.ToQueueItem());
            scope?.Dispose(); scope = null; cache = null;
        }
        public void Add(QueuedFunction action)
        {
            using (IServiceScope scope = scopeFactory.CreateScope())
            {
                CacheContext _context = scope.ServiceProvider.GetService<CacheContext>();
                QueuedFunction.DBItem dbi = action.ToDBItem();
                _context.QueuedFunctions.Add(dbi);
                _context.SaveChanges();
                queued.TryAdd(action.Hash, 0);
                CustomQueue emptier = queues[action.Queue];
                emptier.Enqueue(action);
            }
        }

        public void AddQueue(List<int> Endpoints)
        {
            if (Endpoints == null) Endpoints = [-1];
            using (IServiceScope scope = scopeFactory.CreateScope())
            {
                CacheContext _context = scope.ServiceProvider.GetService<CacheContext>();
                _context.qcounts.Add(new() { Endpoints = Endpoints, Deleting = false });
                _context.SaveChanges();
                queues.Add(new() { Endpoints = Endpoints, Deleting = false });
            }
        }

        public void SetEndpoints(int id, List<int> Endpoints)
        {
            if (Endpoints == null) Endpoints = [-1];
            using (IServiceScope scope = scopeFactory.CreateScope())
            {
                CacheContext _context = scope.ServiceProvider.GetService<CacheContext>();
                List<Qcount> qs = _context.qcounts.ToList();
                qs[id - 1].Endpoints = Endpoints;
                _context.Entry(qs[id - 1]).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
                queues[id - 1].Endpoints = Endpoints;
            }
        }

        public void DeleteQueue(int id)
        {
            queues[id-1].Deleting = true;
            using (IServiceScope scope = scopeFactory.CreateScope())
            {
                CacheContext _context = scope.ServiceProvider.GetService<CacheContext>();
                List<Qcount> qs = _context.qcounts.ToList();
                Qcount qc = qs[id - 1];
                qc.Deleting = true;
                _context.Entry(qc).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
                if (queues[id - 1].Count > 0) return;
                _context.qcounts.Remove(qs[id - 1]);
                _context.SaveChanges();
                queues.Remove(queues[id - 1]);
            }
        }

        public void AddResponse(string id, object obj) 
        { 
            responses.TryAdd(id, obj);
            queued.Remove(id, out _);
        }

        public object Get(string id, bool delete) 
        { 
            object obj = responses[id]; 
            if (delete) responses.Remove(id, out _);
            if (responses.Count <= 0) responses = new(); 
            return obj; 
        }

        public string? IsQueued(string id) => queued.ContainsKey(id) ? id : null;

        public int Count() 
        { 
            int c = 0; 
            foreach (var q in queues) c += q.Count; 
            return c;
        }

        public CustomQueue Dequeue(int take, int q)
        {
            CustomQueue deq = [];
            while (take > 0)
            {
                QueuedFunction deqr;
                bool taken;
                if (q == 0)
                    taken = Emptier().TryDequeue(out deqr);
                else
                    taken = queues[q - 1].TryDequeue(out deqr);
                if (taken) deq.Enqueue(deqr);
                else break;
                take--;
            }
            return deq;
        }

        public CustomQueue? Emptier(int function = -1)
        {
            if (queues.Count == 0) return null;
            CustomQueue? res = null;
            int? c = null;
            foreach (var q in queues)
                if (c == null || q.Count < c)
                    if ((function == -1 || q.Endpoints.Contains(-1) || q.Endpoints.Contains(function)) && !q.Deleting)
                    {
                        res = q;
                        c = q.Count;
                    }
            return res;
        }

        public ConcurrentQueue<QueuedFunction>? Fuller()
        {
            if (queues.Count == 0) return null;
            ConcurrentQueue<QueuedFunction> res = queues[0];
            int c = queues[0].Count;
            foreach (var q in queues) if (q.Count > c)
                {
                    res = q;
                    c = q.Count;
                }
            return res;
        }

        public object Request(Function function, List<string> itemIds, string body, string hashtarget, bool delete = false, string callback = "")
        {
            string tranid = Util.Hash(hashtarget);
            QueuedFunction qf = new QueuedFunction()
            { Queue = queues.IndexOf(Emptier((int)function)), Function = function, ItemIds = itemIds, Hash = tranid, Body = body, Callback = callback };
            string dn = function.GetDisplayName();
            thingsreceived.AddOrUpdate(dn, 1, (key, oldValue) => oldValue + 1);


            if (!isasync)
                using (IServiceScope scope = scopeFactory.CreateScope())
                    return qf.Execute(scope.ServiceProvider.GetService<Context>());


            if (IsQueued(tranid) != null) return new { Status = "PENDING", Token = tranid };

            try 
            {
                object r = getTranStatus(tranid);
                Get(tranid, delete);
                return r;
            } catch { }
            if (qf.Queue == -1) return new { Status = "ERROR", Result = "No queue available for this function."};
            if (queues.Count == 0) return new { Status = "ERROR", Result = "No queues available." };
            Add(qf);
            return new { Status = "PENDING", Token = tranid }; ;
        }

        public List<CustomQueue> GetQueues() => queues;
        public CustomQueue GetQueue(int id) => queues[id-1];
        public object getTranStatus(string id)
        {
            try 
            {
                dynamic r = responses[id];
                int? statusCode = GetStatusCode(r);
                if (statusCode == 200) return new { Status = "ACCEPTED", Result = r.Value };
                switch (statusCode)
                {
                    case 404: return new { Status = "REJECTED", Result = "404 Not Found" };
                    case 400: return new { Status = "REJECTED", Result = "400 Bad Request" };
                    case 500: return new { Status = "REJECTED", Result = "500 Internal server error" };
                }
                return new { Status = "REJECTED", Result = $"{statusCode} Unknown error" };
            } catch { }
            bool isInQueue = false;
            QueuedFunction? tmp = null;
            foreach (var q in queues)
            {
                tmp = q.ToList().FirstOrDefault(_ => _.Hash == id);
                if (tmp != null)
                {
                    isInQueue = true;
                    break;
                }
            }
            if (isInQueue && tmp != null) return new { Status = "PENDING", Item = "Transaction is being queued."};
            if (queued.ContainsKey(id)) return new { Status = "PENDING", Item = "Transaction is being processed." };
            return new { Status = "NONEXISTANT", Item = "Transaction doesn't exist or it has already been retrieved." };
        }

        private static int? GetStatusCode(ActionResult<object> actionResult)
        {
            IConvertToActionResult convertToActionResult = (IConvertToActionResult)actionResult.Value; // ActionResult implicit implements IConvertToActionResult
            var actionResultWithStatusCode = convertToActionResult.Convert() as IStatusCodeActionResult;
            return actionResultWithStatusCode?.StatusCode;
        }
    }
}
