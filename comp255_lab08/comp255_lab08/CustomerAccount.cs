using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comp255_lab08
{
    class CustomerAccount
    {
        // initialize instance variables.
        private string firstName;
        private string lastName;
        private int accountNumber;
        private double balance;
        public static int numberOfAccounts = 0;

        // Default Constructor
        public CustomerAccount() { }

        // Custom Constructor -- same name as class
        public CustomerAccount(string FName, string LName, double Bal) {
            FirstName = FName;
            LastName = LName;
            AccountNumber = numberOfAccounts++;
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
