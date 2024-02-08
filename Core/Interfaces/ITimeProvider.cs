using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ITimeProvider
    {
        DateTime Now { get; }

        DateTime? ParseDate(string input);
    }
}
