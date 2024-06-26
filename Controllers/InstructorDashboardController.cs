using CRUDApi.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUDApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorDashboardController : ControllerBase
    {
        private readonly LMSContext _context;
        public InstructorDashboardController(LMSContext context)
        {
            _context = context;
        }
        #region Last 3 News

        [HttpGet]
        [Route("Last3News")]
        public IActionResult GetLast3News()
        {
            var last3News = _context.News
                .OrderByDescending(n => n.CreatedAt)
                .Take(3)
                .ToList();

            return Ok(last3News);
        }



        #endregion        //[HttpGet]

        #region Get All Quiz&Task info


        //[Route("instructor-info")]
        //public IActionResult GetTaskAndInstructorInfo()
        //{
        //    var taskAndInstructorInfo = _context.Tasks
        //        .Include(t => t.Instructor)
        //        .Include(t => t.CourseCycle)
        //        .Select(t => new
        //        {
        //            TaskName = t.Title,
        //            InstructorName = t.Instructor.FullName,
        //            Grade = t.Grade,
        //            StartDate = t.StartDate,
        //            EndDate = t.EndDate,
        //            EnrolledStudentsCount = t.CourseCycle.StudentEnrollments.Count(),
        //            SubmittedTasksCount = t.TaskAnswers.Count()
        //        })
        //        .ToList();

        //    return Ok(taskAndInstructorInfo);
        //}



        //--------------------------------------------------------------------------------------



        //[HttpGet]
        //public async Task<ActionResult> GetAllTasksAndQuizzes()
        //{
        //    var tasks = await _context.Tasks.Include(t => t.Instructor)
        //                                    .Include(t => t.CourseCycle)
        //                                    .ToListAsync();

        //    var quizzes = await _context.Quizzes.Include(q => q.Instructor)
        //                                        .Include(q => q.CourseCycle)
        //                                        .ToListAsync();

        //    var taskDetails = tasks.Select(task => new
        //    {
        //        Type = "Task",
        //        Name = task.Title,
        //        InstructorName = task.Instructor.FullName,
        //        task.Grade,
        //        task.StartDate,
        //        task.EndDate,
        //        StudentEnrollmentCount = _context.StudentEnrollments.Count(se => se.CourseCycleId == task.CourseCycleId),
        //        StudentTaskSubmissionCount = _context.TaskAnswers.Where(ta => ta.TaskId == task.TaskId)
        //                                                          .Select(ta => ta.StudentId)
        //                                                          .Distinct()
        //                                                          .Count()
        //    });

        //    var quizDetails = quizzes.Select(quiz => new
        //    {
        //        Type = "Quiz",
        //        Name = quiz.Title,
        //        InstructorName = quiz.Instructor.FullName,
        //        quiz.Grade,
        //        quiz.StartDate,
        //        quiz.EndDate,
        //        StudentEnrollmentCount = _context.StudentEnrollments.Count(se => se.CourseCycleId == quiz.CourseCycleId),
        //        //StudentTaskSubmissionCount = _context.TaskAnswers.Where(ta => ta.Task.CourseCycleId == quiz.CourseCycleId)
        //        //                                                  .Select(ta => ta.StudentId)
        //        //                                                  .Distinct()
        //        //                                                  .Count()

        //        // StudentTaskSubmissionCount = _context.StudentQuizGrades.Count(sg => sg.QuizId == quiz )

        //    });

        //    var allDetails = taskDetails.Concat(quizDetails);

        //    return Ok(allDetails);
        //}


        //---------------------------------------------------------------------------------------------------------



        [HttpGet("Get All Quiz&Task info")]
        public async Task<ActionResult> GetAllTasksAndQuizzes()
        {
            var tasks = await _context.Tasks.Include(t => t.Instructor)
                                            .Include(t => t.CourseCycle)
                                            .ToListAsync();

            var quizzes = await _context.Quizzes.Include(q => q.Instructor)
                                                .Include(q => q.CourseCycle)
                                                .ToListAsync();

            var taskDetails = tasks.Select(task => new
            {
                Type = "Task",
                Name = task.Title,
                InstructorName = task.Instructor.FullName,
                task.Grade,
                task.StartDate,
                task.EndDate,
                StudentSubmissionCount = _context.TaskAnswers.Count(ta => ta.TaskId == task.TaskId)
            });

            var quizDetails = quizzes.Select(quiz => new
            {
                Type = "Quiz",
                Name = quiz.Title,
                InstructorName = quiz.Instructor.FullName,
                quiz.Grade,
                quiz.StartDate,
                quiz.EndDate,
                StudentSubmissionCount = _context.StudentQuizGrades.Count(qa => qa.QuizId == quiz.QuizId)
            });

            var allDetails = taskDetails.Concat(quizDetails);

            return Ok(allDetails);
        }



        #endregion

        #region  Quiz info by QuizId


        // for quiz 

        [HttpGet("GetQuizDetails/{quizId}")]
        public async Task<ActionResult> GetQuizDetails(string quizId)
        {
            var quiz = await _context.Quizzes.Include(q => q.Instructor)
                                             .Include(q => q.CourseCycle)
                                             .FirstOrDefaultAsync(q => q.QuizId == quizId);

            if (quiz == null)
            {
                return NotFound();
            }

            var studentEnrollmentCount = await _context.StudentEnrollments
                                                       .Where(se => se.CourseCycleId == quiz.CourseCycleId)
                                                       .CountAsync();

            //var studentTaskSubmissionCount = await _context.TaskAnswers
            //                                               .Where(ta => ta.Task.CourseCycleId == quiz.CourseCycleId)
            //                                               .Select(ta => ta.StudentId)
            //                                               .Distinct()
            //                                               .CountAsync();

            //var studentQuizSubmissionCount = await _context.QuizAnswers
            //                             .Where(qa => qa.Quiz.QuizId == quizId) // Assuming there's a navigation property Quiz in QuizAnswer
            //                             .Select(qa => qa.StudentId)
            //                             .Distinct()
            //                             .CountAsync();

            var submittedCount = _context.StudentQuizGrades.Count(sg => sg.QuizId == quizId);



            var quizDetails = new
            {
                quiz.Title,
                InstructorName = quiz.Instructor.FullName,
                quiz.Grade,
                quiz.StartDate,
                quiz.EndDate,
                //StudentEnrollmentCount = studentEnrollmentCount,
                //StudentTaskSubmissionCount = studentTaskSubmissionCount
                SubmittedCount = submittedCount

            };

            return Ok(quizDetails);
        }


        #endregion

        #region Task Info by TaskId



        //for task 

        [HttpGet("GetTaskDetails/{taskId}")]
        public async Task<ActionResult> GetTaskDetails(string taskId)
        {
            var task = await _context.Tasks.Include(t => t.Instructor)
                                           .FirstOrDefaultAsync(t => t.TaskId == taskId);

            if (task == null)
            {
                return NotFound();
            }

            //var studentEnrollmentCount = await _context.StudentEnrollments
            //                                           .Where(se => se.CourseCycleId == task.CourseCycleId)
            //                                           .CountAsync();

            var studentTaskSubmissionCount = await _context.TaskAnswers
                                                           .Where(ta => ta.TaskId == taskId)
                                                           .Select(ta => ta.StudentId)
                                                           .Distinct()
                                                           .CountAsync();

            var taskDetails = new
            {
                task.Title,
                InstructorName = task.Instructor.FullName,
                task.Grade,
                task.StartDate,
                task.EndDate,
                //StudentEnrollmentCount = studentEnrollmentCount,
                StudentTaskSubmissionCount = studentTaskSubmissionCount
            };

            return Ok(taskDetails);
        }


        #endregion
    }
}
