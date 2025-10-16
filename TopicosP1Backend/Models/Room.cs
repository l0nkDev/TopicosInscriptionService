using System.Diagnostics.CodeAnalysis;

namespace CareerApi.Models
{
    public class Room
    {
        public long Id { get; set; }
        required public Module Module { get; set; }
        required public long Number { get; set; }
        public RoomDTO Simple() => new(this);

        public class RoomDTO(Room room)
        {
            public long Id { get; set; } = room.Id;
            public long Number { get; set; } = room.Number;
        }
    }
}
