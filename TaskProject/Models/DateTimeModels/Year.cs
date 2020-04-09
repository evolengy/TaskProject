using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskProject.Models.DateTimeModels
{
	[NotMapped]
    public class Year
    {
        public List<Month> Months { get; private set; }
        public int YearId { get; private set; }

        public Year(int yearId)
        {
            YearId = yearId;
        }

        public Year()
        {
            Months = new List<Month>()
            {
                new Month(1,"Январь"),
                new Month(2,"Февраль"),
                new Month(3,"Март"),
                new Month(4,"Апрель"),
                new Month(5,"Май"),
                new Month(6,"Июнь"),
                new Month(7,"Июль"),
                new Month(8,"Август"),
                new Month(9,"Сентябрь"),
                new Month(10,"Октябрь"),
                new Month(11,"Ноябрь"),
                new Month(12,"Декабрь"),
            };
        }
    }
}
