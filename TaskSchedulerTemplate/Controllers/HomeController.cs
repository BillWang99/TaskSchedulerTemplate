using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TaskSchedulerTemplate.Interface.Home;
using TaskSchedulerTemplate.Models;
using TaskSchedulerTemplate.ViewModels.Home;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace TaskSchedulerTemplate.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRegisterService _register;
        private readonly ILoginSerivce _loginSerivce;

        public HomeController(ILogger<HomeController> logger, IRegisterService register, ILoginSerivce loginSerivce)
        {
            _logger = logger;
            _register = register;
            _loginSerivce = loginSerivce;
        }

        //登入畫面
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(LoginViewModel model)
        {
            //如果資料不符合規則
            if (!ModelState.IsValid)
            {
                //回到註冊畫面並顯示錯誤訊息
                return View("Index", model);
            }

            //檢查帳號是否存在
            bool AcExist = _loginSerivce.AcExist(model.Member_Account_);

            //帳號不存在
            if (!AcExist)
            {
                ModelState.AddModelError("Member_Account_", "帳號不存在");
                //回到註冊畫面並顯示錯誤訊息
                return View("Index", model);
                
            }

            bool UserLogin = _loginSerivce.LoginValidation(model);

            if (!UserLogin)
            {
                //回到註冊畫面並顯示錯誤訊息
                return View("Index", model);
                ModelState.AddModelError("Member_Account_", "帳號或密碼錯誤");
            }

            //cookie記錄登入資料
            UserCookie Data = _loginSerivce.UserCookie(model.Member_Account_);
            var claims = new List<Claim>
                    {
                        new Claim("Account", Data.Member_Account_),
                        new Claim(ClaimTypes.Name, Data.Member_Name_),
                        new Claim("Staffcode", Data.Member_Staffcode_),


                    };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            //return Json("Success");

            return RedirectToAction("index", "Main");
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
