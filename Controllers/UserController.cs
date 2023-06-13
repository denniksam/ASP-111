using ASP_111.Data;
using ASP_111.Models.User;
using ASP_111.Services.Hash;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace ASP_111.Controllers
{
    public class UserController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IHashService _hashService;

        public UserController(DataContext dataContext, IHashService hashService)
        {
            _dataContext = dataContext;
            _hashService = hashService;
        }

        public ViewResult Profile()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]  // этот метод будет вызываться по AJAX при аутентификации
        public JsonResult Auth([FromBody] AuthAjaxModel model)
        {
            var user = _dataContext.Users.FirstOrDefault(
                u => u.Login == model.Login 
                    && u.PasswordHash == _hashService.GetHash(model.Password)
                );
            // сохранение информации об аутентификации в сессии
            if(user != null)
            {
                HttpContext.Session.SetString("userId", user.Id.ToString());
            }
            return Json(new { Success = (user != null) });
        }

        public RedirectToActionResult Logout()
        {
            /* Выход из авторизированного режима всегда должен
             * перенаправить на страницу, которая доступна без авторизации
             * Чаще всего - на домашнюю страницу
             */
            HttpContext.Session.Remove("userId");
            return RedirectToAction("Index", "Home");
        }

        public IActionResult SignUp(SignUpFormModel? formModel)
        {
            SignUpViewModel viewModel;
            if(Request.Method == "POST" && formModel != null)
            {
                // передача формы
                viewModel = ValidateSignUpForm(formModel);
                viewModel.FormModel = formModel;
                // сохранить данные, необходимые для View, в сессии и redirect
                HttpContext.Session.SetString("FormData",
                    System.Text.Json.JsonSerializer.Serialize(viewModel));
                return RedirectToAction(nameof(SignUp));
            }
            else
            {
                // проверить есть ли сохраненные данные, если есть - использовать
                //  и удалить
                if (HttpContext.Session.Keys.Contains("FormData"))
                {
                    String? data = HttpContext.Session.GetString("FormData");
                    if(data != null)
                    {
                        viewModel = System.Text.Json.JsonSerializer
                            .Deserialize<SignUpViewModel>(data)!;
                    }
                    else
                    {
                        viewModel = null!;
                    }
                    HttpContext.Session.Remove("FormData");
                }
                else
                {
                    // первый заход - начало заполнения формы
                    viewModel = new();
                    viewModel.FormModel = null;  // нечего проверять
                }
            }
            return View(viewModel);  // передаем модель в представление
        }
        /* Закончить валидацию данных формы
         * Реализовать поведение, при котором успешная валидация и создание
         * пользователя приведут к выводу сообщения об успешной регистрации
         * и очистят все поля формы (и рез-ты валидации)
         */

        private SignUpViewModel ValidateSignUpForm(SignUpFormModel formModel)
        {
            SignUpViewModel viewModel = new();

            #region Login Validation
            if (String.IsNullOrEmpty(formModel.Login)) 
            {
                viewModel.LoginMessage = "Логин не может быть пустым";
            }
            else if(formModel.Login.Length < 3)
            {
                viewModel.LoginMessage = "Логин слишком короткий (3 символа минимум)";
            }
            else if(_dataContext.Users.Any(u => u.Login == formModel.Login))
            {
                viewModel.LoginMessage = "Данный логин уже занят";
            }
            else
            {
                viewModel.LoginMessage = null;  // все проверки логина пройдены
            }
            #endregion
            
            #region Password validation
            if (String.IsNullOrEmpty(formModel.Password))
            {
                viewModel.PasswordMessage = "Пароль не может быть пустым";
            }
            else if (formModel.Password.Length < 3)
            {
                viewModel.PasswordMessage = "Пароль слишком короткий (3 символа минимум)";
            }
            else if( ! Regex.IsMatch(formModel.Password, @"\d"))
            {
                viewModel.PasswordMessage = "Пароль должен содержать цифры";
            }
            else
            {
                viewModel.PasswordMessage = null;  // все проверки пароля пройдены
            }
            #endregion

            // сохранение файла
            String nameAvatar = null!;
            if (formModel.Avatar != null)  // файл передан
            {
                /* При приема файла важно:
                 * - проверить допустимые расширения (тип)
                 * - проверить максимальный размер
                 * - заменить имя файла
                 * Создаем папку (вручную) wwwroot/avatars/
                 *  и в нее будем сохранять файлы, расширения - оставляем
                 *  какие есть, а вместо имени - используем GUID
                 */
                if(formModel.Avatar.Length > 1048576)
                {
                    viewModel.AvatarMessage = "Файл слишком большой (макс 1 МБ)";
                }
                // определяем расширение файла
                String ext = Path.GetExtension(formModel.Avatar.FileName);
                // проверить расширение на перечень допустимых

                // формируем имя для файла
                nameAvatar = Guid.NewGuid().ToString() + ext;

                formModel.Avatar.CopyTo(
                    new FileStream("wwwroot/avatars/" + nameAvatar, FileMode.Create));
            }

            if(viewModel.LoginMessage == null 
                && viewModel.PasswordMessage == null
                && viewModel.AvatarMessage == null)
            {
                // Все проверки пройдены успешно - добавляем пользователя в БД
                _dataContext.Users.Add(new()
                {
                    Id = Guid.NewGuid(),
                    Login = formModel.Login,
                    PasswordHash = _hashService.GetHash(formModel.Password),
                    Email = formModel.Email!,
                    CreatedDt = DateTime.Now,
                    Name = formModel.RealName!,
                    Avatar = nameAvatar
                });
                _dataContext.SaveChanges();
            }

            return viewModel;
        }
    }
}
/* Д.З. Реализовать валидацию всех полей формы, вывод сообщений
 * Сохранение файла реализовывать только если все поля формы прошли проверку
 */
