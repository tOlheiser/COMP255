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

            // when state is set to "read"
            // grey out the fields, make them read only
            // When state is set to "write"
            // make the textboxes writable & make background white.
            // Array.Resize(ref myArr, myArr.Length + 1);
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e) {
            // Get the state, array length, and current index.

            // if state = "read", 
            if () {
                // if arraylength = 0 -> return, display message "No accounts. Please submit an account".
                // if arraylength = 1 -> return, display message "This is the only account"
                // if arraylength > 1 -> Set the currentIndex to currentIndex - 1.
                // ---- If currentIndex = 0 -> set the currentIndex to arrayLength

            // state must = "write"
            } else { 
                // validate the form inputs
                // create a new CustomerAccount object & store it in Form's customerAccounts with Array.Resize
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e) {
            // Get the state, array length, and current index.

            // if the form state = "read", 
            if () {
                // if arraylength = 0 -> return, display message "No accounts. Please submit an account".
                // if arraylength = 1 -> return, display message "This is the only account"
                // if arraylength > 1 -> Set the currentIndex to currentIndex - 1.
                // ---- If currentIndex = arrayLength -> set the currentIndex to 0 
            }
            // state must = "write"
            else
            {
                // validate the form inputs
                // create a new CustomerAccount object & store it in Form's customerAccounts with Array.Resize
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e) {
            // Clear the inputs
            // Set form state to "write"
            // maybe initializer handles if write, do this, if read, do that -> to textboxes
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e) {
            // 

            // if form state = read
            if () {
                return;
            // otherwise, validate inputs. 
            // create a new CustomerAccount object, passing in the inputs.
            // append the new CustomerAccount object to the form array with Array.Resize
            // grey out the fields & make textboxes read only
            } else if () {

            }
        }
    }
    class CustomerAccount {
        // initialize instance variables.
        private string firstName;
        private string lastName;
        private int accountNumber;
        private double balance;

        // Constructor -- same name as class
        public CustomerAccount(string first, string last, int accountNum, double accountBalance) {
            firstName = first;
            lastName = last;
            accountNumber = accountNum;
            balance = accountBalance;
        }

        public string FirstName { 
            get { return firstName; }
        }

        public string LastName {
            get { return lastName; }
        }

        public int AccountNumber {
            get { return accountNumber; }
        }

        public double Balance { 
            get { return balance; }
        }
    }

    class Form {
        private string[] customerAccounts = new string[1]; // stores the CustomerAccount objects.
        // track state of whether the form is read-only (viewing records) or writable.
        private string state = "write";
        private int currentIndex;

        public string[] CustomerAccounts {
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


    }

    class AccountInformation {
        static void Main() { 
            // initialize form
            Form CustomerForm = new Form();


        }
    }

}
