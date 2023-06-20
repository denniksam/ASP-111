namespace ASP_111.Data.Entities
{
    public class Comment
    {
        public Guid      Id       { get; set; }
        public String    Content  { get; set; } = null!;
        public Guid      AuthorId { get; set; }
        public Guid      ThemeId  { get; set; }
        public Guid?     ReplyId  { get; set; }
        public String?   ImageUrl { get; set; }
        public DateTime  CreateDt { get; set; }
        public DateTime? DeleteDt { get; set; }
    }
}
