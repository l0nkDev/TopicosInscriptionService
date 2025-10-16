using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using TopicosP1Backend.Exceptions;
using TopicosP1Backend.Scripts;

namespace CareerApi.Models
{
    public class Inscription
    {
        public long Id { get; set; }
        public IEnumerable<GroupInscription> GroupInscriptions { get; set; } = new List<GroupInscription>();
        public IEnumerable<Group> Groups { get; set; } = new List<Group>();
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
            public List<Group.GroupDTO> Groups { get; set; } = (from i in ins.Groups select i.Simple()).ToList();
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

        public static async Task<ActionResult<object>> PostInscription(Context _context, InscriptionPost i)
        {
            try 
            { 
                List<string> errors = [];
                var student = await _context.Students.IgnoreAutoIncludes().FirstOrDefaultAsync(_ => _.Id == i.Student);
                var period = await _context.Periods.FirstOrDefaultAsync(_ => _.Number == i.Period && _.Gestion.Year == i.Gestion);
                if (student == null || period == null) return new NotFoundResult();
                if (i.GroupIds.Count <= 0) return new BadRequestResult();
                Inscription n = new() { Student = student, Period = period, DateTime = DateTime.Now, Type = i.Type };
                bool added = false;
                foreach (var groupid in i.GroupIds)
                {
                    Group? group = await _context.Groups.FindAsync(groupid);
                    if (group == null)
                        return new NotFoundResult();
                    if (group.Quota <= 0)
                    {
                        errors.Add($"Grupo {group.Subject.Code}-{group.Code} ({group.Id}) no tiene cupos.");
                        Console.WriteLine($"Grupo {group.Subject.Code}-{group.Code} ({group.Id}) no tiene cupos.");
                        continue;
                    }
                    group.Quota--;
                    _context.Entry(group).State = EntityState.Modified;
                    bool saved = false;
                    while (!saved)
                    {
                        try
                        {
                            await _context.SaveChangesAsync();
                            saved = true;
                            Console.WriteLine($"Grupo {group.Subject.Code}-{group.Code} ({group.Id}) inscrito. Quedan {group.Quota} cupos.");
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
                            if (group.Quota <= 0)
                            {
                                Console.WriteLine($"Grupo {group.Subject.Code}-{group.Code} ({group.Id}) no tiene cupos.");
                                errors.Add($"Grupo {group.Subject.Code}-{group.Code} ({group.Id}) no tiene cupos.");
                                continue;
                            }
                            group.Quota--;
                            _context.Entry(group).State = EntityState.Modified;
                        }
                    }
                    if (!saved) continue;
                    if (!added) _context.Inscriptions.Add(n);
                    added = true;
                    GroupInscription ngi = new() { Group = group, Inscription = n };
                    _context.GroupInscriptions.Add(ngi);
                    StudentGroups? sg = await _context.StudentGroups.FirstOrDefaultAsync(_ => _.Student == student && _.Group == group);
                    if (sg == null)
                    {
                        if (i.Type != 2)
                        {
                            sg = new StudentGroups() { Group = group, Student = student };
                            _context.StudentGroups.Add(sg);
                        }
                    }
                    else
                    {
                        if (i.Type == 2) sg.Status = 2;
                        _context.Entry(sg).State = EntityState.Modified;
                    }
                }
                await _context.SaveChangesAsync();
                Console.WriteLine($"Inscription for student {student.Id} successful.");
                return new { Errors = errors, Result = n.Simple() };
            } catch(NoGroupsException ex) {
                Console.WriteLine(ex);
                return new BadRequestObjectResult(new { Error = ex.Message });
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
