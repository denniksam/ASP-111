namespace ASP_111.Services
{
    public class DateTimeService
    {
        private readonly DateService _dateService;
        private readonly TimeService _timeService;

        public DateTimeService(DateService dateService, TimeService timeService)
        {
            _dateService = dateService;
            _timeService = timeService;
        }

        public DateTime GetNow() => _dateService.GetDate() 
            + _timeService.GetTime().ToTimeSpan();

        // public DateTime GetNow() => DateTime.Now;
    }
}
