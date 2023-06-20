namespace ASP_111.Data.Entities
{
    public class Rate
    {
        public Guid     ItemId { get; set; }
        public Guid     UserId { get; set; }
        public DateTime Moment { get; set; }
        public int      Rating { get; set; }
    }
}
