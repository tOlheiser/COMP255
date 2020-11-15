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
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e) {

        }

        private void NextButton_Click(object sender, RoutedEventArgs e) {

        }

        private void AddButton_Click(object sender, RoutedEventArgs e) {

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e) {

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
