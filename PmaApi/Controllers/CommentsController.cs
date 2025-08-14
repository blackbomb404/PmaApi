using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pma.Context;
using PmaApi.Models.Domain;
using PmaApi.Models.DTOs;
using PmaApi.Models.DTOs.Comment;

namespace PmaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController(PmaContext context) : ControllerBase
    {
        // GET: api/Comment
        [HttpGet]
        public async Task<ActionResult<PagedResources<CommentOutputDto>>> GetComments([FromQuery] QueryParameters queryParameters)
        {
            return new PagedResources<CommentOutputDto>
            {
                Items = await context.Comments
                    .AsNoTracking()
                    .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
                    .Take(queryParameters.PageSize)
                    .Select(c => new CommentOutputDto
                    {
                        Id = c.Id,
                        TaskId = c.TaskId,
                        Content = c.Content,
                        Author = new CommentAuthorDto
                        {
                            Id = c.UserId,
                            Name = $"{c.User.FirstName} {c.User.LastName}",
                            PhotoUrl = c.User.PhotoUrl,
                        },
                        CreatedAt = c.CreatedAt,
                        UpdatedAt = c.UpdatedAt
                    })
                    .ToListAsync(),
                Pagination = new Pagination
                {
                    PageNumber = queryParameters.PageNumber,
                    PageSize = queryParameters.PageSize,
                    TotalCount = (short)await context.Comments.CountAsync(),
                }
            };
        }

        // GET: api/Comment/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CommentOutputDto>> GetComment(long id)
        {
            var commentOutputDto = await context.Comments
                .AsNoTracking()
                .Select(c => new CommentOutputDto
                {
                    Id = c.Id,
                    TaskId = c.TaskId,
                    Content = c.Content,
                    Author = new CommentAuthorDto
                    {
                        Id = c.UserId,
                        Name = $"{c.User.FirstName} {c.User.LastName}",
                        PhotoUrl = c.User.PhotoUrl,
                    },
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt
                })
                .FirstOrDefaultAsync(c => c.Id == id);

            if (commentOutputDto is null)
            {
                return NotFound();
            }

            return commentOutputDto;
        }

        // PUT: api/Comment/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComment(long id, CommentUpdateDto commentUpdateDto)
        {
            if (id != commentUpdateDto.Id)
            {
                return BadRequest();
            }

            var comment = await context.Comments.FindAsync(id);
            if (comment is null)
            {
                return NotFound();
            }
            
            comment.Content = commentUpdateDto.Content;
            comment.UpdatedAt = DateTime.UtcNow;
            // context.Entry(comment).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // POST: api/Comment
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> PostComment(CommentCreationDto commentCreationDto)
        {
            var comment = new Comment
            {
                Content = commentCreationDto.Content,
                TaskId = commentCreationDto.TaskId,
                UserId = commentCreationDto.UserId
            };
            
            context.Comments.Add(comment);
            await context.SaveChangesAsync();
            
            var userData = await context.Users
                .Where(u => u.Id == comment.UserId)
                .Select(u => new { Name = $"{u.FirstName} {u.LastName}", PhotoUrl = u.PhotoUrl })
                .FirstOrDefaultAsync();

            var commentOutputDto = new CommentOutputDto
            {
                Id = comment.Id,
                TaskId = comment.TaskId,
                Content = comment.Content,
                Author = new CommentAuthorDto
                {
                    Id = comment.UserId,
                    Name = userData.Name,
                    PhotoUrl = userData.PhotoUrl,
                },
                CreatedAt = comment.CreatedAt,
                UpdatedAt = comment.UpdatedAt
            };
            return CreatedAtAction(nameof(GetComment), new { id = comment.Id }, commentOutputDto);
        }

        // DELETE: api/Comment/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(long id)
        {
            var comment = await context.Comments.FindAsync(id);
            if (comment is null)
            {
                return NotFound();
            }

            context.Comments.Remove(comment);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool CommentExists(long id)
        {
            return context.Comments.Any(e => e.Id == id);
        }
    }
}
