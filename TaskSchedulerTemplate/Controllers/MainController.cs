using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TaskSchedulerTemplate.Controllers
{
    public class MainController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public MainController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
            var claim = _httpContextAccessor.HttpContext.User.Claims.ToList();
            string user_name = claim.Where(x => x.Type == ClaimTypes.Name).First().Value;
            ViewData["user_name"] = user_name;
            return View();
        }
    }
}
