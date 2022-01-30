using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGAssignment
{
    //==================================================================
    // Student Number : S10222137, S10223630
    // Student Name : Chen Junrui Justin, Armirola Roseller Iii Tumolva
    // Module Group : T04
    //==================================================================
    class SeniorCitizen : Ticket
    {
        public int YearOfBirth { get; set; }

        public SeniorCitizen() { }
        public SeniorCitizen(Screening scr, int yob) : base(scr)
        {
            YearOfBirth = yob;
        }

        public override double CalculatedPrice()
        {
            if (DateTime.Now <= base.Screening.Movie.OpeningDate.AddDays(7))
            {
                if (base.Screening.ScreenType == "2D")
                {
                    return 12.50;
                }
                else
                {
                    return 14.00;
                }
            }
            else
            {
                if (base.Screening.ScreenType == "2D")
                {
                    if (base.Screening.ScreeningDateTime.DayOfWeek == DayOfWeek.Friday || base.Screening.ScreeningDateTime.DayOfWeek == DayOfWeek.Saturday || base.Screening.ScreeningDateTime.DayOfWeek == DayOfWeek.Sunday)
                    {
                        return 12.50;
                    }
                    else
                    {
                        return 5.00;
                    }
                }
                else
                {
                    if (base.Screening.ScreeningDateTime.DayOfWeek == DayOfWeek.Friday || base.Screening.ScreeningDateTime.DayOfWeek == DayOfWeek.Saturday || base.Screening.ScreeningDateTime.DayOfWeek == DayOfWeek.Sunday)
                    {
                        return 14.00;
                    }
                    else
                    {
                        return 6.00;
                    }
                }
            }
        }

        public override string ToString()
        {
            return base.ToString() + "YearOfBirth: " + YearOfBirth;
        }
    }
}
