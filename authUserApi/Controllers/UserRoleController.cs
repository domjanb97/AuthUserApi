using authUserApi.Models;
using authUserApi.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace authUserApi.Controllers
{
    [Route("userrole")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly AuthuserContext _context;

        public UserRoleController(AuthuserContext context)
        {
            _context = context;
        }

        [HttpPost("assingRole")]
        public async Task<IActionResult> AssignRole([FromBody] AssingRoleDto assingRoleDto)
        {
            try
            {
                var user = await _context.Users.Include(u => u.Roles)
                    .SingleOrDefaultAsync(u => u.Id == assingRoleDto.UserId);

                var role = await _context.Roles.FindAsync(assingRoleDto.RoleId);

                if (user is null || role is null)
                {
                    return StatusCode(404, new { message = "nincs ilyen user vagy role" });
                }

                user.Roles.Add(role);
                await _context.SaveChangesAsync();

                return StatusCode(201, new { message = "Szerepkör felvéve" });
            }
            catch (Exception e)
            {

                return StatusCode(400, new { message = e.Message });
            }
        }
    }
}
