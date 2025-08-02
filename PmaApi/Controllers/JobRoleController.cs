using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pma.Context;
using Pma.Models.DTOs.JobRole;
using PmaApi.Models.Domain;

namespace PmaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobRoleController(PmaContext context) : ControllerBase
    {
        // GET: api/JobRole
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobRoleOutputDto>>> GetJobRoles()
        {
            return await context.JobRoles
                .Select(jobRole => new JobRoleOutputDto(jobRole.Id, jobRole.Name, jobRole.Description))
                .ToListAsync();
        }

        // GET: api/JobRole/5
        [HttpGet("{id}")]
        public async Task<ActionResult<JobRoleOutputDto>> GetJobRole(long id)
        {
            var jobRole = await context.JobRoles.FindAsync(id);

            if (jobRole is null)
            {
                return NotFound();
            }

            return new JobRoleOutputDto(jobRole.Id, jobRole.Name, jobRole.Description);
        }

        // PUT: api/JobRole/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutJobRole(long id, JobRoleInputDto jobRoleInputDto)
        {
            if (id != jobRoleInputDto.Id)
            {
                return BadRequest();
            }

            var  jobRole = await context.JobRoles.FindAsync(id);
            if (jobRole is null)
            {
                return NotFound();
            }
            jobRole.Name = jobRoleInputDto.Name;
            jobRole.Description = jobRoleInputDto.Description;
            // context.Entry(jobRoleInputDto).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobRoleExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // POST: api/JobRole
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<JobRole>> PostJobRole(JobRoleInputDto jobRoleInputDto)
        {
            var jobRole = new JobRole { Name = jobRoleInputDto.Name };
            context.JobRoles.Add(jobRole);
            await context.SaveChangesAsync();
            
            var jobRoleOutputDto = new JobRoleOutputDto(jobRole.Id, jobRole.Name, jobRole.Description);

            return CreatedAtAction(nameof(GetJobRole), new { id = jobRole.Id }, jobRoleOutputDto);
        }

        // DELETE: api/JobRole/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJobRole(long id)
        {
            var jobRole = await context.JobRoles.FindAsync(id);
            if (jobRole is null)
            {
                return NotFound();
            }

            context.JobRoles.Remove(jobRole);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool JobRoleExists(long id)
        {
            return context.JobRoles.Any(e => e.Id == id);
        }
    }
}
