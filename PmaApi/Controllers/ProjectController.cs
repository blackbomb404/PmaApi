using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pma.Context;
using Pma.Models.DTOs.Project;
using Pma.Models.DTOs.User;
using PmaApi.Models.Domain;
using PmaApi.Models.DTOs;
using PmaApi.Models.DTOs.Project;
using PmaApi.Models.DTOs.Task;

namespace PmaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController(PmaContext context) : ControllerBase
    {
        // GET: api/Project
        [HttpGet]
        public async Task<ActionResult<PagedResources<ProjectListDto>>>GetProjects([FromQuery] QueryParameters queryParameters)
        {
            return new PagedResources<ProjectListDto>
            {
                Items = await ProjectsCollection()
                    .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
                    .Take(queryParameters.PageSize)
                    .ToListAsync(),
                Pagination = new Pagination
                {
                    PageNumber = queryParameters.PageNumber,
                    PageSize = queryParameters.PageSize,
                    TotalCount = (short)await context.Projects.CountAsync()
                }
            };
        }

        // GET: api/Project/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDetailsDto>> GetProject(long id)
        {
            var project = await context.Projects
                .AsNoTracking()
                .Select(p => new ProjectDetailsDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    Status = p.Status,
                    Members = p.Members.Select(m => new UserOverviewDto
                    {
                        Id = m.Id,
                        FirstName = m.FirstName,
                        LastName = m.LastName,
                        PhotoUrl = m.PhotoUrl
                    }),
                    Tasks = p.Tasks.Select(t => new TaskListDto
                    {
                        Id = t.Id,
                        Name = t.Name,
                        StartDate = t.StartDate,
                        EndDate = t.EndDate,
                        Status = t.Status,
                        Priority = t.Priority,
                        Order = t.Order,
                        ProjectId = t.ProjectId,
                        Members = t.Members.Select(m => new UserOverviewDto
                        {
                            Id = m.Id,
                            FirstName = m.FirstName,
                            LastName = m.LastName,
                            PhotoUrl = m.PhotoUrl
                        })
                    })
                })
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project is null)
            {
                return NotFound();
            }
            
            return project;
        }

        // PUT: api/Project/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject(long id, ProjectUpdateDto projectUpdateDto)
        {
            if (id != projectUpdateDto.Id)
            {
                return BadRequest();
            }

            var project = await context.Projects.FindAsync(id);
            if (project is null)
            {
                return NotFound();
            }
            project.Name = projectUpdateDto.Name;
            project.Description = projectUpdateDto.Description;
            project.StartDate = projectUpdateDto.StartDate;
            project.EndDate = projectUpdateDto.EndDate;
            project.Status = projectUpdateDto.Status;
            project.UserProjects = projectUpdateDto.MemberIds
                .Select(memberId => new UserProject { UserId = memberId })
                .ToList();
            // context.Entry(project).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }
        
        [HttpPost("{id}/assign-to-members")]
        public async Task<IActionResult> AssignProjectToMembers(long id, ProjectAssignmentInputDto projectAssignmentInputDto)
        {
            // 1. Find the project and INCLUDE its current members
            var project = await context.Projects
                .Include(p => p.UserProjects)
                .SingleOrDefaultAsync(p => p.Id == id);
    
            if (project is null)
            {
                return NotFound();
            }
    
            // 2. Get the current and new member IDs
            var currentMemberIds = project.UserProjects.Select(up => up.UserId).ToList();
            var newMemberIds = projectAssignmentInputDto.MemberIds;
    
            // 3. Find members to remove
            var membersToRemove = project.UserProjects
                .Where(up => !newMemberIds.Contains(up.UserId))
                .ToList();
    
            // 4. Find member IDs to add
            var memberIdsToAdd = newMemberIds
                .Where(newId => !currentMemberIds.Contains(newId))
                .ToList();
    
            // 5. Add and remove relationships
            // Remove old relationships
            foreach (var userProject in membersToRemove)
            {
                project.UserProjects.Remove(userProject);
            }
    
            // Add new relationships
            foreach (var memberId in memberIdsToAdd)
            {
                project.UserProjects.Add(new UserProject { UserId = memberId, ProjectId = project.Id });
            }
    
            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
                {
                    return NotFound();
                }
                throw;
            }
    
            return NoContent();
        }

        // POST: api/Project
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProjectListDto>> PostProject(ProjectCreateDto projectCreateDto)
        {
            var project = new Project
            {
                Name = projectCreateDto.Name,
                Description = projectCreateDto.Description,
                StartDate = projectCreateDto.StartDate,
                EndDate = projectCreateDto.EndDate,
                Status = projectCreateDto.Status,
                UserProjects = projectCreateDto
                    .MemberIds
                    .Select(memberId => new UserProject { UserId = memberId })
                    .ToList()
            };
            context.Projects.Add(project);
            await context.SaveChangesAsync();

            var createdProject = await ProjectsCollection()
                .FirstOrDefaultAsync(p => p.Id == project.Id);
            
            if (createdProject is null)
            {
                return NotFound();
            }
            
            return CreatedAtAction(nameof(GetProject), new { id = project.Id }, createdProject);
        }

        // DELETE: api/Project/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(long id)
        {
            var project = await context.Projects.FindAsync(id);
            if (project is null)
            {
                return NotFound();
            }

            context.Projects.Remove(project);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectExists(long id)
        {
            return context.Projects.Any(e => e.Id == id);
        }

        private IQueryable<ProjectListDto> ProjectsCollection()
        {
            return context.Projects
                .AsNoTracking()
                .Select(p => new ProjectListDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    Status = p.Status,
                    Members = p.Members.Select(m => new UserOverviewDto
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
