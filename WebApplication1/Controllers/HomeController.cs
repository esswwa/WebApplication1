using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Configuration;
using System.Diagnostics;
using WebApplication1.Models;
using static System.Net.Mime.MediaTypeNames;
using Application = WebApplication1.Models.Application;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DemoexzContext _dbContext;

        public HomeController(ILogger<HomeController> logger, DemoexzContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<IActionResult> SignIn()
        {
            return View();
        }
        public async Task<IActionResult> Exit()
        {
            UserCheck.User = null;

            return RedirectToAction("SignIn");
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(string Login, string Password)
        {

            var user = _dbContext.Users.FirstOrDefault(i => i.Login == Login && i.Password == Password);
            if(user != null)
            {
                UserCheck.User = user;
                return RedirectToAction("Applications", "Home");
            }
            return View();
        }

        public async Task<IActionResult> Applications()
        {
            List<Application> applications = new List<Application>();
            _dbContext.Users.ToList();
            _dbContext.Roles.ToList();
            _dbContext.Equipment.ToList();
            _dbContext.TypeMalfunctions.ToList();

            if (UserCheck.User != null)
            {
                if (UserCheck.User.IdRole == 0)
                    applications = _dbContext.Applications.Where(i => i.IdUser == UserCheck.User.IdUser).ToList();
                else if (UserCheck.User.IdRole == 1)
                    applications = _dbContext.Applications.ToList();
                else if (UserCheck.User.IdRole == 2)
                    applications = _dbContext.Applications.Where(i => i.IdUser == UserCheck.User.IdUser || i.IdExecutor == UserCheck.User.IdUser).ToList();
                ViewBag.Applications = applications;
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Applications(string search)
        {

            if(UserCheck.User != null)
            {
                List<Application> applications = new List<Application>();
                _dbContext.Users.ToList();
                _dbContext.Roles.ToList();
                _dbContext.Equipment.ToList();
                _dbContext.TypeMalfunctions.ToList();
                int n;
                Console.WriteLine(search);
                if (UserCheck.User.IdRole == 0)
                {
                    if (search != null)
                    {
                        bool isNumeric = int.TryParse(search, out n);
                        if (isNumeric)
                            applications = _dbContext.Applications.Where(i => i.IdApplication == int.Parse(search) && i.IdUser == UserCheck.User.IdUser).ToList();
                        else
                            applications = _dbContext.Applications.Where(i => i.Description.Contains(search) && i.IdUser == UserCheck.User.IdUser).ToList();
                    }
                    else
                        applications = _dbContext.Applications.Where(i => i.IdUser == UserCheck.User.IdUser).ToList();

                    ViewBag.Applications = applications;
                }
                else if (UserCheck.User.IdRole == 1)
                {
                    if (search != null)
                    {
                        bool isNumeric = int.TryParse(search, out n);
                        if (isNumeric)
                            applications = _dbContext.Applications.Where(i => i.IdApplication == int.Parse(search)).ToList();
                        else
                            applications = _dbContext.Applications.Where(i => i.Description.Contains(search)).ToList();
                    }
                    else
                        applications = _dbContext.Applications.ToList();

                    ViewBag.Applications = applications;
                }
                else if (UserCheck.User.IdRole == 2)
                {
                    if (search != null)
                    {
                        bool isNumeric = int.TryParse(search, out n);
                        if (isNumeric)
                            applications = _dbContext.Applications.Where(i => i.IdApplication == int.Parse(search) && (i.IdUser == UserCheck.User.IdUser || i.IdExecutor == UserCheck.User.IdUser)).ToList();
                        else
                            applications = _dbContext.Applications.Where(i => i.Description.Contains(search) && (i.IdUser == UserCheck.User.IdUser || i.IdExecutor == UserCheck.User.IdUser)).ToList();
                    }
                    else
                        applications = _dbContext.Applications.Where(i => i.IdUser == UserCheck.User.IdUser || i.IdExecutor == UserCheck.User.IdUser).ToList();


                    ViewBag.Applications = applications;
                }
            }
            return View();
        }

        public async Task<IActionResult> Statistics()
        {

            if (UserCheck.User != null)
            {
                _dbContext.Users.ToList();
                _dbContext.Roles.ToList();
                _dbContext.Equipment.ToList();
                var types = _dbContext.TypeMalfunctions.ToList();
                var applications = _dbContext.Applications.Where(i => i.Phase == "Выполнено").ToList();
                int count = 0;
                Dictionary<int, int> countTypes = new Dictionary<int, int>();
                
                foreach(var application in applications)
                {
                    count++;
                    int name = application.IdType;
                    if (countTypes.ContainsKey(name))
                        countTypes[name]++;
                    else
                        countTypes.Add(name, 1);
                }
                Dictionary<string, int> countTypes2 = new Dictionary<string, int>();
                foreach(var type in countTypes)
                {
                    string key = _dbContext.TypeMalfunctions.FirstOrDefault(i => i.IdtypeMalfunction == type.Key).Type;
                    countTypes2.Add(key, type.Value);
                }

                ViewBag.Count = count;
                ViewBag.TypeMalfunctions = countTypes2;

                return View();
            }
            return RedirectToAction("SignIn");
        }
       

        public async Task<IActionResult> AddApplication()
        {
            _dbContext.Roles.ToList();
            ViewBag.IdEquipment = _dbContext.Equipment.ToList();
            ViewBag.IdType = _dbContext.TypeMalfunctions.ToList();
            ViewBag.IdUser = _dbContext.Users.Where(i => i.IdRoleNavigation.Role1 == "Пользователь").ToList();
            ViewBag.IdExecutor = _dbContext.Users.Where(i => i.IdRoleNavigation.Role1 == "Исполнитель").ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddApplication(int IdEquipment, int IdType, int IdExecutor, int IdUser, string Description, string Cost, string Material, string Status, string Phase, DateOnly DateAdd, DateOnly DateEnd, string Time)
        {
            var applications = _dbContext.Applications.ToList();
            var application = new Application {IdApplication = _dbContext.Applications.Max(i => i.IdApplication) + 1,  IdEquipment = IdEquipment, IdType= IdType, IdUser = IdUser, Description = Description, Cost = Cost, Material = Material, Status = Status, Phase = Phase, DateAdd = DateAdd, DateEnd = DateEnd, Time = Time };
            _dbContext.Applications.Add(application);
            await _dbContext.SaveChangesAsync();
            RedirectToAction("SignIn");
            return View();
        }

        public async Task<IActionResult> EditApplication(int id)
        {
            _dbContext.Roles.ToList();
            _dbContext.Equipment.ToList();
            _dbContext.TypeMalfunctions.ToList();
            _dbContext.Users.ToList();
            ViewBag.IdExecutor = _dbContext.Users.Where(i => i.IdRoleNavigation.Role1 == "Исполнитель").ToList();
            var application = _dbContext.Applications.FirstOrDefault(i => i.IdApplication == id);
            ViewBag.Application = application;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> EditApplication(int id, int IdExecutor, string Description,string Phase)
        {
            var applications = _dbContext.Applications.ToList();
            var application = _dbContext.Applications.FirstOrDefault(i => i.IdApplication == id);
            application.IdExecutor = IdExecutor;
            application.Description = Description;
            application.Phase = Phase;
            applications.Add(application);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Applications");
        }

        public async Task<IActionResult> Privacy()
        {
            return View();
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}