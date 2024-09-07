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

        //�n�J�e��
        public IActionResult Index()
        {
            return View();
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
