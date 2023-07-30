using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Notes.API.Data;
using Notes.API.Model.Entity;

namespace Notes.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotesController : Controller
    {
        private readonly DataContext _context;

        public NotesController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNotes() => Ok(await _context.Notes.ToListAsync());

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetNoteById([FromRoute]Guid id)
        {
            var note = await _context.Notes.FindAsync(id);
            if (note == null) return NotFound("Note not found!");
            return Ok(note);
        }

        [HttpPost]
        public async Task<IActionResult> AddNote([FromBody] Note note)
        {
            if (ModelState.IsValid)
            {
                note.guid = Guid.NewGuid();
                await _context.Notes.AddAsync(note);
                await _context.SaveChangesAsync();

                return Ok(await _context.Notes.ToListAsync());
            }

            return BadRequest("Couldn't be added!");
        }

        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> UpdateNote([FromRoute] Guid id, [FromBody] Note note)
        {
            var existingNote = await _context.Notes.FindAsync(id);
            if (existingNote is not null)
            {
                existingNote.Title = note.Title;
                existingNote.Description = note.Description;
                existingNote.IsArchived = note.IsArchived;

                _context.Notes.Update(existingNote);
                await _context.SaveChangesAsync();

                return Ok(await _context.Notes.ToListAsync());
            }

            return NotFound("Note not found!");
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteNote([FromRoute] Guid id)
        {
            var note = await _context.Notes.FindAsync(id);
            if (note is not null)
            {
                _context.Notes.Remove(note);
                await _context.SaveChangesAsync();

                return Ok(await _context.Notes.ToListAsync());
            }
            return NotFound("Note not found!");
        }
    }
}
