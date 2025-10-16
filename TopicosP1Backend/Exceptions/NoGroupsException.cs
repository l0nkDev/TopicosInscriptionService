namespace TopicosP1Backend.Exceptions
{
    public class NoGroupsException: Exception
    {
        public NoGroupsException() : base("No groups were provided for the inscription.") { }
    }
}
