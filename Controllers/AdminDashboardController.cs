using CRUDApi.Context;
using CRUDApi.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
namespace CRUDApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    [Route("api/[controller]")]
    [ApiController]
    public class AdminDashboardController : ControllerBase
    {
        private readonly LMSContext _context;
        public AdminDashboardController(LMSContext context)
        {
            _context = context;
        }
        #region Stats
        [HttpGet("Stats")]
        public async Task<IActionResult> Stats()
        {
             var activeStudentes = (from u in _context.Users
                                   join ur in _context.UserRoles on u.UserId equals ur.UserId
                                   where ur.RoleId== "ROLE002"&&u.Status=="ACTIVE"
                                   select u.UserId).Count();

            var activeCourses = _context.Courses.Count();

            var activeInstructors= _context.UserRoles
                             .Where(ur => ur.RoleId == "ROLE002")
                             .Select(ur => ur.UserId)
                             .Distinct()
                             .Count();

            var newComers =await (from u in _context.Users
                            join ui in _context.StudentInfos on u.UserId equals ui.UserId
                            where ui.Level == 1
                            select u.FullName).ToListAsync();
            var data = new StatsDto
            {
                activeCourses = activeCourses,
                activeInstructors = activeInstructors,
                activeStudents = activeStudentes,
                newComers = newComers
            };
            return Ok(data);
        }
        #endregion

        #region MyRegion

        #endregion

    }
}
