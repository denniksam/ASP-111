using ASP_111.Data;
using ASP_111.Models.Forum.Index;
using ASP_111.Models.Forum.Section;
using ASP_111.Models.Forum.Topic;
using ASP_111.Services.AuthUser;
using ASP_111.Services.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;

namespace ASP_111.Controllers
{
    public class ForumController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly ILogger<ForumController> _logger;
        private readonly IAuthUserService _authUserService;
        private readonly IValidationService _validationService;

        public ForumController(DataContext dataContext, ILogger<ForumController> logger, IAuthUserService authUserService, IValidationService validationService)
        {
            _dataContext = dataContext;
            _logger = logger;
            _authUserService = authUserService;
            _validationService = validationService;
        }

        public IActionResult Topic([FromRoute] Guid id)
        {
            var topic = _dataContext
                .Topics
                .Include(t => t.Author)
                .FirstOrDefault(t => t.Id == id);

            if (topic == null)
            {
                return NotFound();
            }

            TopicPageModel model = new()
            {
                Topic = new(topic)
            };
            model.Themes = _dataContext
                .Themes
                .Include(t => t.Author)
                .Include(t => t.Comments)
                .Where(t => t.TopicId == topic.Id && t.DeleteDt == null)
                .Select(t => new ThemeViewModel(t))
                .ToList();

            if (HttpContext.Session.Keys.Contains("AddThemeMessage"))
            {
                model.ErrorMessages =
                    JsonSerializer.Deserialize<Dictionary<String, String?>>(
                        HttpContext.Session.GetString("AddThemeMessage")!);

                HttpContext.Session.Remove("AddThemeMessage");
            }

            return View(model);
        }

        [HttpPost]
        public RedirectToActionResult AddTheme(ThemeFormModel formModel)
        {
            var messages = _validationService.ErrorMessages(formModel);
            foreach (var (key, message) in messages)
            {
                if (message != null)  // есть сообщение об ошибке
                {
                    HttpContext.Session.SetString(
                        "AddThemeMessage",
                        JsonSerializer.Serialize(messages)
                    );
                    return RedirectToAction(nameof(Topic), new { id = formModel.TopicId });
                }
            }
            // проверяем что пользователь аутентифицирован
            Guid? userId = _authUserService.GetUserId(HttpContext);
            if (userId != null)
            {
                Guid themeId = Guid.NewGuid();
                DateTime dt = DateTime.Now;

                _dataContext.Themes.Add(new()
                {
                    Id = themeId,
                    AuthorId = userId.Value,
                    TopicId = formModel.TopicId,
                    Title = formModel.Title,
                    CreateDt = dt,
                });
                _dataContext.Comments.Add(new()
                {
                    Id = Guid.NewGuid(),
                    AuthorId = userId.Value,
                    Content = formModel.Content,
                    ThemeId = themeId,
                    CreateDt = dt,
                });
                _dataContext.SaveChanges();
            }

            return RedirectToAction(nameof(Topic), new { id = formModel.TopicId });
        }
        /* Д.З. Отображение тем (страница Topic)
         * Обеспечить возможность приложить файл к форме создания новой темы
         * и использовать его для сущности комментария (первого в теме)
         * Добавить навигационное свойство Author к сущности Theme, 
         * настроить его связь и внедрить в выборку
         */


        //                     <a  asp-route-id="@..." 
        public IActionResult Section( [FromRoute] Guid id )
        {
            /* Д.З. Работа с карточками Топиков.
             * Создать алгоритм формирования строки из даты-времени по следующим
             * правилам:
             *  - если дата сегодняшняя, то выводится только время (до секунд)
             *  - если вчерашняя, то слово "вчера" и время до минут
             *  - если меньше чем 10 дней назад (2-9), то надпись "N дней назад"
             *  - иначе просто дата 11.08.2010
             * Реализовать в виде службы (сервиса) и внедрить в проект,
             * испытать на карточках форума
             * 
             * Добавить ссылку на профиль пользователя к его аватарке на
             * карточке с Топиком. (Нажатие на автарку переводит в профиль
             * автора топика)
             */
            var section = _dataContext.Sections
                .Include(s => s.Author)
                .FirstOrDefault(s => s.Id == id );

            if ( section == null )
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                return NotFound();
            }
            SectionPageModel sectionViewModel = new()
            {
                Section = new ForumSectionViewModel
                {
                    Id = section.Id.ToString(),
                    Title = section.Title,
                    Description = section.Description,
                    CreateDt = section.CreateDt.ToShortDateString(),
                    Author = new(section.Author),
                }
            };

            if (HttpContext.Session.Keys.Contains("AddTopicMessage"))
            {
                sectionViewModel.ErrorMessages =
                    JsonSerializer.Deserialize<Dictionary<String, String?>>(
                        HttpContext.Session.GetString("AddTopicMessage"));

                HttpContext.Session.Remove("AddTopicMessage");
            }

            sectionViewModel.Topics =
                _dataContext.Topics
                .Include(t => t.Author)
                .Where(t => t.DeleteDt == null)
                .OrderByDescending(t => t.CreateDt)
                .AsEnumerable()
                .Select(t => new TopicViewModel()
                {
                    Id = t.Id.ToString(),
                    Title = t.Title,
                    Description = t.Description,
                    CreateDt = t.CreateDt.ToShortDateString(),
                    ImageUrl = "/img/" + t.ImageUrl,
                    Author = new(t.Author)
                }).ToList();

            return View(sectionViewModel);
        }

        [HttpPost]
        public RedirectToActionResult AddTopic(TopicFormModel formModel)
        {
            var messages = _validationService.ErrorMessages(formModel);
            foreach (var (key, message) in messages)
            {
                if (message != null)  // есть сообщение об ошибке
                {
                    HttpContext.Session.SetString(
                        "AddTopicMessage",
                        JsonSerializer.Serialize(messages)
                    );
                    return RedirectToAction(nameof(Section), new { id = formModel.SectionId });
                }
            }
            // проверяем что пользователь аутентифицирован
            Guid? userId = _authUserService.GetUserId(HttpContext);
            if (userId != null)
            {
                String? nameAvatar = null;
                if (formModel.ImageFile != null)
                {
                    // определяем расширение файла
                    String ext = Path.GetExtension(formModel.ImageFile.FileName);
                    // проверить расширение на перечень допустимых

                    // формируем имя для файла
                    nameAvatar = Guid.NewGuid().ToString() + ext;

                    using FileStream fstream = new(
                        "wwwroot/img/" + nameAvatar, 
                        FileMode.Create);

                    formModel.ImageFile.CopyTo(fstream);
                }

                _dataContext.Topics.Add(new()
                {
                    Id = Guid.NewGuid(),
                    AuthorId = userId.Value,
                    SectionId = formModel.SectionId,
                    Title = formModel.Title,
                    Description = formModel.Description,
                    CreateDt = DateTime.Now,
                    ImageUrl = nameAvatar
                });
                _dataContext.SaveChanges();
            }

            return RedirectToAction(nameof(Section), new {id = formModel.SectionId});
        }

        public IActionResult Index()
        {
            int n = 0;
            ForumIndexModel model = new()
            {
                Sections = _dataContext
                    .Sections
                    .Include(s => s.Author)
                    .Where(s => s.DeleteDt == null)
                    .OrderBy(s => s.CreateDt)
                    .AsEnumerable()
                    .Select(s => new ForumSectionViewModel
                    {
                        Id = s.Id.ToString(),
                        Title = s.Title,
                        Description = s.Description,
                        CreateDt = s.CreateDt.ToShortDateString(),
                        ImageUrl = s.ImageUrl == null
                            ? $"/img/section/section{n++}.png"
                            : $"/img/section/{s.ImageUrl}",
                        Author = new(s.Author),
                    }),
                // проверяем есть ли в сессии сообщение о валидации формы,
                // если есть, извлекаем, десериализуем и передаем на 
                // представление (все сообщения) вместе с данными формы, которые
                // подставятся обратно в поля формы
            };


            return View(model);
            // В представлении проверяем наличие данных валидации
            // если они есть, то в целом форма не принята,
            // выводим сообщения под каждым полем:
            // если сообщение null, то нет ошибок, поле принято
            // иначе - ошибка и ее текст в сообщении
        }

        [HttpPost]
        public RedirectToActionResult AddSection(ForumSectionFormModel model)
        {
            var messages = _validationService.ErrorMessages(model);
            foreach( var (key, message) in messages)
            {
                if(message != null)
                {
                    // есть сообщение об ошибке - 
                    // сериализуем все сообщения, сохраняем в сессии и
                    // перенаправляем на Index
                }
            }
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
/* Задача: валидация (сервис валидации)
 * Задание: реализовать средства проверки моделей форм на правильность данных
 * Особенности: разные поля нужно проверять по-разному, а в разных моделях
 *  бывают одинаковые правила проверки.
 * + Нужно формирование сообщений о результатах проверки 
 * Готовые решения:
 *  https://learn.microsoft.com/en-us/aspnet/core/mvc/models/validation
 *  
 * Идея:
 * class Model {
 *  ...
 *  [ValidationRules(ValidationRule.NotEmpty, ValidationRule.Name)]
 *  String name
 *  
 *  [ValidationRules(ValidationRule.NotEmpty, ValidationRule.Password)]
 *  String password
 *  
 *  [ValidationRules(ValidationRule.Login)]
 *  String login
 *  
 *  }
 *  
 *  _service.ErrorMessages(model) 
 *    [ "name" => "Не может быть пустым", 
 *      "password" => "должен содержать цифру",
 *      "login" => null ]
 */
/* o LINQ
 * LINQ-to-SQL (LINQ-to-Entity)
 *  - собирает информацию о запросе и строит его SQL выражение
 *  _context.Users.Where(u => u.Name == 'Admin' )
 *  ==> IQueryable ( "SELECT * FROM Users u WHERE u.Name = 'Admin'
 *  
 * LINQ-to-Objects
 *  - циклическая (итеративная) обработка коллекций
 *  collection.Users.Where(u => u.Name == 'Admin' )
 *  ==> IEnumerable
 *  
 *  _context.Users.Where(u => u.Name == 'Admin' ).ToList()
 *  _context.Users.ToList().Where(u => u.Name == 'Admin' )
 */
