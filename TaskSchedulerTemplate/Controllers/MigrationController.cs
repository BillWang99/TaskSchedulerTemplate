using Microsoft.AspNetCore.Mvc;
using TaskSchedulerTemplate.Service;

namespace TaskSchedulerTemplate.Controllers
{
    public class MigrationController : Controller
    {
        public IActionResult Index()
        {
            MigrationService.ApplyUpDateDB();
            return Json("Ready to Work");
        }

        

        
    }
}
