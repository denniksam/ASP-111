namespace ASP_111.Data.Entities
{
    public class Section
    {
        public Guid      Id          { get; set; }
        public String    Title       { get; set; } = null!;
        public String    Description { get; set; } = null!;
        public String?   ImageUrl    { get; set; }
        public Guid      AuthorId    { get; set; }
        public DateTime  CreateDt    { get; set; }
        public DateTime? DeleteDt    { get; set; }
    }
}
