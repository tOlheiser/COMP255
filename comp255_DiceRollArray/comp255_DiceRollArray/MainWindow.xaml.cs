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

            // Initialize the Side & RollCount array and give them the size of the Dice Sides input
            int[] Side = new int[DiceSides];
            int[] RollCount = new int[DiceSides];

            for (int i = 0; i < DiceSides; i++) {
                Side[i] = i + 1; // Populate Side with proper side values.
                RollCount[i] = 0; // Initialize each index value at 0
            }

            // Populate the RollCount array
            for (int i = 0; i < RollInput; i++) {
                // Generate a value and store it into the current array index
                int Roll = rnd.Next(1, DiceSides + 1);

                // Begin another loop, checking to see which side lines up to the roll result
                for (int j = 0; j < DiceSides; j++) { 
                    // Is the side at the current index == to the roll?
                    if (Side[j] == Roll) {
                        // if so, increment the corresponding roll count by 1.
                        RollCount[j]++;
                    }
                }
            }

            // Call the method to show the graph
            ShowGraph(Side, RollCount);
        }

        // Create a Random object
        private static Random rnd;

        // Create a ShowGraph method with two int parameters
        public void ShowGraph(int[] Labels, int[] Values) {
            string GraphOutput; // initialize local variable

            // Loop over the labels
            for (int i = 0; i < Labels.Length; i++) {
                GraphOutput = ""; // Reset the graph value on each iteration

                // loop over the contents of the Values array
                for (int j = 0; j < Values[i]; j++) {
                    GraphOutput += "*"; // Add an asterisk on each iteration
                }

                // Add a new row to the listbox
                OutputListbox.Items.Add($"{Labels[i]}\t{GraphOutput}");
            }
        }
    }
}