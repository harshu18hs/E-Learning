using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectModel
{
    public class Calendar
    {
        [Display(Name = "Current Date & Time ")]
        public DateTime CurrentDateTime { get; set; }

         public int Id { get; set; }
         public DateTime Start { get; set; }
         public DateTime End { get; set; }
         public string Text { get; set; }
         public string Color { get; set; }
        }
    }

