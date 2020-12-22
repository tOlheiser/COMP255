using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comp255_Final {
    class Invoice {
        // Default Constructor
        public Invoice() { }

        // Custom Constructor
        public Invoice(int InvID, string CustName, string CustAddress, string CustEmail,
                       DateTime InvDate, bool ShipStatus) {
            InvoiceID = InvID;
            CustomerName = CustName;
            CustomerAddress = CustAddress;
            CustomerEmail = CustEmail;
            InvoiceDate = InvDate;
            ShippingStatus = ShipStatus;
        }

        // Getters and Setters
        public int InvoiceID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerEmail { get; set; }
        public DateTime InvoiceDate { get; set; }
        public bool ShippingStatus { get; set; }

        // Override ToString to display useful info in the listbox
        public override string ToString() {
            // Determine what the value of shipped is
            string Shipped = ShippingStatus == true ? "Yes" : "No";
            // Output formatted string
            return $"{InvoiceID, 5}{" ",-20}{CustomerName, -48}{CustomerEmail.Trim(), -34}{Shipped, -8}";
        }

    }
}
