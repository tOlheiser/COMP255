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

namespace Assignment3_part1_olheiser {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Calculate_Button_Click(object sender, RoutedEventArgs e) {
            // declare variables
            decimal StartingBalance, AnnualInterestRate, SavingsGoal;
            decimal MonthlyInterestRate, InterestAmount, PreviousBalance, CurrentBalance;
            int MonthNumber;
            string OutputTable, OutputRow;

            // Get inputs
            StartingBalance = Convert.ToDecimal(StartingBalance_Textbox.Text);
            AnnualInterestRate = Convert.ToDecimal(AnnualInterestRate_Textbox.Text);
            SavingsGoal = Convert.ToDecimal(SavingsGoal_Textbox.Text);

            // Calculate initial values
            CurrentBalance = StartingBalance;
            MonthNumber = 0;
            MonthlyInterestRate = AnnualInterestRate / 12 / 100;
            OutputTable = "";

            // Method 1 - While Loop with Textbox for output
            // Compose a heading row
            // note alignment formatting
            OutputTable = $"{"Month",-8}{"Interest",-10}{"Balance",-12}\n";
            OutputTable += "===========================\n";

            while (CurrentBalance < SavingsGoal) { 
                // Count the month
                MonthNumber++;

                InterestAmount = CurrentBalance * MonthlyInterestRate;
                CurrentBalance += InterestAmount;

                // syntax: $"{varname, +/-Alignment# :FormatCode}" N2 gives 2 decimal places
                OutputRow = $"{MonthNumber,-8}{InterestAmount,-10:N2}{CurrentBalance,-12:N2}\n";
                OutputTable += OutputRow; // Add row to my table

                // Output Result
                Output_Textbox.Text = OutputTable;
            }
            // Method 2 - Do Loop with a Listbox for output
            CurrentBalance = StartingBalance;
            MonthNumber = 0;

            // Compose a heading row in the Listbox
            // add items to the listbox "items" collection
            Output_Listbox.Items.Clear(); // Empty out the listbox
            Output_Listbox.Items.Add($"{"Month",-8}{"Interest",-13}{"Balance",-16}"); // Can only add one line at a time.
            // Click on listbox, under the text tab, change the font to Consolas
            Output_Listbox.Items.Add("======================================");

            do
            {
                // Count the month
                MonthNumber++;

                InterestAmount = CurrentBalance * MonthlyInterestRate;
                CurrentBalance += InterestAmount; // Add interest onto current balance

                // syntax: $"{varname, +/-Alignment# :FormatCode}" N2 gives 2 decimal places
                OutputRow = $"{MonthNumber,-8}{InterestAmount,-10:N2}{CurrentBalance,-12:N2}";

                // output the row to listbox
                Output_Listbox.Items.Add(OutputRow);
            } while (CurrentBalance < SavingsGoal);

            
        }
    }
}
