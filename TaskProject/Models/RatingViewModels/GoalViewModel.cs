using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskProject.Models.RatingViewModels
{
    public class GoalViewModel
    {
        public GoalViewModel(string email, int allGoals, int completeGoals)
        {
            Email = email;
            AllGoals = allGoals;
            CompleteGoals = completeGoals;
        }

        [Display(Name = "Пользователь")]
        public string Email { get; }
        [Display(Name = "Количество поставленных задач")]
        public int AllGoals { get; }
        [Display(Name = "Количество выполненных задач")]
        public int CompleteGoals { get; }
    }
}
