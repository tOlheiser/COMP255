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

namespace COMP255_CustomerAccounts {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();


            CustomerAccount[] customerAccounts = new CustomerAccount[0];

            // when state is set to "read"
            // grey out the fields, make them read only
            // When state is set to "write"
            // make the textboxes writable & make background white.
            // Array.Resize(ref myArr, myArr.Length + 1);
        } // End MainWindow

        private void PreviousButton_Click(object sender, RoutedEventArgs e) {
            //FirstNameTextbox.Text = customerAccounts[0]; //AccountInformation.State;
            //AccountInformation.State = AccountInformation.State == "read" ? "write" : "read";
            // Get the state, array length, and current index.

            // if state = "read", 
            //if () {
            // if arraylength = 0 -> return, display message "No accounts. Please submit an account".
            // if arraylength = 1 -> return, display message "This is the only account"
            // if arraylength > 1 -> Set the currentIndex to currentIndex - 1.
            // ---- If currentIndex = 0 -> set the currentIndex to arrayLength

            // state must = "write"
            //} else { 
            // validate the form inputs
            // create a new CustomerAccount object & store it in Form's customerAccounts with Array.Resize
            //}
        }

        private void NextButton_Click(object sender, RoutedEventArgs e) {
            // Get the state, array length, and current index.

            // if the form state = "read", 
            //if () {
            // if arraylength = 0 -> return, display message "No accounts. Please submit an account".
            // if arraylength = 1 -> return, display message "This is the only account"
            // if arraylength > 1 -> Set the currentIndex to currentIndex - 1.
            // ---- If currentIndex = arrayLength -> set the currentIndex to 0 
            // }
            // state must = "write"
            //else
            //{
            // validate the form inputs
            // create a new CustomerAccount object & store it in Form's customerAccounts with Array.Resize
            //}
        }

        private void AddButton_Click(object sender, RoutedEventArgs e) {
            // Clear the inputs
            // Set form state to "write"
            // maybe initializer handles if write, do this, if read, do that -> to textboxes
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e) {
            // 

            // if form state = read
            //if () {
            //  return;
            // otherwise, validate inputs. 
            // create a new CustomerAccount object, passing in the inputs.
            // append the new CustomerAccount object to the form array with Array.Resize
            // grey out the fields & make textboxes read only
            //} else if () {

            //            }
        }
    } // End MainWindow Class

    

    class CustomerAccount {
        // initialize instance variables.
        private string firstName;
        private string lastName;
        private int accountNumber;
        private double balance;

        // Default Constructor
        /*public CustomerAccount {

        }*/

        // Custom Constructor -- same name as class
        public CustomerAccount(string FName, string LName, int AccNumber, double Bal) {
            FirstName = FName;
            LastName = LName;
            AccountNumber = AccNumber;
            Balance = Bal;
        }

        public string FirstName {
            get { return firstName; }
            set { firstName = FirstName; }
        }

        public string LastName {
            get { return lastName; }
            set { lastName = LastName; }
        }

        public int AccountNumber {
            get { return accountNumber; }
            set { accountNumber = AccountNumber; }
        }

        public double Balance {
            get { return balance; }
            set { balance = Balance; }
        }
    } // End CustomerAccount
    
    class Form {
        private CustomerAccount[] customerAccounts = new CustomerAccount[0]; // stores the CustomerAccount objects.
        // track state of whether the form is read-only (viewing records) or writable.
        public string state; 
        private int currentIndex;

        public Form() {
            state = "write";
            currentIndex = 0;
        }

        public CustomerAccount[] CustomerAccounts {
            get {
                return customerAccounts;
            }

            set {
                // resize?
            }
        }
        
        public string State { 
            get {
                return state;
            }

            set {
                state = value;
            }
        }
        public int CurrentIndex { 
            get {
                return currentIndex;
            }

            set {
                currentIndex = value;
            }
        }


    } // End Form Class

    public static class AccountInformation {
        private static CustomerAccount[] customerAccounts = new CustomerAccount[0];
        public static string State = "write";
        public static int FormIndex = 0;
        // set first before calling.
        public static string FirstName, LastName;
        public static int AccountNumber;
        public static double Balance;

        private static CustomerAccount CustomerAccounts {
            get { return customerAccounts[FormIndex]; }
            set {
                Array.Resize(ref customerAccounts, customerAccounts.Length + 1);
                customerAccounts[customerAccounts.Length] = new CustomerAccount(FirstName, LastName, AccountNumber, Balance);
            }
        }
    }
    
    
    /*
        class AccountInformation {
            static void Main() { 
                // initialize form
                Form CustomerForm = new Form();


            }
        }

    }*/
}// End namespace
