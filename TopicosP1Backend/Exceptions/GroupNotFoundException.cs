namespace TopicosP1Backend.Exceptions
{
    public class GroupNotFoundException: Exception
    {
        public GroupNotFoundException() : base("No group with the given Id exists.") { }
    }
}
