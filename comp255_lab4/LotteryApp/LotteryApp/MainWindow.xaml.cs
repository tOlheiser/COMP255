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

namespace LotteryApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow() {
            InitializeComponent();
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e) {
            // Create an instance of the random object
            rnd = new Random();

            // Output the random values to the textboxes
            Output1Textbox.Text = GenerateLotto(3, 1, 10);
            Output2Textbox.Text = GenerateLotto(4, 1, 10);
            Output3Textbox.Text = GenerateLotto(5, 1, 40);
            Output4Textbox.Text = GenerateLotto(5, 1, 50);
            Output5Textbox.Text = GenerateLotto(1, 1, 43);
        }

        // Create a Random object
        private static Random rnd;

        // declare a function that returns a string, which takes in three integers as arguments.
        static string GenerateLotto(int Set, int LowRange, int HighRange) {
            // initialize an empty string to store my Lotto Numbers into.
            string LottoString = "";
            // initialize my counter.
            int i = 1;
            
            // while the counter is less than the set of required numbers, generate lotto numbers.
            while (i <= Set) {
                // append my lotto numbers to the string
                LottoString += GetLottoNumber(LowRange, HighRange);
                // increment
                i++;
            }

            // return the string
            return LottoString;
        }

        static string GetLottoNumber(int Low, int High) {
            string LottoNumber = rnd.Next(Low, High).ToString("00") + " ";
            return LottoNumber;
        }

        // "int value under 10 format to two digits"
        // https://stackoverflow.com/questions/2947675/int-value-under-10-convert-to-string-two-digit-number

        // "Random.Next returning the same sequence of values"
        // https://stackoverflow.com/questions/1654887/random-next-returns-always-the-same-values?lq=1
    }
}
