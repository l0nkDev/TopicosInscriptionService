namespace CareerApi.Models
{
    public class TimeSlot
    {
        public long Id { get; set; }
        required public string Day { get; set; }
        required public TimeOnly StartTime { get; set; }
        required public TimeOnly EndTime { get; set; }
        required public Room Room { get; set; }
        required public Group Group { get; set; }
        public TimeSlotDTO Simple() => new(this);

        public class TimeSlotDTO(TimeSlot ts)
        {
            public long Id { get; set; } = ts.Id;
            public string Day { get; set; } = ts.Day;
            public TimeOnly StartTime { get; set; } = ts.StartTime;
            public TimeOnly EndTime { get; set; } = ts.EndTime;
        }

        public class TimeSlotPost
        {
            required public long Id { get; set; }
            required public string Day { get; set; }
            required public TimeOnly StartTime { get; set; }
            required public TimeOnly EndTime { get; set; }
            required public long Module { get; set; }
            required public long Room { get; set; }

        }

    }
}
