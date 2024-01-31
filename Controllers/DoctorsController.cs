using CRUDApi.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRUDApi.Data;
using CRUDApi.Entities;
using CRUDApi.DTOs;

namespace CRUDApi.Controllers
{
    [Route("api/Cources")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly ApiDbContext _context;
        public DoctorsController(ApiDbContext context)
        {
            _context = context;
        }

        // Cources EndPoints

        #region Get All Cources
        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            return Ok(await _context.Courses.ToListAsync());
        }
        #endregion

        #region Get Cource By Id
        [HttpGet("{Id}")]
        public ActionResult<Course> GetCourceByIdAsync(string Id)
        {
            var Cources = _context.Courses.Find(Id);
            if(Cources == null)
            {
                return NotFound();
            }
            return Cources;
        }
        #endregion

        #region Create Cource
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CourceDTO courceDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var course = new Course
            {
                CourseId = courceDTO.CourseId,
                Name = courceDTO.Name,
                Hours = courceDTO.Hours,
                CreatedAt = DateTime.UtcNow,
                ImagePath = courceDTO.ImagePath,
                FacultyId = courceDTO.FacultyId,
            };

            await _context.Courses.AddAsync(course);
            await _context.SaveChangesAsync();
            return Ok();
        }
        #endregion

        #region Update Cource
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(string id,[FromBody] CourceDTO courceDTO)
        {
            if (id != courceDTO.CourseId)
            {
                return BadRequest("CourseId in the request body does not match the route parameter.");
            }
            var existingCourse = await _context.Courses.FindAsync(id);
            if (existingCourse == null)
            {
                return NotFound();
            }

            existingCourse.Name = courceDTO.Name;
            existingCourse.Hours = courceDTO.Hours;
            existingCourse.CreatedAt = courceDTO.CreatedAt;
            existingCourse.ImagePath = courceDTO.ImagePath;
            existingCourse.FacultyId = courceDTO.FacultyId;

            await _context.SaveChangesAsync();
            return Ok();
        }
        #endregion

        #region Delete Cource
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var Cource = await _context.Courses.FindAsync(id);

            if(Cource == null)
                return NotFound();

            _context.Courses.Remove(Cource);
            await _context.SaveChangesAsync();
            return Ok();
        }
        #endregion



    }
}
