using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TaskProject.Models
{
    [NotMapped]
    public class Health
    {
        private float growth, weight;
        private int age;

        public Health(float _growth, float _weight, int _age)
        {
            growth = _growth;
            weight = _weight;
            age = _age;

            IMT = GetIMT(growth, weight);
            IMTAgeGroup = GetAgeGroup(age);
            
        }

        public IMT IMT { get; private set; }
        public IMTAgeGroup IMTAgeGroup { get; private set; }

        private IMT GetIMT(float growth, float weigth)
        {
            IMT imt = new IMT();

            if (growth == 0 || weight == 0)
            {
                return imt = null;
            }

            float mGrowth = growth / 100;
            float count = weigth /(mGrowth * mGrowth);   

            imt = ListIMT.Where(i => i.MinCount <= count && i.MaxCount > count).SingleOrDefault();
            imt.Count = count;

            return imt;
        }
        private IMTAgeGroup GetAgeGroup(int age)
        {
            IMTAgeGroup group = new IMTAgeGroup();

            if(age == 0)
            {
                return group = null;
            }

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
                MinAge = 19,
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
}
