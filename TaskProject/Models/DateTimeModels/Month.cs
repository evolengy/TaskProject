using System.ComponentModel.DataAnnotations.Schema;

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
