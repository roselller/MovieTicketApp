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
    class Order
    {
        public int OrderNo { get; set; }
        public DateTime OrderDateTime { get; set; }
        public double Amount { get; set; }
        public string Status { get; set; }
        public List<Ticket> ticketList = new List<Ticket>();

        public Order() { }
        public Order(int on, DateTime odt)
        {
            OrderNo = on;
            OrderDateTime = odt;
        }

        public void AddTicket(Ticket addt)
        {
            ticketList.Add(addt);
        }

        public override string ToString()
        {
            return "OrderNo: " + OrderNo + "OrderDateTime: " + OrderDateTime + "Amount: " + Amount + "Status: " + Status;
        }
    }
}
