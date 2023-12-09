using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Time
{
    public class TestTimeProvider : ITimeProvider
    {
        private DateTime simulatedTime;

        public TestTimeProvider(DateTime initialTime)
        {
            simulatedTime = initialTime;
        }

        public void SetSimulatedTime(DateTime time)
        {
            simulatedTime = time;
        }

        public DateTime Now => simulatedTime;
    }
}
