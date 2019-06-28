using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoryWebSite.Data;
using StoryWebSite.Models;

namespace StoryWebSite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Stories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Story>>> GetStory()
        {
            return await _context.Story.ToListAsync();
        }

        // GET: api/Stories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Story>> GetStory(int id)
        {
            var story = await _context.Story.FindAsync(id);

            if (story == null)
            {
                return NotFound();
            }

            return story;
        }

        // PUT: api/Stories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStory(int id, Story story)
        {
            if (id != story.StoryId)
            {
                return BadRequest();
            }

            _context.Entry(story).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Stories
        [HttpPost]
        public async Task<ActionResult<Story>> PostStory(Story story)
        {
            _context.Story.Add(story);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStory", new { id = story.StoryId }, story);
        }

        // DELETE: api/Stories/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Story>> DeleteStory(int id)
        {
            var story = await _context.Story.FindAsync(id);
            if (story == null)
            {
                return NotFound();
            }

            _context.Story.Remove(story);
            await _context.SaveChangesAsync();

            return story;
        }

        private bool StoryExists(int id)
        {
            return _context.Story.Any(e => e.StoryId == id);
        }
    }
}
