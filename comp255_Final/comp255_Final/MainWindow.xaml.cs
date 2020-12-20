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
        int InvoiceIndex;
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
            InvoiceIndex = 0;
            LoadInvoiceItems(InvoiceCollection[InvoiceIndex].InvoiceID);
            DisplayInvoiceRecords();
            DisplayInvoiceItemRecords();

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

                        // Add the object to the list collection
                        InvoiceCollection.Add(TempInvoice);
                    }
                }
            }
        }

        void LoadInvoiceItems(int InvoiceID) {
            // Clear the InvoiceItemCollection
            InvoiceItemCollection.Clear();

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
                                                                      (string)Reader["ItemName"],
                                                                      (string)Reader["ItemDescription"],
                                                                      (int)Reader["ItemQuantity"],
                                                                      Convert.ToDouble(Reader["ItemPrice"]) );

                        // Add the object to the list collection
                        InvoiceItemCollection.Add(TempInvoiceItem);
                    }
                }
            }
        } // end LoadInvoiceItems

        void DisplayInvoiceRecords() {
            // Loop over InvoiceCollection to add string representations of Invoice objects
            for (int i = 0; i < InvoiceCollection.Count; i++) {
                InvoiceListbox.Items.Add(InvoiceCollection[i].ToString());
            }
        }

        void DisplayInvoiceItemRecords() {
            // Loop over InvoiceItemCollection to add string representations of InvoiceItem objects
            for (int i = 0; i < InvoiceItemCollection.Count; i++) {
                InvoiceItemsListbox.Items.Add(InvoiceItemCollection[i].ToString());
            }
        }

        void DisplayInvoiceFields() {
            // Update the Invoice fields
            InvoiceIDTextbox.Text = Convert.ToString(CurrentInvoice.InvoiceID);
            CustomerNameTextbox.Text = CurrentInvoice.CustomerName;
            CustomerAddressTextbox.Text = CurrentInvoice.CustomerAddress;
            InvoiceDateTextbox.Text = Convert.ToString(CurrentInvoice.InvoiceDate);
            CustomerEmailTextbox.Text = CurrentInvoice.CustomerEmail;
            ShippedCheckbox.IsChecked = CurrentInvoice.ShippingStatus; 
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

        void CalculatePriceTotals() {
            // Loop over each InvoiceItem, adding to the Subtotal
            for (int i = 0; i < InvoiceItemCollection.Count; i++) {
                Subtotal += InvoiceItemCollection[i].ItemPrice;
            }
            // Calculate and update the price data
            PST = Subtotal * 0.06;
            GST = Subtotal * 0.05;
            Total = Subtotal + GST + PST;
        }

    }
}
