using ASP_111.Models;
using ASP_111.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ASP_111.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        // декларируем зависимости
        private readonly IDateService _dateService;
        private readonly TimeService _timeService;
        private readonly DateTimeService _dateTimeService;

        // признак readonly говорит о том, что данные должны инициализироваться конструктором
        public HomeController(                // параметр в конструкторе требует передачи ссылки,
            ILogger<HomeController> logger,   // иначе объект контроллера не может быть построен
            IDateService dateService,         // - это является зависимостью
            TimeService timeService,
            DateTimeService dateTimeService)
        {
            _logger = logger;
            _dateService = dateService;
            _timeService = timeService;
            _dateTimeService = dateTimeService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Intro()
        {
            return View();
        }

        public IActionResult Razor()
        {
            return View();
        }
        
        public ViewResult Services()
        {
            ViewData["date"] = _dateService.GetDate();
            ViewData["time"] = _timeService.GetTime();
            ViewData["datetime"] = _dateTimeService.GetNow();

            ViewData["datetime-hash"] = _dateTimeService.GetHashCode();
            ViewData["time-hash"] = _timeService.GetHashCode();
            ViewData["date-hash"] = _dateService.GetHashCode();
            ViewData["ctrl-hash"] = this.GetHashCode();

            return View();
        }
        public ViewResult Data()
        {
            return View();
        }
        public ViewResult Sessions()
        {
            if (HttpContext.Session.Keys.Contains("StoredValue"))
            {
                ViewData["StoredValue"] = HttpContext.Session.GetString("StoredValue");
            }
            else
            {
                ViewData["StoredValue"] = "";
            }
            return View();
        }
        public RedirectToActionResult SetSession()
        {
            HttpContext.Session.SetString("StoredValue", "Данные в сессии");
            return RedirectToAction(nameof(Sessions));
            /* Browser                  ASP
             * 
             * SetSession -----------> redirect
             *  302       <----------     save in session
             *  location: Sessions                    |
             *                                        |
             * Sessions --------------> View          |
             *   200    <-------------    get from session
             */
        }
        public RedirectToActionResult RemoveSession()
        {
            HttpContext.Session.Remove("StoredValue");
            return RedirectToAction(nameof(Sessions));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}