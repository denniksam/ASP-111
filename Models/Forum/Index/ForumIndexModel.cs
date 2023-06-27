namespace ASP_111.Models.Forum.Index
{
    public class ForumIndexModel
    {
        public IEnumerable<ForumSectionViewModel> Sections { get; set; } = null!;
        public Dictionary<String, String?>? ErrorValidationMessages { get; set; }
        public ForumSectionFormModel? FormModel { get; set; }
    }
}
