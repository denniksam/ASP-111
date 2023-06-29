namespace ASP_111.Models.Forum.Section
{
    // Модель с данными для всей страницы Section
    public class SectionViewModel
    {
        public string SectionId { get; set; } = null!;
        public Dictionary<String, String?>? ErrorMessages { get; set; }
    }
}
