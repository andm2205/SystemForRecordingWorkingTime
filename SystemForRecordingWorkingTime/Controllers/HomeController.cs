using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using SystemForRecordingWorkingTime.Models;

namespace SystemForRecordingWorkingTime.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Authorization()
        {
            User user = new User()
            {
                Surname = "Марченко",
                Name = "Андрей",
                Patronymic = "Александрович",
                Role = UserRole.Administrator,
                Phone = 79233188266,
                Email = "andm2205@yandex.ru"
            };
            return View(user);
        }
        [HttpGet]
        public IActionResult GeneralInformation(User user)
        {
            return View(user);
            //return Json(new { html = View("GeneralInformation") });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}