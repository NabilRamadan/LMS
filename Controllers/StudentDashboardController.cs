using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRUDApi.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CRUDApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class StudentDashboardController : ControllerBase
    {
        private readonly LMSContext _context;
        public StudentDashboardController(LMSContext context)
        {
            _context = context;
        }

        #region Unsubmitted quizees&Tasks


        [HttpGet]
        [Route("GetUnsubmittedQuizzesAndTasks")]
        public IActionResult GetUnsubmittedQuizzesAndTasks()
        {



            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (emailClaim == null)
            {
                return BadRequest("Email claim not found in token.");
            }

            var email = emailClaim.Value;

            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            var studentId = user.UserId;


            var unsubmittedQuizzes = _context.Quizzes
                .Where(q => !q.StudentQuizGrades.Any(s => s.StudentId == studentId))
            .ToList();

            var unsubmittedTasks = _context.Tasks
                .Where(t => !t.TaskAnswers.Any(a => a.StudentId == studentId))
                .ToList();

            var unsubmittedData = new
            {
                Quizzes = unsubmittedQuizzes,
                Tasks = unsubmittedTasks
            };

            return Ok(unsubmittedData);
        }


        #endregion

    }
}
