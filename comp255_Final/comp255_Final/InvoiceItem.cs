using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comp255_Final {
    class InvoiceItem {
        // initialize instance variables
        private int itemID;
        private string itemName;
        private string itemDescription;
        private int itemQuantity;
        private double itemPrice;

        // Default Constructor
        public InvoiceItem() { }

        // Custom Constructor
        public InvoiceItem(int ItmID, string ItmName, string ItmDescription,
                           int ItmQuantity, double ItmPrice) {
            ItemID = ItmID;
            ItemName = ItmName;
            ItemDescription = ItmDescription;
            ItemQuantity = ItmQuantity;
            ItemPrice = ItmPrice;
        }

        // Getters and Setters
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public int ItemQuantity { get; set; }
        public double ItemPrice { get; set; }
    }
}
