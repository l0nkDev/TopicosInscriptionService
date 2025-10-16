using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using TopicosP1Backend.Scripts;

namespace CareerApi.Models
{
    public class Group
    {
        public long Id { get; set; }
        required public string Code { get; set; }
        required public string Mode { get; set; }
        required public Period Period { get; set; }
        required public Subject Subject { get; set; }
        required public Teacher Teacher { get; set; }
        [ConcurrencyCheck]
        public long Quota { get; set; }
        public IEnumerable<TimeSlot> TimeSlots { get; set; } = new List<TimeSlot>();
        public IEnumerable<GroupInscription> GroupInscriptions { get; set; } = new List<GroupInscription>();
        public IEnumerable<Inscription> Inscriptions { get; set; } = new List<Inscription>();
        public IEnumerable<StudentGroups> StudentGroups { get; set; } = new List<StudentGroups>();
        public IEnumerable<Student> Students { get; set; } = new List<Student>();

        public GroupDTO Simple() => new(this);

        public class GroupDTO(Group groupi)
        {
            public long Id { get; set; } = groupi.Id;
            public Subject.SubjectSimple Subject { get; set; } = groupi.Subject.Simple();
            public string Code { get; set; } = groupi.Code;
            public string Mode { get; set; } = groupi.Mode;
            public long Quota { get; set; } = groupi.Quota;
            public Period.PeriodDTO Period { get; set; } = groupi.Period.Simple();
            public Teacher.TeacherDTO Teacher { get; set; } = groupi.Teacher.Simple();
            public List<TimeSlot.TimeSlotDTO> Timeslots { get; set; } = (from g in (groupi.TimeSlots) select g.Simple()).ToList();
        }
        public class GroupPost
        {
            required public long Id { get; set; }
            required public string Subject { get; set; }
            required public string Code { get; set; }
            required public string Mode { get; set; }
            required public long Quota { get; set; }
            required public long Period { get; set; }
            required public long Gestion { get; set; }
            required public long Teacher { get; set; }
        }
        public static async Task<ActionResult<IEnumerable<GroupDTO>>> GetGroups(Context _context)
        {
            List<Group> l = await _context.Groups.AsSplitQuery().ToListAsync();
            return (from i in l select i.Simple()).ToList();
        }

        public static async Task<ActionResult<GroupDTO>> GetGroup(Context _context, long id)
        {
            var @group = await _context.Groups.AsSplitQuery().FirstOrDefaultAsync(_=>_.Id == id);

            if (@group == null)
            {
                return new NotFoundResult();
            }

            return @group.Simple();
        }

        public static async Task<ActionResult<GroupDTO>> PutGroup(Context _context, long id, GroupPost g)
        {
            Group group = await _context.Groups.FindAsync(id);
            Period period = await _context.Periods.FirstOrDefaultAsync(_ => _.Number == g.Period && _.Gestion.Year == g.Gestion);
            Teacher teacher = await _context.Teachers.FindAsync(g.Teacher);
            Subject subject = await _context.Subjects.FindAsync(g.Subject);
            if (id != g.Id) return new BadRequestResult();
            if (group == null || period == null || teacher == null || subject == null) return new NotFoundResult();
            group.Period = period;
            group.Code = g.Code;
            group.Mode = g.Mode;
            group.Subject = subject;
            group.Teacher = teacher;
            group.Quota = g.Quota;
            _context.Entry(group).State = EntityState.Modified;
            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            { if (!_context.Groups.Any(e => e.Id == id)) return new NotFoundResult(); else throw; }
            return group.Simple();
        }

        public static async Task<ActionResult<GroupDTO>> PostGroup(Context _context, GroupPost g)
        {
            Group @group = new()
            {
                Period = await _context.Periods.FirstOrDefaultAsync(_ => _.Number == g.Period && _.Gestion.Year == g.Gestion),
                Code = g.Code,
                Mode = g.Mode,
                Subject = await _context.Subjects.FindAsync(g.Subject),
                Teacher = await _context.Teachers.FindAsync(g.Teacher),
                Quota = g.Quota,
            };
            _context.Groups.Add(@group);
            await _context.SaveChangesAsync();

            return @group.Simple();
        }

        public static async Task<IActionResult> DeleteGroup(Context _context, long id)
        {
            var @group = await _context.Groups.AsSplitQuery().FirstOrDefaultAsync(_=>_.Id == id);
            if (@group == null)
            {
                return new NotFoundResult();
            }

            _context.Groups.Remove(@group);
            await _context.SaveChangesAsync();

            return new NoContentResult();
        }

        public static async Task<ActionResult<List<TimeSlot.TimeSlotDTO>>> GetTimeSlots(Context _context, long id)
        {
            Group g = await _context.Groups.AsSplitQuery().Include(_=>_.TimeSlots).ThenInclude(_ => _.Room).ThenInclude(_ => _.Module).FirstOrDefaultAsync(_=>_.Id == id);
            return (from i in g.TimeSlots select i.Simple()).ToList();
        }

        public static async Task<ActionResult<TimeSlot.TimeSlotDTO>> PostTimeSlot(Context _context, long id, TimeSlot.TimeSlotPost body)
        {
            Room room = await _context.Rooms.Include(_=>_.Module).FirstOrDefaultAsync(_ => _.Number == body.Room && _.Module.Number == body.Module);
            Group group = await _context.Groups.FindAsync(id);
            if (room == null || group == null) return new NotFoundResult();
            TimeSlot g = new()
            {
                Day = body.Day,
                StartTime = body.StartTime,
                EndTime = body.EndTime,
                Room = room,
                Group = group
            };
            _context.TimeSlots.Add(g);
            _context.SaveChangesAsync();
            return g.Simple();
        }

        public static async Task<ActionResult<TimeSlot.TimeSlotDTO>> PutTimeSlot(Context _context, long id, long ts, TimeSlot.TimeSlotPost body)
        {
            TimeSlot timeslot = await _context.TimeSlots.FindAsync(ts);
            Room room = await _context.Rooms.Include(_=>_.Module).FirstOrDefaultAsync(_ => _.Number == body.Room && _.Module.Number == body.Module);
            Group group = await _context.Groups.FindAsync(id);
            if (room == null || group == null || timeslot == null) return new NotFoundResult();
            timeslot.Day = body.Day;
            timeslot.StartTime = body.StartTime;
            timeslot.EndTime = body.EndTime;
            timeslot.Room = room;
            timeslot.Group = group;
            _context.Entry(timeslot).State = EntityState.Modified;
            _context.SaveChangesAsync();
            return timeslot.Simple();
        }

        public static async Task<ActionResult<TimeSlot.TimeSlotDTO>> DeleteTimeSlot(Context _context, long id, long ts)
        {
            TimeSlot timeslot = await _context.TimeSlots.FindAsync(ts);
            if (timeslot == null) return new NotFoundResult();
            _context.TimeSlots.Remove(timeslot);
            _context.SaveChangesAsync();
            return new OkResult();
        }
    }
}
