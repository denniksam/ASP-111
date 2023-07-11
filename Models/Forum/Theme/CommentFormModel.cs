using ASP_111.Services.Validation;

namespace ASP_111.Models.Forum.Theme
{
    public class CommentFormModel
    {
        [ValidationRules(ValidationRule.NotEmpty)]
        public String Content { get; set; } = null!;

        public Guid ThemeId { get; set; }
    }
}
