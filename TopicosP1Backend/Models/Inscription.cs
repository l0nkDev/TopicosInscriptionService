using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using TopicosP1Backend.Exceptions;
using TopicosP1Backend.Scripts;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CareerApi.Models
{
    public class Inscription
    {
        public long Id { get; set; }
        public List<GroupInscription> GroupInscriptions { get; set; } = [];
        public List<Group> Groups { get; set; } = [];
        required public Student Student { get; set; }
        required public Period Period { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        required public int Type { get; set; } //0 = Inscripcion, 1 = Adicion, 2 = Retiro
        public InscriptionDTO Simple() => new(this);
        public class InscriptionDTO(Inscription ins)
        {
            public long Id { get; set; } = ins.Id;
            public Student.StudentDTO Student { get; set; } = ins.Student.Simple();
            public Period.PeriodDTO Period { get; set; } = ins.Period.Simple();
            public DateTime DateTime { get; set; } = ins.DateTime;
            public int Type { get; set; } = ins.Type;
            public List<Group.GroupDTO> Groups { get; set; } = [.. from i in ins.Groups select i.Simple()];
        }

        public class InscriptionPost
        {
            public long Id { get; set; }
            public long Student { get; set; }
            public long Period { get; set; }
            public long Gestion { get; set; }
            public int Type { get; set; }
            public List<long> GroupIds { get; set; } = [];
        }

        public class GIPost
        {
            required public long Group { get; set; }
        }

        public static async Task<ActionResult<IEnumerable<InscriptionDTO>>> GetInscriptions(Context _context)
        {
            List<Inscription> l = await _context.Inscriptions.IgnoreAutoIncludes().Include(_ => _.Student).Include(_ => _.Period).ThenInclude(_ => _.Gestion).Include(_ => _.Groups).ThenInclude(_ => _.Subject)
                .Include(_ => _.Student).Include(_ => _.Period).ThenInclude(_ => _.Gestion).Include(_ => _.Groups).ThenInclude(_ => _.Teacher)
                .ToListAsync();
            return (from i in l select i.Simple()).ToList();
        }

        public static async Task<ActionResult<InscriptionDTO>> GetInscription(Context _context, long id)
        {
            var inscription = await _context.Inscriptions.IgnoreAutoIncludes().Include(_ => _.Student).Include(_ => _.Period).ThenInclude(_ => _.Gestion).Include(_ => _.Groups).ThenInclude(_ => _.Subject)
                .Include(_ => _.Student).Include(_ => _.Period).ThenInclude(_ => _.Gestion).Include(_ => _.Groups).ThenInclude(_ => _.Teacher)
                .Include(_ => _.Student).Include(_ => _.Period).ThenInclude(_ => _.Gestion).Include(_ => _.Groups).ThenInclude(_ => _.Period).ThenInclude(_ => _.Gestion)
                .FirstOrDefaultAsync(_ => _.Id == id);

            if (inscription == null)
            {
                return new NotFoundResult();
            }

            return inscription.Simple();
        }

        public static void TimeslotConflictCheck(List<Group> groups)
        {
            List<TimeSlot> timeSlots = [];
            foreach (Group g in groups)
            {
                foreach (TimeSlot slot in g.TimeSlots)
                {
                    foreach (TimeSlot inlist in timeSlots)
                    {
                        if (slot.Day == inlist.Day && (
                   
                            (slot.StartTime >= inlist.StartTime && slot.StartTime < inlist.EndTime) ||
                            (slot.EndTime > inlist.StartTime && slot.EndTime <= inlist.EndTime)))
                        {
                            throw new TimeslotConflictException(g, inlist.Group);
                        }
                    }
                    timeSlots.Add(slot);
                }
            }
        }

        public static async void UndoQuotaAdditions(Context _context, List<Group> successes)
        {
            foreach (Group group in successes)
            {
                group.Quota++;
                _context.Entry(group).State = EntityState.Modified;
                bool saved = false;
                while (!saved)
                {
                    try
                    {
                        await _context.SaveChangesAsync();
                        saved = true;
                    }
                    catch (DbUpdateConcurrencyException e)
                    {
                        foreach (var entry in e.Entries)
                            if (entry.Entity is Group)
                            {
                                var dbValues = await entry.GetDatabaseValuesAsync();
                                entry.OriginalValues.SetValues(dbValues);
                                entry.CurrentValues.SetValues(dbValues);
                            }
                        group.Quota++;
                        _context.Entry(group).State = EntityState.Modified;
                    }
                }
            }
        }

        public static bool hasQuota(Group group)
        {
            return (group.Quota > 0);
        }

        public static async Task<bool> TryQuotaAddition(Context _context, Group group, List<Group> successes, List<string> errors)
        {
            bool allowPartialInscription = false;
            if (!hasQuota(group))
            {
                errors.Add($"Grupo {group.Subject.Code}-{group.Code} ({group.Id}) no tiene cupos.");
                Console.WriteLine($"\nGrupo {group.Subject.Code}-{group.Code} ({group.Id}) no tiene cupos.");
                if (!allowPartialInscription) throw new NoQuotaLeftException($"{group.Subject.Code}-{group.Code} ({group.Id})");
                return false;
            }
            group.Quota--;
            _context.Entry(group).State = EntityState.Modified;
            while (true)
            {
                try
                {
                    await _context.SaveChangesAsync();
                    successes.Add(group);
                    Console.WriteLine($"\nGrupo {group.Subject.Code}-{group.Code} ({group.Id}) inscrito. Quedan {group.Quota} cupos.");
                    return true;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    foreach (var entry in ex.Entries)
                        if (entry.Entity is Group)
                        {
                            var dbValues = await entry.GetDatabaseValuesAsync();
                            entry.OriginalValues.SetValues(dbValues);
                            entry.CurrentValues.SetValues(dbValues);
                        }
                    if (!hasQuota(group))
                    {
                        Console.WriteLine($"\nGrupo {group.Subject.Code}-{group.Code} ({group.Id}) no tiene cupos.");
                        errors.Add($"Grupo {group.Subject.Code}-{group.Code} ({group.Id}) no tiene cupos.");
                        if (!allowPartialInscription) throw new NoQuotaLeftException($"{group.Subject.Code}-{group.Code} ({group.Id})");
                        return false;
                    }
                    group.Quota--;
                    _context.Entry(group).State = EntityState.Modified;
                }
            }
        }

        public static async Task<bool> RegisterStudentToGroup(Context _context, Student student, Inscription inscription, Group group, int type)
        {
            GroupInscription ngi = new() { Group = group, Inscription = inscription };
            _context.GroupInscriptions.Add(ngi);
            StudentGroups? sg = await _context.StudentGroups.FirstOrDefaultAsync(_ => _.Student == student && _.Group == group);
            if (sg == null)
            {
                if (type != 2)
                {
                    sg = new StudentGroups() { Group = group, Student = student };
                    _context.StudentGroups.Add(sg);
                }
            }
            else
            {
                if (type == 2) sg.Status = 2;
                _context.Entry(sg).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public static async Task<ActionResult<object>> PostInscription(Context _context, InscriptionPost i)
        {
            List<string> errors = [];
            List<Group> groups = [];
            List<Group> successes = [];
            try
            {
                var student = await _context.Students.IgnoreAutoIncludes().FirstOrDefaultAsync(_ => _.Id == i.Student) ?? throw new StudentNotFoundException(i.Student);
                var period = await _context.Periods.FirstOrDefaultAsync(_ => _.Number == i.Period && _.Gestion.Year == i.Gestion) ?? throw new PeriodNotFoundException(i.Period, i.Gestion);
                if (i.GroupIds.Count <= 0) throw new NoGroupsException();
                Inscription n = new() { Student = student, Period = period, DateTime = DateTime.Now, Type = i.Type };
                bool added = false;
                foreach (var groupid in i.GroupIds)
                {
                    Group? group = await _context.Groups.FindAsync(groupid) ?? throw new GroupNotFoundException();
                    groups.Add(group);
                }
                TimeslotConflictCheck(groups);
                foreach (Group group in groups)
                {
                    if (!await TryQuotaAddition(_context, group, successes, errors)) continue;
                    if (!added)
                    {
                        _context.Inscriptions.Add(n);
                        added = true;
                    }
                    await RegisterStudentToGroup(_context, student, n, group, i.Type);
                }
                if (successes.Count <= 0)
                {
                    throw new NoGroupsException();
                }
                Console.WriteLine($"\nInscription for student {student.Id} successful.");
                return new { Errors = errors, Result = n.Simple() };
            }
            catch (NoGroupsException ex)
            {
                Console.WriteLine(ex);
                return new BadRequestObjectResult(new { Error = ex.Message });
            }
            catch (StudentNotFoundException ex)
            {
                Console.WriteLine(ex);
                return new NotFoundObjectResult(new { Error = ex.Message });
            }
            catch (PeriodNotFoundException ex)
            {
                Console.WriteLine(ex);
                return new NotFoundObjectResult(new { Error = ex.Message });
            }
            catch (GroupNotFoundException ex)
            {
                Console.WriteLine(ex);
                return new NotFoundObjectResult(new { Error = ex.Message });
            }
            catch (TimeslotConflictException ex)
            {
                Console.WriteLine(ex);
                return new ConflictObjectResult(new { Error = ex.Message });
            }
            catch (NoQuotaLeftException ex)
            {
                UndoQuotaAdditions(_context, successes);
                Console.WriteLine(ex);
                return new ConflictObjectResult(new { Error = ex.Message });
            }
        }

        public static async Task<IActionResult> DeleteInscription(Context _context, long id)
        {
            var inscription = await _context.Inscriptions.FindAsync(id);
            if (inscription == null)
            {
                return new NotFoundResult();
            }

            _context.Inscriptions.Remove(inscription);
            await _context.SaveChangesAsync();

            return new NoContentResult();
        }
    }
}
