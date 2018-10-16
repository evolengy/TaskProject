using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TaskProject.Models.DateTimeModels
{
    [NotMapped]
    public class Month
    {
        public int MonthId { get; set; }
        public string MonthName { get; set; }

        public Month(int monthId, string monthName)
        {
            MonthId = monthId;
            MonthName = monthName;
        }
    }
}
