using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text.Json.Serialization;
using TopicosP1Backend.Scripts;

namespace CareerApi.Models
{
    public class Period
    {
        public long Id { get; set; }
        required public long Number { get; set; }
        required public Gestion Gestion { get; set; }
        public PeriodDTO Simple() => new(this);
        public YearlessPeriod Yearless() => new(this);

        public class PeriodDTO(Period period)
        {
            public long Id { get; set; } = period.Id;
            public long Number { get; set; } = period.Number;
            public Gestion.YearOnly Gestion { get; set; } = period.Gestion.YearItem();
        }

        public class PostDTO
        {
            public long Id { get; set; }
            required public long Number { get; set; }
            required public long Gestion { get; set; }
        }

        public class YearlessPeriod(Period period)
        {
            public long Id { get; set; } = period.Id;
            public long Number { get; set; } = period.Number;
        }
        public static async Task<ActionResult<IEnumerable<PeriodDTO>>> GetPeriods(Context _context)
        {
            var periods = await _context.Periods.ToListAsync();
            return (from a in periods select a.Simple()).ToList();
        }
        public static async Task<ActionResult<PeriodDTO>> GetPeriod(Context _context, long id)
        {
            var period = await _context.Periods.FindAsync(id);
            if (period == null) return new NotFoundResult();
            return period.Simple();
        }
        public static async Task<ActionResult<PeriodDTO>> PostPeriod(Context _context, PostDTO period)
        {
            Gestion? g = await _context.Gestions.FindAsync(period.Gestion);
            if (g == null) { g = new() { Year = period.Gestion }; _context.Gestions.Add(g); }
            Period p = new() { Id = period.Id, Number = period.Number, Gestion = g };
            _context.Periods.Add(p);
            await _context.SaveChangesAsync();
            return p.Simple();
        }
        public static async Task<IActionResult> DeletePeriod(Context _context, long id)
        {
            var period = await _context.Periods.FindAsync(id);
            if (period == null) return new NotFoundResult();
            _context.Periods.Remove(period);
            await _context.SaveChangesAsync();
            return new NoContentResult();
        }
    }
}
