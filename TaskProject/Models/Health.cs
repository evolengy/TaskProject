using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace TaskProject.Models
{
	public class Health
    {
        public Health()
        {
            IsSetValue = false;
            UserListGrowth = new List<UserGrowth>();
            UserListWeight = new List<UserWeight>();
        }

        public DateTime? DateBirth { get; set; }
        public DateTime? DateDeath { get; set; }

        public int HealthId { get; set; }

        public string Sex { get; set; }

        public virtual List<UserGrowth> UserListGrowth { get; set; }
        public virtual List<UserWeight> UserListWeight { get; set; }

        public bool IsDead { get; set; }
        public bool IsSetValue { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public IMT GetIMT(float growth, float weight)
        {
            IMT imt = new IMT();

            if (growth == 0 || weight == 0)
            {
                return imt = null;
            }

            float mGrowth = growth / 100;
            float count = weight / (mGrowth * mGrowth);

            imt = ListIMT.Where(i => i.MinCount <= count && i.MaxCount > count).SingleOrDefault();
            imt.Count = count;

            return imt;
        }
        public IMTAgeGroup GetAgeGroup()
        {
            IMTAgeGroup group = new IMTAgeGroup();

            int age = GetAge();

            group = ListAges.Where(a => a.MinAge <= age && a.MaxAge >= age).SingleOrDefault();
            return group;
        }

        private List<IMT> ListIMT = new List<IMT>()
        {
            new IMT
            {
                Class = "Анорексия",
                HealthRisk="Высокий",
                Advice = "Рекомендуется повышение веса",
                MinCount = 0f,
                MaxCount = 17.5f
            },
            new IMT
            {
                Class = "Дефицит массы тела",
                HealthRisk = "Нет",
                Advice = "Похудение не требуется",
                MinCount = 17.5f,
                MaxCount = 18.5f
            },
            new IMT
            {
                Class = "Норма",
                HealthRisk = "Нет",
                Advice = "Похудение не требуется",
                MinCount = 18.5f,
                MaxCount = 25f
            },
            new IMT
            {
                Class = "Избыток веса",
                HealthRisk = "Повышенный",
                Advice = "Рекомендуется похудение",
                MinCount = 25f,
                MaxCount = 30f
            },
            new IMT
            {
                Class = "Ожирение I степени (легкое)",
                HealthRisk = "Повышенный",
                Advice = "Рекомендуется снижение массы тела",
                MinCount = 30f,
                MaxCount = 35f
            },
            new IMT
            {
                Class = "Ожирение II степени (умеренное)",
                HealthRisk = "Высокий",
                Advice = "Настоятельно рекомендуется снижение массы тела",
                MinCount = 35f,
                MaxCount = 40f
            },
            new IMT
            {
                Class = "Ожирение III степени (тяжелое)",
                HealthRisk = "Очень высокий",
                Advice = "Необходимо немедленное снижение массы тела",
                MinCount = 40f,
                MaxCount = 1000f
            }
        };
        private List<IMTAgeGroup> ListAges = new List<IMTAgeGroup>()
        {
            new IMTAgeGroup
            {
                MinAge = 0,
                MaxAge = 24,
                MinIMTCount = 19,
                MaxIMTCount = 24
            },
            new IMTAgeGroup
            {
                MinAge = 25,
                MaxAge = 34,
                MinIMTCount = 20,
                MaxIMTCount = 25
            },
            new IMTAgeGroup
            {
                MinAge = 35,
                MaxAge = 44,
                MinIMTCount = 21,
                MaxIMTCount = 26
            },
            new IMTAgeGroup
            {
                MinAge = 45,
                MaxAge = 54,
                MinIMTCount = 22,
                MaxIMTCount = 27
            },
            new IMTAgeGroup
            {
                MinAge = 55,
                MaxAge = 64,
                MinIMTCount = 23,
                MaxIMTCount = 28
            },
            new IMTAgeGroup
            {
                MinAge = 65,
                MaxAge = 1000,
                MinIMTCount = 24,
                MaxIMTCount = 29
            }
        };

        /*Умер ли персонаж или нет*/
        public void CheckDead()
        {
            if (this.IsDead || GetCurrentHealth() <= 0)
            {
                this.IsDead = true;
            }
        }

        /*Получить количество прожитых лет*/
        public int GetAge()
        {
            int age;
            if (DateBirth == null)
            {
                return age = 0;
            }

            if (DateBirth.Value.Day <= TimeZoneInfo.ConvertTimeToUtc(DateTime.Now).Day && DateBirth.Value.Month <= TimeZoneInfo.ConvertTimeToUtc(DateTime.Now).Month)
            {
                return age = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now).Year - DateBirth.Value.Year;
            }
            else
            {
                return age = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now).AddYears(-1).Year - DateBirth.Value.Year;
            }

        }

        /*Получить количество дней до даты смерти*/
        public double GetCurrentHealth()
        {
            double currentHealth;
            return currentHealth = (DateDeath - DateBirth - (TimeZoneInfo.ConvertTimeToUtc(DateTime.Now).Date - DateBirth)).Value.TotalDays;
        }

        /*Получить количество дней от рождения до даты смерти*/
        public double GetMaxHealth()
        {
            double maxHealth;
            return maxHealth = (DateDeath - DateBirth).Value.TotalDays;
        }
    }

    public class ViewSetProfileModel
    {
        [Required(ErrorMessage = "Не задана дата рождения")]
        public DateTime? DateBirth { get; set; }
        public DateTime DateDeath { get; set; }

        [Required(ErrorMessage = "Не задан рост"), Range(50, 300, ErrorMessage = "Указан неверный рост ( от 50 см до 300 см )")]
        public float Growth { get; set; }
        [Required(ErrorMessage = "Не задан вес"), Range(30, 300, ErrorMessage = "Указан неверный вес (от 30 кг до 300 кг")]
        public float Weight { get; set; }

        public string Sex { get; set; }
        public bool IsMarriage { get; set; }
        public bool IsSexRelation { get; set; }
        public bool IsStrongTan { get; set; }
        public bool IsAlcoholDrink { get; set; }
        public bool IsSmoke { get; set; }
        public bool IsStress { get; set; }
        public bool IsSport { get; set; }
        public bool IsDiabetes { get; set; }
        public bool IsParents75 { get; set; }
        public bool IsParents90 { get; set; }
        public bool IsAdversePlace { get; set; }
        public bool IsAspirin { get; set; }
        public bool IsVitamit { get; set; }
        public bool IsDentalFloss { get; set; }
        public bool IsRegularChair { get; set; }
        public bool IsFriedFood { get; set; }
        public bool IsFattyFood { get; set; }
        public bool IsFastFood { get; set; }
        public bool IsCoffee { get; set; }
        public string Food { get; set; }

        public SelectList SexSelect { get; set; } = new SelectList(
                new List<SelectListItem>
                {
                        new SelectListItem {Text = "Мужской", Value = "Man"},
                        new SelectListItem {Text = "Женский", Value = "Woman"}
                }, "Value", "Text"
                );
        public SelectList SmokeSelect { get; set; } = new SelectList(

            new List<SelectListItem>
            {
                new SelectListItem{Text = "Да" , Value = "true"},
                new SelectListItem{Text = "Нет", Value = "false"}
            }, "Value", "Text"
        );
        public SelectList FriedFoodSelect { get; set; } = new SelectList(

            new List<SelectListItem>
            {
                new SelectListItem{Text = "Да" , Value = "true"},
                new SelectListItem{Text = "Нет", Value = "false"}
            }, "Value", "Text"
        );
        public SelectList FattyFoodSelect { get; set; } = new SelectList(

            new List<SelectListItem>
            {
                new SelectListItem{Text = "Да" , Value = "true"},
                new SelectListItem{Text = "Нет", Value = "false"}
            }, "Value", "Text"
        );
        public SelectList FoodSelect { get; set; } = new SelectList(

            new List<SelectListItem>
            {
                new SelectListItem{Text = "Овощи" , Value = "green"},
                new SelectListItem{Text = "Мясо", Value = "meat"}
            }, "Value", "Text"
        );
        public SelectList FastFoodSelect { get; set; } = new SelectList(

            new List<SelectListItem>
            {
                new SelectListItem{Text = "Да" , Value = "true"},
                new SelectListItem{Text = "Нет", Value = "false"}
            }, "Value", "Text"
        );
        public SelectList CoffeeSelect { get; set; } = new SelectList(

            new List<SelectListItem>
            {
                new SelectListItem{Text = "Да" , Value = "true"},
                new SelectListItem{Text = "Нет", Value = "false"}
            }, "Value", "Text"
        );
        public SelectList AlcoholSelect { get; set; } = new SelectList(

            new List<SelectListItem>
            {
                new SelectListItem{Text = "Да" , Value = "true"},
                new SelectListItem{Text = "Нет", Value = "false"}
            }, "Value", "Text"
        );
        public SelectList AdversePlaceSelect { get; set; } = new SelectList(

            new List<SelectListItem>
            {
                new SelectListItem{Text = "Да" , Value = "true"},
                new SelectListItem{Text = "Нет", Value = "false"}
            }, "Value", "Text"
        );
        public SelectList AspirinSelect { get; set; } = new SelectList(

            new List<SelectListItem>
            {
                new SelectListItem{Text = "Да" , Value = "true"},
                new SelectListItem{Text = "Нет", Value = "false"}
            }, "Value", "Text"
        );
        public SelectList DentalFlossSelect { get; set; } = new SelectList(

            new List<SelectListItem>
            {
                new SelectListItem{Text = "Да" , Value = "true"},
                new SelectListItem{Text = "Нет", Value = "false"}
            }, "Value", "Text"
        );
        public SelectList RegularChairSelect { get; set; } = new SelectList(

            new List<SelectListItem>
            {
                new SelectListItem{Text = "Да" , Value = "true"},
                new SelectListItem{Text = "Нет", Value = "false"}
            }, "Value", "Text"
        );
        public SelectList SexRelationSelect { get; set; } = new SelectList(

            new List<SelectListItem>
            {
                new SelectListItem{Text = "Да" , Value = "true"},
                new SelectListItem{Text = "Нет", Value = "false"}
            }, "Value", "Text"
        );
        public SelectList StrongTanSelect { get; set; } = new SelectList(

            new List<SelectListItem>
            {
                new SelectListItem{Text = "Да" , Value = "true"},
                new SelectListItem{Text = "Нет", Value = "false"}
            }, "Value", "Text"
        );
        public SelectList MarriageSelect { get; set; } = new SelectList(

            new List<SelectListItem>
            {
                new SelectListItem{Text = "Да" , Value = "true"},
                new SelectListItem{Text = "Нет", Value = "false"}
            }, "Value", "Text"
        );
        public SelectList StressSelect { get; set; } = new SelectList(

            new List<SelectListItem>
            {
                new SelectListItem{Text = "Да" , Value = "true"},
                new SelectListItem{Text = "Нет", Value = "false"}
            }, "Value", "Text"
        );
        public SelectList DiabetesSelect { get; set; } = new SelectList(

            new List<SelectListItem>
            {
                new SelectListItem{Text = "Да" , Value = "true"},
                new SelectListItem{Text = "Нет", Value = "false"}
            }, "Value", "Text"
        );
        public SelectList Parents75Select { get; set; } = new SelectList(

            new List<SelectListItem>
            {
                new SelectListItem{Text = "Да" , Value = "true"},
                new SelectListItem{Text = "Нет", Value = "false"}
            }, "Value", "Text"
        );
        public SelectList Parents90Select { get; set; } = new SelectList(

           new List<SelectListItem>
           {
                new SelectListItem{Text = "Да" , Value = "true"},
                new SelectListItem{Text = "Нет", Value = "false"}
           }, "Value", "Text"
       );
        public SelectList SportsSelest { get; set; } = new SelectList(

            new List<SelectListItem>
            {
                new SelectListItem{Text = "Да" , Value = "true"},
                new SelectListItem{Text = "Нет", Value = "false"}
            }, "Value", "Text"
        );
        public SelectList VitaminSelect { get; set; } = new SelectList(

            new List<SelectListItem>
            {
                new SelectListItem{Text = "Да" , Value = "true"},
                new SelectListItem{Text = "Нет", Value = "false"}
            }, "Value", "Text"
        );
    }

    [NotMapped]
    public class IMT
    {
        public float Count { get; set; }
        public float MinCount { get; set; }
        public float MaxCount { get; set; }
        public string Class { get; set; }
        public string HealthRisk { get; set; }
        public string Advice { get; set; }
    }

    [NotMapped]
    public class IMTAgeGroup
    {
        public int MinAge { get; set; }
        public int MaxAge { get; set; }

        public float MinIMTCount { get; set; }
        public float MaxIMTCount { get; set; }
    }

    public class UserGrowth
    {
        public int UserGrowthId { get; set; }
        public float Value { get; set; }
        public DateTime Date { get; set; }

        [ForeignKey("Health")]
        public int HealthId { get; set; }
        public Health Health { get; set; }
    }

    public class UserWeight
    {
        public int UserWeightId { get; set; }
        public float Value { get; set; }
        public DateTime Date { get; set; }

        [ForeignKey("Health")]
        public int HealthId { get; set; }
        public Health Health { get; set; }
    }
}

