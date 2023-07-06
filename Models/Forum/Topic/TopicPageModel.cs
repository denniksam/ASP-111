using ASP_111.Models.Forum.Section;

namespace ASP_111.Models.Forum.Topic
{
    public class TopicPageModel
    {
        public TopicViewModel Topic { get; set; } = null!;
        public List<ThemeViewModel> Themes { get; set; } = null!;
        public ThemeFormModel? FormModel { get; set; }
        public Dictionary<String, String?>? ErrorMessages { get; set; }
    }
}
