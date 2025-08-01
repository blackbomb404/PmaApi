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

namespace PmaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(PmaContext context) : ControllerBase
    {
        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserOutputDto>>> GetUsers()
        {
            return await context.Users
                .Select(user => new UserOutputDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    PhotoUrl = user.PhotoUrl,
                    JobRoleName = user.JobRole.Name,
                    AccessRoleName = user.AccessRole.Name
                })
                .ToListAsync();
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserOutputDto>> GetUser(long id)
        {
            var user = await context.Users
                .Include(u => u.JobRole)
                .Include(u => u.AccessRole)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (user is null)
            {
                return NotFound();
            }

            var userOutputDto = new UserOutputDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                PhotoUrl = user.PhotoUrl,
                JobRoleName = user.JobRole.Name,
                AccessRoleName = user.AccessRole.Name
            };
            
            return userOutputDto;
        }

        // PUT: api/User/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(long id, UserUpdateDto userUpdateDto)
        {
            var user = await context.Users.FindAsync(id);
            if (user is null)
            {
                return NotFound();
            }
            
            // manual mapping
            user.FirstName = userUpdateDto.FirstName;
            user.LastName = userUpdateDto.LastName;
            user.PhoneNumber = userUpdateDto.PhoneNumber;
            user.Email = userUpdateDto.Email;
            user.PhotoUrl = userUpdateDto.PhotoUrl;
            user.JobRoleId = userUpdateDto.JobRoleId;
            user.AccessRoleId = userUpdateDto.AccessRoleId;
            // context.Entry(userUpdateDto).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // POST: api/User
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(UserCreateDto userCreateDto)
        {
            var userEntity = new User
            {
                FirstName = userCreateDto.FirstName,
                LastName = userCreateDto.LastName,
                PhoneNumber = userCreateDto.PhoneNumber,
                Email = userCreateDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(userCreateDto.Password),
                JobRoleId = userCreateDto.JobRoleId,
                AccessRoleId = userCreateDto.AccessRoleId
            };
            context.Users.Add(userEntity);
            await context.SaveChangesAsync();
            
            return CreatedAtAction(nameof(GetUser), new { id = userEntity.Id }, userEntity);
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(long id)
        {
            var user = await context.Users.FindAsync(id);
            if (user is null)
            {
                return NotFound();
            }
            context.Users.Remove(user);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(long id)
        {
            return context.Users.Any(e => e.Id == id);
        }
    }
}
