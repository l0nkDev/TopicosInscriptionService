namespace TopicosP1Backend.Exceptions
{
    public class NoQuotaLeftException: Exception
    {
        public NoQuotaLeftException(string code): base($"Group {code} has no room for more students and partial inscriptions are disabled.") { }
    }
}
