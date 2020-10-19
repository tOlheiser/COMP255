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
        }
    }
}

// initialize arrays
string[] Months = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
int[] Dataset = { 10, 15, 25, 5, 3, 30, 11, 8, 12, 35, 20, 7 };

// Call Method
ShowGraph(Months, Dataset);
            
        }
        public void ShowGraph(string[] Labels, int[] Values)
{
    string LabelOutput, GraphOutput; // initialize string variables for the output

    // Looping over the Labels array
    for (int i = 0; i < Labels.Length; i++)
    {
        LabelOutput = Labels[i]; // reassign the label on each iteration
        GraphOutput = ""; // Reset the graph value on each iteration

        // Add '*' to my graph output so long as the counter is < the current graph value.
        for (int j = 0; j < Values[i]; j++)
        {
            GraphOutput += "*";
        }

        // Add an item to the listbox
        OutputListbox.Items.Add($"{LabelOutput}\t{GraphOutput}");
    }
}