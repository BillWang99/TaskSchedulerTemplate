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

        //登入畫面
        public IActionResult Index()
        {
            return View();
        }

        //註冊畫面
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        //註冊功能
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            //如果資料不符合規則
            if (!ModelState.IsValid)
            {
                //回到註冊畫面並顯示錯誤訊息
                return View("Register", model);
            }

            //檢查帳號是否重複
            bool HasAc = _register.AcHasBeenUsed(model.Member_Account_);

            if (HasAc)
            {
                //帳號重複，回到註冊畫面並顯示錯誤訊息
                return View("Register", model);
            }
            else
            {
                //進行註冊
                _register.RegisterAccount(model);
            }

            //回到登入畫面
            return Redirect("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
