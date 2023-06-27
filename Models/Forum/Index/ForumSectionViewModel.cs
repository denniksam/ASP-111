namespace ASP_111.Models.Forum.Index
{
    public class ForumSectionViewModel
    {
        public String Id          { get; set; } = null!;
        public String Title       { get; set; } = null!;
        public String Description { get; set; } = null!;
        public String ImageUrl    { get; set; } = null!;
        public String CreateDt    { get; set; } = null!;
        public UserViewModel Author { get; set; } = null!;
    }
}
