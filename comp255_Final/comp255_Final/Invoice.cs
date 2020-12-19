using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comp255_Final {
    class Invoice {
        // initialize instance variables
        private int invoiceID;
        private string customerName;
        private string customerAddress;
        private string customerEmail;
        private DateTime invoiceDate;
        private bool shippingStatus;

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
    }
}
