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

        //�n�J�e��
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(LoginViewModel model)
        {
            //�p�G��Ƥ��ŦX�W�h
            if (!ModelState.IsValid)
            {
                //�^����U�e������ܿ��~�T��
                return View("Index", model);
            }

            //�ˬd�b���O�_�s�b
            bool AcExist = _loginSerivce.AcExist(model.Member_Account_);

            //�b�����s�b
            if (!AcExist)
            {
                ModelState.AddModelError("Member_Account_", "�b�����s�b");
                //�^����U�e������ܿ��~�T��
                return View("Index", model);
                
            }

            bool UserLogin = _loginSerivce.LoginValidation(model);

            if (!UserLogin)
            {
                //�^����U�e������ܿ��~�T��
                return View("Index", model);
                ModelState.AddModelError("Member_Account_", "�b���αK�X���~");
            }

            //cookie�O���n�J���
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

        //���U�e��
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        //���U�\��
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            //�p�G��Ƥ��ŦX�W�h
            if (!ModelState.IsValid)
            {
                //�^����U�e������ܿ��~�T��
                return View("Register", model);
            }

            //�ˬd�b���O�_����
            bool HasAc = _register.AcHasBeenUsed(model.Member_Account_);

            if (HasAc)
            {
                //�b�����ơA�^����U�e������ܿ��~�T��
                return View("Register", model);
            }
            else
            {
                //�i����U
                _register.RegisterAccount(model);
            }

            //�^��n�J�e��
            return Redirect("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
