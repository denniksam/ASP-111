using ASP_111.Services.Validation;

namespace ASP_111.Models.Forum.Topic
{
    public class ThemeFormModel
    {
        [ValidationRules(ValidationRule.NotEmpty)]
        public String Title { get; set; } = null!;


        public Guid TopicId { get; set; }


        [ValidationRules(ValidationRule.NotEmpty)]
        public String Content { get; set; } = null!;   // для первого комментария
    }
}
