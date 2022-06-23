using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private static String[] UserRoles =
            Enum.GetNames(typeof(Models.UserRole));
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Registration(Registration userData)
        {
            if (_context.Users.Any(a => a.Email == userData.Email && a.Password != null))
                throw new Exception("You are already registered");
            User user = _context.Users.Single(a => userData.Email == a.Email);
            user.Password = userData.Password;
            _context.SaveChanges();
            return RedirectToAction("Login");
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(Login userData)
        {
            User user = _context.Users.Single(a => a.Email == userData.Email);
            if (user.Password == null)
                throw new Exception("You need to register");
            _ = AuthenticateUser(user);
            return RedirectToAction("GeneralInformation");
        }
        [Authorize]
        [HttpGet]
        public IActionResult Logout()
        {
            _ = LogoutUser();
            return RedirectToAction("GeneralInformation");
        }
        [Authorize]
        [HttpGet]
        public IActionResult GeneralInformation([FromQuery]String email)
        {
            if (email == null)
                email = User.Identity.Name;
            User user = _context.Users.Single(a => a.Email == User.Identity.Name);
            ViewBag.User = user;
            if (email == User.Identity.Name)
                ViewBag.CurrentUser = true;
            IQueryable<User> users;
            if (new String[] { "Administrator", "Director" }.Any(a => User.IsInRole(a)))
                users = _context.Users;
            else if (User.IsInRole("Supervisor"))
                users = _context.Users
                    .Where(
                    a =>
                    a.Role == UserRole.Employee
                    || a.Id == user.Id);
            else if (User.IsInRole("Employee"))
                users = _context.Users.Where(a => a.Id == user.Id);
            else
                throw new Exception("unknown role");
            return View(users.ToArray());
        }
        [Authorize]
        [HttpGet]
        public IActionResult RequestList()
        {
            IQueryable<Request> list;
            if (User.IsInRole("Employee"))
                list =
                    _context.Requests
                    .Where(a => a.ApplicantUser.Email == User.Identity.Name);
            else if (new String[] { "Administrator", "Director", "Supervisor" }.Any(a => User.IsInRole(a)))
                list = _context.Requests
                    .Where((a => !(a.ApplicantUser.Role == UserRole.Employee
                    && a.RequestStatus == RequestStatus.New)));
            else
                throw new Exception("unknown role");
            return View(
                list
                .Include(a => a.ApprovingUser)
                .Include(a => a.ApplicantUser)
                .Include(a => a.StatedDates)
                .Include(a => ((ReplacebleRequest)a).ReplacementUser)
                .ToList());
        }
        public enum SortingTypeEnum
        {
            None = 0,
            Asc,
            Desc
        }
        public abstract class ListSettings<D>
        {
            public abstract IQueryable<D> GetQuery(IQueryable<D> query);
            public struct ListSetting<T>
            {
                public SortingTypeEnum SortingType { get; set; }
                public T FilterValue { get; set; }
                private IEnumerable<T> selectableFilterValues;
                public IEnumerable<T> SelectableFilterValues
                {
                    get => selectableFilterValues;
                    set
                    {
                        value = new T[] { default }.Concat(value);
                        selectableFilterValues = value;
                    }
                }
            }
        }
        public class RequestListSettings : ListSettings<Request>
        {
            public ListSetting<RequestStatus?> RequestStatus { get; set; } = new();
            public ListSetting<String?> ApplicantUserEmail { get; set; } = new();
            public ListSetting<DateTime?> CreateDate { get; set; } = new();
            public ListSetting<DateTime?> SubmissionDate { get; set; } = new();
            public ListSetting<String?> ApprovingUserEmail { get; set; } = new();
            public ListSetting<String?> Comment { get; set; } = new();
            public ListSetting<String?> TypeName { get; set; } = new();
            public ListSetting<String?> ReplacementUserEmail { get; set; } = new();
            public override IQueryable<Request> GetQuery(IQueryable<Request> query)
            {
                var list = query;
                var data = this;
                if (data.ApplicantUserEmail.FilterValue != null)
                    list = list.Where(a => a.ApplicantUser.Email == data.ApplicantUserEmail.FilterValue);
                if (data.CreateDate.FilterValue != null)
                    list = list.Where(a => a.CreateDate == DateOnly.FromDateTime(data.CreateDate.FilterValue.Value));
                if (data.SubmissionDate.FilterValue != null)
                    list = list.Where(a => a.SubmissionDate == DateOnly.FromDateTime(data.SubmissionDate.FilterValue.Value));
                if (data.ApprovingUserEmail.FilterValue != null)
                    list = list.Where(a => a.ApprovingUser.Email == data.ApprovingUserEmail.FilterValue);
                if (data.Comment.FilterValue != null)
                    list = list.Where(a => a.Comment == data.Comment.FilterValue);
                if (data.TypeName.FilterValue != null)
                    list = list.Where(a => a.Discriminator == data.TypeName.FilterValue);
                if (data.ReplacementUserEmail.FilterValue != null)
                    list = list.Where(a => a is ReplacebleRequest).Where(a => ((ReplacebleRequest)a).ReplacementUser.Email == data.ReplacementUserEmail.FilterValue);
                if (data.ApplicantUserEmail.SortingType == SortingTypeEnum.None)
                    if (data.ApplicantUserEmail.SortingType == SortingTypeEnum.Asc)
                        list = list.OrderBy(a => a.ApplicantUser.Email);
                    else
                        list = list.OrderByDescending(a => a.ApplicantUser.Email);
                if (data.CreateDate.SortingType != SortingTypeEnum.None)
                    if (data.ApplicantUserEmail.SortingType == SortingTypeEnum.Asc)
                        list = list.OrderBy(a => a.CreateDate);
                    else
                        list = list.OrderByDescending(a => a.CreateDate);
                if (data.SubmissionDate.SortingType != SortingTypeEnum.None)
                    if (data.SubmissionDate.SortingType == SortingTypeEnum.Asc)
                        list = list.OrderBy(a => a.SubmissionDate);
                    else
                        list = list.OrderByDescending(a => a.SubmissionDate);
                if (data.ApprovingUserEmail.SortingType != SortingTypeEnum.None)
                    if (data.ApprovingUserEmail.SortingType == SortingTypeEnum.Asc)
                        list = list.OrderBy(a => a.ApprovingUser.Email);
                    else
                        list = list.OrderByDescending(a => a.ApprovingUser.Email);
                if (data.Comment.SortingType != SortingTypeEnum.None)
                    if (data.Comment.SortingType == SortingTypeEnum.Asc)
                        list = list.OrderBy(a => a.Comment);
                    else
                        list = list.OrderByDescending(a => a.Comment);
                if (data.TypeName.SortingType != SortingTypeEnum.None)
                    if (data.TypeName.SortingType == SortingTypeEnum.Asc)
                        list = list.OrderBy(a => a.Discriminator);
                    else
                        list = list.OrderByDescending(a => a.Discriminator);
                if (data.ReplacementUserEmail.SortingType != SortingTypeEnum.None)
                    if (data.ReplacementUserEmail.SortingType == SortingTypeEnum.Asc)
                        list = list.OrderBy(a => a is ReplacebleRequest ? ((ReplacebleRequest)a).ReplacementUser.Email : "");
                    else
                        list = list.OrderByDescending(a => a is ReplacebleRequest ? ((ReplacebleRequest)a).ReplacementUser.Email : "");
                return list;
            }
        }
        public class FilterData
        {
            public RequestStatus? RequestStatus { get; set; }

            public String ApplicantUserEmail { get; set; }
            public DateTime? CreateDate { get; set; }
            public DateTime? SubmissionDate { get; set; }
            public String ApprovingUserEmail { get; set; }
            public String Comment { get; set; }
            public String TypeName { get; set; }
            public String ReplacementUserEmail { get; set; }
            public RequestStatus? RequestStatusSort { get; set; }

            public String ApplicantUserEmailSort { get; set; }
            public String CreateDateSort { get; set; }
            public String SubmissionDateSort { get; set; }
            public String ApprovingUserEmailSort { get; set; }
            public String CommentSort { get; set; }
            public String TypeNameSort { get; set; }
            public String ReplacementUserEmailSort { get; set; }
        }
        [Authorize]
        [HttpPost]
        public IActionResult RequestList(FilterData data)
        {
            IQueryable<Request> list;
            if (User.IsInRole("Employee"))
                list =
                    _context.Requests
                    .Where(a => a.ApprovingUser.Email == User.Identity.Name);
            else if (new String[] { "Administrator", "Director", "Supervisor" }.Any(a => User.IsInRole(a)))
                list = _context.Requests
                    .Where((a => !(a.ApplicantUser.Role == UserRole.Employee
                    && a.RequestStatus == RequestStatus.New)));
            else
                throw new Exception("unknown role");
            if (data.ApplicantUserEmail != null)
                list = list.Where(a => a.ApplicantUser.Email == data.ApplicantUserEmail);
            if (data.CreateDate.HasValue)
                list = list.Where(a => a.CreateDate == DateOnly.FromDateTime(data.CreateDate.Value));
            if (data.SubmissionDate.HasValue)
                list = list.Where(a => a.SubmissionDate == DateOnly.FromDateTime(data.SubmissionDate.Value));
            if (data.ApprovingUserEmail != null)
                list = list.Where(a => a.ApprovingUser.Email == data.ApprovingUserEmail);
            if (data.Comment != null)
                list = list.Where(a => a.Comment == data.Comment);
            if (data.TypeName != null)
                list = list.Where(a => a.Discriminator == data.TypeName);
            if (data.ReplacementUserEmail != null)
                list = list.Where(a => a is ReplacebleRequest).Where(a => ((ReplacebleRequest)a).ReplacementUser.Email == data.ReplacementUserEmail);
            if (data.ApplicantUserEmailSort != "None")
                if (data.ApplicantUserEmailSort == "Asc")
                    list = list.OrderBy(a => a.ApplicantUser.Email);
                else
                    list = list.OrderByDescending(a => a.ApplicantUser.Email);
            if (data.CreateDateSort != "None")
                if (data.ApplicantUserEmailSort == "Asc")
                    list = list.OrderBy(a => a.CreateDate);
                else
                    list = list.OrderByDescending(a => a.CreateDate);
            if (data.SubmissionDateSort != "None")
                if (data.SubmissionDateSort == "Asc")
                    list = list.OrderBy(a => a.SubmissionDate);
                else
                    list = list.OrderByDescending(a => a.SubmissionDate);
            if (data.ApprovingUserEmailSort != "None")
                if(data.ApprovingUserEmailSort == "Asc")
                    list = list.OrderBy(a => a.ApprovingUser.Email);
                else
                    list = list.OrderByDescending(a => a.ApprovingUser.Email);
            if (data.CommentSort != "None")
                if(data.CommentSort == "Asc")
                    list = list.OrderBy(a => a.Comment);
                else
                    list = list.OrderByDescending(a => a.Comment);
            if (data.TypeNameSort != "None")
                if(data.TypeNameSort == "Asc")
                    list = list.OrderBy(a => a.Discriminator);
                else
                    list = list.OrderByDescending(a => a.Discriminator);
            if (data.ReplacementUserEmailSort != "None")
                if(data.ReplacementUserEmailSort == "Asc")
                    list = list.OrderBy(a => a is ReplacebleRequest ? ((ReplacebleRequest)a).ReplacementUser.Email : "");
                else
                    list = list.OrderByDescending(a => a is ReplacebleRequest ? ((ReplacebleRequest)a).ReplacementUser.Email : "");
            return View(
                list
                .Include(a => a.ApprovingUser)
                .Include(a => a.ApplicantUser)
                .Include(a => a.StatedDates)
                .Include(a => ((ReplacebleRequest)a).ReplacementUser)
                .ToList());
        }
        [Authorize]
        [HttpGet]
        public IActionResult CreateRequest()
        {
            ViewBag.RequestTypeList = Models.Request.MappedInheritorTypesList;
            return View();
        }
        [Authorize]
        [HttpPost]
        public IActionResult CreateRequestOfType(RequestTypeChooser requestType)
        {
            ViewBag.RequestTypeList = Models.Request.MappedInheritorTypesList.ToList();
            ViewBag.PartialViewName = Enum.GetName(requestType.TypeIndex.GetType(), requestType.TypeIndex);
            return View("CreateRequest");
        }
        [Authorize]
        [HttpPost]
        [ActionName("CreateDayOffRequest")]
        public IActionResult CreateRequest(CreateDayOffRequest data)
        {
            return CreateRequest((CreateRequest)data);
        }
        [Authorize]
        [HttpPost]
        [ActionName("CreateVacationRequest")]
        public IActionResult CreateRequest(CreateVacationRequest data)
        {
            return CreateRequest((CreateRequest)data);
        }
        [Authorize]
        [HttpPost]
        [ActionName("CreateRemoteWorkRequest")]
        public IActionResult CreateRequest(CreateRemoteWorkRequest data)
        {
            return CreateRequest((CreateRequest)data);
        }
        [Authorize]
        [HttpPost]
        [ActionName("CreateDayOffAtTheExpenseOfWorkingOutRequest")]
        public IActionResult CreateRequest(CreateDayOffAtTheExpenseOfWorkingOutRequest data)
        {
            return CreateRequest((CreateRequest)data);
        }
        [Authorize]
        [HttpPost]
        [ActionName("CreateDayOffAtTheExpenseOfVacationRequest")]
        public IActionResult CreateRequest(CreateDayOffAtTheExpenseOfVacationRequest data)
        {
            return RedirectToAction("CreateRequest", (CreateRequest)data);
        }
        [Authorize]
        [HttpPost]
        public IActionResult CreateRequest(CreateRequest data)
        {
            using(var transaction = _context.Database.BeginTransaction())
            {
                User user = _context.Users.Single(a => a.Email == User.Identity.Name);
                var request = data.GetRequest(user, _context);
                _context.Requests.Add(request);
                _context.SaveChanges();
                data.AddLinkedRequestData(user, _context, request.Id);
                _context.SaveChanges();
                transaction.Commit();
            }
            return RedirectToAction("RequestList");
        }
        [Authorize]
        [HttpPost]
        [HttpGet]
        public IActionResult WorkingCalendar([FromForm] WorkingCalendarFilter filterData)
        {
            if(filterData.UserEmail == null)
                filterData.UserEmail = User.Identity.Name;
            User user = _context.Users.Single(a => a.Email == filterData.UserEmail);
            var requests = _context.Requests
                .Include(a => ((ReplacebleRequest)a).ReplacementUser)
                .Include(a => a.StatedDates)
                .Include(a => a.ApplicantUser)
                .Include(a => ((DayOffAtTheExpenseOfWorkingOutRequest)a).WorkingOutDates)
                .Where(a => a.ApplicantUserId == user.Id
                || (a is ReplacebleRequest && ((ReplacebleRequest)a).ReplacementUserId == user.Id));
            ViewBag.Filter = filterData;
            return View(new WorkingCalendar(requests.ToArray(), user.Id) {Month=filterData.Month, Year=filterData.Year });
        }
        [Authorize(Roles = "Administrator, Supervisor, Director")]
        [HttpGet]
        [HttpPost]
        public IActionResult WorkingSchedule([FromForm] WorkingScheduleFilter filterData)
        {
            if(filterData.UserEmails == null)
                filterData.UserEmails = _context.Users.Select(a => a.Email).ToArray();
            ViewBag.Filter = filterData;
            var d = new WorkingSchedule(
                _context.Requests
                .Include(a => ((ReplacebleRequest)a).ReplacementUser)
                .Include(a => a.StatedDates)
                .Include(a => a.ApplicantUser)
                .Include(a => ((DayOffAtTheExpenseOfWorkingOutRequest)a).WorkingOutDates)
                .ToArray(),
                _context.Users.Where(a => filterData.UserEmails.Contains(a.Email)).ToArray(),
                filterData.Month,
                filterData.Year);
            return View(d);
        }
        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public IActionResult ProductionCalendar()
        {
            return View();
        }
        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public IActionResult CreateProductionCalendar([FromQuery] Int32 year, [FromQuery] Int32 month)
        {
            return View(new CreateProductionCalendar(month, year));
        }
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public IActionResult CreateProductionCalendar(CreateProductionCalendar data)
        {
            _context.Calendars.Add(new ProductionCalendar(data));
            _context.SaveChanges();
            return RedirectToAction("ProductionCalendar");
        }
        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public IActionResult EditProductionCalendar([FromQuery] Int32 year, [FromQuery] Int32 month)
        {
            return View(_context.Calendars
                .Include(a => a.Days)
                .Single(
                a => a.Year == year 
                && a.Month == month 
                && a.Status == ProductionCalendarStatus.Created));
        }
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public IActionResult EditProductionCalendar(ProductionCalendar data)
        {
            _context.CalendarDays.UpdateRange(data.Days);
            _context.SaveChanges();
            return RedirectToAction("ProductionCalendar");
        }
        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public IActionResult ActivateCalendar([FromQuery] Int32 year, [FromQuery] Int32 month)
        {
            ProductionCalendar calendar = _context.Calendars.Single(a => a.Year == year && a.Month == month);
            calendar.Status = ProductionCalendarStatus.Activated;
            _context.SaveChanges();
            return RedirectToAction("ProductionCalendar");
        }
        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public IActionResult CreateUser(CreateUser userData)
        {
            _context.Users.Add(new Models.User()
            {
                Surname = userData.Surname,
                Name = userData.Name,
                Patronymic = userData.Patronymic,
                Role = userData.Role,
                Phone = userData.Phone,
                Email = userData.Email
            });
            _context.SaveChanges();
            return RedirectToAction("CreateUser");
        }
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public IActionResult AdministratorEditUser(AdministratorEditUser userData)
        {
            User user = _context.Users.Single(a => a.Id == userData.Id);
            user.Surname = userData.Surname;
            user.Name = userData.Name;
            user.Patronymic = userData.Patronymic;
            user.Role = userData.Role;
            user.JobTitle = user.JobTitle;
            user.Phone = userData.Phone;
            user.Email = userData.Email;
            _context.SaveChanges();
            return RedirectToAction("GeneralInformation", user.Email);
        }
        [HttpPost]
        [Authorize()]
        public IActionResult EditUser(EditUser userData)
        {
            User user = _context.Users.Single(a => a.Id == userData.Id);
            if (user.Email != User.Identity.Name)
                throw new Exception("you do not have permission");
            user.Phone = userData.Phone;
            user.Email = userData.Email;
            _context.SaveChanges();
            return RedirectToAction("GeneralInformation", user.Email);
        }
        [HttpGet]
        [Authorize(Roles = "Supervisor, Director")]
        public IActionResult AgreeRequest(Int32 id)
        {
            return View(new AgreeRequest(
                _context.Requests.Single(
                a => a.ApprovingUser.Email == User.Identity.Name
                && a.Id == id
                && a.RequestStatus == RequestStatus.SentForApproval)));
        }
        [HttpPost]
        [Authorize(Roles = "Supervisor, Director")]
        public IActionResult AgreeRequest(AgreeRequest data)
        {
            var request = _context.Requests.Single(
                a => a.ApprovingUser.Email == User.Identity.Name
                && a.Id == data.RequestId
                && a.RequestStatus == RequestStatus.SentForApproval);
            request.RequestStatus = data.Status;
            request.Comment = data.Comment;
            if (data.Replaceble)
            {
                ((ReplacebleRequest)request).ReplacementUserId =
                    _context.Users.Single(a => a.Email == data.ReplacementUserEmail).Id;
            }
            if (data.Movable)
            {
                _context.StatedDates.AddRange(data.StatedDates.Select(a => new StatedDate()
                {
                    RequestId = request.Id,
                    Value = a
                }));
            }
            _context.SaveChanges();
            
            return RedirectToAction("RequestList");
        }

        [HttpGet]
        [Authorize(Roles = "Director")]
        public IActionResult ApproveRequest(Int32 id)
        {
            return View(new AgreeRequest(
                _context.Requests.Single(
                a => a.Id == id
                && a.RequestStatus == RequestStatus.Agreed)));
        }
        [HttpPost]
        [Authorize(Roles = "Director")]
        public IActionResult ApproveRequest(ApproveRequest data)
        {
            var request = _context.Requests.Single(
                a => a.Id == data.RequestId
                && a.RequestStatus == RequestStatus.Agreed);
            request.RequestStatus = data.Status;
            return RedirectToAction("RequestList");
        }
        [HttpGet]
        [Authorize]
        public IActionResult SendForApprovalRequest(Int32 id)
        {
            RequestStatus[] RequestStatuses = new[] { RequestStatus.New, RequestStatus.Withdrawn };
            var request = _context.Requests.Single(
                a => 
                a.Id == id 
                && a.ApplicantUser.Email == User.Identity.Name
                && RequestStatuses.Contains(a.RequestStatus));
            request.RequestStatus = RequestStatus.SentForApproval;
            _context.SaveChanges();
            return RedirectToAction("RequestList");
        }
        [HttpGet]
        [Authorize]
        public IActionResult WithdrawRequest(Int32 id)
        {
            var request = _context.Requests.Single(
                a => 
                a.Id == id 
                && a.ApplicantUser.Email == User.Identity.Name
                && a.RequestStatus == RequestStatus.SentForApproval);
            request.RequestStatus = RequestStatus.Withdrawn;
            _context.SaveChanges();
            return RedirectToAction("RequestList");
        }
        [HttpGet]
        [Authorize]
        public IActionResult CancelRequest(Int32 id)
        {
            var request = _context.Requests.Single(
                a =>
                a.Id == id
                && a.ApplicantUser.Email == User.Identity.Name
                && a.StatedDates.Any(b => b.Value < DateOnly.FromDateTime(DateTime.Now)));
            request.RequestStatus = RequestStatus.Canceled;
            _context.SaveChanges();
            return RedirectToAction("RequestList");
        }
        [Authorize]
        [ActionName("EditRequestView")]
        public IActionResult EditRequest([FromRoute]Int32 id)
        {
            Request request = _context.Requests
                .Include(a => a.ApprovingUser)
                .Include(a => a.ApplicantUser)
                .Include(a => a.StatedDates)
                .Include(a => ((ReplacebleRequest)a).ReplacementUser)
                .Include(a => a.StatedDates)
                .Include(a => ((DayOffAtTheExpenseOfWorkingOutRequest)a).WorkingOutDates)
                .Include(a => ((RemoteWorkRequest)a).WorkPlans)
                .Single(a => a.Id == id);
            ViewBag.PartialViewName = request.Discriminator;
            ViewBag.RequestId = id;
            return View("EditRequest", request.GetData());
        }
        [Authorize]
        [ActionName("EditDayOffRequest")]
        public IActionResult EditRequest([FromForm] CreateDayOffRequest data, [FromRoute] Int32 id)
        {
            return EditRequest((CreateRequest)data, id);
        }
        [Authorize]
        [ActionName("EditVacationRequest")]
        public IActionResult EditRequest([FromForm] CreateVacationRequest data, [FromRoute] Int32 id)
        {
            return EditRequest((CreateRequest)data, id);
        }
        [Authorize]
        [ActionName("EditRemoteWorkRequest")]
        public IActionResult EditRequest([FromForm] CreateRemoteWorkRequest data, [FromRoute] Int32 id)
        {
            return EditRequest((CreateRequest)data, id);
        }
        [Authorize]
        [ActionName("EditDayOffAtTheExpenseOfWorkingOutRequest")]
        public IActionResult EditRequest([FromForm] CreateDayOffAtTheExpenseOfWorkingOutRequest data, [FromRoute] Int32 id)
        {
            return EditRequest((CreateRequest)data, id);
        }
        [Authorize]
        [ActionName("EditDayOffAtTheExpenseOfVacationRequest")]
        public IActionResult EditRequest([FromForm] CreateDayOffAtTheExpenseOfVacationRequest data, [FromRoute] Int32 id)
        {
            return EditRequest((CreateRequest)data, id);
        }
        private IActionResult EditRequest(CreateRequest data, Int32 id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                User user = _context.Users.Single(a => a.Email == User.Identity.Name);
                data.DeleteRequest(user, _context, id);
                _context.SaveChanges();
                data.AddLinkedRequestData(user, _context, id);
                _context.SaveChanges();
                var request = data.GetRequest(user, _context);
                request.Id = id;
                _context.Requests.Update(request);
                _context.SaveChanges();
                transaction.Commit();
            }
            return RedirectToAction("RequestList");
        }
        [Route("/Error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        private async Task AuthenticateUser(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString())
            };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(
                claims, 
                "ApplicationCookie", 
                ClaimsIdentity.DefaultNameClaimType, 
                ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, 
                new ClaimsPrincipal(claimsIdentity));
        }
        private async Task LogoutUser()
        {
            await HttpContext.SignOutAsync();
        }
    }
}