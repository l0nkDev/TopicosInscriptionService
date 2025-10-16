namespace CareerApi.Models
{
    public class SubjectDependency
    {
        public long Id { get; set; }
        required public Subject Prerequisite { get; set; } 
        required public Subject Postrequisite { get; set; }
        required public StudyPlan StudyPlan { get; set; }
    }
}
