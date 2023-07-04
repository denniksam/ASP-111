using ASP_111.Models.Forum.Index;
using ASP_111.Models.User;

namespace ASP_111.Models.Forum.Section
{
    public class TopicViewModel
    {
        public String  Id { get; set; } = null!;
        public String  Title { get; set; } = null!;
        public String? Description { get; set; }
        public String? ImageUrl { get; set; }
        public String  CreateDt { get; set; } = null!;
        public ProfileViewModel Author { get; set; } = null!;
    }
}
