namespace TopicosP1Backend.Exceptions
{
    public class PeriodNotFoundException: Exception
    {
        public PeriodNotFoundException(long period, long gestion) : base($"No period {period}-{gestion} exists.") { }
    }
}
