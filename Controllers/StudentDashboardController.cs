using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRUDApi.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using CRUDApi.DTOs;

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
        public async Task< IActionResult> GetUnsubmittedQuizzesAndTasks()
        {



            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (emailClaim == null)
            {
                return BadRequest("Email claim not found in token.");
            }

            var email = emailClaim.Value;

            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            var studentId = user.UserId;


            var unsubmittedQuizzes =await (from q in _context.Quizzes
                                   .Where(q => !q.StudentQuizGrades.Any(s => s.StudentId == studentId))
                                      select new GetUnsubmittedQuizzesAndTasksDto
                                      {
                                          title = q.Title,
                                          grade = q.Grade,
                                          endDate = q.EndDate,
                                          startDate = q.StartDate,
                                          createdAt = q.CreatedAt
                                      }).ToListAsync();

            /*var unsubmittedQuizzes = _context.Quizzes
                .Where(q => !q.StudentQuizGrades.Any(s => s.StudentId == studentId))
            .ToList();*/

            var unsubmittedTasks =await (from t in _context.Tasks
                                   .Where(t => !t.TaskAnswers.Any(s => s.StudentId == studentId))
                                      select new GetUnsubmittedQuizzesAndTasksDto
                                      {
                                          title = t.Title,
                                          grade = t.Grade,
                                          endDate = t.EndDate,
                                          startDate = t.StartDate,
                                          createdAt = t.CreatedAt
                                      }).ToListAsync();


            /*var unsubmittedTasks = _context.Tasks
                .Where(t => !t.TaskAnswers.Any(a => a.StudentId == studentId))
                .ToList();*/



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
