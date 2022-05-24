using ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class CalendarRepository : ICalendarRepository
    {
        public Calendar DisplayDateTime()
        {
            return new Calendar
            {
                CurrentDateTime = DateTime.Now
            };

        }

    }
}
