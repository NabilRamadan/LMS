using CRUDApi.Context;
using CRUDApi.DTOs;
using CRUDApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks.Dataflow;

namespace CRUDApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarController : BaseApiController
    {

        private readonly LMSContext _context;

        public CalendarController(LMSContext context)
        {
            _context = context;
        }

        #region Get Events By Start And End
        [HttpGet("GetByStartAndEnd")]
        public async Task<IActionResult> getByStartAndEndDtae(DateTime start, DateTime end)
        {
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (emailClaim == null)
            {
                return BadRequest("Email claim not found in token.");
            }

            var email = emailClaim.Value;

            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            var studentId = user.UserId;

            var Data = (from cal in _context.Calendars
                        where cal.StartDate>=start && cal.StartDate<end&& cal.UserId == studentId
                        select new PostCalendarDto
                        {
                            Start = cal.StartDate,
                            End = cal.EndDate,
                            Body = cal.Body
                        }).ToList();
            if (Data == null)
            {
                return NoContent();
            }
            return Ok(Data);
        }

        #endregion

        #region Get All Events of Calendar
        [HttpGet("GetAllCalendar")]
        public async Task<ActionResult<IEnumerable<CalendarDto>>> GetCalendars()
        {
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (emailClaim == null)
            {
                return BadRequest("Email claim not found in token.");
            }

            var email = emailClaim.Value;

            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            var studentId = user.UserId;
            var calendars = await _context.Calendars
                .Where(c=>c.UserId== studentId)
                .Select(c => new CalendarDto
            {
                CalendarId = c.CalendarId,
                UserId = c.UserId,
                Start = c.StartDate,
                End = c.EndDate,
                Body = c.Body
            }).ToListAsync();

            return calendars;
        }

        #endregion

        #region Get Event By Calendar Id
        [HttpGet("{id}")]
        public async Task<ActionResult<CalendarDto>> GetCalendar(string id)
        {
            var calendar = await _context.Calendars.FindAsync(id);

            if (calendar == null)
            {
                return NotFound();
            }

            var calendarDto = new CalendarDto
            {
                CalendarId = calendar.CalendarId,
                UserId = calendar.UserId,
                Start = calendar.StartDate,
                End = calendar.EndDate,
                Body = calendar.Body
            };

            return calendarDto;
        }
        #endregion

        #region Create New Event In Calendar
        [HttpPost]
        public async Task<ActionResult<CalendarDto>> PostCalendar(PostCalendarDto postCalendarDto)
        {
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (emailClaim == null)
            {
                return BadRequest("Email claim not found in token.");
            }

            var email = emailClaim.Value;

            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            var studentId = user.UserId;

            var calendar = new Calendar
            {
                CalendarId = Guid.NewGuid().ToString(),
                UserId = studentId,
                StartDate = postCalendarDto.Start,
                EndDate = postCalendarDto.End,
                Body = postCalendarDto.Body
            };

            _context.Calendars.Add(calendar);
            await _context.SaveChangesAsync();
            return Ok("Added Succefully");
        }

        #endregion


        #region Update An Event In Calendar
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCalendar(string id, PostCalendarDto calendarDto)
        {
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (emailClaim == null)
            {
                return BadRequest("Email claim not found in token.");
            }

            var email = emailClaim.Value;

            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            var studentId = user.UserId;

            /*if (id != calendarDto.CalendarId)
            {
                return BadRequest();
            }*/

            var calendar = await _context.Calendars.FindAsync(id);
            if (calendar == null)
            {
                return NotFound();
            }

            calendar.UserId = studentId;
            calendar.StartDate = calendarDto.Start;
            calendar.EndDate = calendarDto.End;
            calendar.Body = calendarDto.Body;

            _context.Entry(calendar).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            if (!CalendarExists(id))
            {
                return NotFound();
            }


            return Ok("Updated");
        }

        #endregion
        
        #region Delte Event In Calendar
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCalendar(string id)
        {
            var calendar = await _context.Calendars.FindAsync(id);
            if (calendar == null)
            {
                return NotFound();
            }

            _context.Calendars.Remove(calendar);
            await _context.SaveChangesAsync();

            return Ok("Deleted");
        } 
        #endregion

        private bool CalendarExists(string id)
        {
            return _context.Calendars.Any(e => e.CalendarId == id);
        }


    }
}
