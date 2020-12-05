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

namespace comp255_lab08
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        // Create a list collection of customer accounts
        List<CustomerAccount> CustomerAccounts = new List<CustomerAccount>();
        int CurrentIndex = -1; // Track the position of the index
        bool IsNewRecord;

        public MainWindow() {
            InitializeComponent();

            // When window is first loaded... 
            CurrentIndex = 0; // initialize the index

            // Populate the List with data
            LoadData();

            // Display the first record in the list
            DisplayRecord(CurrentIndex);
        } // End MainWindow

        private void PreviousButton_Click(object sender, RoutedEventArgs e) {
            // If SaveRecord() fails to validate, return out
            if (!SaveRecord()) return;

            // Continue to decrement while currentrecord is greater than 0
            if (CurrentIndex > 0) {
                CurrentIndex--;
                // the index is 0. Set the index to the last element of the list
            }
            else {
                CurrentIndex = CustomerAccounts.Count - 1;
            }

            DisplayRecord(CurrentIndex);
        }

        private void NextButton_Click(object sender, RoutedEventArgs e) {
            // If SaveRecord() fails to validate, return out
            if (!SaveRecord()) return;

            // continue to increment while CurrentRecord is less than the last index
            if (CurrentIndex < CustomerAccounts.Count - 1)
            {
                CurrentIndex++;
            } // otherwise, set index = to the first index.
            else
            {
                CurrentIndex = 0;
            }

            DisplayRecord(CurrentIndex);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e) {
            SaveRecord(); // Save record as it is.
            ClearFields(); // Clear the inputs
            IsNewRecord = true; // update state
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e) {
            SaveRecord(); // Saves the current record as is.
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e) {
            // Add a check to make sure there is an item to be deleted.
            if (CustomerAccounts.Count == 0)
            {
                MessageBox.Show($"There are no records to delete.");
                return;
            }

            // Remove the record at the current position
            CustomerAccounts.RemoveAt(CurrentIndex);

            // Make the previous account record the current record
            CurrentIndex = CurrentIndex - 1 < 0 ? 0 : CurrentIndex - 1;

            // Display the current record
            if (CustomerAccounts.Count > 0)
            {
                DisplayRecord(CurrentIndex);
                // If there are no records, clear the inputs and update the state of IsNewRecord
            }
            else
            {
                ClearFields();
                IsNewRecord = true;
            }
        }

        // ===== Methods
        void DisplayRecord(int RecordNumber) {
            // populate textboxes with data from the current record.
            FirstNameTextbox.Text       = CustomerAccounts[RecordNumber].FirstName;
            LastNameTextbox.Text        = CustomerAccounts[RecordNumber].LastName;
            AccountNumberTextbox.Text   = CustomerAccounts[RecordNumber].AccountNumber.ToString();
            BalanceTextbox.Text         = CustomerAccounts[RecordNumber].Balance.ToString();

        }

        void LoadData() {

            // Setup and open a connection
            using (SqlConnection connection = new SqlConnection()) {
                // Set the connection string
                connection.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Owner\Development\comp-255\comp255_lab08\comp255_lab08\CustomerAccounts.mdf; Integrated Security=True";

                // Open the connection to make use of it
                connection.Open();

                // Create a query to select all records
                string query = "SELECT AccountNumber, FirstName, LastName, Balance " +
                    "FROM CustomerAccounts;";

                // Create the Select command passing in the query & connection
                SqlCommand SelectCommand = new SqlCommand(query, connection);

                // execute the command and obtain a data reader containing the data
                using (SqlDataReader Reader = SelectCommand.ExecuteReader()) {
                    // Reads all the rows from the reader, returns false when no rows are left
                    while (Reader.Read()) {
                        // Add the data to a list collection
                        AddAccount((string)Reader["FirstName"],
                                   (string)Reader["LastName"],
                                   (int)Reader["AccountNumber"],
                                   Convert.ToDouble(Reader["Balance"]));
                    }
                }
            }
        }

        void ClearFields() {
            FirstNameTextbox.Clear();
            LastNameTextbox.Clear();
            AccountNumberTextbox.Clear();
            BalanceTextbox.Clear();
        }

        bool SaveRecord() {
            // Check to see if any fields are blank
            if (FirstNameTextbox.Text == "" ||
                LastNameTextbox.Text == "" ||
                AccountNumberTextbox.Text == "" ||
                BalanceTextbox.Text == ""
               )
            {
                MessageBox.Show("All fields must contain a value.");
                return false;
                // if it isn't a new record, update  
            }
            else if (!IsNewRecord) {
                // Update the current record of CustomerAccounts
                CustomerAccounts[CurrentIndex].FirstName = FirstNameTextbox.Text;
                CustomerAccounts[CurrentIndex].LastName = LastNameTextbox.Text;
                CustomerAccounts[CurrentIndex].AccountNumber = Convert.ToInt32(AccountNumberTextbox.Text);
                CustomerAccounts[CurrentIndex].Balance = Convert.ToDouble(BalanceTextbox.Text);

                return true;
            } else {
                // Add the new record to the list
                AddAccount(FirstNameTextbox.Text, LastNameTextbox.Text,
                    Convert.ToInt32(AccountNumberTextbox.Text), Convert.ToDouble(BalanceTextbox.Text));

                // Set pointer to the new element
                CurrentIndex = CustomerAccounts.Count - 1;

                // Update the values in the window
                DisplayRecord(CurrentIndex);

                // Reset the new record flag
                IsNewRecord = false;

                return true; // Save is FINE
            }
        }

        public void AddAccount(string FName, string LName, int AccNumber, double Bal) {
            // create an Account object, passing in the method values
            CustomerAccount TempAccount = new CustomerAccount(FName, LName, AccNumber, Bal);

            // Add the temporary placeholder account into the list collection
            CustomerAccounts.Add(TempAccount);
        }

    } // End MainWindow Class

}// End namespace
