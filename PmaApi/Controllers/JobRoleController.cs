using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pma.Context;
using PmaApi.Models.Domain;

namespace PmaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobRoleController(PmaContext context) : ControllerBase
    {
        // GET: api/JobRole
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobRole>>> GetJobRoles()
        {
            return await context.JobRoles.ToListAsync();
        }

        // GET: api/JobRole/5
        [HttpGet("{id}")]
        public async Task<ActionResult<JobRole>> GetJobRole(long id)
        {
            var jobRole = await context.JobRoles.FindAsync(id);

            if (jobRole == null)
            {
                return NotFound();
            }

            return jobRole;
        }

        // PUT: api/JobRole/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutJobRole(long id, JobRole jobRole)
        {
            if (id != jobRole.Id)
            {
                return BadRequest();
            }

            context.Entry(jobRole).State = EntityState.Modified;

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
        public async Task<ActionResult<JobRole>> PostJobRole(JobRole jobRole)
        {
            context.JobRoles.Add(jobRole);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetJobRole), new { id = jobRole.Id }, jobRole);
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
