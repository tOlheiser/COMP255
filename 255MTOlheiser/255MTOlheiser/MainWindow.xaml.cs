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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Initialize listbox with the first row & divider
            OutputListbox.Items.Add("Year\tQ1 Sales\tQ2 Sales\tQ3 Sales\tQ4 Sales\tSales Total\tTaxes\t\tExpenses\tNet Profit");
            OutputListbox.Items.Add("========================================================================================================================");
        }

        // Click Event for the Calculate Button
        private void Button_Click(object sender, RoutedEventArgs e){
            // Output 'title row'
            // Loop from start year input to end year input to display sales 
            // INSIDE LOOP: IF Sales/Taxes/Expenses is checked, run each of their functions to display data.

            // {Output to the listbox} OutputListbox.Items.Add("4");

        }
    }
}
