using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using static CareerApi.Models.Student;
using TopicosP1Backend.Scripts;

namespace CareerApi.Models
{
    public class Teacher
    {
        public long Id { get; set; }
        required public string FirstName { get; set; }
        required public string LastName { get; set; }
        public IEnumerable<Group> Groups { get; set; } = new List<Group>();
        public TeacherDTO Simple() => new(this);

        public class TeacherDTO(Teacher teacher)
        {
            public long Id { get; set; } = teacher.Id;
            public string FirstName { get; set; } = teacher.FirstName;
            public string LastName { get; set; } = teacher.LastName;
        }
        public class TeacherPost
        {
            required public long Id { get; set; }
            required public string FirstName { get; set; }
            required public string LastName { get; set; }
        }
        public static async Task<ActionResult<IEnumerable<TeacherDTO>>> GetTeachers(Context _context)
        {
            var l = await _context.Teachers.IgnoreAutoIncludes().ToListAsync();
            return (from i in l select i.Simple()).ToList();
        }

        public static async Task<ActionResult<TeacherDTO>> GetTeacher(Context _context, long id)
        {
            var student = await _context.Teachers.IgnoreAutoIncludes().FirstOrDefaultAsync(_ => _.Id == id);
            if (student == null) return new NotFoundResult();
            return student.Simple();
        }

        [HttpPut("{id}")]
        public static async Task<ActionResult<TeacherDTO>> PutTeacher(Context _context, long id, TeacherPost student)
        {
            Teacher s = await _context.Teachers.IgnoreAutoIncludes().FirstOrDefaultAsync(_ => _.Id == id);
            if (s == null) return new NotFoundResult();
            if (s.Id != student.Id) return new BadRequestResult();
            s.FirstName = student.FirstName;
            s.LastName = student.LastName;
            _context.Entry(s).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return s.Simple();
        }

        [HttpPost]
        public static async Task<ActionResult<TeacherDTO>> PostTeacher(Context _context, TeacherPost student)
        {
            Teacher s = new() { FirstName = student.FirstName, LastName = student.LastName };
            _context.Teachers.Add(s);
            await _context.SaveChangesAsync();
            return s.Simple();
        }

        [HttpDelete("{id}")]
        public static async Task<IActionResult> DeleteTeacher(Context _context, long id)
        {
            var student = await _context.Teachers.IgnoreAutoIncludes().FirstOrDefaultAsync(_ => _.Id == id);
            if (student == null) return new NotFoundResult();
            _context.Teachers.Remove(student);
            await _context.SaveChangesAsync();
            return new NoContentResult();
        }
    }
}
