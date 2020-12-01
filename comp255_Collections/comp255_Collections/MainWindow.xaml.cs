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
    public partial class MainWindow : Window
    {
        CustomerAccount[] CustomerAccounts;
        int CurrentRecord;
        bool IsNewRecord;

        public MainWindow()
        {
            InitializeComponent();

            // When window is first loaded, initialize these values...
            CurrentRecord = 0;
            CustomerAccounts = new CustomerAccount[] {
                new CustomerAccount("Jeff", "Spicoli", 1333, 12.75),
                new CustomerAccount("Stacy", "Hamilton", 1450, 13.90),
                new CustomerAccount("Mike", "Damone", 1650, 200.80)
            };

            // Display the first record in the array
            DisplayRecord(CurrentRecord);
        } // End MainWindow

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            // If SaveRecord() fails to validate, return out
            if (!SaveRecord()) return;

            // Continue to decrement while currentrecord is greater than 0
            if (CurrentRecord > 0)
            {
                CurrentRecord--;
                // if CurrentRecord == 0, set the value to the last element of the arry
            }
            else
            {
                CurrentRecord = CustomerAccounts.Length - 1;
            }

            DisplayRecord(CurrentRecord);
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            // If SaveRecord() fails to validate, return out
            if (!SaveRecord()) return;

            // continue to increment while CurrentRecord is less than the last index
            if (CurrentRecord < CustomerAccounts.Length - 1)
            {
                CurrentRecord++;
            }
            else
            {
                // otherwise set CurrentRecord == to first index.
                CurrentRecord = 0;
            }

            DisplayRecord(CurrentRecord);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
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

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveRecord(); // Saves the current record as is.
        }

        // ===== Methods
        void DisplayRecord(int RecordNumber)
        {
            // Displays all the inputs of CustomerAccounts[RecordNumber]
            FirstNameTextbox.Text = CustomerAccounts[RecordNumber].FirstName;
            LastNameTextbox.Text = CustomerAccounts[RecordNumber].LastName;
            AccountNumberTextbox.Text = CustomerAccounts[RecordNumber].AccountNumber.ToString();
            BalanceTextbox.Text = CustomerAccounts[RecordNumber].Balance.ToString();
        }

        bool SaveRecord()
        {
            // Check to see if any fields are blank
            if (FirstNameTextbox.Text == "" ||
                LastNameTextbox.Text == "" ||
                AccountNumberTextbox.Text == "" ||
                BalanceTextbox.Text == ""
               )
            {
                MessageBox.Show("All fields must contain a value.");
                return false;
                // Check to see if it is a new record
            }
            else if (!IsNewRecord)
            {
                // Update the current record of CustomerAccounts
                CustomerAccounts[CurrentRecord].FirstName = FirstNameTextbox.Text;
                CustomerAccounts[CurrentRecord].LastName = LastNameTextbox.Text;
                CustomerAccounts[CurrentRecord].AccountNumber = Convert.ToInt32(AccountNumberTextbox.Text);
                CustomerAccounts[CurrentRecord].Balance = Convert.ToDouble(BalanceTextbox.Text);

                return true;
            }
            else
            {
                // Expand the array by one element
                Array.Resize(ref CustomerAccounts, CustomerAccounts.Length + 1);

                // Set pointer to the new element
                CurrentRecord = CustomerAccounts.Length - 1;

                // Give values to the new record
                CustomerAccounts[CurrentRecord] = new CustomerAccount(
                    FirstNameTextbox.Text,
                    LastNameTextbox.Text,
                    Convert.ToInt32(AccountNumberTextbox.Text),
                    Convert.ToDouble(BalanceTextbox.Text)
                );

                // Update the values in the window
                DisplayRecord(CurrentRecord);

                // Reset the new record flag
                IsNewRecord = false;

                return true; // Save is FINE
            }
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
        public CustomerAccount()
        {

        }

        // Custom Constructor -- same name as class
        public CustomerAccount(string FName, string LName, int AccNumber, double Bal)
        {
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
