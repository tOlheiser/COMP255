using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

namespace Savings_Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            // initialize variables
            int Month = 1;
            decimal SavingsGoal, SavedEachMonth, AnnualInterestRate;
            decimal InterestEarned = 0.00M;
            decimal PreviousBalance = 0, CurrentBalance = 0;
            string TableHeader, TableDivider, TableRow;

            // Get inputs
            SavingsGoal = Convert.ToDecimal(SavingsGoalTextbox.Text);
            SavedEachMonth = Convert.ToDecimal(SavedEachMonthTextbox.Text);
            AnnualInterestRate = Convert.ToDecimal(AnnualInterestRateTextbox.Text);

            // Set up row header
            TableHeader = $"{"Month",-8}{"Saved",-10}{"Interest",-13}{"Balance",-13}\n";
            TableDivider = "==========================================\n";

            // Add the row header to the listbox
            OutputListbox.Items.Add(TableHeader);
            OutputListbox.Items.Add(TableDivider);

            // Output data into the table
            while (CurrentBalance < SavingsGoal) {
                
                // If this is the first entry, don't calculate interest
                if (PreviousBalance == 0) {
                    // Set previous balance = how much the person intends on saving each month
                    PreviousBalance = SavedEachMonth;
                    CurrentBalance = PreviousBalance;

                    // Output the formatted row to the user
                    TableRow = $"{Month,-8}{SavedEachMonth,-10}{InterestEarned,-13}{PreviousBalance,-13}";
                    OutputListbox.Items.Add(TableRow);
                    
                    // Increment Month
                    Month++;
                } else {
                    // Calculate values
                    InterestEarned = PreviousBalance * AnnualInterestRate / 100 / 12;
                    CurrentBalance = PreviousBalance + InterestEarned + SavedEachMonth;
                    PreviousBalance = CurrentBalance;

                    // Compose output row
                    TableRow = $"{Month,-8}{SavedEachMonth,-10}{InterestEarned,-13:N2}{CurrentBalance,-13:N2}";
                    OutputListbox.Items.Add(TableRow);
                    
                    // Increment Month
                    Month++;
                }
            }
        }
    }
}
