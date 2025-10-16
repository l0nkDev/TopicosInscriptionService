using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using TopicosP1Backend.Scripts;
using static CareerApi.Models.StudyPlan;

namespace CareerApi.Models
{
    public class Career
    {
        public long Id { get; set; }
        required public string Name { get; set; }
        public IEnumerable<StudyPlan> StudyPlans { get; } = new List<StudyPlan>();
        public CareerDTO SimpleList() => new CareerDTO(this);
        public class CareerDTO(Career career)
        {
            public long Id { get; set; } = career.Id;
            public string Name { get; set; } = career.Name;
            public IEnumerable<StudyPlanDTO> StudyPlans { get; } = from a in career.StudyPlans select a.Simple();
        }

        public class CareerPost
        {
            public long Id { get; set; }
            required public string Name { get; set; }
        }
        public static async Task<IEnumerable<CareerDTO>> GetCareers(Context _context)
        {
            var careers = await _context.Careers.AsSplitQuery().ToListAsync();
            return from a in careers select a.SimpleList();
        }

        public static async Task<ActionResult<CareerDTO>> GetCareer(Context _context, long id)
        {
            var career = await _context.Careers.AsSplitQuery().FirstOrDefaultAsync(_=>_.Id == id);
            if (career == null) return new NotFoundResult();
            return career.SimpleList();
        }

        public static async Task<IActionResult> PutCareer(Context _context, long id, CareerPost c)
        {
            if (id != c.Id) return new BadRequestResult();
            Career subject = await _context.Careers.FindAsync(id);
            if (subject == null) return new NotFoundResult();
            subject.Name = c.Name;
            _context.Entry(subject).State = EntityState.Modified;

            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Careers.Any(e => e.Id == id)) return new NotFoundResult();
                else throw;
            }
            return new NoContentResult();
        }

        public static async Task<ActionResult<CareerDTO>> PostCareer(Context _context, CareerPost c)
        {
            Career career = new Career { Name = c.Name };
            _context.Careers.Add(career);
            await _context.SaveChangesAsync();
            return career.SimpleList();
        }

        public static async Task<IActionResult> DeleteCareer(Context _context, long id)
        {
            var career = await _context.Careers.FindAsync(id);
            if (career == null) return new NotFoundResult();
            _context.Careers.Remove(career);
            await _context.SaveChangesAsync();
            return new NoContentResult();
        }
    }
}
