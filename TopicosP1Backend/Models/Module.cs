using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TopicosP1Backend.Scripts;

namespace CareerApi.Models
{
    public class Module
    {
        [Key]
        public long Number { get; set; }
        public IEnumerable<Room> Rooms { get; set; } = new List<Room>();
        public ModuleDTO Simple() => new(this);

        public class ModuleDTO(Module module)
        {
            public long Number { get; set; } = module.Number;
            public IEnumerable<Room.RoomDTO> Rooms { get; } =
                from a in module.Rooms select a.Simple();
        }

        public class ModulePost
        {
            public long Number { get; set; }
        }
        public static async Task<ActionResult<IEnumerable<ModuleDTO>>> GetModules(Context _context)
        {
            var l = await _context.Modules.Include(_ => _.Rooms).ToListAsync();
            return (from i in l select i.Simple()).ToList();
        }

        public static async Task<ActionResult<ModuleDTO>> GetModule(Context _context, long id)
        {
            var @module = await _context.Modules.Include(_ => _.Rooms).FirstOrDefaultAsync(_ => _.Number == id);
            if (@module == null) return new NotFoundResult();
            return @module.Simple();
        }


        public static async Task<ActionResult<ModuleDTO>> PostModule(Context _context, ModulePost m)
        {
            if (await _context.Modules.FindAsync(m.Number) != null) return new BadRequestResult();
            Module @module = new() { Number = m.Number };
            _context.Modules.Add(@module);
            await _context.SaveChangesAsync();
            return @module.Simple();
        }

        public static async Task<IActionResult> DeleteModule(Context _context, long id)
        {
            var @module = await _context.Modules.FindAsync(id);
            if (@module == null) return new NotFoundResult();
            _context.Modules.Remove(@module);
            await _context.SaveChangesAsync();
            return new NoContentResult();
        }

        public static async Task<ActionResult<List<Room.RoomDTO>>> GetModRooms(Context _context, long id)
        {
            var @module = await _context.Modules.Include(_ => _.Rooms).FirstOrDefaultAsync(_ => _.Number == id);
            if (@module == null) return new NotFoundResult();
            return @module.Simple().Rooms.ToList();
        }

        public static async Task<ActionResult<Room.RoomDTO>> PostModRoom(Context _context, long id, ModulePost room)
        {
            Module? module = await _context.Modules.FindAsync(id);
            if (module == null) return new NotFoundResult();
            Room r = new() { Module = module, Number = room.Number };
            _context.Rooms.Add(r);
            await _context.SaveChangesAsync();
            return r.Simple();
        }

        public static async Task<ActionResult<Room.RoomDTO>> DeleteModRoom(Context _context, long id, long room)
        {
            Room? r = await _context.Rooms.FirstOrDefaultAsync(_ => _.Module.Number == id && _.Number == room);
            if (r == null) return new NotFoundResult();
            _context.Rooms.Remove(r);
            await _context.SaveChangesAsync();
            return new OkResult();
        }
    }
}
