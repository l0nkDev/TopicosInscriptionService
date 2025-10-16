namespace TopicosP1Backend.Exceptions
{
    public class StudentNotFoundException: Exception
    {
        public StudentNotFoundException(long id) : base($"No student with the Id: {id} exists.") { }
    }
}
