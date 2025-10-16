using CareerApi.Models;

namespace TopicosP1Backend.Exceptions
{
    public class TimeslotConflictException: Exception
    {
        public TimeslotConflictException(Group g1, Group g2) : base($"The groups {g1.Subject.Code}-{g1.Code} and {g2.Subject.Code}-{g2.Code} selected have conflicting timeslots.") { }
    }
}
