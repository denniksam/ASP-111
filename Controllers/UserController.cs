using ASP_111.Data;
using ASP_111.Models.User;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace ASP_111.Controllers
{
    public class UserController : Controller
    {
        private readonly DataContext _dataContext;

        public UserController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public ViewResult SignUp(SignUpFormModel? formModel)
        {
            SignUpViewModel viewModel;
            if(Request.Method == "POST" && formModel != null)
            {
                // передача формы
                viewModel = ValidateSignUpForm(formModel);
                viewModel.FormModel = formModel;
            }
            else
            {
                // первый заход - начало заполнения формы
                viewModel = new();
                viewModel.FormModel = null;  // нечего проверять
            }
            return View(viewModel);  // передаем модель в представление
        }

        private SignUpViewModel ValidateSignUpForm(SignUpFormModel formModel)
        {
            SignUpViewModel viewModel = new();
            if(String.IsNullOrEmpty(formModel.Login)) 
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

            // сохранение файла
            if(formModel.Avatar != null)  // файл передан
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
                String name = Guid.NewGuid().ToString() + ext;

                formModel.Avatar.CopyTo(
                    new FileStream("wwwroot/avatars/" + name, FileMode.Create));
            }
            return viewModel;
        }
    }
}
/* Д.З. Реализовать валидацию всех полей формы, вывод сообщений
 * Сохранение файла реализовывать только если все поля формы прошли проверку
 */
