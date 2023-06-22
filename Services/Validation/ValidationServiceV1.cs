using System.Reflection;

namespace ASP_111.Services.Validation
{
    public class ValidationServiceV1 : IValidationService
    {
        public Dictionary<string, string?> ErrorMessages(object model)
        {
            throw new NotImplementedException();
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
                    if(validationAttribute.Rules  // набор правил для проверки
                        .Contains(ValidationRule.NotEmpty))
                    {
                        isValid &= _ValidateNotEmpty(
                            propertyInfo.GetValue(model)
                        );
                    }
                }
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
