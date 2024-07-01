using CRUDApi.Context;
using CRUDApi.DTOs;
using CRUDApi.Entities;
using CRUDApi.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Octokit;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CRUDApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    //[RemoveNullPropertiesResultFilter]

    public class StudentsController : BaseApiController
    {

        private readonly LMSContext _context;
        //private readonly IWebHostEnvironment _environment;
        public StudentsController(LMSContext context/*IWebHostEnvironment environment*/)
        {
            _context = context;
            //_environment = environment;
        }

        private string CurrentSemester()
        {
            var semester = _context.Semesters
                .OrderByDescending(s => s.CreatedAt)
                .Select(s => s.SemesterId)
                .First();
            return semester;
        }



        #region Get Current User Cources With Instructor Name
        [HttpGet("CurrentCourcesInfo")]
        public async Task<ActionResult<CourseDTO>> GetEnrolledCourses()
        {
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (emailClaim == null)
            {
                return BadRequest("Email claim not found in token.");
            }

            var email = emailClaim.Value;

            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            var studentId = user.UserId;
            if (user.Status.ToLower() == "not active")
            {
                return NoContent();
            }
            string currentSemester = CurrentSemester();

            var courses =await (from cs in _context.CourseSemesters
                           join c in _context.Courses on cs.CourseId equals c.CourseId
                           join se in _context.StudentEnrollments on cs.CycleId equals se.CourseCycleId
                           join u in _context.Users on se.StudentId equals u.UserId
                           where se.StudentId == studentId &&cs.SemesterId== currentSemester
                           select new CourseDTO
                           {
                               CycleId = cs.CycleId,
                               Name = c.Name,
                               Hours = (int)c.Hours,
                               ImagePath = c.ImagePath,
                               InstructorFullName = cs.InstructorCourseSemesters
                        .FirstOrDefault(ics => ics.Instructor.UserRoles.Any(r => r.RoleId == "ROLE002"))
                        .Instructor.FullName
                           })
                          .ToListAsync();

            if (courses == null || !courses.Any())
            {
                return NotFound("No Cources Founded");
            }

            return Ok(courses);
        }
        #endregion

        #region Get Course Materials By Course Id
        [HttpGet("CurrentCourseMaterial")]
        public async Task<ActionResult<AllMaterialDto>> GetCourseMaterials(string CycleId)
        {
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (emailClaim == null)
            {
                return BadRequest("Email claim not found in token.");
            }

            var email = emailClaim.Value;

            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            //        // If the user exists, return their user_id
            //        return user?.UserId;

            var studentId = user.UserId;

            // Validate student enrollment in the course
            var enrolled = _context.StudentEnrollments.Any(se => se.StudentId == studentId && se.CourseCycleId == CycleId);
            if (!enrolled)
            {
                return Unauthorized("Student not enrolled in this course");
            }

            // Get course materials using courseId and studentId
            var materials =await (from cs in _context.CourseSemesters
                             join l in _context.Lectures on cs.CycleId equals l.CourseCycleId
                             join lf in _context.LectureFiles on l.LectureId equals lf.LectureId
                             join s in _context.Semesters on cs.SemesterId equals s.SemesterId
                             where cs.CycleId == CycleId
                             select new AllMaterialDto
                             {
                                 LectureId = l.LectureId,
                                 LectureName = l.Title,
                                 SemesterName = s.SemesterId,
                                 Type = l.Type,
                                 CreatedAt = l.CreatedAt
                                 ,path=lf.FilePath
                             })
                            .ToListAsync();

            if (materials == null || !materials.Any())
            {
                return NotFound();
            }

            return Ok(materials);
        }
        #endregion

        #region GET CurrentCourseQuizzes
        // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("CurrentCourseQuizzes")]
        public async Task<ActionResult<QuizDto>> GetQuizzes(string cycleId)
        {
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (emailClaim == null)
            {
                return BadRequest("Email claim not found in token.");
            }

            var email = emailClaim.Value;

            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            //        // If the user exists, return their user_id
            //        return user?.UserId;

            var studentId = user.UserId;


            // Validate student enrollment in the course cycle
            var enrolled = _context.StudentEnrollments.Any(se => se.StudentId == studentId && se.CourseCycleId == cycleId);
            if (!enrolled)
            {
                return Unauthorized("Student not enrolled in the specified course cycle");
            }

            // Get quizzes and calculate availability
            var quizzes = await(from q in _context.Quizzes
                           where q.CourseCycleId == cycleId
                           select new AllQuizesDto
                           {
                               Id = q.QuizId,
                               Title = q.Title,
                               numberOfQuestion=_context.Questions.Count(qu=>qu.QuizId==q.QuizId),
                               StartDate = (DateTime)q.StartDate,
                               EndDate = (DateTime)q.EndDate,
                               Status = (_context.StudentQuizGrades
                               .FirstOrDefault(sqg => sqg.StudentId == studentId && sqg.QuizId == q.QuizId))!=null
                               ?"Solved" :
                               ((q.StartDate >= DateTime.Now && q.EndDate <= DateTime.Now) ? "Not Available" : " Available")
                           
                           })

                          //.AsEnumerable()  // Client-side evaluation if needed
                                        //.OrderByDescending(q => q.CreatedAt)
                          .ToListAsync();
         

            if (quizzes == null || !quizzes.Any())
            {
                return NotFound();
            }
            await Console.Out.WriteLineAsync((quizzes[0].StartDate <= DateTime.Now && quizzes[0].EndDate >= DateTime.Now).ToString());

            return Ok(quizzes);
        }
        #endregion
        
        #region Get All Tasks
        [HttpGet("CurrentCoursesTasks")]
        public async Task<ActionResult<TaskDto>> GetAllTasks()
        {
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (emailClaim == null)
            {
                return BadRequest("Email claim not found in token.");
            }

            var email = emailClaim.Value;

            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            //        // If the user exists, return their user_id
            //        return user?.UserId;

            var studentId = user.UserId;
            if (user.Status.ToLower() == "not active")
            {
                return NoContent();
            }
            string currentSemester = CurrentSemester();

            // Get tasks for the enrolled student
            var tasks = (from t in _context.Tasks
                        join cs in _context.CourseSemesters on t.CourseCycleId equals cs.CycleId
                        join c in _context.Courses on cs.CourseId equals c.CourseId
                        join u in _context.Users on t.InstructorId equals u.UserId
                        join se in _context.StudentEnrollments on cs.CycleId equals se.CourseCycleId
                        join ta in _context.TaskAnswers on t.TaskId equals ta.TaskId into taskss
                        from tas in taskss.DefaultIfEmpty()
                        where se.StudentId == studentId &&cs.SemesterId== currentSemester
                         select new TaskDto{
                             taskId = t.TaskId,
                             taskName = t.Title,
                             taskGrade = t.Grade,
                             startDate = (DateTime)t.StartDate,
                             endDate = (DateTime)t.EndDate,
                             status = tas.Status ?? "No Submited Answers",
                             courseName = c.Name,
                             instructorName = u.FullName,
                            filePath=tas.FilePath??t.FilePath
                        }

                        ).AsEnumerable().ToList();
            /*foreach (var item in tasks)
            {
                if (item.Status == "uploaded")
                {
                    //var taskId = item.TaskId;
                    item.FilePath = _context.TaskAnswers.Where(ta => ta.TaskId == item.TaskId).Select(ta => ta.FilePath).FirstOrDefault();
                }
            }*/

            if (tasks == null || !tasks.Any())
            {
                return NotFound("No Result Founded");
            }

            return Ok(tasks);
        }
        #endregion

        #region Get Current User Info
        [HttpGet("GetStudentInfo")]
        public async Task<ActionResult<StudentInfoDto>> GetStudentInfo()
        {
            // Retrieve the email claim from the token's claims
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (emailClaim == null)
            {
                return BadRequest("Email claim not found in token.");
            }

            var email = emailClaim.Value;

            var user = _context.Users.FirstOrDefault(u => u.Email == email);


            var userId = user.UserId;

            // Retrieve student information using Entity Framework query
            var studentInfo = await (from us in _context.Users
                                     join si in _context.StudentInfos on us.UserId equals si.UserId
                                     join d in _context.Departments on si.DepartmentId equals d.DepartmentId
                                     join f in _context.Faculties on us.FacultyId equals f.FacultyId
                                     join u in _context.Universities on f.UniversityId equals u.UniversityId
                                     where us.UserId == userId
                                     select new StudentInfoDto
                                     {
                                         UserId = us.UserId,
                                         FullName = us.FullName,
                                         Email = us.Email,
                                         Phone = us.Phone,
                                         ImagePath = us.ImagePath,
                                         AcademicId = si.AcademicId,
                                         Level = si.Level,
                                         DepartmentName = d.Name,
                                         FacultyName = f.Name,
                                         UniversityName = u.Name
                                     }).FirstOrDefaultAsync();



            if (studentInfo == null)
            {
                return NotFound("User not found.");
            }

            return Ok(studentInfo);
        }
        #endregion 

        #region GET All Tasks by Course ID
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("CurrentCourseTasks")]

        public async Task<ActionResult<TaskDto>> GetTaskByCourseId(string cycleId)
        {
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (emailClaim == null)
            {
                return BadRequest("Email claim not found in token.");
            }

            var email = emailClaim.Value;

            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            var studentId = user.UserId;
            var enrolled = _context.StudentEnrollments.Any(se => se.StudentId == studentId && se.CourseCycleId == cycleId);
            if (!enrolled)
            {
                return Unauthorized("Student not enrolled in this course");
            }

            var task = await(
                         from t in _context.Tasks.Include(t => t.TaskAnswers)
                         join ta in _context.TaskAnswers on t.TaskId equals ta.TaskId into taskss
                         from tas in taskss.DefaultIfEmpty()
                         where t.CourseCycleId == cycleId //&& tas.StudentId == studentId
                         select new AllTasksDto()
                         {
                             taskId = t.TaskId,
                             taskName = t.Title,
                             //TaskGrade = t.Grade,
                             startDate = (DateTime)t.StartDate,
                             endDate = (DateTime)t.EndDate,
                             //FilePath = t.FilePath,
                             //CreatedAt = (DateTime)t.CreatedAt,
                             status = tas.Status ?? "No Submited Answers"
                         })
                 .ToListAsync();
            if (task == null)
            {
                return NoContent();
            }
            return Ok(task);
        }
        #endregion

        #region Get All Assignments and Quizes grades for all courses
        [HttpGet("AllTasksAndQuizzesGrades")]
        public async Task<ActionResult<IEnumerable<TaskQuizDto>>> GetAllTasksAndQuizzesGrades()
        {
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (emailClaim == null)
            {
                return BadRequest("Email claim not found in token.");
            }

            var email = emailClaim.Value;

            var user =await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return BadRequest("User not found.");
            }

            var studentId = user.UserId;
            //var quizList = new List<Dictionary<string, double>>();
            //var taskList = new List<Dictionary<string, double>>();
            //string? currentSemester = CurrentSemester();
            var quizGradesQuery =await (from sqg in _context.StudentQuizGrades
                                   join q in _context.Quizzes on sqg.QuizId equals q.QuizId
                                   join cs in _context.CourseSemesters on q.CourseCycleId equals cs.CycleId
                                   join c in _context.Courses on cs.CourseId equals c.CourseId
                                   where sqg.StudentId == studentId //&&cs.SemesterId== currentSemester
                                   select new TaskQuizDto
                                   {
                                       courseName = c.Name,
                                       quizGrade = (double?)sqg.Grade,
                                       quizTitle = q.Title,

                                   }).ToListAsync();

            var taskGradesQuery =await (from ta in _context.TaskAnswers
                                   join t in _context.Tasks on ta.TaskId equals t.TaskId
                                   join cs in _context.CourseSemesters on t.CourseCycleId equals cs.CycleId
                                   join c in _context.Courses on cs.CourseId equals c.CourseId
                                   where ta.StudentId == studentId
                                   select new TaskQuizDto
                                   {
                                       courseName = c.Name,
                                       taskGrade = (double?)t.Grade,
                                       taskTitle = t.Title,
                                   }).ToListAsync();

            //var grades = quizGradesQuery.Union(taskGradesQuery);
            var grades = quizGradesQuery.Concat(taskGradesQuery)
                .Select(taskQuizDto =>
                {
                    var nonNullProperties = typeof(TaskQuizDto).GetProperties()
                        .Where(property => property.GetValue(taskQuizDto) != null)
                        .ToDictionary(property => property.Name, property => property.GetValue(taskQuizDto));

                    return nonNullProperties;
                }).ToList();
            if (grades == null || !grades.Any())
            {
                return NoContent();
            }

            return Ok(grades);
        }

        #endregion

        #region Get Task by task Id
        /*
        [HttpGet("GetAssignment")]
        public async Task<IActionResult> GetTaskbytaskId(string taskId)
        {
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (emailClaim == null)
            {
                return BadRequest("Email claim not found in token.");
            }

            var email = emailClaim.Value;

            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            var studentId = user.UserId;
            var task = _context.Tasks
                .Include(t => t.TaskAnswers)
                .FirstOrDefault(t => t.TaskId == taskId);
            var taskdto = new TaskDto
            {
                //InstructorName = task.Instructor.FullName,
                TaskName = task.Title,
                TaskGrade = task.Grade,
                CreatedAt = (DateTime)task.CreatedAt,
                StartDate = (DateTime)task.StartDate,
                EndDate = (DateTime)task.EndDate,
                FilePath = task.FilePath,
                Status = task.TaskAnswers?.FirstOrDefault(tas => tas.StudentId == studentId)?.Status ?? "No Submited Answers"

            };
            var nonNullProperties = typeof(TaskDto).GetProperties()
                .Where(property => property.GetValue(taskdto) != null)
                .ToDictionary(property => property.Name, property => property.GetValue(taskdto));

            if (nonNullProperties == null)
            {
                return NoContent();
            }
            return Ok(nonNullProperties);

        }*/
        #endregion

        #region Get All Grades by course Id
        [HttpGet("GetAllGradesForCurrentCourse")]
        public async Task< IActionResult> GetCourseGrades(string courseId)
        {
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (emailClaim == null)
            {
                return BadRequest("Email claim not found in token.");
            }

            var email = emailClaim.Value;

            var user =await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            //        // If the user exists, return their user_id
            //        return user?.UserId;

            var studentId = user.UserId;
            // Retrieve quiz grades
            var quizGrades = await(from q in _context.Quizzes
                              join sqg in _context.StudentQuizGrades on q.QuizId equals sqg.QuizId
                              where q.CourseCycleId == courseId && sqg.StudentId == studentId
                              orderby sqg.CreatedAt
                              select new QuizGradeDto
                              {
                                  Title = q.Title,
                                  studentGrade = sqg.Grade,
                                  fullGrade=q.Grade,
                                  Date = (DateTime)sqg.CreatedAt
                              }).ToListAsync();

            var taskGrades = await(from ta in _context.TaskAnswers
                              join t in _context.Tasks on ta.TaskId equals t.TaskId
                              where t.CourseCycleId == courseId && ta.StudentId == studentId
                              select new QuizGradeDto
                              {

                                  Title = t.Title,
                                  studentGrade = ta.Grade,
                                  fullGrade=t.Grade,
                                  Date = (DateTime)ta.CreatedAt
                              }).ToListAsync();
            var grades = quizGrades.Concat(taskGrades);
            if (grades == null)
            {
                return NoContent();
            }
            return Ok(grades);

        }
        #endregion

        #region Upload File
        [HttpPost("File/Upload")]
        public async Task< IActionResult> UploadFile([FromForm] IFormFile file, string taskid)
        {
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (emailClaim == null)
            {
                return BadRequest("Email claim not found in token.");
            }

            var email = emailClaim.Value;

            var user =await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            var studentId = user.UserId;
            //var role = user.UserRoles;

            var ftpServer = "site1439.siteasp.net";
            var ftpPort = 21;
            var ftpLogin = "site1439";
            var ftpPassword = "p@5Y6gZ!Q_e7";
            var uploadPath = "/wwwroot/Uploads/StudentAssignment";

            var existingTaskAnswer = _context.TaskAnswers.FirstOrDefault(ta => ta.TaskId == taskid && ta.StudentId == studentId);

            if (file == null || file.Length == 0)
            {
                return BadRequest("Invalid file");
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var ftpUri = new Uri($"ftp://{ftpServer}:{ftpPort}{uploadPath}/{fileName}");

            var ftpRequest = (FtpWebRequest)WebRequest.Create(ftpUri);

            ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
            ftpRequest.Credentials = new NetworkCredential(ftpLogin, ftpPassword);

            using (var fileStream = file.OpenReadStream())
            using (var ftpStream = ftpRequest.GetRequestStream())
            {
                fileStream.CopyTo(ftpStream);
            }

            if (existingTaskAnswer != null)
            {
                // Delete the existing file from the FTP server
                var deleteFtpUri = new Uri($"ftp://{ftpServer}:{ftpPort}{uploadPath}/{existingTaskAnswer.FileName}");

                var deleteFtpRequest = (FtpWebRequest)WebRequest.Create(deleteFtpUri);
                deleteFtpRequest.Method = WebRequestMethods.Ftp.DeleteFile;
                deleteFtpRequest.Credentials = new NetworkCredential(ftpLogin, ftpPassword);

                using (var deleteResponse = (FtpWebResponse)deleteFtpRequest.GetResponse())
                {
                    if (deleteResponse.StatusCode != FtpStatusCode.FileActionOK)
                    {
                        return BadRequest($"Failed to delete existing file. Server response: {deleteResponse.StatusDescription}");
                    }
                }

                existingTaskAnswer.FilePath = $"https://nabilramadan.runasp.net/Uploads/StudentAssignment/{fileName}";
                existingTaskAnswer.CreatedAt = DateTime.Now;
                existingTaskAnswer.FileName = fileName;

                _context.TaskAnswers.Update(existingTaskAnswer);
                _context.SaveChanges();
                return Ok("File Updated successfully");

            }
            else
            {
                // Create a new TaskAnswer record in the database
                var newTaskAnswer = new TaskAnswer
                {
                    AnswerId = Guid.NewGuid().ToString(),
                    TaskId = taskid,
                    StudentId = user.UserId,
                    Grade = null,
                    FilePath = $"https://nabilramadan.runasp.net/Uploads/StudentAssignment/{fileName}",
                    Status = "uploaded",
                    FileName = fileName,
                    CreatedAt = DateTime.Now
                };

                _context.TaskAnswers.Add(newTaskAnswer);
                _context.SaveChanges();
                return Ok("File uploaded successfully");
            }

        }

        #endregion

        #region Get Material by Material Id
        [HttpGet("Getfilesoflecture")]
        public async Task<ActionResult> getMaterial(string lectureId)
        {
            var material =await (from l in _context.Lectures
                            join lf in _context.LectureFiles on l.LectureId equals lf.LectureId
                            where l.LectureId == lectureId
                            select new CourseMaterialDto
                            {
                                fileName = lf.Name,
                                FilePath = lf.FilePath,
                                CreatedAt = lf.CreatedAt

                            }).ToListAsync();
            var materialout = new List<Dictionary<string, object?>>();
            foreach (var item in material)
            {
                materialout.Add(typeof(CourseMaterialDto).GetProperties()
                    .Where(property => property.GetValue(item) != null)
                    .ToDictionary(property => property.Name, property => property.GetValue(item)));
            }
            if (materialout == null)
            {
                return NoContent();
            }
            return Ok(materialout);
        }
        #endregion

        #region GetQuiz
        [HttpGet("Quiz")]
        public async Task<IActionResult> GetQuiz(string quizId)
        {
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (emailClaim == null)
            {
                return BadRequest("Email claim not found in token.");
            }

            var email = emailClaim.Value;

            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            var studentId = user.UserId;
            var quiz = await _context.Quizzes
                .Include(q => q.Questions)
                .ThenInclude(q => q.QuestionAnswers)
                .FirstOrDefaultAsync(q => q.QuizId == quizId);
            
            if (quiz == null)
            {
                return NotFound();
            }

            /*if (DateTime.Now < quiz.StartDate)
            {
                return Ok("Quiz start date has not yet passed.");
            }
            else if(DateTime.Now > quiz.EndDate)
            {
                return Ok("The Quiz time has Ended...!");
            }*/
            /*var check = _context.StudentQuizGrades
                .FirstOrDefault(sqg => sqg.StudentId == studentId && sqg.QuizId == quizId);*/
            var quizDto = new QuizDto
            {
                Id = quiz.QuizId,
                Title = quiz.Title,
                // Notes = quiz.Notes,
                StartDate = (DateTime)quiz.StartDate,
                EndDate = (DateTime)quiz.EndDate,
                Duration=(TimeSpan)(quiz.EndDate - quiz.StartDate).Value,
                Grade = quiz.Grade,
                CourseId = quiz.CourseCycleId,
                InstructorId = quiz.InstructorId,
                CreatedAt = (DateTime)quiz.CreatedAt,
                
                
                Questions = quiz.Questions.Select(q => new QuestionDto
                {
                    id = q.QuestionId,
                    text = q.Text,
                    type = q.Type,
                    grade = q.Grade,
                    createdAt = (DateTime)q.CreatedAt,
                    Answers = q.QuestionAnswers.Select(a => new AnswerDto
                    {
                        id = a.AnswerId,
                        text = a.Text,
                        createdAt = (DateTime)a.CreatedAt
                    }).ToList()
                }).ToList()
            };

            return Ok(quizDto);
        }
        #endregion

        #region SubmitQuizzes
        [HttpPost("quiz/submit")]
        public async Task< IActionResult> SubmitQuizzes([FromBody] QuizAnswerRequestDto quizAnswers)
        {
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (emailClaim == null)
            {
                return BadRequest("Email claim not found in token.");
            }

            var email = emailClaim.Value;

            var user =await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            var studentId = user.UserId;

            var quiz =await _context.Quizzes
                .Include(q => q.Questions)
                .ThenInclude(q => q.QuestionAnswers)
                .SingleOrDefaultAsync(q => q.QuizId == quizAnswers.QuizId);
            if (quiz == null)
            {
                return NotFound($"Quiz with ID {quizAnswers.QuizId} not found.");
            }
            var results = new  List<QuizResultDto>();

            double? totalStudentGrades = 0;
            double? totalGrades = 0;
            foreach (var question in quiz.Questions)
            {
                totalGrades += question.Grade; 
            }
            foreach (var answer in quizAnswers.Answers)
            {
                var result = new Dictionary<string, bool>();
                var question = quiz.Questions.FirstOrDefault(q => q.QuestionId == answer.QuestionId);
                if (question == null)
                {
                    results.Add(new QuizResultDto
                    {
                        QuestionId = answer.QuestionId,
                        IsCorrect = false,
                        Grade = 0
                    });
                }
                

             
                var correctAnswerIds = question.QuestionAnswers
                    .Where(a => a.IsCorrect)
                    .Select(a => a.AnswerId);

                bool isCorrect = correctAnswerIds.Contains(answer.AnswerId);
                results.Add(new QuizResultDto
                {
                    QuestionId = answer.QuestionId,
                    IsCorrect = isCorrect,
                    Grade = isCorrect ? question.Grade : 0
                });

                if (isCorrect)
                {
                    totalStudentGrades += question.Grade;
                }

                var quizAnswer = new QuizAnswer
                {
                    AnswerId = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.Now,
                    QuestionAnswersId =answer.AnswerId,
                    StudentId= studentId
                };
                _context.QuizAnswers.Add(quizAnswer);
            }
            var studentQuizGrade = new StudentQuizGrade
            {
                StudentId = studentId,
                QuizId = quizAnswers.QuizId,
                Grade = totalStudentGrades,
                CreatedAt = DateTime.Now
            };
            _context.StudentQuizGrades.Add(studentQuizGrade);
            var finalResult = new QuizSubmissionResponseDto
            {
                results = results,
                totalStudentGrade = totalStudentGrades,
                totalGrade=totalGrades
            };

            _context.SaveChanges();

            return Ok(finalResult);
        }
        #endregion 

        #region Get Task by task Id
        [HttpGet("GetAssignment")]
        public async Task<IActionResult> GetTaskbyTaskId(string taskId)
        {
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (emailClaim == null)
            {
                return BadRequest("Email claim not found in token.");
            }

            var email = emailClaim.Value;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            var studentId = user.UserId;
            var task =await _context.Tasks
                .Include(t => t.TaskAnswers)
                .FirstOrDefaultAsync(t => t.TaskId == taskId);

            if (task == null)
            {
                return NotFound("Task not found.");
            }
            var taskdto = new TaskDto
            {
                //InstructorName = task.Instructor.FullName,
                taskName = task.Title,
                taskGrade = task.Grade,
                createdAt = (DateTime)task.CreatedAt,
                startDate = (DateTime)task.StartDate,
                endDate = (DateTime)task.EndDate,
                filePath = task.FilePath,
                status = task.TaskAnswers?.FirstOrDefault(tas => tas.StudentId == studentId)?.Status ?? "No Submited Answers"
            };
            if (taskdto.status == "uploaded")
            {
                taskdto.filePath =_context.TaskAnswers.Where(ta => ta.TaskId == taskId).Select(ta => ta.FilePath).FirstOrDefault();
            }
            var nonNullProperties = typeof(TaskDto).GetProperties()
                .Where(property => property.GetValue(taskdto) != null)
                .ToDictionary(property => property.Name, property => property.GetValue(taskdto));

            if (nonNullProperties == null)
            {
                return NoContent();
            }
            return Ok(nonNullProperties);

        }
        #endregion

        #region Get All Grades in One Course For A Student *
        /*[HttpGet("GetAllGradesForaCourse")]
        public async Task<IActionResult> getGradesForCurrentCourseForAstudent(string CycleId)
        {
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (emailClaim == null)
            {
                return BadRequest("Email claim not found in token.");
            }

            var email = emailClaim.Value;

            var user =await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            var studentId = user.UserId;
            var quizGrades =await (from q in _context.Quizzes
                              join sqg in _context.StudentQuizGrades on q.QuizId equals sqg.QuizId
                              where q.CourseCycleId == CycleId && sqg.StudentId == studentId
                              orderby sqg.CreatedAt
                              select new GradeDto
                              {
                                  Id = q.QuizId,
                                  Title = q.Title,
                                  Grade = sqg.Grade,
                                  Date = (DateTime)sqg.CreatedAt
                              }).ToListAsync();

            var taskGrades =await (from ta in _context.TaskAnswers
                              join t in _context.Tasks on ta.TaskId equals t.TaskId
                              where t.CourseCycleId == CycleId && ta.StudentId == studentId
                              select new GradeDto
                              {
                                  Id = t.TaskId,
                                  Title = t.Title,
                                  Grade = ta.Grade,
                                  Date = (DateTime)ta.CreatedAt
                              }).ToListAsync();
            var grades = quizGrades.Concat(taskGrades).ToList();
            if (grades == null)
            {
                return NoContent();
            }
            return Ok(grades);
        }*/
        #endregion














        #region UnUsed

        #region GetSubmitAssignmentByCycleId
        [HttpGet("GetSubmitAssignmentByCycleId")]
        public async Task<IActionResult> GetTaskanswer(string cycleId)
        {
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (emailClaim == null)
            {
                return BadRequest("Email claim not found in token.");
            }

            var email = emailClaim.Value;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            var studentId = user.UserId;

            var task = from ta in _context.TaskAnswers
                       where (ta.StudentId == studentId && ta.Task.CourseCycleId == cycleId)
                       select (
                       new TaskDto()
                       {
                           taskId = ta.AnswerId,
                           createdAt = (DateTime)ta.CreatedAt,
                           filePath = ta.FilePath,
                           status = ta.Status
                       });
            //.Include(t => t.TaskAnswers)  
            if (task == null)
            {
                return NoContent();
            }
            return Ok(task);

        }
        #endregion

        #region get quizes grades
        /*[HttpGet("AllQuizzesGrades")]

        public async Task<ActionResult<IEnumerable<TaskQuizDto>>> GetAllQuizzesGrades()
        {
            try
            {
                var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

                if (emailClaim == null)
                {
                    return BadRequest("Email claim not found in token.");
                }

                var email = emailClaim.Value;

                var user = _context.Users.FirstOrDefault(u => u.Email == email);

                if (user == null)
                {
                    return BadRequest("User not found.");
                }

                var studentId = user.UserId;

                var quizGradesQuery = (from sqg in _context.StudentQuizGrades
                                       join q in _context.Quizzes on sqg.QuizId equals q.QuizId
                                       join cs in _context.CourseSemesters on q.CourseCycleId equals cs.CycleId
                                       join c in _context.Courses on cs.CourseId equals c.CourseId
                                       where sqg.StudentId == studentId
                                       select new TaskQuizDto
                                       {
                                           courseName = c.Name,
                                           quizGrade = (double?)sqg.Grade,
                                           quizTitle = q.Title,
                                       }).ToList();
                if (quizGradesQuery == null || !quizGradesQuery.Any())
                {
                    return NoContent();
                }

                return Ok(quizGradesQuery);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }*/
        #endregion 
        #endregion

        
    }
}