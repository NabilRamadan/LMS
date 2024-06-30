using CRUDApi.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRUDApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminDashboardController : ControllerBase
    {
        private readonly LMSContext _context;
        public AdminDashboardController(LMSContext context)
        {
            _context = context;
        }

    }
}
