using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRGAssignment
{
    class Recommend : IComparable<Recommend>
    {
        public Movie Movie { get; set; }
        public int Sold { get; set; }
        public Recommend() { }
        public Recommend(Movie m, int s)
        {
            Movie = m;
            Sold = s;
        }
        
        public int CompareTo(Recommend r)
        {
            if (Sold < r.Sold)
                return 1;
            else if (Sold == r.Sold)
                return 0;
            else
                return -1;
        }
    }
}
