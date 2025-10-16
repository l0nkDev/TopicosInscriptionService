using static CareerApi.Models.Subject;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace CareerApi.Models
{
    public class SpSubject
    {
        public long Id { get; set; }
        required public StudyPlan StudyPlan { get; set; }
        required public Subject Subject { get; set; }
        required public int Credits { get; set; }
        required public int Level { get; set; }
        required public int Type { get; set; }
        public SpSubjectDTO SimpleList() => new(this);
        public class SpSubjectDTO(SpSubject spSubject)
        {
            public string Code { get; set; } = spSubject.Subject.Code;
            public string Title { get; set; } = spSubject.Subject.Title;
            public int Credits { get; set; } = spSubject.Credits;
            public int Level { get; set; } = spSubject.Level;
            public string Type { get; set; } = spSubject.Type switch
            { 0 => "Exclusive", 1 => "Shared", 2 => "Optative", 3 => "Elective", 4 => "Degree", _ => "Undefined" };
            public IEnumerable<SubjectSimple> Prerequisites { get; set; } = from a in spSubject.Subject.PreDependencies where a.StudyPlan == spSubject.StudyPlan select a.Prerequisite.Simple();
            public IEnumerable<SubjectSimple> Postrequisites { get; set; } = from a in spSubject.Subject.PostDependencies where a.StudyPlan == spSubject.StudyPlan select a.Postrequisite.Simple();
        }
    }
}
