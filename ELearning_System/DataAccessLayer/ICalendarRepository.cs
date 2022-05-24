using ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface ICalendarRepository 
    {
        public Calendar DisplayDateTime();
    }
}
