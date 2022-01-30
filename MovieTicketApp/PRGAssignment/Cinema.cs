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
    class Cinema
    {
        public string Name { get; set; }
        public int HallNo { get; set; }
        public int Capacity { get; set; }
        public Cinema() { }
        public Cinema(string n, int hn, int c)
        {
            Name = n;
            HallNo = hn;
            Capacity = c;
        }
        public override string ToString()
        {
            return "Name: " + Name + "HallNo: " + HallNo + "Capacity: " + Capacity;
        }
    }
}
