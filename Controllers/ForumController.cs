using ASP_111.Data;
using ASP_111.Models.Forum.Index;
using ASP_111.Services.AuthUser;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ASP_111.Controllers
{
    public class ForumController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly ILogger<ForumController> _logger;
        private readonly IAuthUserService _authUserService;

        public ForumController(DataContext dataContext, ILogger<ForumController> logger, IAuthUserService authUserService)
        {
            _dataContext = dataContext;
            _logger = logger;
            _authUserService = authUserService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public RedirectToActionResult AddSection(ForumSectionFormModel model)
        {
            // проверяем что пользователь аутентифицирован
            Guid? userId = _authUserService.GetUserId(HttpContext);
            if (userId != null)
            {
                _dataContext.Sections.Add(new()
                {
                    Id = Guid.NewGuid(),
                    Title = model.Title,
                    Description = model.Description,
                    CreateDt = DateTime.Now,
                    ImageUrl = null,
                    DeleteDt = null,
                    AuthorId = userId.Value,
                });
                _dataContext.SaveChanges();
                _logger.LogInformation("Add OK");
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
/* Д.З. Реализовать загрузку картинок, прикрепленных к форме
 * создания новых разделов форума, и сохранение имен их файлов в БД.
 */
