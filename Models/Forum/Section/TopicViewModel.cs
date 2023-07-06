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
        public UserViewModel Author { get; set; } = null!;

        public TopicViewModel()
        {
            
        }
        public TopicViewModel(Data.Entities.Topic topic)
        {
            Id = topic.Id.ToString();
            Title = topic.Title;
            Description = topic.Description;
            ImageUrl = "/img/" + topic.ImageUrl;
            CreateDt = topic.CreateDt.ToShortDateString();
            Author = new(topic.Author);
        }
    }
}
