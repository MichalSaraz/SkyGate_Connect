using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Time
{
    public class TestTimeProvider : TimeProviderBase
    {
        public new DateTime Now { get; private set; }

        public TestTimeProvider()
        {
            SetSimulatedTime(2023, 10, 15);
        }

        public void SetSimulatedTime(int year, int month, int day)
        {
            Now = new DateTime(year, month, day);
        }

        protected override DateTime? ParseDateWithCurrentYear(DateTime date)
        {
            var currentYear = 2023;
            return new DateTime(currentYear, date.Month, date.Day).Date;
        }
    }    
}
