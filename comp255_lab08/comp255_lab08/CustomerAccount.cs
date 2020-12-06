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
        private string email;
        private string phoneNumber;
        private DateTime balanceDate;

        // Default Constructor
        public CustomerAccount() { }

        // Custom Constructor -- same name as class
        public CustomerAccount(string FName, string LName, int AccountNum, double Bal, 
                               string EmailAdd, string PhoneNum, DateTime BalDate) {
            FirstName = FName;
            LastName = LName;
            AccountNumber = AccountNum;
            Balance = Bal;
            Email = EmailAdd;
            PhoneNumber = PhoneNum;
            BalanceDate = BalDate;
        }

        public string FirstName { get; set; }
        // Incorrect: set { firstName = FirstName; }
        // Correct: set { firstName = value; }

        public string LastName { get; set; }

        public int AccountNumber { get; set; }

        public double Balance { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime BalanceDate { get; set; }



    } // End CustomerAccount
}
