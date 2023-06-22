using System.Reflection;
using System.Text.RegularExpressions;

namespace ASP_111.Services.Validation
{
    public class ValidationServiceV1 : IValidationService
    {
        public Dictionary<string, string?> ErrorMessages(object model)
        {
            Dictionary<string, string?> result = new();

            foreach (var propertyInfo in model.GetType().GetProperties())
            {
                var validationAttribute =
                    propertyInfo.GetCustomAttribute<ValidationRules>();

                if (validationAttribute != null)
                {
                    // закладываем имя поля как ключ в словарь
                    result[propertyInfo.Name] = null;

                    // проходим циклам по всем правилам проверки
                    foreach (var rule in validationAttribute.Rules)
                    {
                        // получаем сообщение по данному правилу
                        String? message = _RuleMessage(
                            propertyInfo.GetValue(model), rule);

                        if(message != null)  // есть ошибка
                        {
                            if (result[propertyInfo.Name] == null)
                            {
                                // если до этого сообщений не было - ставим
                                result[propertyInfo.Name] = message;
                            }
                            else
                            {
                                // уже были сообщения о других ошибках - добавляем
                                result[propertyInfo.Name] += ", " + message;
                            }
                        }                        
                    }
                }
            }
            return result;
        }
        private String? _RuleMessage(object? data, ValidationRule rule)
        {
            return rule switch
            {
                ValidationRule.NotEmpty => _NotEmptyMessage(data),
                ValidationRule.Name => _NameMessage(data),
                _ => "Неизвестное правило проверки"
            };
        }
        private String? _NameMessage(object? data)
        {
            if (_ValidateName(data))
            {
                return null;
            }
            return "Не соответствует имени: содержит спецсимволы";
        }
        private String? _NotEmptyMessage(object? data)
        {
            if(_ValidateNotEmpty(data))
            {
                return null;
            }
            return "Не может быть пустым";
        }

        public bool IsValid(object model)
        {
            bool isValid = true;
            // как узнать какие у объекта model есть свойства?
            // использовать рефлексию - информацию о типе данных объекта
            foreach(var propertyInfo in model.GetType().GetProperties())
            {
                // извлекаем информацию об атрибуте ValidationRules
                var validationAttribute =
                    propertyInfo.GetCustomAttribute<ValidationRules>();

                // проверяем есть ли этот атрибут
                if (validationAttribute != null)
                {
                    // проходим циклом по всем правилам валидации
                    foreach(var rule in validationAttribute.Rules)
                    {
                        // и вызываем для них метод проверки
                        isValid &= _ValidateRule(
                            propertyInfo.GetValue(model),
                            rule);
                    }                    
                }
            }
            return isValid;
        }

        private bool _ValidateRule(object? data, ValidationRule rule)
        {
            return rule switch
            {
                ValidationRule.NotEmpty => _ValidateNotEmpty(data),
                ValidationRule.Name => _ValidateName(data),
                _ => false
            };
        }

        private bool _ValidateName(object? data)
        {
            if (data is String str)
            {
                return ! Regex.IsMatch(str, @"\W");                
            }
            else
            {
                return false;   // имя может быть только строкой
            }
        }
       
        private bool _ValidateNotEmpty(object? data)
        {
            if(data == null)
            {
                return false;
            }
            if(data is String str)
            {
                return str.Length != 0;
            }
            if(data is int x)
            {
                return x != 0;
            }
            return true;
        }

    }
}
/* OOP Reflection - рефлексия (типов) - работа с типом данных (который
 * в ООП-языках сам-по-себе является объектом)
 */
