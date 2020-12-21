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

        // Setting up variables and list collections to track state
        double Subtotal = 0; double GST = 0; double PST = 0; double Total = 0;
        int InvoiceIndex = 0; int InvoiceItemsIndex = 0; int PreviousIndex = 0;
        int InvoiceCount = 0; int InvoiceItemsCount = 0;
        bool IsNewInvoice = false; bool IsNewInvoiceItem = false;
        Invoice CurrentInvoice = new Invoice();
        InvoiceItem CurrentInvoiceItem = new InvoiceItem();

        public MainWindow() {
            InitializeComponent();

            // Load the invoices 
            LoadInvoices();
        }

        // ---- Events
        // Selection changed
        private void InvoiceListbox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            // Update the current index of the Invoice listbox
            InvoiceIndex = InvoiceListbox.SelectedIndex;

            // error handling - no item selected
            if (InvoiceIndex == -1) {

                // Check to see if it's a new record
                if (IsNewInvoice == true) {
                    // update the listbox selected index
                    // InvoiceCount = quick fix; InvoiceListbox.Items.Count returned '0'.
                    InvoiceListbox.SelectedIndex = InvoiceCount; 
                    InvoiceListbox.ScrollIntoView(CurrentInvoice);
                    IsNewInvoice = false; // reset value
                    return;
                }

                // Find the new record in the listbox
                InvoiceIndex = InvoiceListbox.Items.IndexOf(CurrentInvoice);

                // Select the new item and scroll to it
                InvoiceListbox.SelectedIndex = InvoiceIndex;
                InvoiceListbox.ScrollIntoView(CurrentInvoice);

                // Break out of the event
                return;
            }

            // Store the updated value of the CurrentInvoice object
            CurrentInvoice = (Invoice)InvoiceListbox.SelectedItem;// Update the CurrentInvoice

            // Load & Display Data
            LoadInvoiceItems(CurrentInvoice.InvoiceID);
            DisplayInvoiceFields();
        }

        private void InvoiceItemsListbox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            // Updated the index of the Invoice Items listbox
            InvoiceItemsIndex = InvoiceItemsListbox.SelectedIndex;

            // error handling - no item selected
            if (InvoiceItemsIndex == -1) {

                // Check to see if it's a new record
                if (IsNewInvoiceItem == true) {
                    // update the listbox selected index
                    InvoiceItemsListbox.SelectedIndex = InvoiceItemsCount;
                    InvoiceItemsListbox.ScrollIntoView(CurrentInvoiceItem);
                    IsNewInvoiceItem = false; // reset value
                    return;
                }

                // Select the previously selected item and scroll to it
                InvoiceItemsListbox.SelectedIndex = PreviousIndex;
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
            // If the data validates successfully, execute an update query
            if (IsDataValid("Invoice")) {
                UpdateInvoiceRecord();
            }

            // Make the listbox's selected index equal to the current state of Invoice Index
            InvoiceListbox.SelectedIndex = InvoiceIndex;

        }

        private void SaveInvoiceItemButton_Click(object sender, RoutedEventArgs e) {

            // If data validates successfully, execute update query
            if (IsDataValid("InvoiceItem")) {
                PreviousIndex = InvoiceItemsListbox.SelectedIndex;
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
            }
        }

        private void NewInvoiceItemButton_Click(object sender, RoutedEventArgs e) {
            // If the data successfully validates, 
            if (IsDataValid("InvoiceItem")) {
                InsertInvoiceItemRecord(); // execute insert query
                LoadInvoices(); // load data
                LoadInvoiceItems(CurrentInvoiceItem.InvoiceID);
                DisplayInvoiceItemFields(); // display field values
            }
        }

        // Delete Buttons
        private void DeleteInvoiceButton_Click(object sender, RoutedEventArgs e) {
            // Delete only if there are items to delete.
            if (InvoiceItemsListbox.Items.Count > 0) {
                DeleteInvoiceRecords(); // execute query to delete all invoice records
                DeleteInvoice(); // execute query to delete the invoice
            }
        }

        private void DeleteInvoiceItemButton_Click(object sender, RoutedEventArgs e) {
            // Delete only if there are items to delete.
            if (InvoiceItemsListbox.Items.Count > 0) {
                DeleteInvoiceItem(); // execute query to delete single invoice item
            }
        }

        // ---- Database Methods

        void LoadInvoices() {
            // Clear the listbox and reset count
            InvoiceListbox.Items.Clear();
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
                        InvoiceCount++; // Increment the count
                    }
                }
            }
        }

        void LoadInvoiceItems(int InvoiceID) {
            // Clear the listbox as well as reset the count and subtotal
            InvoiceItemsListbox.Items.Clear();
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

                    // Load the data
                    LoadInvoiceItems(CurrentInvoice.InvoiceID);

                    // Check to see if there are any items in the Invoice listbox
                    if (InvoiceListbox.Items.Count > 0) {
                        // Set the value of the current invoice and display form data
                        CurrentInvoice = (Invoice)InvoiceListbox.Items[0];
                        DisplayInvoiceFields();
                    }
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

                    // Check to see if the InvoiceListbox has any items
                    if (InvoiceListbox.Items.Count > 0) {
                        // set the current invoice value
                        CurrentInvoice = (Invoice)InvoiceListbox.Items[0];
                        LoadInvoiceItems(CurrentInvoice.InvoiceID); // Load InvoiceItems for the current Invoice
                        DisplayInvoiceFields(); // display form field data
                        DisplayInvoiceItemFields();
                        // Otherwise, clear invoice item form fields
                    } else {
                        ClearInvoiceItemsFields();
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
            // Update the Invoice fields
            if (CurrentInvoice != null) {
                InvoiceIDTextbox.Text = Convert.ToString(CurrentInvoice.InvoiceID);
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
