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

namespace PrimeNumbers
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ErrorLabel.Content = ""; // clear the error label.
        }

        private void CalculatePrimesButton_Click(object sender, RoutedEventArgs e) {
            // initialize variables
            string Output = "";
            int LowerBound, UpperBound; 

            // validate inputs
            if (Int32.TryParse(LowerBoundTextbox.Text, out LowerBound) == true) {
                if (LowerBound <= 1) {
                    //MessageBox.Show("The lower bound value must be greater than 1.", "ERROR");
                    ErrorLabel.Content = "The lower bound value must be greater than 1.";
                    return;
                } 
            } else {
                ErrorLabel.Content = "Enter a valid number.";
                return;
            }

            // found that if I place this as an 'else if' statement, it doesn't work.
            if (Int32.TryParse(UpperBoundTextbox.Text, out UpperBound) == true) {
                if (UpperBound < LowerBound) {
                    //MessageBox.Show("The upper bound value must be greater than the lower bound.", "ERROR");
                    ErrorLabel.Content = "The upper bound value must be greater than the lower bound.";
                    return;
                }
            } else {
                ErrorLabel.Content = "Enter a valid number.";
                return;
            }

            // If I've made it this far, I'm error free. Clear the Error Label
            ErrorLabel.Content = "";

            // Determine the output
            while (LowerBound <= UpperBound) {
                if ( IsPrime(LowerBound) ) Output += $"{LowerBound}\n";
                LowerBound++; // increment
            }

            // Output to the textbox
            OutputTextbox.Text = Output;
        }

        static bool IsPrime(int NumberCheck) {
            // initialize counter variable
            int i = 2;

            // While loop to check to see if 
            while (i < Math.Sqrt(NumberCheck)) {
                // If the number is divisible by i, it's not a prime number.
                if (NumberCheck % i == 0) return false;
                i++; // increment
            }

            // if NumberCheck wasn't divisible on any iteration, it's a prime number
            return true;
        }
    }
}
