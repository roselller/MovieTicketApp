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
    class Screening : IComparable<Screening>
    {
        public int ScreeningNo { get; set; }
        public DateTime ScreeningDateTime { get; set; }
        public string ScreenType { get; set; }
        public int SeatsRemaining { get; set; }
        public Cinema Cinema { get; set; }
        public Movie Movie { get; set; }

        public Screening() { }
        public Screening(int sn, DateTime sdt, string st, Cinema c, Movie m)
        {
            ScreeningNo = sn;
            ScreeningDateTime = sdt;
            ScreenType = st;
            Cinema = c;
            Movie = m;
        }

        public int CompareTo(Screening s)
        {
            if (SeatsRemaining > s.SeatsRemaining)
                return 1;
            else if (SeatsRemaining == s.SeatsRemaining)
                return 0;
            else
                return -1;
        }

        public override string ToString()
        {
            return "ScreeningNo: " + ScreeningNo + "ScreeningDateTime: " + ScreeningDateTime + "ScreenType: " + ScreenType + "SeatsReamining: " + SeatsRemaining + "Cinema: " + Cinema + "Movie: " + Movie;
        }
    }
}
