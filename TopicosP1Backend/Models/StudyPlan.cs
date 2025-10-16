using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System.ComponentModel.DataAnnotations;
using TopicosP1Backend.Scripts;

namespace CareerApi.Models
{
    public class StudyPlan
    {
        [Key]
        required public string Code { get; set; }
        public Career Career { get; set; } = null!;
        public IEnumerable<SpSubject> SpSubjects { get; set; } = new List<SpSubject>();
        public IEnumerable<Subject> Subjects { get; set; } = new List<Subject>();
        public IEnumerable<Student> Students { get; set; } = new List<Student>();
        public StudyPlanDTO Simple() => new(this);

        public class StudyPlanDTO(StudyPlan studyPlan)
        {
            public string Code { get; set; } = studyPlan.Code;
            public string Career { get; set; } = studyPlan.Career.Name;
            public IEnumerable<SpSubject.SpSubjectDTO> Subjects { get; set; } = 
                from a in studyPlan.SpSubjects select a.SimpleList();
        }

        public class StudyPlanPost
        {
            required public string Code { get; set; }
            required public long Career { get; set; }
        }

        public class StudyPlanSubjectPost
        {
            public long Id { get; set; }
            required public string StudyPlan { get; set; }
            required public string Subject { get; set; }
            required public int Credits { get; set; }
            required public int Level { get; set; }
            required public int Type { get; set; }
        }
        public class SPSDependency
        {
            required public string Code { get; set; }
        }
        public static async Task<ActionResult<StudyPlanDTO>> GetStudyPlan(Context context, string id)
        {
            var studyPlan = await context.StudyPlans.AsSplitQuery().FirstOrDefaultAsync(i => i.Code == id);
            if (studyPlan == null) return new NotFoundResult();
            StudyPlanDTO res = new(studyPlan);
            return res;
        }
        public static async Task<IEnumerable<StudyPlanDTO>> GetStudyPlans(Context context)
        {
            var db = await context.StudyPlans.AsSplitQuery().ToListAsync();
            var studyplans = from sp in db select new StudyPlanDTO(sp);
            return studyplans;
        }
        public static async Task<IActionResult> PutStudyPlan(Context _context, string id, StudyPlanPost sp)
        {
            if (id != sp.Code) return new BadRequestResult();
            StudyPlan stp = await _context.StudyPlans.FindAsync(id);
            Career car = await _context.Careers.FindAsync(sp.Career);
            StudyPlan studyplan = stp;
            if (stp == null || car == null) return new NotFoundResult();
            studyplan.Career = car;
            _context.Entry(studyplan).State = EntityState.Modified;

            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.StudyPlans.Any(e => e.Code == id)) return new NotFoundResult();
                else throw;
            }
            return new NoContentResult();
        }

        public static async Task<ActionResult<StudyPlanDTO>> PostStudyPlan(Context _context, StudyPlanPost c)
        {
            if (await _context.StudyPlans.FindAsync(c.Code) != null) return new BadRequestResult();
            Career? carr = await _context.Careers.FindAsync(c.Career);
            if (carr == null) return new NotFoundResult();
            StudyPlan sp = new StudyPlan { Code = c.Code, Career = carr};
            _context.StudyPlans.Add(sp);
            await _context.SaveChangesAsync();
            return sp.Simple();
        }

        public static async Task<IActionResult> DeleteStudyPlan(Context _context, string id)
        {
            var sp = await _context.StudyPlans.IgnoreAutoIncludes().FirstOrDefaultAsync(_=>_.Code == id);
            if (sp == null) return new NotFoundResult();
            _context.StudyPlans.Remove(sp);
            await _context.SaveChangesAsync();
            return new NoContentResult();
        }
        public static async Task<ActionResult<List<SpSubject.SpSubjectDTO>>> GetSpSubject(Context context, string id)
        {
            var studyPlan = await context.StudyPlans.AsSplitQuery().FirstOrDefaultAsync(i => i.Code == id);
            if (studyPlan == null) return new NotFoundResult();
            var subs = studyPlan.SpSubjects;
            return (from i in subs select i.SimpleList()).ToList();
        }
        public static async Task<ActionResult<SpSubject.SpSubjectDTO>> PostSpSubject(Context context, string id, StudyPlanSubjectPost spsub)
        {
            StudyPlan studyPlan = await context.StudyPlans.AsSplitQuery().FirstOrDefaultAsync(i => i.Code == id);
            Subject subject = await context.Subjects.AsSplitQuery().FirstOrDefaultAsync(_=>_.Code == spsub.Subject);
            if (studyPlan == null || subject == null) return new NotFoundResult();
            var n = new SpSubject() { Credits = spsub.Credits, Level = spsub.Level, Type = spsub.Type, StudyPlan = studyPlan, Subject = subject };
            context.SpSubjects.Add(n);
            await context.SaveChangesAsync();
            return n.SimpleList();
        }
        public static async Task<ActionResult<SpSubject.SpSubjectDTO>> PutSpSubject(Context context, string id, string sub, StudyPlanSubjectPost spsub)
        {
            var sps = await context.SpSubjects.AsSplitQuery().IgnoreAutoIncludes().Include(_=>_.Subject).FirstOrDefaultAsync(i => i.StudyPlan.Code == id && i.Subject.Code == sub);
            if (sps == null) return new NotFoundResult();
            sps.Level = spsub.Level; sps.Credits = spsub.Credits; sps.Type = spsub.Type;
            context.Entry(sps).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return sps.SimpleList();
        }
        public static async Task<ActionResult<SpSubject.SpSubjectDTO>> DeleteSpSubject(Context context, string id, string sub)
        {
            var sps = await context.SpSubjects.FirstOrDefaultAsync(i => i.StudyPlan.Code == id && i.Subject.Code == sub);
            if (sps == null) return new NotFoundResult();
            context.SpSubjects.Remove(sps);
            await context.SaveChangesAsync();
            return new NoContentResult();
        }
        public static async Task<ActionResult<List<Subject.SubjectSimple>>> GetSpSubDependencies(Context context, string id, string sub)
        {
            var sps = await context.SpSubjects.AsSplitQuery().FirstOrDefaultAsync(i => i.StudyPlan.Code == id && i.Subject.Code == sub);
            if (sps == null) return new NotFoundResult();
            var lis = context.SubjectDependencies.Where(_ => _.StudyPlan.Code == id && _.Postrequisite.Code == sub);
            return (from i in lis select i.Prerequisite.Simple()).ToList();
        }
        public static async Task<ActionResult<Subject.SubjectSimple>> PostSpSubDependency(Context context, string id, string sub, SPSDependency dep)
        {
            var sps = await context.SpSubjects.FirstOrDefaultAsync(i => i.StudyPlan.Code == id && i.Subject.Code == sub);
            var subject = await context.Subjects.FindAsync(dep.Code);
            var sp = await context.StudyPlans.FindAsync(id);
            if (sps == null || subject == null) return new NotFoundResult();
            var pre = new SubjectDependency() { Prerequisite = subject, Postrequisite = sps.Subject, StudyPlan = sp };
            context.SubjectDependencies.Add(pre);
            await context.SaveChangesAsync();
            return subject.Simple();
        }
        public static async Task<ActionResult<Subject.SubjectSimple>> DeleteSpSubDependency(Context context, string id, string sub, string pre)
        {
            var entry = await context.SubjectDependencies.FirstOrDefaultAsync(i => i.StudyPlan.Code == id && i.Prerequisite.Code == pre && i.Postrequisite.Code == sub );
            if (entry == null) return new NotFoundResult();
            context.SubjectDependencies.Remove(entry);
            await context.SaveChangesAsync();
            return new NoContentResult();
        }
    }
}
