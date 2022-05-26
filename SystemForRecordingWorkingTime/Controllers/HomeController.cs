using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.Security.Claims;
using SystemForRecordingWorkingTime.Models;
using User = SystemForRecordingWorkingTime.Models.User;

namespace SystemForRecordingWorkingTime.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(Login loginData)
        {
            User user = _context.Users.Single(a => loginData.Email == a.Email && loginData.Password == a.Password);
            _ = Authenticate(user.Email);
            return RedirectToAction("GeneralInformation", "Home");
        }
        [Authorize]
        [HttpGet]
        public IActionResult GeneralInformation()
        {
            return View(_context.Users.ToList());
        }
        [Authorize]
        [HttpGet]
        public IActionResult EditUser(Int32 id)
        {
            return View(_context.Users.Single(a => a.Id == id));
        }
        [Authorize]
        [HttpPost]
        public IActionResult EditUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
            return RedirectToAction("GeneralInformation");
        }
        [Authorize]
        [HttpGet]
        public IActionResult RequestList()
        {
            return View(_context.Requests.AsEnumerable());
        }
        [Authorize]
        [HttpGet]
        public IActionResult CreateRequest()
        {
            ViewBag.RequestTypeList = Models.Request.MappedInheritorTypes.Select(a => new SelectListItem()
            {
                Text = a.Name
            });
            return View();
        }
        [Authorize]
        [HttpPost]
        public IActionResult CreateRequest1(RequestTypeChooser requestTypeChooser)
        {
            var list = Models.Request.MappedInheritorTypes.Select(a => new SelectListItem()
            {
                Text = a.Name
            }).ToList();
            ViewBag.RequestTypeList = list;
            ViewBag.PartialViewName = list[requestTypeChooser.TypeIndex].Text;
            return View("CreateRequest");
        }
        [Authorize]
        [HttpPost]
        public IActionResult CreateRequest([FromBody] Request request)
        {
            _context.Requests.Add(request);
            _context.SaveChanges();
            return RedirectToAction("RequestList");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        private async Task Authenticate(object userName)
        {
            var a = Convert.ToString(userName) ?? throw new ArgumentNullException(nameof(userName));
            var claims = new List<Claim>{new Claim(ClaimsIdentity.DefaultNameClaimType, a)};
            ClaimsIdentity id = new ClaimsIdentity(
                claims, 
                "ApplicationCookie", 
                ClaimsIdentity.DefaultNameClaimType, 
                ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, 
                new ClaimsPrincipal(id));
        }
    }
}