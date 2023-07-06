namespace ASP_111.Models.Forum.Topic
{
    public class ThemeViewModel
    {
        public String Id { get; set; } = null!;
        public String Title { get; set; } = null!;
        public String CreatedDt { get; set; } = null!;
        public UserViewModel Author { get; set; } = null!;
    }
}
