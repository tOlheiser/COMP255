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

namespace comp255_Final {

    public partial class MainWindow : Window {

        // declaring variables to track price information
        double Subtotal = 0; double GST = 0; double PST = 0; double Total = 0;

        // declaring variable to track the current index and previous index
        int InvoiceIndex = -1; int InvoiceItemsIndex = 0; 
        int PreviousInvoiceIndex = 0; int PreviousInvoiceItemIndex = 0;

        // declare variables to track how many records there are
        int InvoiceCount = 0; int InvoiceItemsCount = 0;
        
        // declare variables that track if a new record has been submitted
        bool IsNewInvoice = false; bool IsNewInvoiceItem = false;

        // Initialize the Invoice & InvoiceItems objects and collections
        Invoice CurrentInvoice = new Invoice();
        InvoiceItem CurrentInvoiceItem = new InvoiceItem();
        List<Invoice> InvoiceCollection = new List<Invoice>();
        List<InvoiceItem> InvoiceItemCollection = new List<InvoiceItem>();

        public MainWindow() {
            InitializeComponent();

            // Load the invoices 
            LoadInvoices();

            // Initialize listbox labels
            InvoiceHeadings.Content = $"{"Invoice ID",5}{" ", -10}{"Customer",-40}{"Email",-40}{"Shipped",-8}";
            InvoiceItemHeadings.Content = $"{"Item ID",5}{" ",-3}{"Item Name",-30}{"Item Description",-40}" +
                $"{"Item Price",-20}{"Item Quantity",-20}{"Price",-5}";

            // Clear the error labels
            InvoiceErrorLabel.Content = "";
            InvoiceItemErrorLabel.Content = "";
        }

        // ---- Events

        // Selection changed
        private void InvoiceListbox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            // Update the current index of the Invoice listbox
            InvoiceIndex = InvoiceListbox.SelectedIndex;

            // Clear the error labels
            InvoiceErrorLabel.Content = "";
            InvoiceItemErrorLabel.Content = "";

            // Check to see if it's a new record
            if (IsNewInvoice == true) {
                // update the listbox selected index
                // InvoiceCount = quick fix; InvoiceListbox.Items.Count returned '0'.
                InvoiceListbox.SelectedIndex = InvoiceCount;
                InvoiceListbox.ScrollIntoView(CurrentInvoice);
                IsNewInvoice = false; // reset value
                return;
            }
            // check if no item has been selected
            else if (InvoiceIndex == -1) {
                // Find the new record in the listbox
                InvoiceIndex = InvoiceListbox.Items.IndexOf(CurrentInvoice);

                // Select the new item and scroll to it
                InvoiceListbox.SelectedIndex = PreviousInvoiceIndex;
                InvoiceListbox.ScrollIntoView(CurrentInvoice);

                // Break out of the event
                return;
            }

            // Store the updated value of the CurrentInvoice object
            CurrentInvoice = (Invoice)InvoiceListbox.SelectedItem;// Update the CurrentInvoice

            // Update the InvoiceID property of the CurrentInvoiceItem
            CurrentInvoiceItem.InvoiceID = CurrentInvoice.InvoiceID;

            // Load & Display Data
            LoadInvoiceItems(CurrentInvoice.InvoiceID);
            DisplayInvoiceFields();
        }

        private void InvoiceItemsListbox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            // Updated the index of the Invoice Items listbox
            InvoiceItemsIndex = InvoiceItemsListbox.SelectedIndex;
            
            // Clear the error labels
            InvoiceErrorLabel.Content = "";
            InvoiceItemErrorLabel.Content = "";

            // Check to see if it's a new record
            if (IsNewInvoiceItem == true) {
                // update the listbox selected index
                InvoiceListbox.SelectedIndex = PreviousInvoiceIndex;
                InvoiceItemsListbox.SelectedIndex = InvoiceItemsCount;
                IsNewInvoiceItem = false; // reset value
                return;
            } // if no item selected
            else if (InvoiceItemsIndex == -1) {
                // Select the previously selected item and scroll to it
                InvoiceItemsListbox.SelectedIndex = PreviousInvoiceItemIndex;
                InvoiceItemsListbox.ScrollIntoView(CurrentInvoiceItem);

                // Break out of the event
                return;
            }
            // Load & Display Data
            CurrentInvoiceItem = (InvoiceItem)InvoiceItemsListbox.SelectedItem;
            DisplayInvoiceItemFields();
        }

        // Save Buttons
        private void SaveInvoiceButton_Click(object sender, RoutedEventArgs e) {
            // if nothing is selected, break out of the event
            if (InvoiceListbox.SelectedIndex == -1) {
                return;
            // If the data validates successfully, execute an update query
            } else if (IsDataValid("Invoice")) {
                PreviousInvoiceIndex = InvoiceListbox.SelectedIndex;
                UpdateInvoiceRecord();
            }

            // Make the listbox's selected index equal to the current state of Invoice Index
            InvoiceListbox.SelectedIndex = InvoiceIndex;

        }

        private void SaveInvoiceItemButton_Click(object sender, RoutedEventArgs e) {
            // if nothing is selected, break out of the event
            if (InvoiceItemsListbox.SelectedIndex == -1) {
                return;
            } // If data validates successfully, execute update query
            else if (IsDataValid("InvoiceItem")) {
                PreviousInvoiceItemIndex = InvoiceItemsListbox.SelectedIndex;
                UpdateInvoiceItemRecord();
            }

            // Set the listbox's selected index
            InvoiceItemsListbox.SelectedIndex = InvoiceItemsIndex;
        }

        // New Buttons
        private void NewInvoiceButton_Click(object sender, RoutedEventArgs e) {
            // If the data successfully validates, 
            if (IsDataValid("Invoice")) {
                InsertInvoiceRecord(); // execute insert query
                LoadInvoices(); // load data
                LoadInvoiceItems(CurrentInvoiceItem.InvoiceID);
                DisplayInvoiceFields(); // display field data for the new invoice
                ClearInvoiceItemsFields(); // clear the old form data

                // If it's the first item added, make that the selected index.
                if (InvoiceListbox.Items.Count == 1) {
                    InvoiceListbox.SelectedIndex = 0;
                } // otherwise, make the most recent item the selected index. 
                else {
                    InvoiceListbox.SelectedIndex = InvoiceListbox.Items.Count - 1;
                }
            }
        }

        private void NewInvoiceItemButton_Click(object sender, RoutedEventArgs e) {
            // If an Invoice hasn't been selected display error then break out
            if (InvoiceIndex == -1) {
                InvoiceItemErrorLabel.Content = "Select an Invoice before adding an Invoice Item.";
                return;
            // proceed if the data successfully validates 
            } else if (IsDataValid("InvoiceItem")) {
                PreviousInvoiceIndex = InvoiceListbox.SelectedIndex; // update state of previous index
                InsertInvoiceItemRecord(); // execute insert query
                LoadInvoices(); // load data
                LoadInvoiceItems(CurrentInvoiceItem.InvoiceID);
                DisplayInvoiceItemFields(); // display field values

                // If it's the first item added, make that the selected index.
                if (InvoiceItemsListbox.Items.Count == 1) {
                    InvoiceItemsListbox.SelectedIndex = 0;
                } // otherwise, make the most recent item the selected index. 
                else {
                    InvoiceItemsListbox.SelectedIndex = InvoiceItemsListbox.Items.Count - 1;
                }
            }
        }

        // Delete Buttons
        private void DeleteInvoiceButton_Click(object sender, RoutedEventArgs e) {
            DeleteInvoiceRecords(); // execute query to delete all invoice records
            DeleteInvoice(); // execute query to delete the invoice
        }

        private void DeleteInvoiceItemButton_Click(object sender, RoutedEventArgs e) {
            // Update the state of the previous invoice index
            PreviousInvoiceIndex = InvoiceItemsIndex;
            
            // If no Invoice Items are selected, don't proceed.
            if (InvoiceItemsIndex == -1) {
                return;
            } // Delete only if there are items to delete. 
            else if (InvoiceItemsListbox.Items.Count > 0) {
                DeleteInvoiceItem(); // execute query to delete single invoice item

                /* Set the Listbox selected index to PreviousInvoiceIndex - 1 
                unless it returns -1, then set index to 0. */
                InvoiceItemsListbox.SelectedIndex = PreviousInvoiceIndex - 1 < 0 ? 0 : PreviousInvoiceIndex - 1;
            }
        }

        // ---- Database Methods

        void LoadInvoices() {
            // Clear the listbox, InvoiceCollection, and reset count
            InvoiceListbox.Items.Clear();
            InvoiceCollection.Clear();
            InvoiceCount = 0;

            // Setup and open a connection
            using (SqlConnection connection = new SqlConnection()) {
                // Set the connection string
                connection.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ShoppingCart.mdf; Integrated Security=True";

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

                        // Add the object to the listbox
                        InvoiceListbox.Items.Add(TempInvoice);
                        InvoiceCollection.Add(TempInvoice);
                        InvoiceCount++; // Increment the count
                    }
                }
            }
        }

        void LoadInvoiceItems(int InvoiceID) {
            // Clear the listbox & InvoiceItem collection and reset count & subtotal
            InvoiceItemsListbox.Items.Clear();
            InvoiceItemCollection.Clear();
            InvoiceItemsCount = 0;
            Subtotal = 0;

            // Setup and open a connection
            using (SqlConnection connection = new SqlConnection()) {
                // Set the connection string
                connection.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ShoppingCart.mdf; Integrated Security=True";

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
                        // Add onto the subtotal as I load the items into the listbox
                        Subtotal += (int)Reader["ItemQuantity"] * Convert.ToDouble(Reader["ItemPrice"]);
                        // Add the object to the list collection
                        InvoiceItemsListbox.Items.Add(TempInvoiceItem);
                        InvoiceItemCollection.Add(TempInvoiceItem);
                        InvoiceItemsCount++; // increment the count
                    }
                }
            }
        } 

        void UpdateInvoiceRecord() {
            string query;

            using (SqlConnection connection = new SqlConnection()) {
                // Set the connection string
                connection.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ShoppingCart.mdf; Integrated Security=True";

                // Open the connection to make use of it
                connection.Open();

                // Build out the query
                query = "UPDATE Invoices SET " +
                    $"InvoiceDate = '{InvoiceDateTextbox.Text}', " +
                    $"Shipped = '{ShippedCheckbox.IsChecked}', " +
                    $"CustomerName = '{CustomerNameTextbox.Text}', " +
                    $"CustomerAddress = '{CustomerAddressTextbox.Text}', " +
                    $"CustomerEmail = '{CustomerEmailTextbox.Text}' " +
                    $"WHERE InvoiceID = {CurrentInvoice.InvoiceID};";

                // Create the update command passing in the query & connection
                using (SqlCommand UpdateCommand = new SqlCommand(query, connection)) {
                    // execute
                    UpdateCommand.ExecuteNonQuery();
                }

                // Load the data & add to the listbox
                LoadInvoices();
            }
        }

        void UpdateInvoiceItemRecord() {
            string query;

            using (SqlConnection connection = new SqlConnection()) {
                // Set the connection string
                connection.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ShoppingCart.mdf; Integrated Security=True";

                // Open the connection to make use of it
                connection.Open();

                // Build out the query string
                query = "UPDATE InvoiceItems SET " +
                    $"ItemName = '{ItemNameTextbox.Text}', " +
                    $"ItemDescription = '{ItemDescriptionTextbox.Text}', " +
                    $"ItemPrice = {Convert.ToDouble(ItemPriceTextbox.Text)}, " +
                    $"ItemQuantity = {Convert.ToInt32(ItemQuantityTextbox.Text)} " +
                    $"WHERE ItemID = {CurrentInvoiceItem.ItemID};";

                // Create the update command passing in the query & connection
                using (SqlCommand UpdateCommand = new SqlCommand(query, connection)) {
                    // execute
                    UpdateCommand.ExecuteNonQuery();
                }

                // Update the display
                LoadInvoiceItems(CurrentInvoiceItem.InvoiceID);
            }
        }

        void InsertInvoiceRecord() {
            IsNewInvoice = true; // update state
            string query; // initialize query string

            using (SqlConnection connection = new SqlConnection()) {
                // setup the connection string
                connection.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ShoppingCart.mdf; Integrated Security=True";

                // open the connection
                connection.Open();

                // generate a new primary key
                query = "SELECT MAX(InvoiceID) FROM Invoices;";

                int NewInvoiceID;

                // Execute the select command after passing in the query and connection
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

                // Update the CurrentInvoice object to reflect the new data
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
            IsNewInvoiceItem = true; // update state
            string query; // initialize query string

            // create a connection to the db
            using (SqlConnection connection = new SqlConnection()) {
                // setup the connection string
                connection.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ShoppingCart.mdf; Integrated Security=True";

                // open the connection
                connection.Open();

                // generate a new primary key
                query = "SELECT MAX(ItemID) FROM InvoiceItems;";

                int NewItemID; // create a variable to store the primary key

                // execute the command to get te MAX ItemID, add 1 to it, then store in a variable
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

                // Update the CurrentInvoiceItem object with the new data
                CurrentInvoiceItem.ItemID = NewItemID;
                CurrentInvoiceItem.ItemName = ItemNameTextbox.Text;
                CurrentInvoiceItem.ItemDescription = ItemDescriptionTextbox.Text;
                CurrentInvoiceItem.ItemPrice = Convert.ToDouble(ItemPriceTextbox.Text);
                CurrentInvoiceItem.ItemQuantity = Convert.ToInt32(ItemQuantityTextbox.Text);

                // execute the query to insert the record
                using (SqlCommand InsertCommand = new SqlCommand(query, connection)) {
                    InsertCommand.ExecuteNonQuery();
                }
            }
        }

        void DeleteInvoiceItem() {
            string query;

            // Create a connection to the DB
            using (SqlConnection connection = new SqlConnection()) {
                // Create connection string
                connection.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ShoppingCart.mdf; Integrated Security=True";

                // open the connection
                connection.Open();

                // create a SQL delete statement
                query = $"DELETE FROM InvoiceItems WHERE ItemID = {CurrentInvoiceItem.ItemID};";

                // Create a command object
                using (SqlCommand DeleteCommand = new SqlCommand(query, connection)) {
                    // Tries to delete, catches and throws the error if no matching records
                    try {
                        // execute delete command
                        DeleteCommand.ExecuteNonQuery();
                    } catch (SqlException ex) {
                        // catch & throw the error
                        Exception error = new Exception("Error no matching records to delete", ex);
                        throw error;
                    }

                    // Load the data
                    LoadInvoiceItems(CurrentInvoice.InvoiceID);

                    // Check to see if there are any listbox items left
                    if (InvoiceItemsListbox.Items.Count > 0) {
                        // if there are, set the value of the CurrentInvoiceItem and display form data
                        CurrentInvoiceItem = (InvoiceItem)InvoiceItemsListbox.Items[0];
                        DisplayInvoiceItemFields();
                    } else {
                        // If there are no items, clear the form fields
                        ClearInvoiceItemsFields();
                    }
                }
            }
        }

        void DeleteInvoiceRecords() {
            string query;

            // Create a connection to the database
            using (SqlConnection connection = new SqlConnection()) {
                // Create a connection string
                connection.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ShoppingCart.mdf; Integrated Security=True";

                // open the connection
                connection.Open();

                // create a SQL delete statement
                query = $"DELETE FROM InvoiceItems WHERE InvoiceID = {CurrentInvoice.InvoiceID};";

                // Create a command object
                using (SqlCommand DeleteCommand = new SqlCommand(query, connection)) {
                    // Try to run the delete command, otherwise catch & throw the error
                    try {
                        DeleteCommand.ExecuteNonQuery();
                    } catch (SqlException ex) {
                        Exception error = new Exception("Error no matching records to delete", ex);
                        throw error;
                    }

                    // Load the data & display form fields
                    LoadInvoiceItems(CurrentInvoice.InvoiceID);
                    DisplayInvoiceFields();
                }
            }
        }

        void DeleteInvoice() {
            string query;

            // Create a connection to the db
            using (SqlConnection connection = new SqlConnection()) {
                // create the connection string
                connection.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ShoppingCart.mdf; Integrated Security=True";

                // open the connection
                connection.Open();

                // create a SQL delete statement
                query = $"DELETE FROM Invoices WHERE InvoiceID = {CurrentInvoice.InvoiceID};";

                // Create a command object
                using (SqlCommand DeleteCommand = new SqlCommand(query, connection)) {
                    // try to run the delete command, otherwise catch and throw the error
                    try {
                        DeleteCommand.ExecuteNonQuery();
                    } catch (SqlException ex) {
                        Exception error = new Exception("Error no matching records to delete", ex);
                        throw error;
                    }

                    // load the data
                    LoadInvoices();
                    ClearInvoiceItemsFields();

                    // Check to see if the InvoiceListbox has any items
                    if (InvoiceListbox.Items.Count > 0) {
                        // set the current invoice value
                        CurrentInvoice = (Invoice)InvoiceListbox.Items[0];
                        DisplayInvoiceFields(); // display invoiceform field data

                        // Load InvoiceItems for the current Invoice
                        LoadInvoiceItems(CurrentInvoice.InvoiceID); 
                    } 
                }
            }
        }

        // ---- Methods

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

        // Display & Clear field methods
        void DisplayInvoiceFields() {
            // Update the Invoice fields if the current invoice object is not null
            if (CurrentInvoice != null) {
                InvoiceIDTextbox.Text = Convert.ToString(CurrentInvoice.InvoiceID);
                CustomerNameTextbox.Text = CurrentInvoice.CustomerName;
                CustomerAddressTextbox.Text = CurrentInvoice.CustomerAddress;
                InvoiceDateTextbox.Text = CurrentInvoice.InvoiceDate.ToShortDateString();
                CustomerEmailTextbox.Text = CurrentInvoice.CustomerEmail;
                ShippedCheckbox.IsChecked = CurrentInvoice.ShippingStatus;

                // Calculate price data before updating price fields
                CalculatePriceTotals();

                // Update price fields
                SubtotalTextbox.Text = Convert.ToString(Math.Round(Subtotal, 2));
                PSTTextbox.Text = Convert.ToString(Math.Round(PST, 2));
                GSTTextbox.Text = Convert.ToString(Math.Round(GST, 2));
                TotalTextbox.Text = Convert.ToString(Math.Round(Total, 2));
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
            SubtotalTextbox.Text = Convert.ToString(Math.Round(Subtotal, 2));
            PSTTextbox.Text = Convert.ToString(Math.Round(PST, 2));
            GSTTextbox.Text = Convert.ToString(Math.Round(GST, 2));
            TotalTextbox.Text = Convert.ToString(Math.Round(Total, 2));
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

        void CalculatePriceTotals() {
            PST = Subtotal * 0.06;
            GST = Subtotal * 0.05;
            Total = Subtotal + GST + PST;
        }
    }
}
