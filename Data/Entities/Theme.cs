namespace ASP_111.Data.Entities
{
    public class Theme
    {
        public Guid      Id       { get; set; }
        public String    Title    { get; set; } = null!;
        public Guid      AuthorId { get; set; }
        public Guid      TopicId  { get; set; }
        public DateTime  CreateDt { get; set; }
        public DateTime? DeleteDt { get; set; }
    }
}
