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

namespace comp255_DiceRollArray
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ErrorLabel.Content = ""; // Clear the error label onload
        }

        private void RollButton_Click(object sender, RoutedEventArgs e) {
            // initialize variables
            int RollInput;
            int DiceSides;
            rnd = new Random(); // Create an instance of the random object

            // Validate inputs 
            if (!Int32.TryParse(DiceSidesTextbox.Text, out DiceSides)) {
                ErrorLabel.Content = "Please enter a valid number";
                return;
            } else if (!Int32.TryParse(DiceRollsTextbox.Text, out RollInput)) {
                ErrorLabel.Content = "Please enter a valid number";
                return;
            } else if (DiceSides < 4 || DiceSides > 20) {
                ErrorLabel.Content = "The sides must fall within 4-20";
                return;
            } else if (RollInput < 0) {
                ErrorLabel.Content = "You must enter a positive number";
                return;
            }

            // Clear the Error Label when all checks have passed
            ErrorLabel.Content = "";

            // initialize arrays
            int[] Side = { DiceSides };
            int[] RollCount = new int[RollInput];

            // Populate the RollCount array
            for (int i = 0; i < RollInput; i++) {
                // Generate a value and store it into the current array index
                RollCount[i] = rnd.Next(1, DiceSides + 1);
            }

            // Call the method to show the graph
            ShowGraph(Side, RollCount);
        }

        // Create a Random object
        private static Random rnd;

        // Create a ShowGraph method with two int parameters
        public void ShowGraph(int[] Labels, int[] Values) {
            // initialize local variables
            string GraphOutput;
            int LabelOutput;

            // Loop to display the labels
            for (int i = 1; i <= Labels[0]; i++) {
                LabelOutput = i; // reassign the label on each iteration
                GraphOutput = ""; // Reset the graph value on each iteration

                // loop over the contents of the Values array
                for (int j = 0; j < Values.Length; j++) {
                    // Check that the current label equals the current index of the values array 
                    if (LabelOutput == Values[j]) {
                        // increment the label's graph value
                        GraphOutput += "*";
                    }
                }

                // Add a new row to the listbox
                OutputListbox.Items.Add($"{LabelOutput}\t{GraphOutput}");
            }
        }
    }
}