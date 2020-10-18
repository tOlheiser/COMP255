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

namespace Loan_Repayment_Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e) {
            // initialize variables
            string TableHeader, TableRow = "";
            int Months, NthPayment;
            decimal Principal, Balance, MonthlyPayments, AnnualInterestRate, ExtraPayment, InterestAccrued;

            // get inputs
            Months = Convert.ToInt16(MonthsTextbox.Text);
            NthPayment = Convert.ToInt16(NthPaymentTextbox.Text);
            Principal = Convert.ToDecimal(LoanPrincipalTextbox.Text);
            MonthlyPayments = Convert.ToDecimal(MonthlyPaymentsTextbox.Text);
            ExtraPayment = Convert.ToDecimal(ExtraPaymentsTextbox.Text);
            AnnualInterestRate = Convert.ToDecimal(AnnualInterestRateTextbox.Text);

            // Do a little set up
            Balance = Principal;

            // Compose Header String
            TableHeader = $"{"Month",-8}{"Payment",-11}{"Extra",-11}{"Interest",-13}{"Balance",-15}\n";
            TableHeader += "====================================================\n";

            for (int i = 0; i <= Months; i++) {
                // If it's the first entry (no interest accrued/payments made)
                if (i == 0) {
                    TableRow = $"{Balance,51:N2}\n";
                // If the current month iteration is divisible by nth payment
                } else if (i % NthPayment == 0) {
                    // Perform calculations
                    InterestAccrued = Balance * AnnualInterestRate / 100 / 12;
                    Balance = (Balance + InterestAccrued) - ExtraPayment - MonthlyPayments;

                    // Create a new table row
                    TableRow += $"{i,-8}{MonthlyPayments,-11:N2}{ExtraPayment,-13:N2}{InterestAccrued,-11:N2}{Balance,-15:N2}\n";
                } else { // proceed normally
                    // Perform calculations
                    InterestAccrued = Balance * AnnualInterestRate / 100 / 12;
                    Balance = (Balance + InterestAccrued) - MonthlyPayments;

                    // Create a new table row
                    TableRow += $"{i,-8}{MonthlyPayments,-24:N2}{InterestAccrued,-11:N2}{Balance,-15:N2}\n";
                }
            }

            /* ----------------------------------------
             * Format the textbox so that it's neat and tidy
             */

            // Output the data
            OutputTextbox.Text = TableHeader + TableRow;
        }
    }
}
