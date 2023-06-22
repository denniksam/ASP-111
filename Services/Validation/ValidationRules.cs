namespace ASP_111.Services.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ValidationRules : Attribute
    {
        public ValidationRule[] Rules { get; init; }
        public ValidationRules(params ValidationRule[] rules)
        { 
            Rules = rules;
        }
    }
}
/* Вариадические функции (вариадики, variadic) - 
 *  функции с переменным кол-вом параметров
 *  fun(1), fun(1,2,3), fun(1,2,3,4,5,6)
 *  В C# для их описания используется params
 */
