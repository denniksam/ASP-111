namespace ASP_111.Services
{
    public class TimeService
    {
        public TimeOnly GetTime() => TimeOnly.FromDateTime(DateTime.Now);
    }
}
