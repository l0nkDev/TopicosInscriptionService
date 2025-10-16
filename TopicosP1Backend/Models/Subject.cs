using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TopicosP1Backend.Scripts;

namespace CareerApi.Models
{
    public class Subject
    {
        [Key]
        required public string Code { get; set; }
        required public string Title { get; set; }
        public IEnumerable<SpSubject> SpSubjects { get; set; } = new List<SpSubject>();
        public IEnumerable<StudyPlan> StudyPlans{ get; set; } = new List<StudyPlan>();
        public IEnumerable<SubjectDependency> PreDependencies { get; set; } = new List<SubjectDependency>();
        public IEnumerable<Subject> Prerequisites { get; set; } = new List<Subject>();
        public IEnumerable<SubjectDependency> PostDependencies { get; set; } = new List<SubjectDependency>();
        public IEnumerable<Subject> Postrequisites { get; set; } = new List<Subject>();
        public IEnumerable<Group> Groups { get; set; } = new List<Group>();
        public SubjectDTO SimpleList() => new(this);
        public SubjectSimple Simple() => new(this);
        public SubjectWithGroups WithGroups() => new(this);
        public class SubjectDTO(Subject subject)
        {
            public string Code { get; set; } = subject.Code;
            public string Title { get; set; } = subject.Title;
            public IEnumerable<SubjectSimple> Prerequisites { get; set; } = from a in subject.PreDependencies select a.Prerequisite.Simple();
            public IEnumerable<SubjectSimple> Postrequisites { get; set; } = from a in subject.PostDependencies select a.Prerequisite.Simple();
        }

        public class SubjectSimple
        {
            public string Code { get; set; }
            public string Title { get; set; }
            public SubjectSimple(Subject subject)
            {
                Code = subject.Code;
                Title = subject.Title;
            }
        }

        public class PostSubject
        {
            public string Code { get; set; }
            public string Title { get; set; }
        }

        public class SubjectWithGroups(Subject subject)
        {
            public string Code { get; set; } = subject.Code;
            public string Title { get; set; } = subject.Title;
            public IEnumerable<Group.GroupDTO> Groups { get; set; } = from a in subject.Groups where a.Quota > 0 && a.Period.Id == 42 select a.Simple();

        }
        public static async Task<ActionResult<IEnumerable<SubjectSimple>>> GetAll(Context _context)
        {
            var l = await _context.Subjects.AsSplitQuery().ToListAsync();
            return (from a in l select a.Simple()).ToList();
        }

        // GET: api/Subjects/5
        [HttpGet("{id}")]
        public static async Task<ActionResult<SubjectSimple>> Get(Context _context, string id)
        {
            var subject = await _context.Subjects.AsSplitQuery().FirstOrDefaultAsync(_=>_.Code == id);
            if (subject == null) return new NotFoundResult();
            return subject.Simple();
        }

        public static async Task<ActionResult<Subject>> Post(Context _context, PostSubject s)
        {
            if (await _context.Subjects.FindAsync(s.Code) != null) return new BadRequestResult();
            Subject subject = new Subject() { Code = s.Code, Title = s.Title };
            _context.Subjects.Add(subject);
            try { await _context.SaveChangesAsync(); }
            catch { return new ConflictResult(); }
            return subject;
        }
        public static async Task<IActionResult> Delete(Context _context, string id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null) return new NotFoundResult();
            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync();
            return new NoContentResult();
        }
        public static async Task<IActionResult> Put(Context _context, string id, PostSubject s)
        {
            if (id != s.Code) return new BadRequestResult();
            Subject subject = await _context.Subjects.FindAsync(id);
            if (subject == null) return new NotFoundResult();
            subject.Title = s.Title;
            _context.Entry(subject).State = EntityState.Modified;

            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Subjects.Any(e => e.Code == id)) return new NotFoundResult();
                else throw;
            }
            return new NoContentResult();
        }
    }
}
