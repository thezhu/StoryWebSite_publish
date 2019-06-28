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
    public class ImageBlocksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ImageBlocksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ImageBlocks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImageBlock>>> GetImageBlock()
        {
            return await _context.ImageBlock.ToListAsync();
        }

        // GET: api/ImageBlocks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ImageBlock>> GetImageBlock(int id)
        {
            var imageBlock = await _context.ImageBlock.FindAsync(id);

            if (imageBlock == null)
            {
                return NotFound();
            }

            return imageBlock;
        }

        // PUT: api/ImageBlocks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImageBlock(int id, ImageBlock imageBlock)
        {
            if (id != imageBlock.ImageBlockId)
            {
                return BadRequest();
            }

            _context.Entry(imageBlock).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImageBlockExists(id))
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

        // POST: api/ImageBlocks
        [HttpPost]
        public async Task<ActionResult<ImageBlock>> PostImageBlock(ImageBlock imageBlock)
        {
            _context.ImageBlock.Add(imageBlock);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetImageBlock", new { id = imageBlock.ImageBlockId }, imageBlock);
        }

        // DELETE: api/ImageBlocks/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ImageBlock>> DeleteImageBlock(int id)
        {
            var imageBlock = await _context.ImageBlock.FindAsync(id);
            if (imageBlock == null)
            {
                return NotFound();
            }

            _context.ImageBlock.Remove(imageBlock);
            await _context.SaveChangesAsync();

            return imageBlock;
        }

        private bool ImageBlockExists(int id)
        {
            return _context.ImageBlock.Any(e => e.ImageBlockId == id);
        }
    }
}
