using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comp255_Final {
    class InvoiceItem {
        // initialize instance variables
        private int itemID;
        private int invoiceID;
        private string itemName;
        private string itemDescription;
        private int itemQuantity;
        private double itemPrice;

        // Default Constructor
        public InvoiceItem() { }

        // Custom Constructor
        public InvoiceItem(int ItmID, int InvID, string ItmName, string ItmDescription,
                           int ItmQuantity, double ItmPrice) {
            ItemID = ItmID;
            InvoiceID = InvID;
            ItemName = ItmName;
            ItemDescription = ItmDescription;
            ItemQuantity = ItmQuantity;
            ItemPrice = ItmPrice;
        }

        // Getters and Setters
        public int ItemID { get; set; }
        public int InvoiceID { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public int ItemQuantity { get; set; }
        public double ItemPrice { get; set; }

        // Override ToString to display useful info in the listbox
        // ---> Reference: https://tinyurl.com/lcc96th
        public override string ToString() {
            // Test data
            // Implement formatted string later.
            return ItemName + " has an ItemID of " + ItemID;
        }

    }
}
