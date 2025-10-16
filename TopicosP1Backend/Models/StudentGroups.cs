using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace CareerApi.Models
{
    public class StudentGroups
    {
        public long Id { get; set; }
        required public Group Group { get; set; }
        required public Student Student { get; set; }
        public int Grade { get; set; } = 0;
        public int Status { get; set; } = 0;   //0=Inscrita, 1=Terminada, 2=Retirada
        public HistoryEntry Simple() => new(this);

        public class HistoryEntry(StudentGroups sg)
        {
            public long Id { get; set; } = sg.Id;
            public string Code { get; set; } = sg.Group.Subject.Code;
            public string Title { get; set; } = sg.Group.Subject.Title;
            public int Grade { get; set; } = sg.Grade;
            public int Status { get; set; } = sg.Status;
        }

        public class SgDTO
        {
            public long Id { get; set; }
            public string Code { get; set; }
            public string Title { get; set; }
            public int Grade { get; set; }
            public int Status { get; set; }
        }
    }
}
