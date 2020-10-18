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

namespace comp255_invoice
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // When program starts, clear error message.
            ErrorMessageLabel.Content = "";
        }

        private void Calculate_Button_Click(object sender, RoutedEventArgs e) {
            // Initialize variables
            double Item1, Item2, Item3, Item4;
            double Item1_PST, Item1_GST;
            double Item2_PST, Item2_GST;
            double Item3_PST, Item3_GST;
            double Item4_PST, Item4_GST;
            double Total_PST, Total_GST;

            double subtotal, shippingCharge, total;
            const double gst = 0.05;
            const double pst = 0.06;
            const double shipping = 0.02;

            double CommissionRate = 0;
            double commission;

            bool validate = true;
      
            // ----- Grab the user inputs
            Double.TryParse(Item1_TextBox.Text, out Item1);
            Double.TryParse(Item2_TextBox.Text, out Item2);
            Double.TryParse(Item3_TextBox.Text, out Item3);
            Double.TryParse(Item4_TextBox.Text, out Item4);

            // ----- Validate user inputs
            // Is every field empty?
            if (Item1_TextBox.Text == "" &&
                Item2_TextBox.Text == "" &&
                Item3_TextBox.Text == "" &&
                Item4_TextBox.Text == "") {

                ErrorMessageLabel.Content = "Please enter a numerical value.";
                validate = false;
            } else {
                /* Note: When I tried using if/else here, the form would proceed if I had a valid input 
                in front of an invalid one. */
                // If the field isn't empty, evaluate if it's a valid value. 
                if (Item1_TextBox.Text != "") {
                    // Is it a valid number?
                    if (Double.TryParse(Item1_TextBox.Text, out Item1) == false) {
                        ErrorMessageLabel.Content = "Please enter a numerical value.";
                        validate = false;
                        // Is it a negative number?
                    } else if (Item1 < 0) {
                        ErrorMessageLabel.Content = "Please enter a positive value.";
                        validate = false;
                    }
                } 
                
                if (Item2_TextBox.Text != "") {
                    if (Double.TryParse(Item2_TextBox.Text, out Item2) == false) {
                        ErrorMessageLabel.Content = "Please enter a numerical value.";
                        validate = false;
                    } else if (Item2 < 0) {
                        ErrorMessageLabel.Content = "Please enter a positive value.";
                        validate = false;
                    }
                } 
                
                if (Item3_TextBox.Text != "") {
                    if (Double.TryParse(Item3_TextBox.Text, out Item3) == false) {
                        ErrorMessageLabel.Content = "Please enter a numerical value.";
                        validate = false;
                    } else if (Item3 < 0) {
                        ErrorMessageLabel.Content = "Please enter a positive value.";
                        validate = false;
                    }
                } 
                
                if (Item4_TextBox.Text != ""){
                    if (Double.TryParse(Item4_TextBox.Text, out Item4) == false) {
                        ErrorMessageLabel.Content = "Please enter a numerical value.";
                        validate = false;
                    } else if (Item4 < 0){
                        ErrorMessageLabel.Content = "Please enter a positive value.";
                        validate = false;
                    }
                }
            }
            
            // If validate is false, clear the results.
            if (validate == false) {
                Subtotal_TextBox.Text = "";
                Shipping_TextBox.Text = "";
                PST_TextBox.Text = "";
                GST_TextBox.Text = "";
                Total_TextBox.Text = "";
                Commission_TextBox.Text = "";
                return;
            }

            // If no errors, clear the error label.
            ErrorMessageLabel.Content = "";

            // Check for Negative Numbers
            Item1 = Item1 < 0 ? 0 : Item1;
            Item2 = Item2 < 0 ? 0 : Item2;
            Item3 = Item3 < 0 ? 0 : Item3;
            Item4 = Item4 < 0 ? 0 : Item4;

            // Sum the values to determine Subtotal
            subtotal = Item1 + Item2 + Item3 + Item4;
            
            // If the Shipping Radio Button is checked, determine the value.
            shippingCharge = ShippingRadio.IsChecked == true ? subtotal * shipping : 0.00;


            // ----- Determine the tax values & total
            // Calculate taxes for item 1 
            Item1_PST = PST1.IsChecked == true ? Item1 * pst : 0;
            Item1_GST = GST1.IsChecked == true ? Item1 * gst : 0;

            // Calculate taxes for item 2
            Item2_PST = PST2.IsChecked == true ? Item2 * pst : 0;
            Item2_GST = GST2.IsChecked == true ? Item2 * gst : 0;

            // Calculate taxes for item 3
            Item3_PST = PST3.IsChecked == true ? Item3 * pst : 0;
            Item3_GST = GST3.IsChecked == true ? Item3 * gst : 0;

            // Calculate taxes for item 4
            Item4_PST = PST4.IsChecked == true ? Item4 * pst : 0;
            Item4_GST = GST4.IsChecked == true ? Item4 * gst : 0;

            // Sum to get total tax values
            Total_GST = Item1_GST + Item2_GST + Item3_GST + Item4_GST;
            Total_PST = Item1_PST + Item2_PST + Item3_PST + Item4_PST;

            // Sum of everything to get total
            total = subtotal + shippingCharge + Total_PST + Total_GST;


            // ----- Determine the commission rate & set the color
            if (total <= 100) {
                CommissionRate = .035;
                Commission_TextBox.Background = Brushes.Black;
            } else if (total > 100 && total <= 225) {
                CommissionRate = .05;
                Commission_TextBox.Background = Brushes.Blue;
            } else if (total > 225 && total <= 500){
                CommissionRate = 0.07;
                Commission_TextBox.Background = Brushes.Green;
            } else if (total > 500) {
                CommissionRate = .11;
                Commission_TextBox.Background = Brushes.Red;
            }

            // Calculate commission
            commission = total * CommissionRate;

            // ----- Display Values
            Subtotal_TextBox.Text = String.Format("{0:C}", subtotal);
            Shipping_TextBox.Text = String.Format("{0:C}", shippingCharge);
            PST_TextBox.Text = String.Format("{0:C}", Total_PST); 
            GST_TextBox.Text = String.Format("{0:C}", Total_GST); 
            Total_TextBox.Text = String.Format("{0:C}", total); 
            Commission_TextBox.Text = String.Format("{0:C}", commission); 
        }
    }
}
