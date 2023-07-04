using ASP_111.Models.Forum.Index;

namespace ASP_111.Models.Forum.Section
{
    // Модель с данными для всей страницы Section
    public class SectionPageModel
    {
        public ForumSectionViewModel Section { get; set; } = null!;
        public Dictionary<String, String?>? ErrorMessages { get; set; }
        public List<TopicViewModel> Topics { get; set; } = null!;
    }
}
