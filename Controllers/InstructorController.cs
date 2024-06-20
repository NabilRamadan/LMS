﻿using AutoMapper.Configuration.Conventions;
using CRUDApi.Context;
using CRUDApi.DTOs;
using CRUDApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;

namespace CRUDApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class InstructorController : ControllerBase
    {
        private readonly LMSContext _context;
        public InstructorController(LMSContext context)
        {
            _context = context;
        }


        #region Get Current User Info
        [HttpGet("GetInstructorInfo")]
        public async Task<IActionResult> GetInstructorInfo()
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

            var InstructorInfo = await (from us in _context.Users
                                     join f in _context.Faculties on us.FacultyId equals f.FacultyId
                                     join u in _context.Universities on f.UniversityId equals u.UniversityId
                                     where us.UserId == userId
                                     select new InstructorInfoDto
                                     {
                                         FullName = us.FullName,
                                         Email = us.Email,
                                         Phone = us.Phone,
                                         ImagePath = us.ImagePath,
                                         FacultyName = f.Name,
                                         UniversityName = u.Name
                                     }).FirstOrDefaultAsync();



            if (InstructorInfo == null)
            {
                return NotFound("User not found.");
            }

            return Ok(InstructorInfo);
        }
        #endregion

        #region Get All Courses For Current User
        [HttpGet("CurrentCoursesInfo")]
        public async Task<ActionResult<AllCourcesForInstructorDto>> GetAllCourses()
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

            var userid = user.UserId;
            var course = (from cs in _context.CourseSemesters
                          join c in _context.Courses on cs.CourseId equals c.CourseId
                          join ics in _context.InstructorCourseSemesters on cs.CycleId equals ics.CourseCycleId
                          where ics.InstructorId == userid
                          select new AllCourcesForInstructorDto
                          {
                              CycleId = cs.CycleId,
                              Hours = c.Hours,
                              ImagePath=c.ImagePath,
                              Name=c.Name,
                          }).ToList();
           
            if (course == null)
            {
                return NoContent();
            }
            return Ok(course);
        }
        #endregion

        #region Get All Folders of Courses
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("CurrentCourseMaterial")]
        public async Task<ActionResult<AllMaterialDto>> GetCourseMaterials(string CycleId)
        {
            var materials = (from cs in _context.CourseSemesters
                             join l in _context.Lectures on cs.CycleId equals l.CourseCycleId
                             where cs.CycleId == CycleId
                             select new AllMaterialDto
                             {
                                 LectureId = l.LectureId,
                                 LectureName = l.Title,
                                 Type = l.Type,
                                 CreatedAt = l.CreatedAt
                                
                             })
                            .ToList();
            var material =new List <Dictionary<string,object?>>();
            foreach (var item in materials)
            {
                material.Add(typeof(AllMaterialDto).GetProperties()
                    .Where(property => property.GetValue(item) != null)
                    .ToDictionary(property => property.Name, property => property.GetValue(item)));
            }
            if (material == null || !material.Any())
            {
                return NotFound();
            }

            return Ok(material);
        }
        #endregion

        #region Get files of lecture
        [HttpGet("Getfilesoflecture")]
        public async Task<ActionResult> getMaterial(string lectureId)
        {
            var material = (from l in _context.Lectures
                            join lf in _context.LectureFiles on l.LectureId equals lf.LectureId
                            where l.LectureId == lectureId
                            select new CourseMaterialDto
                            {
                                LectureFileId=lf.LectureFileId,
                                fileName = lf.Name,
                                FilePath = lf.FilePath,
                                CreatedAt = lf.CreatedAt
                            }).ToList();
            if (material == null)
            {
                return NoContent();
            }
            return Ok(material);
        }
        #endregion

        #region Upload Licture file
        [HttpPost("UploadLectureFile")]
        public async Task<IActionResult> uploadLectureFile([FromForm] IFormFile file, string lectureId,String file_Name)
        {
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (emailClaim == null)
            {
                return BadRequest("Email claim not found in token.");
            }

            var email = emailClaim.Value;

            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            var userId = user.UserId;

            var ftpServer = "site1439.siteasp.net";
            var ftpPort = 21;
            var ftpLogin = "site1439";
            var ftpPassword = "p@5Y6gZ!Q_e7";
            var uploadPath = "/wwwroot/Uploads/Lectures";
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

            var newFile = new LectureFile
            {
                CreatedAt = DateTime.Now,
                FilePath = $"https://nabilramadan.runasp.net/Uploads/Lectures/{fileName}",
                LectureId = lectureId,
                LectureFileId = Guid.NewGuid().GetHashCode(),
                Name = file_Name
            };
            _context.LectureFiles.Add(newFile);
            _context.SaveChanges();
            //return Ok("File uploaded successfully");
            return Created("",newFile);
        }

        #endregion

        #region Update Lecture File
        [HttpPut("UpdateLecturefile")]
        public IActionResult updateLecureFile(int file_Id,string fileName)
        {
            var file =_context.LectureFiles.FirstOrDefault(x => x.LectureFileId == file_Id);
            if (file == null)
            {
                return NotFound();
            }
            file.Name = fileName;
            _context.LectureFiles.Update(file);
            _context.SaveChanges();
            return Ok("Updated Succefuly ..!");
        }
        #endregion

        #region Delete Lecture File
        [HttpDelete("DeleteLectureFile")]
        public IActionResult deleteLectureFile(int FileId)
        {
            var file =_context.LectureFiles.FirstOrDefault(x=>x.LectureFileId==FileId);
            if (file == null)
            {
                return NotFound();
            }
            var name =file.FilePath.Split('/').Last();
            var ftpServer = "site1439.siteasp.net";
            var ftpPort = 21;
            var ftpLogin = "site1439";
            var ftpPassword = "p@5Y6gZ!Q_e7";
            var uploadPath = "/wwwroot/Uploads/Lectures";
            var deleteFtpUri = new Uri($"ftp://{ftpServer}:{ftpPort}{uploadPath}/{name}");

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
            _context.LectureFiles.Remove(file);
            _context.SaveChanges();
            return Ok("Deleted Succefully ..!");
            //return Ok("file name : "+name+"\n file path : "+ file.FilePath);
        }
        #endregion

        #region Get All Assignment For one Course
        [HttpGet("GetCurrentCourseTasks")]
        public async Task<IActionResult> getAllAssignment(string cycleId)
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

            var userId = user.UserId;
            var tasks = (from t in _context.Tasks
                         where t.CourseCycleId == cycleId && t.InstructorId == userId
                         select new TaskDto
                         {
                             taskId = t.TaskId,
                             taskName = t.Title,
                             startDate = (DateTime)t.StartDate,
                             endDate = (DateTime)t.EndDate,
                             filePath = t.FilePath,
                         }).ToList();
            var task = new List<Dictionary<string, object?>>();
            foreach (var item in tasks)
            {
                task.Add(typeof(TaskDto).GetProperties()
                    .Where(property => property.GetValue(item) != null)
                    .ToDictionary(property => property.Name, property => property.GetValue(item)));
            }
            if (task == null || !task.Any())
            {
                return NotFound();
            }

            return Ok(task);
        }
        #endregion

        #region Get All Students who Submit the task
        [HttpGet("GetStudentsWhoUploadThetask")]
        public async Task<IActionResult> getAllStudentsWhoUploadTheTask(string taskId)
        {
            var students=(from ta in _context.TaskAnswers
                         join u in _context.Users on ta.StudentId equals u.UserId
                         where ta.TaskId == taskId
                         select new StudentsWhoUploadTheTaskDto
                         {
                             studentId = ta.StudentId,
                             studentName=u.FullName,
                             filePath=ta.FilePath,
                             timeUploaded=ta.CreatedAt
                         }).ToList();
            if (students == null)
            {
                return NoContent();
            }
            return Ok(students);
        }
        #endregion

        #region Upload An Assignment
        [HttpPost("UploadAssignment")]
        public async Task<IActionResult> uploadAssignmnet([FromForm] IFormFile file, string TaskName, double TaskGrade, DateTime StartDate, DateTime EndDate, string CourseCycleId)
        {
        
        var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (emailClaim == null)
            {
                return BadRequest("Email claim not found in token.");
            }

            var email = emailClaim.Value;

            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            var userId = user.UserId;

            var ftpServer = "site1439.siteasp.net";
            var ftpPort = 21;
            var ftpLogin = "site1439";
            var ftpPassword = "p@5Y6gZ!Q_e7";
            var uploadPath = "/wwwroot/Uploads/Assignments";
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
            var newTask = new Models.Task
            {
                TaskId=Guid.NewGuid().ToString(),
                CourseCycleId=CourseCycleId,
                CreatedAt=DateTime.Now,
                EndDate=EndDate,
                StartDate=StartDate,
                FilePath= $"https://www.nabilramadan.runasp.net/Uploads/Assignments/{fileName}",
                Grade=TaskGrade,
                InstructorId=userId,
                Title=TaskName,
            };
            _context.Tasks.Add(newTask);
            _context.SaveChanges();
            return Ok("Created ");
        }
        #endregion

        #region Update An Assignment 
        [HttpPut("UpdateAnAssignment")]
        public IActionResult updateAnAssignmemt(String taskId ,UploadAssignmentDto ua)
        {
            var task=_context.Tasks.FirstOrDefault(t => t.TaskId == taskId);
            if (task == null) {
                return BadRequest();
            }
            if (ua.TaskName != null)
            {
                task.Title= ua.TaskName;
            }
            if(ua.TaskGrade != null)
            {
                task.Grade= ua.TaskGrade;
            }
            if(ua.StartDate != null)
            {
                task.StartDate= ua.StartDate;
            }
            if(ua.EndDate != null)
            {
                task.EndDate= ua.EndDate;
            }
            _context.Tasks.Update(task);
            _context.SaveChanges();
            return Ok("Updated Succefully ");
        }
        #endregion

        #region Delete An Assignment
        [HttpDelete("DeleteAnAssignment")]
        public IActionResult DeleteAnAssignment(String taskId)
        {
            var file=_context.Tasks.FirstOrDefault(t=>t.TaskId == taskId);
            if (file == null)
            {
                return NotFound();
            }
            var name = file.FilePath.Split('/').Last();
            var ftpServer = "site1439.siteasp.net";
            var ftpPort = 21;
            var ftpLogin = "site1439";
            var ftpPassword = "p@5Y6gZ!Q_e7";
            var uploadPath = "/wwwroot/Uploads/Assignments";
            var deleteFtpUri = new Uri($"ftp://{ftpServer}:{ftpPort}{uploadPath}/{name}");

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
            _context.Tasks.Remove(file);
            _context.SaveChanges();
            return Ok("Deleted Succefully ");
        }
        #endregion

        #region Get All Students Enrolled In Acourse
        [HttpGet("GetAllStudentsEnrolledInAcourse")]
        public async Task<IActionResult> getAllStudentsEnrroledInCourse(string CycleId)
        {
            var students = (from se in _context.StudentEnrollments
                            join cs in _context.CourseSemesters on se.CourseCycleId equals cs.CycleId
                            join u in _context.Users on se.StudentId equals u.UserId
                            where cs.CycleId == CycleId
                            select new getAllStudentsEnrroledInCourseDto
                            {
                                studentId = u.UserId,
                                studentName = u.FullName
                            }).ToList();
            if(students.Count == 0)
            {
                return BadRequest("there no students enrolled in this Course ");
            }
            return Ok(students);
        }
        #endregion

        #region Get All Grades in One Course For A Student
        [HttpGet("GetGradesForCurrentCourseForAstudent")]
        public async Task<IActionResult> getGradesForCurrentCourseForAstudent(string CycleId,string studentId)
        {
            var quizGrades = (from q in _context.Quizzes
                              join sqg in _context.StudentQuizGrades on q.QuizId equals sqg.QuizId
                              where q.CourseCycleId == CycleId && sqg.StudentId == studentId
                              orderby sqg.CreatedAt
                              select new GradeDto
                              {
                                  Id=q.QuizId,
                                  Title = q.Title,
                                  Grade = sqg.Grade,
                                  Date = (DateTime)sqg.CreatedAt
                              }).ToList();

            var taskGrades = (from ta in _context.TaskAnswers
                              join t in _context.Tasks on ta.TaskId equals t.TaskId
                              where t.CourseCycleId == CycleId && ta.StudentId == studentId
                              select new GradeDto
                              {
                                  Id=t.TaskId,
                                  Title = t.Title,
                                  Grade = ta.Grade,
                                  Date = (DateTime)ta.CreatedAt
                              }).ToList();
            var grades = quizGrades.Concat(taskGrades).ToList();
            if (grades == null)
            {
                return NoContent();
            }
            return Ok(grades);
        }
        #endregion

        #region Upload A Lecture Folder
        [HttpPost("UploadLectureFolder")]
        public IActionResult UploadLectureFolder(string title, string CycleId)
        {
            var lecture = new Lecture
            {
                CourseCycleId = CycleId,
                CreatedAt = DateTime.Now,
                LectureId=Guid.NewGuid().ToString(),
                Title=title,
                Type="Lecture"
            };
            _context.Lectures.Add(lecture);
            _context.SaveChanges();
            return Ok(lecture);

        }
        #endregion

        #region Update Lecture Folder
        [HttpPut("UpdateLectureFolderName")]
        public IActionResult updateLectureFolderName(string name,string lectureId)
        {
            var lecture =_context.Lectures.FirstOrDefault(l=>l.LectureId== lectureId);
            if (lecture == null)
            {
                return BadRequest();
            }
            lecture.Title = name;
            _context.Lectures.Update(lecture);
            _context.SaveChanges();
            return Ok(lecture);
        }
        #endregion

        #region Delete Lecture Folder
        [HttpDelete("DeleteLectureFolder")]
        public IActionResult deleteLectureFolder(string lectureId)
        {
            var lecture = _context.Lectures.FirstOrDefault(l => l.LectureId == lectureId);
            if (lecture == null)
            {
                return BadRequest();
            }
            var files = _context.LectureFiles.Where(f => f.LectureId == lectureId);
            if(files!=null)
            {
                _context.LectureFiles.RemoveRange(files);

            }
            _context.Lectures.Remove(lecture);
            _context.SaveChanges();
            return Ok("Deleted Succefully ");
        }
        #endregion

        #region Edit Student Grade
       [HttpPut("editStudentGrade")]
        public IActionResult editStudentGrade(string studentId,String examId,Double grade)
        {
            /*var quiz = from sqg in _context.StudentQuizGrades
                       where sqg.StudentId == studentId && sqg.QuizId == examId
                       select sqg;*/
            var quiz = _context.StudentQuizGrades.Where(q => q.QuizId == examId && q.StudentId==studentId).FirstOrDefault();
            if (quiz != null)
            {
                quiz.Grade=grade;
                quiz.CreatedAt = DateTime.Now;
                _context.StudentQuizGrades.Update(quiz);
                _context.SaveChanges();
                return Ok(quiz);
            }
            var task = _context.TaskAnswers.Where(t => t.TaskId == examId && t.StudentId == studentId).FirstOrDefault();
            if (task != null)
            {
                task.Grade = grade;
                task.CreatedAt = DateTime.Now;
                _context.TaskAnswers.Update(task);
                _context.SaveChanges();
                return Ok(task);
            }
            return BadRequest();
        }
        #endregion

        #region Get All Quizes for one course
        [HttpGet("GetAllQuizesForOneCourse")]
        public async Task<IActionResult>getAllQuizesForOneCourse(string cycleId)
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

            var userId = user.UserId;


            // Validate student enrollment in the course cycle
            /*var enrolled = _context.StudentEnrollments.Any(se => se.StudentId == userId && se.CourseCycleId == cycleId);
            if (!enrolled)
            {
                return Unauthorized("Student not enrolled in the specified course cycle");
            }*/
            var quizzes = (from q in _context.Quizzes
                           where q.CourseCycleId == cycleId && q.InstructorId==userId
                           select new AllQuizesDto
                           {
                               Id = q.QuizId,
                               Title = q.Title,
                               StartDate = (DateTime)q.StartDate,
                               EndDate = (DateTime)q.EndDate,
                               Status = (q.StartDate >= DateTime.Now && q.EndDate <= DateTime.Now) ? "Not Available" : " Available" ?? "is null"

                           })
                          .ToList();

            if (quizzes == null || !quizzes.Any())
            {
                return NotFound();
            }
            return Ok(quizzes);
        }
        #endregion

        #region Get Quiz by id
        [HttpGet("Quiz")]
        public async Task<IActionResult> GetQuiz(string quizId)
        {
            var quiz = await _context.Quizzes
                .Include(q => q.Questions)
                .ThenInclude(q => q.QuestionAnswers)
                .FirstOrDefaultAsync(q => q.QuizId == quizId);

            if (quiz == null)
            {
                return NotFound();
            }
            var quizDto = new QuizDto
            {
                Id = quiz.QuizId,
                Title = quiz.Title,
                // Notes = quiz.Notes,
                StartDate = (DateTime)quiz.StartDate,
                EndDate = (DateTime)quiz.EndDate,
                Duration = (TimeSpan)(quiz.EndDate - quiz.StartDate),
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

        #region Delete Quiz
        [HttpDelete("DeleteQuiz")]
        public IActionResult deleteQuiz(string quizId)
        {
            var quiz = _context.Quizzes.Include(q => q.Questions).ThenInclude(q => q.QuestionAnswers).FirstOrDefault(q => q.QuizId == quizId);

            if (quiz == null)
            {
                return NotFound();
            }

            var questions = quiz.Questions.ToList();

            foreach (var question in questions)
            {
                var answers = question.QuestionAnswers.ToList();
                _context.QuestionAnswers.RemoveRange(answers);
            }

            _context.Questions.RemoveRange(questions);
            _context.Quizzes.Remove(quiz);

            _context.SaveChanges();

            return Ok("Deleted Successfully");
        }
        #endregion

        #region Doctor or Instructor Create quiz 
        // POST: api/quiz/create
        [HttpPost("createQuiz")]
        public async Task<IActionResult> CreateQuiz(CreateQuizDto quizDto)
        {

            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (emailClaim == null)
            {
                return BadRequest("Email claim not found in token.");
            }

            var email = emailClaim.Value;

            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            var userId = user.UserId;

            var quiz = new Quiz
            {
                QuizId = Guid.NewGuid().ToString(),
                Title = quizDto.title,
                Notes = quizDto.notes,
                StartDate = quizDto.startDate,
                EndDate = quizDto.endDate,
                Grade = quizDto.grade,
                InstructorId = userId,
                CourseCycleId=quizDto.courseCycleId,
                CreatedAt = DateTime.Now,
            };
            _context.Quizzes.Add(quiz);
            await _context.SaveChangesAsync();

            foreach (var questionDto in quizDto.Questions)
            {
                var question = new Question
                {
                    QuestionId = Guid.NewGuid().ToString(),
                    Text = questionDto.text,
                    Type = questionDto.type,
                    QuestionNumber = questionDto.questionNumber,
                    Grade = questionDto.grade,
                    QuizId= quiz.QuizId,
                    CreatedAt = DateTime.Now ,
                };

                _context.Questions.Add(question);
                await _context.SaveChangesAsync();

                foreach (var answerDto in questionDto.Answers)
                {
                    var answer = new QuestionAnswer
                    {
                       
                        AnswerId = Guid.NewGuid().ToString(),
                        Text = answerDto.text,
                        IsCorrect = answerDto.isCorrect,
                        CreatedAt = DateTime.Now ,
                        AnswerNumber=answerDto.answerNumber,
                        QuestionId=question.QuestionId
                    };

                    _context.QuestionAnswers.Add(answer);
                    await _context.SaveChangesAsync();
                }
            }

            return Created("",quizDto);
            
            
        }


        #endregion

        #region Doctor or Instructor Update Quiz

        /*        [HttpPut("updateQuiz/{quizId}")]
                public async Task<IActionResult> UpdateQuiz(string quizId, CreateQuizDto updatedQuizDto)
                {

                        var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

                        if (emailClaim == null)
                        {
                            return BadRequest("Email claim not found in token.");
                        }

                        var email = emailClaim.Value;

                        var user = _context.Users.FirstOrDefault(u => u.Email == email);
                        var userId = user.UserId;

                        var quiz = await _context.Quizzes
                            .Include(q => q.Questions)
                            .ThenInclude(q => q.QuestionAnswers)
                            .FirstOrDefaultAsync(q => q.QuizId == quizId);

                        if (quiz == null)
                        {
                            return NotFound("Quiz not found.");
                        }

                        // Update quiz properties
                        quiz.Title = updatedQuizDto.title;
                        quiz.Notes = updatedQuizDto.notes;
                        quiz.StartDate = updatedQuizDto.startDate;
                        quiz.EndDate = updatedQuizDto.endDate;
                        quiz.Grade = updatedQuizDto.grade;
                        quiz.CourseCycleId = updatedQuizDto.courseCycleId;

                        // Update questions and answers
                        foreach (var updatedQuestionDto in updatedQuizDto.Questions)
                        {
                            var existingQuestion = quiz.Questions.FirstOrDefault(q => q.QuestionId == updatedQuestionDto.QuestionId);

                            if (existingQuestion != null)
                            {
                                // Update question properties
                                existingQuestion.Text = updatedQuestionDto.text;
                                existingQuestion.Type = updatedQuestionDto.type;
                                existingQuestion.QuestionNumber = updatedQuestionDto.questionNumber;
                                existingQuestion.Grade = updatedQuestionDto.grade;

                                // Update answers
                                foreach (var updatedAnswerDto in updatedQuestionDto.Answers)
                                {
                                    var existingAnswer = existingQuestion.QuestionAnswers.FirstOrDefault(a => a.AnswerId == updatedAnswerDto.AnswerId);

                                    if (existingAnswer != null)
                                    {
                                        // Update answer properties
                                        existingAnswer.Text = updatedAnswerDto.text;
                                        existingAnswer.IsCorrect = updatedAnswerDto.isCorrect;
                                        existingAnswer.AnswerNumber = updatedAnswerDto.answerNumber;
                                    }
                                    else
                                    {
                                        // Add new answer
                                        var newAnswer = new QuestionAnswer
                                        {
                                            //AnswerId = Guid.NewGuid().ToString(),
                                            //AnswerId = updatedAnswerDto.AnswerId,
                                            Text = updatedAnswerDto.text,
                                            IsCorrect = updatedAnswerDto.isCorrect,
                                            AnswerNumber = updatedAnswerDto.answerNumber,
                                            QuestionId = existingQuestion.QuestionId,
                                            CreatedAt = DateTime.Now
                                        };
                                        _context.QuestionAnswers.Add(newAnswer);
                                    }
                                }
                            }
                            else
                            {
                                // Add new question
                                var newQuestion = new Question
                                {
                                    //QuestionId = Guid.NewGuid().ToString(),
                                    //QuestionId = updatedQuestionDto.QuestionId,
                                    Text = updatedQuestionDto.text,
                                    Type = updatedQuestionDto.type,
                                    QuestionNumber = updatedQuestionDto.questionNumber,
                                    Grade = updatedQuestionDto.grade,
                                    QuizId = quiz.QuizId,
                                    CreatedAt = DateTime.Now
                                };
                                _context.Questions.Add(newQuestion);

                                // Add answers for the new question
                                foreach (var answerDto in updatedQuestionDto.Answers)
                                {
                                    var newAnswer = new QuestionAnswer
                                    {
                                        // AnswerId = Guid.NewGuid().ToString(),
                                        //AnswerId = answerDto.AnswerId,
                                        Text = answerDto.text,
                                        IsCorrect = answerDto.isCorrect,
                                        AnswerNumber = answerDto.answerNumber,
                                        QuestionId = newQuestion.QuestionId,
                                        CreatedAt = DateTime.Now
                                    };
                                    _context.QuestionAnswers.Add(newAnswer);
                                }
                            }
                        }

                        await _context.SaveChangesAsync();

                        return Ok("Quiz updated successfully.");




                }        */





        /*[HttpPut("updateQuiz/{quizId}")]
        public async Task<IActionResult> UpdateQuiz(string quizId, CreateQuizDto quizDto)
        {
            var quiz = _context.Quizzes.Include(q => q.Questions).ThenInclude(q => q.QuestionAnswers)
                        .FirstOrDefault(q => q.QuizId == quizId);

            if (quiz == null)
            {
                return NotFound();
            }

            quiz.Title = quizDto.title;
            quiz.Notes = quizDto.notes;
            quiz.StartDate = quizDto.startDate;
            quiz.EndDate = quizDto.endDate;
            quiz.Grade = quizDto.grade;
            quiz.CourseCycleId = quizDto.courseCycleId;
            quiz.CreatedAt = DateTime.Now; // Assuming you have an UpdatedAt field

            // Remove existing questions and answers
            var existingQuestions = quiz.Questions.ToList();
            foreach (var question in existingQuestions)
            {
                var existingAnswers = question.QuestionAnswers.ToList();
                _context.QuestionAnswers.RemoveRange(existingAnswers);
            }
            _context.Questions.RemoveRange(existingQuestions);
            await _context.SaveChangesAsync();

            // Add updated questions and answers
            foreach (var questionDto in quizDto.Questions)
            {
                var question = new Question
                {
                    QuestionId = Guid.NewGuid().ToString(),
                    Text = questionDto.text,
                    Type = questionDto.type,
                    QuestionNumber = questionDto.questionNumber,
                    Grade = questionDto.grade,
                    QuizId = quiz.QuizId,
                    CreatedAt = DateTime.Now,
                };

                _context.Questions.Add(question);
                await _context.SaveChangesAsync();

                foreach (var answerDto in questionDto.Answers)
                {
                    var answer = new QuestionAnswer
                    {
                        AnswerId = Guid.NewGuid().ToString(),
                        Text = answerDto.text,
                        IsCorrect = answerDto.isCorrect,
                        CreatedAt = DateTime.Now,
                        AnswerNumber = answerDto.answerNumber,
                        QuestionId = question.QuestionId
                    };

                    _context.QuestionAnswers.Add(answer);
                    await _context.SaveChangesAsync();
                }
            }

            return Ok("Quiz updated successfully");
        }*/

        #endregion


    }
}
