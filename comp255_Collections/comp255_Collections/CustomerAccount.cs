using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// !~ Ensure the namespace matches the main .xaml file
//namespace comp255_Collections
namespace COMP255_CustomerAccounts
{
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
}
