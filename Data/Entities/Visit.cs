namespace ASP_111.Data.Entities
{
    public class Visit
    {
        public Guid     Id     { get; set; }
        public Guid     ItemId { get; set; }
        public Guid     UserId { get; set; }
        public DateTime Moment { get; set; }
    }
}
