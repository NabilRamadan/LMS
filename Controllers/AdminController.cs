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
using System.Security.Claims;
namespace CRUDApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly LMSContext _context;
        public AdminController(LMSContext context)
        {
            _context = context;
        }

        private string? CurrentSemester()
        {
            var semester = _context.Semesters
                .OrderByDescending(s => s.CreatedAt)
                .Select(s => s.SemesterId)
                .FirstOrDefault();
            return semester;
        }

        private bool isAdmin(string email)
        {
            /*var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (emailClaim == null)
            {
                return false;
            }
            var email = emailClaim.Value;*/

            var user = (from u in _context.Users
                       join ur in _context.UserRoles on u.UserId equals ur.UserId
                       where u.Email == email
                       select ur.RoleId).FirstOrDefault();

            //var userRole = user.UserRoles.Select(us => us.RoleId).First();
            Console.WriteLine( "============"+ user);
            if (user!= "ROLE001")
            {
                return false;
            }
            return true;

        }
        #region Get Current User Info
        [HttpGet("Get Admin Info")]
        public async Task<IActionResult> GetAdminInfo()
        {
            
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (emailClaim == null)
            {
                return BadRequest("Email claim not found in token.");
            }
            var email = emailClaim.Value;
            if (!isAdmin(email))
            {
                return Unauthorized();
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            var userId = user.UserId;


            var adminInfo = await (from us in _context.Users
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



            if (adminInfo == null)
            {
                return NotFound("User not found.");
            }

            return Ok(adminInfo);
        }
        #endregion
        #region Admin Info
        /*[HttpGet("Admin-Info")]
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
        }*/

        #endregion

        #region Get All Courses In All Semesteres
        [HttpGet("Get All Courses In All Semesters")]
        public async Task< IActionResult> GetAllCoursesInSemester()
        {
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (emailClaim == null)
            {
                return BadRequest("Email claim not found in token.");
            }
            var email = emailClaim.Value;
            if (!isAdmin(email))
            {
                return Unauthorized();
            }

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
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (emailClaim == null)
            {
                return BadRequest("Email claim not found in token.");
            }
            var email = emailClaim.Value;
            if (!isAdmin(email))
            {
                return Unauthorized();
            }
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
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (emailClaim == null)
            {
                return BadRequest("Email claim not found in token.");
            }
            var email = emailClaim.Value;
            if (!isAdmin(email))
            {
                return Unauthorized();
            }
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
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (emailClaim == null)
            {
                return BadRequest("Email claim not found in token.");
            }
            var email = emailClaim.Value;
            if (!isAdmin(email))
            {
                return Unauthorized();
            }
            var course = new Course
            {
                CourseId = Guid.NewGuid().ToString(),
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

        #region Get All Departments with number of students
        [HttpGet("Get All Departments with number of students")]
        public async Task< IActionResult> GetAllStudentsWithDepartmentname()
        {
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (emailClaim == null)
            {
                return BadRequest("Email claim not found in token.");
            }
            var email = emailClaim.Value;
            if (!isAdmin(email))
            {
                return Unauthorized();
            }
            var students =await (from d in _context.Departments
                            select new GetDepartmentStudentsDto
                            {
                                departmentId=d.DepartmentId,
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
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (emailClaim == null)
            {
                return BadRequest("Email claim not found in token.");
            }
            var email = emailClaim.Value;
            if (!isAdmin(email))
            {
                return Unauthorized();
            }
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

        #region Get All CourseCycleIDs in  semester 
         [HttpGet("Get Semester By ID/{semesterId}")]
         public async Task< IActionResult> GetSemesterByID(string semesterId)
         {
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (emailClaim == null)
            {
                return BadRequest("Email claim not found in token.");
            }
            var email = emailClaim.Value;
            if (!isAdmin(email))
            {
                return Unauthorized();
            }
            //var Semester = _context.Semesters.FirstOrDefault(s=>s.SemesterId==semesterId);
            var semester =await (from s in _context.Semesters
                            join cs in _context.CourseSemesters on s.SemesterId equals cs.SemesterId
                            join c in _context.Courses on cs.CourseId equals c.CourseId
                            join ics in _context.InstructorCourseSemesters on cs.CycleId equals ics.CourseCycleId
                            join u in _context.Users on ics.InstructorId equals u.UserId
                            where s.SemesterId == semesterId
                            select new GetSemesterByIDDto
                            {
                                cycleId= cs.CycleId,
                                courseName= c.Name,
                                instructorName= u.FullName

                            }).ToListAsync();
            return Ok(semester);

         }

        #endregion

        #region Get All Courses To Create Semester
        [HttpGet("Get All Courses")]
        public async Task<IActionResult> GetAllCourses()
        {
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (emailClaim == null)
            {
                return BadRequest("Email claim not found in token.");
            }
            var email = emailClaim.Value;
            if (!isAdmin(email))
            {
                return Unauthorized();
            }
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
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (emailClaim == null)
            {
                return BadRequest("Email claim not found in token.");
            }
            var email = emailClaim.Value;
            if (!isAdmin(email))
            {
                return Unauthorized();
            }
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
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (emailClaim == null)
            {
                return BadRequest("Email claim not found in token.");
            }
            var email = emailClaim.Value;
            if (!isAdmin(email))
            {
                return Unauthorized();
            }


            if (createSemesterDto == null)
            {
                return BadRequest();
            }
            var existingSemester = await _context.Semesters.FindAsync(createSemesterDto.semesterId);
            if (existingSemester != null)
            {
                return Conflict("Semester ID already exists");
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
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (emailClaim == null)
            {
                return BadRequest("Email claim not found in token.");
            }
            var email = emailClaim.Value;
            if (!isAdmin(email))
            {
                return Unauthorized();
            }
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

            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (emailClaim == null)
            {
                return BadRequest("Email claim not found in token.");
            }
            var email = emailClaim.Value;
            if (!isAdmin(email))
            {
                return Unauthorized();
            }


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
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (emailClaim == null)
            {
                return BadRequest("Email claim not found in token.");
            }
            var email = emailClaim.Value;
            if (!isAdmin(email))
            {
                return Unauthorized();
            }

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
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (emailClaim == null)
            {
                return BadRequest("Email claim not found in token.");
            }
            var email = emailClaim.Value;
            if (!isAdmin(email))
            {
                return Unauthorized();
            }

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
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (emailClaim == null)
            {
                return BadRequest("Email claim not found in token.");
            }
            var email = emailClaim.Value;
            if (!isAdmin(email))
            {
                return Unauthorized();
            }
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
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (emailClaim == null)
            {
                return BadRequest("Email claim not found in token.");
            }
            var email = emailClaim.Value;
            if (!isAdmin(email))
            {
                return Unauthorized();
            }
            if (departmentDto == null)
            {
                return BadRequest(new { Message = "Invalid department data." });
            }

            var department = new Department
            {
                DepartmentId = Guid.NewGuid().ToString(),
                FacultyId = departmentDto.FacultyId,
                Name = departmentDto.Name,
                CreatedAt = DateTime.UtcNow
            };

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Department created successfully." });
        }
        #endregion

        #region Get All student Ids To enroll Them in Courses *
        /*[HttpGet("Get All student Ids To enroll Them in Courses")]
        public async Task<IActionResult> GetAllstudentIdsToenrollTheminCourses()
        {
            var students =await (from u in _context.Users
                            join r in _context.UserRoles on u.UserId equals r.UserId
                            where r.RoleId== "ROLE003"
                            select new GetAllstudentIdsDto
                            {
                                studentId = u.UserId,
                                studentName = u.FullName,
                                studentEmail=u.Email
                            }).ToListAsync();
            if(students==null) 
            {
                return BadRequest();

            }
            return Ok(students);

        }*/
        #endregion

        #region Get All student Ids To enroll Them in Courses 
        [HttpGet("Get All student Ids To enroll Them in Courses")]
        public async Task<IActionResult> GetAllstudentIdsToenrollTheminCourses([FromQuery] string email = null)
        {
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (emailClaim == null)
            {
                return BadRequest("Email claim not found in token.");
            }
            var emai1 = emailClaim.Value;
            if (!isAdmin(emai1))
            {
                return Unauthorized();
            }
            var studentsQuery = from u in _context.Users
                                join r in _context.UserRoles on u.UserId equals r.UserId
                                where r.RoleId == "ROLE003"
                                select new GetAllstudentIdsDto
                                {
                                    studentId = u.UserId,
                                    studentName = u.FullName,
                                    studentEmail = u.Email
                                };

            if (!string.IsNullOrEmpty(email))
            {
                studentsQuery = studentsQuery.Where(s => s.studentEmail == email);
            }

            var students = await studentsQuery.ToListAsync();

            if (students == null || students.Count == 0)
            {
                return NotFound();
            }

            return Ok(students);
        }
        #endregion

        #region Get All Main Courses
        [HttpGet("Get All Main Courses")]
        public async Task<ActionResult<GetAllMainCoursesDto>> GetAllMainCourses()
        {
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (emailClaim == null)
            {
                return BadRequest("Email claim not found in token.");
            }
            var email = emailClaim.Value;
            if (!isAdmin(email))
            {
                return Unauthorized();
            }
            var courses =await (from c in _context.Courses
                           select new GetAllMainCoursesDto
                           {
                               courseId = c.CourseId,
                               courseName = c.Name,
                               courseHoures = c.Hours
                           }).ToListAsync();
            if (courses == null)
            {
                return BadRequest("There is no Courses");
            }
            return Ok(courses);
        }



        #endregion

        #region Delete Course
        [HttpDelete("Delete Course/{courseId}")]
        public async Task<IActionResult> DeleteCourse(string courseId)
        {
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (emailClaim == null)
            {
                return BadRequest("Email claim not found in token.");
            }
            var email = emailClaim.Value;
            if (!isAdmin(email))
            {
                return Unauthorized();
            }
            var course=await _context.Courses.FirstOrDefaultAsync(c=>c.CourseId==courseId);
            if (course == null)
            {
                return NotFound();
            }
            var test=await _context.CourseSemesters.FirstOrDefaultAsync(c=>c.CourseId == courseId);
            if (test != null)
            {
                return BadRequest("Cant Delete This Course");
            }
            _context.Courses.Remove(course);
            _context.SaveChanges();
            return Ok("Deleted Succefully");
        }
        #endregion

        #region Delete Semester
        [HttpDelete("Delete Semester")]
        public async Task<IActionResult> DeleteSemester(string semesterId)
        {
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (emailClaim == null)
            {
                return BadRequest("Email claim not found in token.");
            }
            var email = emailClaim.Value;
            if (!isAdmin(email))
            {
                return Unauthorized();
            }
            var semester = await _context.Semesters.FirstOrDefaultAsync(s => s.SemesterId== semesterId);
            if (semester == null)
            {
                return NotFound();
            }
            var test = await _context.CourseSemesters.FirstOrDefaultAsync(c => c.SemesterId == semesterId);
            if (test != null)
            {
                return BadRequest("Cant Delete This Semester");
            }
            _context.Semesters.Remove(semester);
            _context.SaveChanges();
            return Ok("Deleted Succefully");

        }
        #endregion

        #region Get All Instructors Ids
        [HttpGet("Get All Instructors Ids")]
        public async Task<IActionResult> GetAllInstructorsIds([FromQuery] string email = null)
        {

            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (emailClaim == null)
            {
                return BadRequest("Email claim not found in token.");
            }
            var userEmail = emailClaim.Value;
            if (!isAdmin(userEmail))
            {
                return Unauthorized();
            }

            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);
            var userId = user.UserId;

            var staffQuery = from u in _context.Users
                                join ur in _context.UserRoles on u.UserId equals ur.UserId
                                join r in _context.Roles on ur.RoleId equals r.RoleId
                                where ur.RoleId != "ROLE003"&&u.UserId!= userId
                                select new GetAllInstructorsIdsDto
                                {
                                    staffEmail = u.Email,
                                    staffId = u.UserId,
                                    staffName=u.FullName,
                                    staffrole=r.Name
                                };
            if (!string.IsNullOrEmpty(email))
            {
                staffQuery = staffQuery.Where(s => s.staffEmail == email);
            }

            var students = await staffQuery.ToListAsync();

            if (students == null || students.Count == 0)
            {
                return NotFound();
            }

            return Ok(students);
        }
        #endregion

        #region Activate Student Acount
         [HttpPut("Activate Student Acount/{studentId}")]
          public async Task<IActionResult> ActivateStudentAcount(string studentId,string status)
          {
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (emailClaim == null)
            {
                return BadRequest("Email claim not found in token.");
            }
            var email = emailClaim.Value;
            if (!isAdmin(email))
            {
                return Unauthorized();
            }

            var student =await (from u in _context.Users
                            join r in _context.UserRoles on u.UserId equals r.UserId
                            where u.UserId== studentId&& r.RoleId == "ROLE003"
                                  select u).FirstOrDefaultAsync();
              if (student == null)
              {
                  return NotFound();
              }
              student.Status= status;
              _context.Users.Update(student);
              _context.SaveChanges();
              return Ok(student.FullName+" IS "+student.Status);


  
        }
        #endregion

        #region Get All CyclesIds For Semester
        [HttpGet("Get All CyclesIds For Semester")]
        public async Task<IActionResult> GetAllCyclesIdsForSemester(string semesterId,string studentId)
        {
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (emailClaim == null)
            {
                return BadRequest("Email claim not found in token.");
            }
            var email = emailClaim.Value;
            if (!isAdmin(email))
            {
                return Unauthorized();
            }
            var cycles =await (from cs in _context.CourseSemesters
                          join c in _context.Courses on cs.CourseId equals c.CourseId
                          join ics in _context.InstructorCourseSemesters on cs.CycleId equals ics.CourseCycleId
                          join u in _context.Users on ics.InstructorId equals u.UserId
                          where cs.SemesterId == semesterId
                          select new GetAllCyclesIdsForSemesterDto
                          {
                              courseName = c.Name,
                              cycleId = cs.CycleId,
                              instructorFullName = u.FullName
                          }).ToListAsync();
            var studentCycles = from se in _context.StudentEnrollments
                                join cs in _context.CourseSemesters on se.CourseCycleId equals cs.CycleId
                                join s in _context.Semesters on cs.SemesterId equals s.SemesterId
                                where s.SemesterId == semesterId && se.StudentId == studentId
                                select cs.CycleId;
            var data = new List<GetAllCyclesIdsForSemesterDto>();
            foreach (var cycle in cycles)
            {
                if(!studentCycles.Contains(cycle.cycleId))
                {
                    data.Add(cycle);
                }
            }
            return Ok(data);
        }
        #endregion



    }
}
