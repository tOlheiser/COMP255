using System;
using System.Collections.Generic;
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

namespace COMP255_CustomerAccounts
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        // Create a list collection of customer accounts
        List<CustomerAccount> CustomerAccounts = new List<CustomerAccount>(); //CustomerAccount[] CustomerAccounts;

        // Track the position of the index
        int CurrentRecordPosition = -1;    //int CurrentRecord;

        // Track the object of the current record
        CustomerAccount CurrentRecord;

        bool IsNewRecord;

        public MainWindow() {
            InitializeComponent();

            // When window is first loaded... 
            CurrentRecordPosition = 0; // initialize the index

            // Add to the list collection.
            AddAccount("Jeff", "Spicoli", 1333, 12.75);
            AddAccount("Stacy", "Hamilton", 1450, 13.90);
            AddAccount("Mike", "Damone", 1650, 200.80);

            // Display the first record in the list
            DisplayRecord(CurrentRecordPosition);
        } // End MainWindow

        private void PreviousButton_Click(object sender, RoutedEventArgs e) {
            // If SaveRecord() fails to validate, return out
            if (!SaveRecord()) return;

            // Continue to decrement while currentrecord is greater than 0
            if (CurrentRecordPosition > 0) {
                CurrentRecordPosition--;
            // the index is 0. Set the index to the last element of the list
            } else {
                CurrentRecordPosition = CustomerAccounts.Count - 1;
            }

            DisplayRecord(CurrentRecordPosition);
        }

        private void NextButton_Click(object sender, RoutedEventArgs e) {
            // If SaveRecord() fails to validate, return out
            if (!SaveRecord()) return;

            // continue to increment while CurrentRecord is less than the last index
            if (CurrentRecordPosition < CustomerAccounts.Count - 1) {
                CurrentRecordPosition++;
            } // otherwise, set index = to the first index.
            else {
                CurrentRecordPosition = 0;
            }

            DisplayRecord(CurrentRecordPosition);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e) {
            // Save record as it is.
            SaveRecord();

            // Clear the inputs
            FirstNameTextbox.Clear();
            LastNameTextbox.Clear();
            AccountNumberTextbox.Clear();
            BalanceTextbox.Clear();

            // Update the state of IsNewRecord
            IsNewRecord = true;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e) {
            SaveRecord(); // Saves the current record as is.
        }

        // ===== Methods
        void DisplayRecord(int RecordNumber) {
            // Displays all the inputs of CustomerAccounts[RecordNumber]
            FirstNameTextbox.Text = CustomerAccounts[RecordNumber].FirstName;
            LastNameTextbox.Text = CustomerAccounts[RecordNumber].LastName;
            AccountNumberTextbox.Text = CustomerAccounts[RecordNumber].AccountNumber.ToString();
            BalanceTextbox.Text = CustomerAccounts[RecordNumber].Balance.ToString();
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
            } else if (!IsNewRecord) {
                // Update the current record of CustomerAccounts
                CustomerAccounts[CurrentRecordPosition].FirstName = FirstNameTextbox.Text;
                CustomerAccounts[CurrentRecordPosition].LastName = LastNameTextbox.Text;
                CustomerAccounts[CurrentRecordPosition].AccountNumber = Convert.ToInt32(AccountNumberTextbox.Text);
                CustomerAccounts[CurrentRecordPosition].Balance = Convert.ToDouble(BalanceTextbox.Text);

                return true;
            } else {
                // Add the new record to the list
                AddAccount(FirstNameTextbox.Text, LastNameTextbox.Text, 
                    Convert.ToInt32(AccountNumberTextbox.Text), Convert.ToDouble(BalanceTextbox.Text));

                // Set pointer to the new element
                CurrentRecordPosition = CustomerAccounts.Count - 1;

                // Update the values in the window
                DisplayRecord(CurrentRecordPosition);

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

    class CustomerAccount
    {
        // initialize instance variables.
        private string firstName;
        private string lastName;
        private int accountNumber;
        private double balance;

        // Default Constructor
        public CustomerAccount() { }

        // Custom Constructor -- same name as class
        public CustomerAccount(string FName, string LName, int AccNumber, double Bal) {
            FirstName = FName;
            LastName = LName;
            AccountNumber = AccNumber;
            Balance = Bal;
        }

        public string FirstName { get; set; }
        // Incorrect: set { firstName = FirstName; }
        // Correct: set { firstName = value; }

        public string LastName { get; set; }

        public int AccountNumber { get; set; }

        public double Balance { get; set; }
    } // End CustomerAccount

}// End namespace
