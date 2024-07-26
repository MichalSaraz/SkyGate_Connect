namespace Core.Time
{
    public class TestTimeProvider : TimeProviderBase
    {
        public new DateTime Now { get; private set; }

        public TestTimeProvider()
        {
            _SetSimulatedTime(2023, 10, 15);
        }

        private void _SetSimulatedTime(int year, int month, int day)
        {
            Now = new DateTime(year, month, day);
        }

        protected override DateTime ParseDateWithCurrentYear(DateTime date)
        {
            var currentYear = 2023;
            return new DateTime(currentYear, date.Month, date.Day, date.Hour, date.Minute, date.Second);
        }
    }    
}
