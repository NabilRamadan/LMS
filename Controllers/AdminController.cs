﻿using CRUDApi.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CRUDApi.DTOs;
using CRUDApi.Models;
using Octokit;
using Microsoft.EntityFrameworkCore;
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

        #region GetAllCoursesInSemester
        [HttpGet("GetAllCoursesInSemester")]
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
                                .Where(si => si.DepartmentId == d.DepartmentId).Count()
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

        #region Get Semester By ID *
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




    }
}
