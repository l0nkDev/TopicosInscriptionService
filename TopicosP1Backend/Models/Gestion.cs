using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using TopicosP1Backend.Scripts;

namespace CareerApi.Models
{
    public class Gestion
    {
        [Key]
        public long Year { get; set; }
        public IEnumerable<Period> Periods { get; } = new List<Period>();
        public GestionDTO Simple() => new(this);
        public YearOnly YearItem() => new(this);

        public class GestionDTO(Gestion gestion) 
        {
            public long Year { get; set; } = gestion.Year;
            public IEnumerable<Period.YearlessPeriod> Periods { get; } =
                from a in gestion.Periods select a.Yearless();
        }
        public class YearOnly(Gestion gestion)
        {
            public long Year { get; set; } = gestion.Year;
        }
        public static async Task<IEnumerable<GestionDTO>> GetGestions(Context context)
        {
            var l = await context.Gestions.Include(_=>_.Periods).ToListAsync();
            return from a in l select a.Simple();
        }

        public static async Task<ActionResult<GestionDTO>> GetGestion(Context context, long id)
        {
            var gestion = await context.Gestions.Include(_ => _.Periods).FirstOrDefaultAsync(_ => _.Year == id);
            if (gestion == null) return new NotFoundResult();
            return gestion.Simple();
        }
        public static async Task<ActionResult<Gestion>> PostGestion(Context _context, Gestion gestion)
        {
            if (await _context.Gestions.FindAsync(gestion.Year) != null) return new BadRequestResult();
            _context.Gestions.Add(gestion);
            await _context.SaveChangesAsync();
            return gestion;
        }
        public static async Task<IActionResult> DeleteGestion(Context _context, long id)
        {
            var gestion = await _context.Gestions.FindAsync(id);
            if (gestion == null) return new NotFoundResult();
            _context.Gestions.Remove(gestion);
            await _context.SaveChangesAsync();
            return new NoContentResult();
        }
    }
}
