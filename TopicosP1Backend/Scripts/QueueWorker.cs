using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using System.Collections.Concurrent;
using TopicosP1Backend.Cache;

namespace TopicosP1Backend.Scripts
{

    public class QueueWorker : BackgroundService
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly APIQueue _queue;
        public int assignedqueue = 0;
        public int take = 1;
        public ConcurrentQueue<QueuedFunction> taken = [];
        public string Status = "";
        private CancellationTokenSource cts = new();

        public QueueWorker(IServiceScopeFactory scopeFactory, APIQueue aPIQueue)
        {
            this.scopeFactory = scopeFactory;
            _queue = aPIQueue;
            Start();
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            bool usecache = _queue.isasync;
            await Task.Run(async () =>
            {
                IServiceScope? scope = null;
                Context? context = null;
                CacheContext? cache = null;
                while (!stoppingToken.IsCancellationRequested)
                {
                    if (_queue.Count() != 0 && (taken = _queue.Dequeue(take, assignedqueue)).Count > 0)
                    {
                        while (taken.Count > 0 && taken.TryDequeue(out QueuedFunction a))
                        {
                            Console.WriteLine($"Worker {this.GetHashCode()}: Running {a.Function.GetDisplayName()}; {string.Join(", ", a.ItemIds)}; {a.Body}.");
                            Status = "Running";
                            if (scope == null)
                            {
                                scope = scopeFactory.CreateScope();
                                context = scope.ServiceProvider.GetService<Context>();
                                if (usecache) cache = scope.ServiceProvider.GetService<CacheContext>();
                            }
                            QueuedFunction.DBItem? exiting = null;
                            if (usecache) exiting = await cache.QueuedFunctions.FirstOrDefaultAsync(_ => _.Hash == a.Hash);
                            if (exiting != null) cache.QueuedFunctions.Remove(exiting);
                            if (usecache) cache.SaveChanges();
                            object res = null;
                            try { res = await a.Execute(context); }
                            catch
                            {
                                Console.WriteLine($"Worker {this.GetHashCode()}: Catastrophic failure in executing task.");
                                int i = _queue.queues.IndexOf(_queue.Emptier((int)a.Function));
                                a.Queue = i;
                                _queue.Add(a);
                            }
                            if (res != null)
                            {
                                Console.WriteLine($"Worker {this.GetHashCode()}: Task completed.");
                                _queue.AddResponse(a.Hash, res);
                                string dn = a.Function.GetDisplayName();
                                if (a.Callback != "") Console.WriteLine(a.Callback);
                                _queue.thingsdone.AddOrUpdate(dn, 1, (key, oldValue) => oldValue + 1);
                                if (exiting != null && _queue.queues[exiting.Queue].Deleting && _queue.queues[exiting.Queue].Count <= 0) _queue.DeleteQueue(exiting.Queue+1);
                            }
                            await Task.Yield();
                        }
                    }
                    else { scope?.Dispose(); scope = null; Status = "Idle"; /*Console.WriteLine($"Idling Worker {this.GetHashCode()}")*/; await Task.Delay(0); }
                }
                scope?.Dispose(); scope = null; await Task.Yield();
            });
        }
        public async void Stop()
        {
            if (cts == null) return; cts.Cancel(); await StopAsync(cts.Token); cts.Dispose();  cts = null; Status = "Stopped";
        }
        public async void Start()
        {
            cts = new(); await StartAsync(cts.Token);
        }
    }
}
