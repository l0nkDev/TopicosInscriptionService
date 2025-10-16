using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace CareerApi.Models
{
    public class StudentStudyPlan
    {
       required  public Student Student { get; set; }
        required public StudyPlan StudyPlans { get; set; }
    }
}
