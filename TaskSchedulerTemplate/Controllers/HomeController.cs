using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TaskSchedulerTemplate.Interface.Home;
using TaskSchedulerTemplate.Models;
using TaskSchedulerTemplate.ViewModels.Home;

namespace TaskSchedulerTemplate.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRegisterService _register;

        public HomeController(ILogger<HomeController> logger, IRegisterService register)
        {
            _logger = logger;
            _register = register;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Register", model);
            }

            bool HasAc = _register.AcHasBeenUsed(model.Member_Account_);

            if (HasAc)
            {
                return View("Register", model);
            }
            else
            {
                _register.RegisterAccount(model);
            }

            return Redirect("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
