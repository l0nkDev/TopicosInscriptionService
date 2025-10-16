using TopicosP1Backend.Cache;

namespace TopicosP1Backend.Scripts
{
    public class wcount
    {
        public long Id { get; set; }
        required public int Queue { get; set; }
        required public int TakeCount { get; set; }
    }

    public class WorkerManager: IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly APIQueue _queue;
        List<QueueWorker> _workers = [];
        public WorkerManager(IServiceScopeFactory scope, APIQueue queue)
        {
            _queue = queue;
            _scopeFactory = scope;
            CacheContext c = scope.CreateScope().ServiceProvider.GetService<CacheContext>();
            List<wcount> wcs = c.wcounts.ToList();
            foreach (wcount w in wcs) _workers.Add(new(_scopeFactory, _queue) { assignedqueue = w.Queue, take = w.TakeCount });
            if (_workers.Count == 0) AddWorker(1);
        }
        public Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;
        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        public object GetWorkers() => (from i in _workers select new { Id = _workers.IndexOf(i)+1, Queue = i.assignedqueue, Take = i.take, i.Status });
        public object GetWorker(int id) => new { Id = id, Queue = _workers[id-1].assignedqueue, Take = _workers[id-1].take, _workers[id-1].Status };
        public void SetWorkerQueue(int id, int q)
        {
            using (IServiceScope scope = _scopeFactory.CreateScope())
            {
                CacheContext c = scope.ServiceProvider.GetService<CacheContext>();
                List<wcount> wcs = c.wcounts.ToList();
                wcount? wc = wcs.ElementAtOrDefault(id - 1);
                if (wc != null) { wc.Queue = q; c.SaveChanges(); }
            }
            _workers[id - 1].assignedqueue = q;
        }
        public void SetWorkerTake(int id, int take)
        {
            using (IServiceScope scope = _scopeFactory.CreateScope())
            {
                CacheContext c = scope.ServiceProvider.GetService<CacheContext>();
                List<wcount> wcs = c.wcounts.ToList();
                wcount? wc = wcs.ElementAtOrDefault(id - 1);
                if (wc != null) { wc.TakeCount = take; c.SaveChanges(); }
            }
            _workers[id - 1].take = take;
        }

        public void AddWorker(int q)
        {
            using (IServiceScope scope = _scopeFactory.CreateScope())
            {
                CacheContext c = scope.ServiceProvider.GetService<CacheContext>();
                wcount wc = new() { Queue = q, TakeCount = 1 };
                c.wcounts.Add(wc);
                c.SaveChanges();
                _workers.Add(new(_scopeFactory, _queue) { assignedqueue = q });
            }
        }

        public void RemoveWorker(int n)
        {
            using (IServiceScope scope = _scopeFactory.CreateScope())
            {
                CacheContext c = scope.ServiceProvider.GetService<CacheContext>();
                List<wcount> wcs = c.wcounts.ToList();
                wcount? wc = wcs.ElementAtOrDefault(n-1);
                if (wc != null) c.wcounts.Remove(wc);
                c.SaveChanges();
                _workers[n-1].Stop();
                _workers.RemoveAt(n-1);
            }
        }

        public void Stop(int n) => _workers[n].Stop();
        public void Start(int n) => _workers[n].Start();
    }
}
