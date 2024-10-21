using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Concertticketing;


namespace Concertticketing
{
    public class PurchaseDetails
    {

        public string EventName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EventDate { get; set; }
        public string Location { get; set; }
        public string Time { get; set; }
        public string TicketType { get; set; }
        public string VIPQuantity { get; set; } // Changed to string
        public string GenAdmissionQuantity { get; set; } // Changed to string
        public string VIPSeats { get; set; }
        public string GenAdmissionSeats { get; set; }
        public double VIPPrice { get; set; }
        public double GenAdmissionPrice { get; set; }
        public double TotalPrice { get; set; }
        public double vipprice { get; set; }
        public double genprice{ get; set; }



    }
}
