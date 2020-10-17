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

namespace _255MTOlheiser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();

            // Initialize listbox with the first row & divider
            OutputListbox.Items.Add($"{"Year",-7}{"Q1 Sales",11}{"Q2 Sales",15}{"Q3 Sales",15}{"Q4 Sales",15}{"Sales Total",16}{"Taxes",13}{"Expenses",14}{"Net Profit",16}");
            OutputListbox.Items.Add("==========================================================================================================================");

            // Clear the Error Label when application loads
            ErrorLabel.Content = "";
        }

        // Click Event for the Calculate Button
        private void Button_Click(object sender, RoutedEventArgs e){

            // Initialize variables
            int StartYear, EndYear, MinSales, MaxSales;
            int Q1Sales, Q2Sales, Q3Sales, Q4Sales;
            double TotalSales, Expenses, NetProfit, Taxes, ExpenseRate;
            string SalesOutput, TaxesOutput, ExpensesOutput;
            rnd = new Random(); // Create an instance of the random object

            // Input Validation

            // Display an error message & break out of the click event if an input is missing.
            if (StartYearTextbox.Text == "" ||
                EndYearTextbox.Text == ""   ||
                MinSalesTextbox.Text == ""  ||
                MaxSalesTextbox.Text == ""  ||
                ExpenseRateTextbox.Text == "") {
                ErrorLabel.Content = "All inputs are required";
                return;
            } // If what's passed in is not a number, display a message and break out of the click event.
            else if (!Int32.TryParse(StartYearTextbox.Text, out StartYear)) {
                ErrorLabel.Content = "Start Year is not a number";
                return;
            } else if (!Int32.TryParse(EndYearTextbox.Text, out EndYear)) {
                ErrorLabel.Content = "End Year is not a number";
                return;
            } else if (!Int32.TryParse(MinSalesTextbox.Text, out MinSales)) {
                ErrorLabel.Content = "Min Sales is not a number";
                return;
            } else if (!Int32.TryParse(MaxSalesTextbox.Text, out MaxSales)) {
                ErrorLabel.Content = "Max Sales is not a number";
                return;
            } else if (!Double.TryParse(ExpenseRateTextbox.Text, out ExpenseRate)) {
                ErrorLabel.Content = "Expense Rate is not a number";
                return;
            } // Break out of the click event if End Year is not greater than Start Year & display a message
            else if (EndYear < StartYear) {
                ErrorLabel.Content = "End Year must be greater than Start Year";
                return;
            // If the years don't fall within an acceptable range, display a message & stop the event.
            } else if (EndYear - StartYear > 25) {
                ErrorLabel.Content = "The difference between the years cannot be greater than 25";
                return;
            // Max Sales must be greater than min sales
            } else if (MaxSales < MinSales) {
                ErrorLabel.Content = "Maximum Sales must be greater than Minimum Sales";
                return;
            // Expense rate must fall between 0 and 100
            } else if (ExpenseRate < 0 || ExpenseRate > 100) {
                ErrorLabel.Content = "The expense rate must be between 0 and 100";
                return;
            }

            // Clear the error label after the form was successfully validated
            ErrorLabel.Content = "";

            // Loop over the required years & perform operations
            while (StartYear <= EndYear) {
                // Generate & store the sales for each quarter in a corresponding variable
                // When GetSales is called, MinSales & MaxSales are passed in as arguments.
                Q1Sales = GetSales(MinSales, MaxSales);
                Q2Sales = GetSales(MinSales, MaxSales);
                Q3Sales = GetSales(MinSales, MaxSales);
                Q4Sales = GetSales(MinSales, MaxSales);

                // Calculate the values using the provided formulas.
                TotalSales = Q1Sales + Q2Sales + Q3Sales + Q4Sales;
                Taxes = GetTaxes(TotalSales);
                Expenses = TotalSales * (ExpenseRate / 100);
                NetProfit = TotalSales - Taxes - Expenses;

                // Determine the Output based on the state of the checkboxes
                /* Since SalesOutput is a string, and TotalSales is an integer, I pass a formatted string
                into SalesOutput if the checkbox is checked. Otherwise, SalesOutput is an empty string.*/
                SalesOutput = SalesCheckbox.IsChecked == true ? $"{TotalSales:N2}" : "";
                TaxesOutput = TaxesCheckbox.IsChecked == true ? $"{Taxes:N2}" : "";
                ExpensesOutput = ExpensesCheckbox.IsChecked == true ? $"{Expenses:N2}" : "";

                // Output a row to the listbox
                OutputListbox.Items.Add($"{StartYear,-7}{Q1Sales,11:N2}{Q2Sales,15:N2}{Q3Sales,15:N2}{Q4Sales,15:N2}{SalesOutput,16}{TaxesOutput,13}{ExpensesOutput,14}{NetProfit,16:N2}");
                
                // Increment the start year prior to the next iteration of the loop.
                StartYear++;
            }
        }

        // Declare the GetTaxes method with a parameter that takes in a 'double' value.
        public double GetTaxes(double Sales) {
            // initialize TaxRate as a double.
            double TaxRate, Tax;

            // If Sales falls within a given range, set the value of TaxRate corresponding to the range.
            if (Sales < 100000) {
                TaxRate = 0;
            } else if (Sales >= 100000 && Sales < 175000) {
                TaxRate = .10;
            } else if (Sales >= 175000 && Sales < 250000) {
                TaxRate = .15;
            } else if (Sales >= 250000 && Sales < 350000) {
                TaxRate = .25;
            } else {
                TaxRate = .35;
            }

            // Calculate the value of Tax then return it
            Tax = Sales * TaxRate / 100;
            return Tax;
        }

        // Declare a GetSales method with two int parameters
        public int GetSales(int MinValue, int MaxValue) {
            // return a random number between MinValue & MaxValue
            return rnd.Next(MinValue, MaxValue);
        }

        // Create a Random object
        private static Random rnd;
    }
}
