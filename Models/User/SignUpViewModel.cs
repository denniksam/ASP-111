namespace ASP_111.Models.User
{
    public class SignUpViewModel
    {
        // если null, то проверка прошла успешно, а если есть значение, то
        // значит это сообщение об ошибке валидации
        public string? LoginMessage { get; set; }
        public string? PasswordMessage { get; set; }
        public string? RepeatMessage { get; set; }
        public string? RealNameMessage { get; set; }
        public string? EmailMessage { get; set; }
        public string? AvatarMessage { get; set; }
        public string? ConfirmMessage { get; set; }

        public SignUpFormModel? FormModel { get; set; }
    }
}

/* Модель для представления - данные для участия в отображении
 * Классы-модели более предпочтительны, чем множество параметров
 * ViewBag или ViewData
 */
