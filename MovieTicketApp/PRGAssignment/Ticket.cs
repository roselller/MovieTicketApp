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
    abstract class Ticket
    {
        public Screening Screening { get; set; }
        public Ticket() { }
        public Ticket(Screening scr)
        {
            Screening = scr;
        }
        public abstract double CalculatedPrice();
        public override string ToString()
        {
            return "Screening: " + Screening;
        }
    }
}
