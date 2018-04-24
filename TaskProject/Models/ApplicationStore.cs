using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskLibrary;

namespace TaskProject.Models
{
    public static class ApplicationStore
    {
        public static void Initialize(ApplicationDbContext context)
        {
            if (!context.Repeats.Any())
            {
                context.Repeats.AddRange(
                    new Repeat
                    {
                        RepeatId = 1,
                        Name = "Без повторений"

                    },
                    new Repeat
                    {
                        RepeatId = 2,
                        Name = "Ежедневное"
                    },
                    new Repeat
                    {
                        RepeatId = 3,
                        Name = "Ежемесячное"
                    },
                    new Repeat
                    {
                        RepeatId = 4,
                        Name = "Ежегодное"
                    }
                    );  
            }

            if (!context.Complications.Any())
            {
                context.Complications.AddRange(
                    new Complication
                    {
                        ComplicationId = 1,
                        Name = "Легкая",
                        Damage = 5,
                        Exp = 10,
                        Gold = 2
                    },
                    new Complication
                    {
                        ComplicationId = 2,
                        Name = "Средняя",
                        Damage = 10,
                        Exp = 20,
                        Gold = 5
                    },
                    new Complication
                    {
                        ComplicationId = 3,
                        Name = "Тяжелая",
                        Damage = 25,
                        Exp = 50,
                        Gold = 10
                    }
                    );
            }

            if (!context.Atributes.Any())
            {
                context.Atributes.AddRange(
                    new Atribute {
                        AtributeId = 1,
                        Name = "Сила и выносливость",
                        Description = "Характеристики, отвечающие за общее физическое здоровье персонажа"
                    },
                    new Atribute {
                        AtributeId = 2,
                        Name = "Харизма",
                        Description = "Характеристика, отвечающая за навыки взаимодействия с другими людьми"
                    },
                    new Atribute {
                        AtributeId = 3,
                        Name = "Интеллект",
                        Description = "Характеристика, отвечающая за умственное развитие персонажа"
                    },
                    new Atribute {
                        AtributeId = 4,
                        Name = "Известность",
                        Description = "Характеристика, отвечающая за влияние персонажа в обществе"
                    },
                    new Atribute {
                        AtributeId = 5,
                        Name = "Психика",
                        Description = "Характеристика, отвечающая за стрессоустойчивость персонажа и его психическое состояние"
                    },
                    new Atribute {
                        AtributeId = 6,
                        Name = "Без характеристики",
                        Description = "Не установлена характеристика"
                    }
                    );

                context.SaveChanges();
            }
        }
    }
}
