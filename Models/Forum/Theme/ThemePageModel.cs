using ASP_111.Models.Forum.Topic;

namespace ASP_111.Models.Forum.Theme
{
    public class ThemePageModel
    {
        public ThemeViewModel Theme { get; set; } = null!;
        public List<CommentViewModel> Comments { get; set; } = null!;

        public CommentFormModel? FormModel { get; set; }
        public Dictionary<String, String?>? ErrorMessages { get; set; }

    }
}
