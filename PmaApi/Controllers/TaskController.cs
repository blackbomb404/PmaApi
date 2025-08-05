using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pma.Context;
using Pma.Models.DTOs.User;
using PmaApi.Models.Domain;
using PmaApi.Models.DTOs;
using PmaApi.Models.DTOs.Task;
using Task = PmaApi.Models.Domain.Task;
using TaskStatus = PmaApi.Models.Domain.TaskStatus;

namespace PmaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController(PmaContext context) : ControllerBase
    {
        // GET: api/Task
        [HttpGet]
        public async Task<ActionResult<PagedResources<TaskListDto>>> GetTasks([FromQuery] QueryParameters queryParameters)
        {
            return new PagedResources<TaskListDto>
            {
                Items = await context.Tasks
                    .AsNoTracking()
                    .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
                    .Take(queryParameters.PageSize)
                    .Select(t => new TaskListDto
                    {
                        Id = t.Id,
                        Name = t.Name,
                        StartDate = t.StartDate,
                        EndDate = t.EndDate,
                        Status = t.Status,
                        Priority = t.Priority,
                        ProjectId = t.ProjectId,
                        Members = t.Members.Select(m => new UserOverviewDto
                        {
                            Id = m.Id,
                            FirstName = m.FirstName,
                            LastName = m.LastName,
                            PhotoUrl = m.PhotoUrl
                        }),
                        Order = t.Order
                    })
                    .ToListAsync(),
                Pagination = new Pagination
                {
                    PageNumber = queryParameters.PageNumber,
                    PageSize = queryParameters.PageSize,
                    TotalCount = (short)await context.Tasks.CountAsync()
                }
            };
        }

        // GET: api/Task/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskDetailsDto>> GetTask(long id)
        {
            var task = await TaskCollection()
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task is null)
            {
                return NotFound();
            }

            return task;
        }

        // PUT: api/Task/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTask(long id, TaskUpdateDto taskUpdateDto)
        {
            if (id != taskUpdateDto.Id)
            {
                return BadRequest();
            }

            var task = await context.Tasks.FindAsync(id);
            if (task is null)
            {
                return NotFound();
            }
            
            task.Name = taskUpdateDto.Name;
            task.Description = taskUpdateDto.Description;
            task.StartDate = taskUpdateDto.StartDate;
            task.EndDate = taskUpdateDto.EndDate;
            task.Status = taskUpdateDto.Status;
            task.Priority = taskUpdateDto.Priority;
            task.UserTasks = taskUpdateDto.MemberIds
                .Select(memberId => new UserTask { UserId = memberId })
                .ToList();
            // context.Entry(task).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        [HttpPost("{id}/assign-to-members")]
        public async Task<IActionResult> AssignTaskToMembers(long id, TaskAssignmentInputDto taskAssignmentInputDto)
        {
            // 1. Find the task and INCLUDE its current members
            // Without `.Include()`, the `task.UserTasks` collection would be empty.
            var task = await context.Tasks
                .Include(t => t.UserTasks)
                .SingleOrDefaultAsync(t => t.Id == id);
    
            if (task is null)
            {
                return NotFound();
            }
    
            // 2. Get the current and new member IDs
            var currentMemberIds = task.UserTasks.Select(ut => ut.UserId).ToList();
            var newMemberIds = taskAssignmentInputDto.MemberIds;
    
            // 3. Find members to remove
            var membersToRemove = task.UserTasks
                .Where(ut => !newMemberIds.Contains(ut.UserId))
                .ToList();
    
            // 4. Find member IDs to add
            var memberIdsToAdd = newMemberIds
                .Where(newId => !currentMemberIds.Contains(newId))
                .ToList();
    
            // 5. Add and remove relationships
            // Remove old relationships
            foreach (var userTask in membersToRemove)
            {
                task.UserTasks.Remove(userTask);
            }
    
            // Add new relationships
            foreach (var memberId in memberIdsToAdd)
            {
                task.UserTasks.Add(new UserTask { UserId = memberId, TaskId = task.Id });
            }
    
            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
                {
                    return NotFound();
                }
                throw;
            }
    
            return NoContent();
        }

        [HttpPost("{id}/reallocate-to-project")]
        public async Task<ActionResult> ReallocateTaskToProject(long id, TaskReallocationInputDto taskAssignmentInputDto)
        {
            if (id != taskAssignmentInputDto.TaskId)
            {
                return BadRequest();
            }
            var task = await context.Tasks
                .Include(t => t.UserTasks)
                .FirstOrDefaultAsync(t => t.Id == id);
            if (task is null)
            {
                return NotFound();
            }
            // 1. Remove from current project
            // var targetProject = await context.Projects
            task.ProjectId = taskAssignmentInputDto.ProjectId;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                
            }
            
            return NoContent();
        }

        [HttpPost("reorder")]
        public async Task<ActionResult> Reorder([FromBody] TaskReorderingInputDto[] taskReorderingInputDtos)
        {
            var taskIds = taskReorderingInputDtos.Select(t => t.TaskId).ToList();
            var tasks = await context.Tasks
                .Where(t => taskIds.Contains(t.Id))
                .ToListAsync();
            
            // Check for missing tasks
            if (tasks.Count != taskIds.Count)
            {
                // Find which tasks were not found and return a more specific error
                var foundTaskIds = tasks.Select(t => t.Id);
                var notFoundTaskIds = taskIds.Except(foundTaskIds);
                return NotFound(new { message = $"Tasks with IDs: {string.Join(", ", notFoundTaskIds)} were not found." });
            }
            foreach (var dto in taskReorderingInputDtos)
            {
                var task = tasks.FirstOrDefault(t => t.Id == dto.TaskId);
                task!.Order = dto.Order;
            }

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            return NoContent();
        }

        // POST: api/Task
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TaskDetailsDto>> PostTask(TaskCreateDto taskCreateDto)
        {
            var task = new Task
            {
                Name = taskCreateDto.Name,
                Description = taskCreateDto.Description,
                StartDate = taskCreateDto.StartDate,
                EndDate = taskCreateDto.EndDate,
                Status = TaskStatus.Created,
                Priority = taskCreateDto.Priority,
                Attachments = [],
                Order = 0,
                ProjectId = taskCreateDto.ProjectId,
                UserTasks = taskCreateDto
                    .MemberIds
                    .Select(memberId => new UserTask { UserId = memberId })
                    .ToList()
            };
            context.Tasks.Add(task);
            await context.SaveChangesAsync();
            
            var taskDetailsDto = await TaskCollection()
                .FirstOrDefaultAsync(t => t.Id == task.Id);
            if (taskDetailsDto is null)
            {
                return NotFound();
            }

            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, taskDetailsDto);
        }

        // DELETE: api/Task/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(long id)
        {
            var task = await context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            context.Tasks.Remove(task);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool TaskExists(long id)
        {
            return context.Tasks.Any(e => e.Id == id);
        }
        
        private IQueryable<TaskDetailsDto> TaskCollection()
        {
            return context.Tasks
                .AsNoTracking()
                .Select(t => new TaskDetailsDto()
                {
                    Id = t.Id,
                    Name = t.Name,
                    Description = t.Description,
                    StartDate = t.StartDate,
                    EndDate = t.EndDate,
                    Status = t.Status,
                    Priority = t.Priority,
                    ProjectId = t.ProjectId,
                    Members = t.Members.Select(m => new UserOverviewDto
                    {
                        Id = m.Id,
                        FirstName = m.FirstName,
                        LastName = m.LastName,
                        PhotoUrl = m.PhotoUrl
                    })
                });
        }
    }
}
