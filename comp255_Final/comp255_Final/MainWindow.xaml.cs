using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace comp255_Final
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        // Setting up variables and list collections to track state
        double Subtotal = 0; double GST = 0; double PST = 0; double Total = 0;
        bool IsNewInvoice, IsNewInvoiceItem;
        int InvoiceIndex = 0; int InvoiceItemsIndex = 0;
        Invoice CurrentInvoice = new Invoice();
        InvoiceItem CurrentInvoiceItem = new InvoiceItem();
        List<Invoice> InvoiceCollection = new List<Invoice>();
        List<InvoiceItem> InvoiceItemCollection = new List<InvoiceItem>();
        
        
        // Might need to store the CurrentIndex
        // I used a KeyCounter to setup the primary key values when Inserting recordsS

        public MainWindow() {
            InitializeComponent();

            // Load the invoices then set the index to the first item of that collection
            LoadInvoices();
            //LoadInvoiceItems(InvoiceCollection[InvoiceIndex].InvoiceID);
            //DisplayInvoiceRecords();
            //DisplayInvoiceItemRecords();

            // On selection change, the index of the selected listbox item will be used here.
            //CurrentInvoice = InvoiceCollection[InvoiceIndex];

            //DisplayInvoiceFields();

        }

        // Events


        // Methods
       /* void LoadData(string SelectedData) {
            // Create methods to load 
            if (SelectedData == "Invoice") {
                LoadInvoice();
            } else {
                LoadInvoiceItem();
            }
        } // end LoadData method
       */

        void LoadInvoices() {
            // Clear the collections
            InvoiceCollection.Clear();
            InvoiceItemCollection.Clear();
            InvoiceListbox.Items.Clear();

            // Setup and open a connection
            using (SqlConnection connection = new SqlConnection()) {
                // Set the connection string
                connection.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Owner\Development\comp-255\comp255_Final\comp255_Final\bin\Debug\ShoppingCart.mdf; Integrated Security=True";

                // Open the connection to make use of it
                connection.Open();

                // Create a query to select all records
                string query = "SELECT * FROM Invoices";

                // Create the select command passing in the query & the connection
                SqlCommand SelectCommand = new SqlCommand(query, connection);

                // Execute the command and obtain a data reader containing the data
                using (SqlDataReader Reader = SelectCommand.ExecuteReader()) {
                    // Reads all the rows from the reader, returns false when no rows are left
                    while (Reader.Read()) {
                        // First, create a temporary instance of the Invoice class
                        Invoice TempInvoice = new Invoice((int)Reader["InvoiceID"],
                                                          (string)Reader["CustomerName"],
                                                          (string)Reader["CustomerAddress"],
                                                          (string)Reader["CustomerEmail"],
                                                          (DateTime)Reader["InvoiceDate"],
                                                          (bool)Reader["Shipped"] ); 

                        // Add the object to the list collection & listbox
                        InvoiceCollection.Add(TempInvoice);
                        InvoiceListbox.Items.Add(TempInvoice);
                    }
                }
            }
        }

        void LoadInvoiceItems(int InvoiceID) {
            // Clear the InvoiceItemCollection
            InvoiceItemCollection.Clear();
            InvoiceItemsListbox.Items.Clear();
            Subtotal = 0;

            // Setup and open a connection
            using (SqlConnection connection = new SqlConnection()) {
                // Set the connection string
                connection.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Owner\Development\comp-255\comp255_Final\comp255_Final\bin\Debug\ShoppingCart.mdf; Integrated Security=True";

                // Open the connection to make use of it
                connection.Open();

                /* Create a query to select all records where the foreign key of InvoiceItems 
                   matches the primary key of Invoices */
                string query = "SELECT * FROM InvoiceItems WHERE InvoiceID = " + InvoiceID + ";";

                // Create the select command passing in the query & the connection
                SqlCommand SelectCommand = new SqlCommand(query, connection);

                // Execute the command and obtain a data reader containing the data
                using (SqlDataReader Reader = SelectCommand.ExecuteReader()) {
                    // Reads all the rows from the reader, returns false when no rows are left
                    while (Reader.Read()) {
                        // First, create a temporary instance of the InvoiceItems class
                        InvoiceItem TempInvoiceItem = new InvoiceItem((int)Reader["ItemID"],
                                                                      (int)Reader["InvoiceID"],
                                                                      (string)Reader["ItemName"],
                                                                      (string)Reader["ItemDescription"],
                                                                      (int)Reader["ItemQuantity"],
                                                                      Convert.ToDouble(Reader["ItemPrice"]) );
                        Subtotal += (int)Reader["ItemQuantity"] * Convert.ToDouble(Reader["ItemPrice"]);
                        // Add the object to the list collection
                        //InvoiceItemCollection.Add(TempInvoiceItem);
                        InvoiceItemsListbox.Items.Add(TempInvoiceItem);
                    }
                }
            }
        } // end LoadInvoiceItems

        void DisplayInvoiceRecords() {
            // Loop over InvoiceCollection to add string representations of Invoice objects
            //InvoiceListbox.Items.Clear();
            for (int i = 0; i < InvoiceCollection.Count; i++) {
                InvoiceListbox.Items.Add(InvoiceCollection[i].ToString());
            }
        }

        void DisplayInvoiceItemRecords() {
            //InvoiceListbox.Items.Clear();
            // Loop over InvoiceItemCollection to add string representations of InvoiceItem objects
            for (int i = 0; i < InvoiceItemCollection.Count; i++) {
                InvoiceItemsListbox.Items.Add(InvoiceItemCollection[i].ToString());
            }
        }

        void DisplayInvoiceFields() {
            // Update the Invoice fields
            if (CurrentInvoice != null) {
                InvoiceIDTextbox.Text = Convert.ToString(CurrentInvoice.InvoiceID);//Convert.ToString(CurrentInvoice.InvoiceID);
                CustomerNameTextbox.Text = CurrentInvoice.CustomerName;
                CustomerAddressTextbox.Text = CurrentInvoice.CustomerAddress;
                InvoiceDateTextbox.Text = Convert.ToString(CurrentInvoice.InvoiceDate);
                CustomerEmailTextbox.Text = CurrentInvoice.CustomerEmail;
                ShippedCheckbox.IsChecked = CurrentInvoice.ShippingStatus;
            }
            
            
        }

        void DisplayInvoiceItemFields() {
            // Update the Invoice Item fields
            ItemIDTextbox.Text = Convert.ToString(CurrentInvoiceItem.ItemID);
            ItemNameTextbox.Text = CurrentInvoiceItem.ItemName;
            ItemDescriptionTextbox.Text = CurrentInvoiceItem.ItemDescription;
            ItemPriceTextbox.Text = Convert.ToString(CurrentInvoiceItem.ItemPrice);
            ItemQuantityTextbox.Text = Convert.ToString(CurrentInvoiceItem.ItemQuantity);

            // Calculate price data before updating price fields
            CalculatePriceTotals();

            // Update price fields
            SubtotalTextbox.Text = Convert.ToString(Subtotal);
            PSTTextbox.Text = Convert.ToString(PST);
            GSTTextbox.Text = Convert.ToString(GST);
            TotalTextbox.Text = Convert.ToString(Total);
        }

        private void InvoiceListbox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            InvoiceIndex = InvoiceListbox.SelectedIndex;
            InvoiceItemErrorLabel.Content = InvoiceIndex;

            if (InvoiceIndex == -1) {
                // Clear the InvoiceItems form fields
                //ClearInvoiceItemsFields();

                // Break out of the event
                return;
            }

            //// Clears the InvoiceItems before adding to it when a different invoice is selected
            //InvoiceItemsListbox.Items.Clear();
            CurrentInvoice = (Invoice)InvoiceListbox.SelectedItem;// Update the CurrentInvoice
            //    CurrentInvoice = InvoiceCollection[InvoiceIndex];
            // 1. Store the selected index
            // 2. reload all data (and fields) using the selected index.
            
            LoadInvoiceItems(CurrentInvoice.InvoiceID);
            DisplayInvoiceItemRecords();

            
            DisplayInvoiceFields(); // Display the current record in the form fields
            //InvoiceListbox.SelectedIndex = InvoiceIndex;

        }

        private void InvoiceItemsListbox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            InvoiceItemsIndex = InvoiceItemsListbox.SelectedIndex;
            InvoiceItemErrorLabel.Content = InvoiceItemsListbox.SelectedIndex;

            // check to see if the Invoice selected has any InvoiceItems
            if (InvoiceItemsIndex == -1) {
                // Clear the InvoiceItems form fields
                ClearInvoiceItemsFields();

                // Break out of the event
                return;
            }

            //ItemIDTextbox.Text = InvoiceItemsIndex.ToString();
            CurrentInvoiceItem = (InvoiceItem)InvoiceItemsListbox.SelectedItem;//InvoiceItemCollection[InvoiceItemsIndex];
            DisplayInvoiceItemFields();

            
        }

        //void CalculatePriceTotals() {
        //    // Loop over each InvoiceItem, adding to the Subtotal
        //    for (int i = 0; i < InvoiceItemCollection.Count; i++) {
        //        Subtotal += InvoiceItemCollection[i].ItemPrice * InvoiceItemCollection[i].ItemQuantity;
        //    }
        //    // Calculate and update the price data
        //    PST = Subtotal * 0.06;
        //    GST = Subtotal * 0.05;
        //    Total = Subtotal + GST + PST;
        //}

        void CalculatePriceTotals() {
            // Loop over each InvoiceItem, adding to the Subtotal
            //for (int i = 0; i < InvoiceListbox.Items.Count; i++) {
              //  Subtotal += InvoiceItemCollection[i].ItemPrice * InvoiceItemCollection[i].ItemQuantity;
                
            //}
            // Calculate and update the price data
            PST = Subtotal * 0.06;
            GST = Subtotal * 0.05;
            Total = Subtotal + GST + PST;
        }

        void ClearInvoiceItemsFields() {
            ItemIDTextbox.Text = "";
            ItemNameTextbox.Text = "";
            ItemDescriptionTextbox.Text = "";
            ItemPriceTextbox.Text = "";
            ItemQuantityTextbox.Text = "";
            SubtotalTextbox.Text = ""; 
            PSTTextbox.Text = "";
            GSTTextbox.Text = "";
            TotalTextbox.Text = "";
        }

        private void SaveInvoiceButton_Click(object sender, RoutedEventArgs e) {
            
            if (IsDataValid("Invoice")) {
                UpdateInvoiceRecord();
                //LoadInvoices();
                //DisplayInvoiceRecords();
            }

            InvoiceErrorLabel.Content = InvoiceListbox.SelectedIndex.ToString();
            InvoiceListbox.SelectedIndex = InvoiceIndex;
            //InvoiceErrorLabel.Content = InvoiceListbox.SelectedIndex;

        }

        private void SaveInvoiceItemButton_Click(object sender, RoutedEventArgs e) {
            
            if (IsDataValid("InvoiceItem")) {
                UpdateInvoiceItemRecord();
                LoadInvoiceItems(CurrentInvoiceItem.InvoiceID);//(CurrentInvoiceItem.ItemID);
                DisplayInvoiceItemFields();
                
                //InvoiceItemErrorLabel = InvoiceItemCollection[InvoiceItemsListbox.SelectedIndex.ToString()];
            }
            
        }

        void UpdateInvoiceRecord() {
            string query;

            Invoice UpdatedInvoice = new Invoice();
            UpdatedInvoice.InvoiceID = Convert.ToInt32(InvoiceIDTextbox.Text);
            UpdatedInvoice.InvoiceDate = Convert.ToDateTime(InvoiceDateTextbox.Text);
            UpdatedInvoice.ShippingStatus = (bool)ShippedCheckbox.IsChecked;
            UpdatedInvoice.CustomerName = CustomerNameTextbox.Text;
            UpdatedInvoice.CustomerAddress = CustomerAddressTextbox.Text;
            UpdatedInvoice.CustomerEmail = CustomerEmailTextbox.Text;

            using (SqlConnection connection = new SqlConnection()) {
                // Set the connection string
                connection.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ShoppingCart.mdf; Integrated Security=True";

                // Open the connection to make use of it
                connection.Open();

                query = "UPDATE Invoices SET " +
                    //$"InvoiceID = {}, " +
                    $"InvoiceDate = '{InvoiceDateTextbox.Text}', " +
                    $"Shipped = '{ShippedCheckbox.IsChecked}', " +
                    $"CustomerName = '{CustomerNameTextbox.Text}', " +
                    $"CustomerAddress = '{CustomerAddressTextbox.Text}', " +
                    $"CustomerEmail = '{CustomerEmailTextbox.Text}' " +
                    $"WHERE InvoiceID = {CurrentInvoice.InvoiceID};";
                    // I need the InvoiceID

                // Create the update command passing in the query & connection
                using (SqlCommand UpdateCommand = new SqlCommand(query, connection)) {
                    // execute
                    UpdateCommand.ExecuteNonQuery();
                }

                // Update the display
                LoadInvoices();

                // Find the new record in the listbox
                InvoiceErrorLabel.Content = InvoiceListbox.Items.IndexOf(UpdatedInvoice);
                int UpdatedIndex = InvoiceListbox.Items.IndexOf(UpdatedInvoice);

                // Select the new item and scroll to it
                InvoiceListbox.SelectedIndex = UpdatedIndex;
                InvoiceListbox.ScrollIntoView(UpdatedInvoice);

            }
        }

        void UpdateInvoiceItemRecord() {
            string query;

            //InvoiceItem UpdatedInvoiceItem = new InvoiceItem();
            CurrentInvoiceItem.ItemID = Convert.ToInt32(ItemIDTextbox.Text);
            //CurrentInvoiceItem.InvoiceID = Convert.ToInt32(InvoiceID.);
            CurrentInvoiceItem.ItemName = ItemNameTextbox.Text;
            CurrentInvoiceItem.ItemDescription = ItemDescriptionTextbox.Text;
            CurrentInvoiceItem.ItemPrice = Convert.ToDouble(ItemPriceTextbox.Text);
            CurrentInvoiceItem.ItemQuantity = Convert.ToInt32(ItemQuantityTextbox.Text);
            //CurrentInvoiceItem = UpdatedInvoiceItem;

            using (SqlConnection connection = new SqlConnection()) {
                // Set the connection string
                connection.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ShoppingCart.mdf; Integrated Security=True";

                // Open the connection to make use of it
                connection.Open();

                query = "UPDATE InvoiceItems SET " +
                    //$"ItemID = {}, " +
                    //$"InvoiceID = {}, " +
                    $"ItemName = '{ItemNameTextbox.Text}', " +
                    $"ItemDescription = '{ItemDescriptionTextbox.Text}', " +
                    $"ItemPrice = {Convert.ToDouble(ItemPriceTextbox.Text)}, " +
                    $"ItemQuantity = {Convert.ToInt32(ItemQuantityTextbox.Text)} " +
                    $"WHERE ItemID = {CurrentInvoiceItem.ItemID};";
                    // I need the InvoiceID

                // Create the update command passing in the query & connection
                using (SqlCommand UpdateCommand = new SqlCommand(query, connection)) {
                    // execute
                    UpdateCommand.ExecuteNonQuery();
                }

                // Update the display
                LoadInvoiceItems(CurrentInvoiceItem.ItemID);

                // Find the new record in the listbox
                InvoiceErrorLabel.Content = InvoiceListbox.Items.IndexOf(CurrentInvoiceItem);
                int UpdatedIndexItem = InvoiceListbox.Items.IndexOf(CurrentInvoiceItem);

                // Select the new item and scroll to it
                InvoiceListbox.SelectedIndex = UpdatedIndexItem;
                InvoiceListbox.ScrollIntoView(CurrentInvoiceItem);
            }
        }

        private void NewInvoiceButton_Click(object sender, RoutedEventArgs e) {
            if (IsDataValid("Invoice")) {
                InvoiceListbox.Items.Clear();
                InsertInvoiceRecord();
                LoadInvoices();
                LoadInvoiceItems(CurrentInvoiceItem.InvoiceID);
                DisplayInvoiceFields();
            }
        }

        private void NewInvoiceItemButton_Click(object sender, RoutedEventArgs e) {
            if (IsDataValid("InvoiceItem")) {
                InsertInvoiceItemRecord();
                LoadInvoices();
                LoadInvoiceItems(CurrentInvoiceItem.InvoiceID);
                DisplayInvoiceItemFields();
            }
        }

        void InsertInvoiceRecord() {
            string query;

            // open a connection to the db
            using (SqlConnection connection = new SqlConnection()) {
                // setup the connection string
                connection.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Owner\Development\comp-255\comp255_Final\comp255_Final\bin\Debug\ShoppingCart.mdf; Integrated Security=True";

                // open the connection
                connection.Open();

                // generate a new primary key
                query = "SELECT MAX(InvoiceID) FROM Invoices;";

                int NewInvoiceID;

                using (SqlCommand SelectCommand = new SqlCommand(query, connection)) {
                    NewInvoiceID = Convert.ToInt32(SelectCommand.ExecuteScalar()) + 1;
                }

                // Construct the insert query
                query = "INSERT into Invoices VALUES (" +
                  $"{NewInvoiceID}, " +
                  $"'{InvoiceDateTextbox.Text}', " +
                  $"'{ShippedCheckbox.IsChecked}', " +
                  $"'{CustomerNameTextbox.Text}', " +
                  $"'{CustomerAddressTextbox.Text}', " +
                  $"'{CustomerEmailTextbox.Text}');";

                CurrentInvoice.InvoiceID = NewInvoiceID;
                CurrentInvoice.InvoiceDate = Convert.ToDateTime(InvoiceDateTextbox.Text);
                CurrentInvoice.ShippingStatus = (bool)ShippedCheckbox.IsChecked;
                CurrentInvoice.CustomerName = CustomerNameTextbox.Text;
                CurrentInvoice.CustomerAddress = CustomerAddressTextbox.Text;
                CurrentInvoice.CustomerEmail = CustomerEmailTextbox.Text;

                // execute the query
                using (SqlCommand InsertCommand = new SqlCommand(query, connection)) {
                    InsertCommand.ExecuteNonQuery();
                }
            }
        }

        void InsertInvoiceItemRecord() {
            string query;

            // open a connection to the db
            using (SqlConnection connection = new SqlConnection()) {
                // setup the connection string
                connection.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Owner\Development\comp-255\comp255_Final\comp255_Final\bin\Debug\ShoppingCart.mdf; Integrated Security=True";

                // open the connection
                connection.Open();

                // generate a new primary key
                query = "SELECT MAX(ItemID) FROM InvoiceItems;";

                int NewItemID;

                using (SqlCommand SelectCommand = new SqlCommand(query, connection)) {
                    NewItemID = Convert.ToInt32(SelectCommand.ExecuteScalar()) + 1;
                }

                // Construct the insert query
                query = "INSERT into InvoiceItems VALUES (" +
                  $"{NewItemID}, " +
                  $"{CurrentInvoiceItem.InvoiceID}, " +
                  $"'{ItemNameTextbox.Text}', " +
                  $"'{ItemDescriptionTextbox.Text}', " +
                  $"{Convert.ToDouble(ItemPriceTextbox.Text)}, " +
                  $"{Convert.ToInt32(ItemQuantityTextbox.Text)}); ";
                //$"WHERE InvoiceID = {CurrentInvoiceItem.InvoiceID};";

                CurrentInvoiceItem.ItemID = NewItemID;
                //CurrentInvoiceItem.InvoiceID = Convert.ToInt32(InvoiceID.);
                CurrentInvoiceItem.ItemName = ItemNameTextbox.Text;
                CurrentInvoiceItem.ItemDescription = ItemDescriptionTextbox.Text;
                CurrentInvoiceItem.ItemPrice = Convert.ToDouble(ItemPriceTextbox.Text);
                CurrentInvoiceItem.ItemQuantity = Convert.ToInt32(ItemQuantityTextbox.Text);

                // execute the query
                using (SqlCommand InsertCommand = new SqlCommand(query, connection)) {
                    InsertCommand.ExecuteNonQuery();
                }
            }
        }

        public bool IsDataValid(string entry) {
            // check to see which form requires validation
            if (entry == "Invoice") {
                // check that the required fields have values
                if (CustomerNameTextbox.Text == "" || 
                    CustomerEmailTextbox.Text == "" || 
                    InvoiceDateTextbox.Text == "") {

                    // Display an error label then return
                    InvoiceErrorLabel.Content = "Customer Name, Email, and Invoice Date must have values.";
                    return false;
                // Otherwise, make the data valid.
                } else if (CurrentInvoice == null) {
                    return false;
                } else {
                    // Clear the label then return true
                    InvoiceErrorLabel.Content = "";
                    return true;
                }
            // Perform checks on the Invoice Item fields
            } else {
                // check to see if the entered data is empty
                if (ItemNameTextbox.Text == "" ||
                    ItemPriceTextbox.Text == "" ||
                    ItemQuantityTextbox.Text == "") {

                    // Display an error label then return
                    InvoiceItemErrorLabel.Content = "Item Name, Price, and Quantity must have values.";
                    return false;
                // check to see if the number is negative
                } else if (Convert.ToDouble(ItemPriceTextbox.Text) < 0 ||
                           Convert.ToInt32(ItemQuantityTextbox.Text) < 0) {

                    // display error message then return
                    InvoiceItemErrorLabel.Content = "Item Price and Quantity must be positive values.";
                    return false;
                // decalre the data is valid
                } else {
                    return true;
                }
            }
        }


    }
}
