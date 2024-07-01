using CRUDApi.Context;
using CRUDApi.DTOs;
using CRUDApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using System.Security.Claims;
using System.Security.Cryptography.Xml;

namespace CRUDApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly LMSContext _context;
        public NewsController(LMSContext context)
        {
            _context=context;
        }

        #region Get All News
        [HttpGet]
        public async Task<IActionResult> GetNews()
        {
            var news = (from n in _context.News
                        join u in _context.Users on n.UserId equals u.UserId
                        where n.UserId==u.UserId
                         select new NewsDto
                        {
                            NewsId = n.NewsId,
                            Content = n.Content,
                            FilePath = n.FilePath,
                            FacultyId = u.FacultyId,
                            UserId = n.UserId,
                            CreatedAt = n.CreatedAt,
                            UserName=u.FullName,
                            FacultyName=u.Faculty.Name,
                            UserImage=u.ImagePath
                        }).ToList();
            if (news == null)
            {
                return BadRequest("No News Today...!");
            }
            return Ok(news);
        } 
        #endregion

        #region Create News
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task< ActionResult<NewsDto>> Create(string newsContent)
        {
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (emailClaim == null)
            {
                return BadRequest("Email claim not found in token.");
            }

            var email = emailClaim.Value;

            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            var userId = user.UserId;
            var data = new News
            {
                NewsId = Guid.NewGuid().ToString(),
                Content = newsContent,
                FilePath = null,
                FacultyId = user.FacultyId,
                UserId =userId,
                CreatedAt = DateTime.Now,
                
            };
            _context.News.Add(data);
            await _context.SaveChangesAsync();
            return Ok("Created Succefully");
        }
        private bool NewsExists(string id)
        {
            return _context.News.Any(e => e.NewsId == id);
        } 
        #endregion

        #region Update News
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNews(string id,[FromBody] string newsContent)
        {
           var news=await _context.News.FirstOrDefaultAsync(e => e.NewsId == id);

            if (news == null)
            {
                return NotFound();
            }
            news.Content = newsContent;
            news.CreatedAt = DateTime.Now;
            _context.News.Update(news);
            _context.SaveChanges();
            return Ok(news);
        } 
        #endregion

        #region Delete New
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNews(string id)
        {
            var news = await _context.News.FindAsync(id);
            if (news == null)
            {
                return NotFound();
            }

            _context.News.Remove(news);
            await _context.SaveChangesAsync();

            return Ok("Deleted Succefully..");
        } 
        #endregion


    }
}
