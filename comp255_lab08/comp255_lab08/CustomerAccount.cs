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
        public static int numberOfAccounts = 3;

        // Default Constructor
        public CustomerAccount() { }

        // Custom Constructor -- same name as class
        public CustomerAccount(string FName, string LName, int AccountNum, double Bal) {
            FirstName = FName;
            LastName = LName;
            AccountNumber = AccountNum;
            Balance = Bal;
            //numberOfAccounts++;
        }

        public string FirstName { get; set; }
        // Incorrect: set { firstName = FirstName; }
        // Correct: set { firstName = value; }

        public string LastName { get; set; }

        public int AccountNumber { get; set; }

        public double Balance { get; set; }

    } // End CustomerAccount
}
