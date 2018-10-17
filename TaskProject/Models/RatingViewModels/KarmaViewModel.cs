using System.ComponentModel.DataAnnotations;

namespace TaskProject.Models.RatingViewModels
{
    public class KarmaViewModel
    {
        public KarmaViewModel(string email, int allKarma, int goodKarma)
        {
            Email = email;
            AllKarma = allKarma;
            GoodKarma = goodKarma;
        }

        [Display(Name = "Пользователь")]
        public string Email { get; }
        [Display(Name = "Количество поступков")]
        public int AllKarma { get; }
        [Display(Name = "Хорошие поступки")]
        public int GoodKarma { get; }
    }
}
