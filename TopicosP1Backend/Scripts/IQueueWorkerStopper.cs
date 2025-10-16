namespace TopicosP1Backend.Scripts
{
    public interface IQueueWorkerStopper
    {
        void StopAsync();
        void StartAsync();
    }
}
