using CRUDApi.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CRUDApi.DTOs;
using CRUDApi.Models;
using Octokit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
namespace CRUDApi.Controllers
{
    [Route("api/[controller]")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly LMSContext _context;
        public AdminController(LMSContext context)
        {
            _context = context;
        }
        #region Admin Info
        [HttpGet("Admin-Info")]
        public async Task<IActionResult> GetAdminDetails()
        {
            var adminRole = await _context.Roles
                .Include(r => r.UserRoles)
                .ThenInclude(ur => ur.User)
                .FirstOrDefaultAsync(r => r.Name == "Admin");

            if (adminRole == null)
            {
                return NotFound("Admin role not found.");
            }

            var adminUsers = adminRole.UserRoles.Select(ur => ur.User).ToList();

            if (!adminUsers.Any())
            {
                return NotFound("No admin users found.");
            }

            var adminUser = adminUsers.FirstOrDefault();

            var adminDetails = new
            {
                adminUser.UserId,
                adminUser.FullName,
                adminUser.Email,
                adminUser.Phone,
                adminUser.ImagePath,
                adminUser.FacultyId,
                adminUser.CreatedAt,
                adminUser.Status,
                Roles = adminUser.UserRoles.Select(ur => ur.Role.Name).ToList(),
            };

            return Ok(adminDetails);
        }

        #endregion

        #region Get All Courses In Semester
        [HttpGet("Get All Courses In Semester")]
        public async Task< IActionResult> GetAllCoursesInSemester()
        {
            var semesterCourses =await (from s in _context.Semesters
                                   select new AllCoursesInSemesterDto
                                   {
                                       semesterId = s.SemesterId,
                                       semesterNumber = s.Number,
                                       courses = (from cs in _context.CourseSemesters
                                                  join c in _context.Courses on cs.CourseId equals c.CourseId
                                                  join ics in _context.InstructorCourseSemesters on cs.CycleId equals ics.CourseCycleId
                                                  join u in _context.Users on ics.InstructorId equals u.UserId
                                                  where cs.SemesterId == s.SemesterId
                                                  select new CourseInSemesterDto
                                                  {
                                                      courseName = c.Name,
                                                      courseId = c.CourseId,
                                                      instructorFullName = u.FullName,
                                                      numberOfStudent = _context.StudentEnrollments
                                                                           .Where(se => se.CourseCycleId == cs.CycleId)
                                                                           .Count()
                                                  }).ToList()

                                   }).ToListAsync();
            return Ok(semesterCourses);
        }
        #endregion

        #region Get Course by ID
        [HttpGet("Get Course by ID")]
        public IActionResult GetCourseById(string courseId)
        {
            var course = from c in _context.Courses
                         where c.CourseId == courseId
                         select new GetCourseDataDto
                         {
                             Name = c.Name,
                             Hours = c.Hours,
                             ImagePath = c.ImagePath,
                             CreatedAt = c.CreatedAt,
                             CourseId = c.CourseId,
                             FacultyId = c.FacultyId
                         };
            if (course == null)
            {
                return NotFound("Course Not Found ....!");
            }
            return Ok(course);
        }
        #endregion

        #region Edit Course
        [HttpPut("Edit Course/{courseId}")]
        public IActionResult EditCourse(string courseId, UpdateCourceDto updateCourceDto)
        {
            var course = _context.Courses.FirstOrDefault(c => c.CourseId == courseId);
            if (course == null)
            {
                return NotFound("Course Not Found ....!");
            }
            if (updateCourceDto.Name != null)
            {
                course.Name = updateCourceDto.Name;
            }

            if (updateCourceDto.Hours != null)
            {
                course.Hours = updateCourceDto.Hours;
            }

            if (updateCourceDto.ImagePath != null)
            {
                course.ImagePath = updateCourceDto.ImagePath;
            }
            _context.Courses.Update(course);
            _context.SaveChanges();
            return AcceptedAtAction(nameof(GetCourseById), new { courseId = course.CourseId }, course);

        }


        #endregion

        #region Create Course
        [HttpPost("Create Course")]
        public IActionResult CreatCourse(CreateCourseDto createCourseDto)
        {
            var course = new Course
            {
                CourseId = createCourseDto.CourseId,
                Name = createCourseDto.Name,
                CreatedAt = DateTime.Now,
                Hours = createCourseDto.Hours,
                ImagePath = createCourseDto.ImagePath,
                FacultyId = "FAC001"
            };
            _context.Courses.Add(course);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetCourseById), new { courseId = course.CourseId }, course);
        }
        #endregion

        #region Get All Students With Department name
        [HttpGet("Get All Students With Department name")]
        public async Task< IActionResult> GetAllStudentsWithDepartmentname()
        {
            var students =await (from d in _context.Departments
                            select new GetDepartmentStudentsDto
                            {
                                departmentName = d.Name,
                                numberofstudent = _context.StudentInfos
                                .Count(si => si.DepartmentId == d.DepartmentId)
                            }).ToListAsync();
            return Ok(students);
        }
        #endregion

        #region Get All Semesters
        [HttpGet("Get All Semeters")]
        public async  Task<IActionResult>getallsemesters(string facultyId = "FAC001")
        {
            var semester =await (from s in _context.Semesters
                            where s.FacultyId == facultyId
                            select new GetSemestersDto
                            {
                                SemesterId = s.SemesterId,
                                StartDate = s.StartDate,
                                EndDate = s.EndDate,
                                Number = s.Number,
                                Years = s.Years

                            }).ToListAsync();
            
            return Ok(semester);

        }
        #endregion

        #region Get All CourseCycleIDs in ONe Semester *
        /* [HttpGet("Get Semester By ID/{semesterId}")]
         public IActionResult GetSemesterByID(string semesterId)
         {
             var semester = _context.Semesters.FirstOrDefault(s=>s.SemesterId==semesterId);

         }*/

        #endregion

        #region Get All Courses To Create Semester
        [HttpGet("Get All Courses")]
        public async Task<IActionResult> GetAllCourses()
        {
            var courses = await(from c in _context.Courses
                           select new GetAllCoursesToCreateSemesterDto
                           {
                               courseID = c.CourseId,
                               courseName = c.Name
                           }).ToListAsync();
            return Ok(courses);
        }
        #endregion

        #region Get All Instrucors To Create Semester
        [HttpGet("Get All Instructors")]
        public async Task<IActionResult> GetAllDoctors()
        {
            var doctors = await(from i in _context.Users
                           join r in _context.UserRoles on i.UserId equals r.UserId
                           where r.RoleId == "ROLE002"
                           select new GetAllDoctorsToCreateSemesterDto
                           {
                               instructorId= i.UserId,
                               instructorName= i.FullName
                           }).ToListAsync();
            return Ok(doctors);


        }
        #endregion

        #region Create Semester
        [HttpPost("Create Semester")]
        public async Task<IActionResult> CreateSemester(CreateSemesterDto createSemesterDto)
        {
            if(createSemesterDto == null)
            {
                return BadRequest();
            }
            var semester=new Semester
            {
                SemesterId= createSemesterDto.semesterId,
                CreatedAt= createSemesterDto.createdAt,
                EndDate= createSemesterDto.endDate,
                FacultyId=  "FAC001",
                Number= createSemesterDto.number,
                StartDate= createSemesterDto.startDate,
                Years= createSemesterDto.years,
            };
            _context.Semesters.Add(semester);
            await _context.SaveChangesAsync();
            return Created("",semester);

        }
        #endregion

        #region Add Courses And Instructors to Semester
        [HttpPost("Add Courses And Instructors to Semester")]
        public async Task<IActionResult> AddCoursesAndInstructorstoSemester(string semesterId,string courseId,string instructorId)
        {
            var semester =await _context.Semesters.FirstOrDefaultAsync(s => s.SemesterId == semesterId);
            if (semester == null)
            {
                return BadRequest();
            }
            var courceSemsterNumber=_context.CourseSemesters.Count(cs=>cs.CycleId.Contains($"{courseId}{semesterId}"));
            var courseSemester = new CourseSemester
            {
                CycleId= $"{courseId}{semesterId}{courceSemsterNumber}",
                CourseId= courseId,
                SemesterId= semesterId,
                CreatedAt=DateTime.Now,
                
            };
            //ait Console.Out.WriteLineAsync("the cycleId Is "+courseSemester.CycleId );
            _context.CourseSemesters.Add(courseSemester);
            var instructorCourseSemester = new InstructorCourseSemester
            {
                CourseCycleId = courseSemester.CycleId,
                InstructorId = instructorId,
                CreatedAt = DateTime.Now
            };
            _context.InstructorCourseSemesters.Add(instructorCourseSemester);

            await _context.SaveChangesAsync();
            return Ok("Created Succefully...");

        }

        #endregion

        #region Enroll the student To the The Courses in Semester
         [HttpPost("Enroll the student To the The Courses in Semester")]
         public async Task<IActionResult> EnrollthestudentTotheTheCoursesinSemester(string studentId,string courseCycleId)
         {
             if (string.IsNullOrWhiteSpace(studentId) || string.IsNullOrWhiteSpace(courseCycleId))
             {
                 return BadRequest("Student ID and Course Cycle ID cannot be null or empty.");
             }

             var studentExists = await _context.Users.AnyAsync(s => s.UserId == studentId);
             var courseCycleExists = await _context.CourseSemesters.AnyAsync(c => c.CycleId == courseCycleId);

             if (!studentExists|| !courseCycleExists)
             {
                 return NotFound($"Student with ID {studentId} OR Course cycle with ID {courseCycleId} does not exist. ");
             }
             var studentEnrollment = new StudentEnrollment
             {
                 StudentId = studentId,
                 CourseCycleId = courseCycleId,
                 CreatedAt = DateTime.Now
             };
            await _context.StudentEnrollments.AddAsync(studentEnrollment);
             await _context.SaveChangesAsync();
             return Ok("Student Enrolled Succefully ");

         }
 

        #endregion

        #region Enroll List Of Course to One Student in Semester
        [HttpPost("Enroll List Of Course to One Student in Semester")]
        public async Task<IActionResult> EnrollListOfCoursetoOneStudentinSemester(string studentId, List<string> courseCycleId)
        {
            /*if (string.IsNullOrWhiteSpace(studentId) )
            {
                return BadRequest("Student ID OR Course Cycle ID cannot be null or empty.");
            }*/

            var studentExists = await _context.Users.AnyAsync(s => s.UserId == studentId);
            if (!studentExists)
            {
                return NotFound($"Student with ID {studentId} does not exist. ");
            }

            foreach (var course in courseCycleId)
            {
                var courseCycleExists = await _context.CourseSemesters.AnyAsync(c => c.CycleId == course);

                if (!studentExists || !courseCycleExists)
                {
                    return NotFound($" Course cycle with ID {course} does not exist. ");
                }
                var studentEnrollment = new StudentEnrollment
                {
                    StudentId = studentId,
                    CourseCycleId = course,
                    CreatedAt = DateTime.Now
                };
                await _context.StudentEnrollments.AddAsync(studentEnrollment);
            }
            await _context.SaveChangesAsync();
            return Ok("Student Enrolled Succefully ");

        }
        #endregion

        #region Enroll List Of Student To One Course in Semester
        [HttpPost("Enroll List Of Student To One Course in Semester")]
        public async Task<IActionResult> EnrollListOfStudentToOneCourseinSemester(List<string> studentId, string courseCycleId)
        {
            /*if (string.IsNullOrWhiteSpace(studentId) )
            {
                return BadRequest("Student ID OR Course Cycle ID cannot be null or empty.");
            }*/

            var courseCycleExists = await _context.CourseSemesters.AnyAsync(c => c.CycleId == courseCycleId);
            if (!courseCycleExists)
            {
                return NotFound($"Course with ID {courseCycleId} does not exist. ");
            }

            foreach (var student in studentId)
            {
                var studentExists = await _context.Users.AnyAsync(u => u.UserId == student);

                if (!studentExists )
                {
                    return NotFound($" Student with ID {student} does not exist. ");
                }
                var studentEnrollment = new StudentEnrollment
                {
                    StudentId = student,
                    CourseCycleId = courseCycleId,
                    CreatedAt = DateTime.Now
                };
                await _context.StudentEnrollments.AddAsync(studentEnrollment);
            }
            await _context.SaveChangesAsync();
            return Ok("Student Enrolled Succefully ");

        }

        #endregion

        #region Department Details


        [HttpGet("Department Info")]
        public async Task<ActionResult<IEnumerable<DepartmentDetailDto>>> GetDepartmentsWithStudents()
        {
            var departments = await _context.Departments
                .Include(d => d.StudentInfos)
                .ThenInclude(si => si.User)
                .Select(d => new DepartmentDetailDto
                {
                    DepartmentId = d.DepartmentId,
                    FacultyId = d.FacultyId,
                    Name = d.Name,
                    CreatedAt = d.CreatedAt,
                    Students = d.StudentInfos.Select(si => new StudentDepartmentInfoDto
                    {
                        AcademicId = si.AcademicId,
                        UserId = si.UserId,
                        Level = si.Level,
                        UserName = si.User.FullName
                    }).ToList()
                })
                .ToListAsync();

            return Ok(departments);
        }


        #endregion

        #region Create Department
        [HttpPost("Create Department")]
        public async Task<ActionResult> CreateDepartment(CreateDepartmentDto departmentDto)
        {
            var department = new Department
            {
                DepartmentId = departmentDto.DepartmentId,
                FacultyId = departmentDto.FacultyId,
                Name = departmentDto.Name,
                CreatedAt = DateTime.UtcNow
            };

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Department created" });
        } 
        #endregion







    }
}
