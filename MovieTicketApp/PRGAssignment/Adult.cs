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
    class Adult : Ticket
    {

        public bool PopcornOffer { get; set; }

        public Adult() { }
        public Adult(Screening scr, bool popoff) : base(scr)
        {
            PopcornOffer = popoff;
        }

        public override double CalculatedPrice()
        {
            double price = 0.0;

            if (base.Screening.ScreenType == "3D")
            {
                if (base.Screening.ScreeningDateTime.DayOfWeek == DayOfWeek.Friday || base.Screening.ScreeningDateTime.DayOfWeek == DayOfWeek.Saturday || base.Screening.ScreeningDateTime.DayOfWeek == DayOfWeek.Sunday)
                {
                    price += 14.00;
                }
                else
                {
                    price += 11.00;
                }
            }
            else
            {
                if (base.Screening.ScreeningDateTime.DayOfWeek == DayOfWeek.Friday || base.Screening.ScreeningDateTime.DayOfWeek == DayOfWeek.Saturday || base.Screening.ScreeningDateTime.DayOfWeek == DayOfWeek.Sunday)
                {
                    price += 12.50;
                }
                else
                {
                    price += 8.50;
                }
            }

            if (PopcornOffer == true)
                price += 3.00;

            return price;
        }

        public override string ToString()
        {
            return base.ToString() + "PopcornOffer: " + PopcornOffer;
        }
    }
}
