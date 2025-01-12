using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Testik
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Color current_color = Color.FromRgb(0, 0, 0);

        public MainWindow()
        {
            InitializeComponent();
            myBox.Fill = new SolidColorBrush(current_color);
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider btn = (Slider)sender;
            TextBox txb;

            switch (btn.Name)
            {
                case "R":
                    txb = Rb;
                    current_color.R = (byte)e.NewValue;
                    break;
                case "G":
                    txb = Gb;
                    current_color.G = (byte)e.NewValue;
                    break;
                case "B":
                    txb = Bb;
                    current_color.B = (byte)e.NewValue;
                    break;

                default:
                    throw new Exception();
            }

            txb.Text = ((byte)e.NewValue).ToString();
            myBox.Fill = new SolidColorBrush(current_color);

            labeL.Content = "#";
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);

            if (!e.Handled && int.Parse(((TextBox)sender).Text.ToString() + e.Text) >= 256)
            {
                MessageBox.Show("Too big.");

                e.Handled = true;
                return;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txb = (TextBox)sender;

            if (txb.Text == "")
            {
                txb.Text = "0";
            }

            Slider sld = txb.Name switch
            {
                "Rb" => R,
                "Gb" => G,
                "Bb" => B,
                _ => throw new Exception(),
            };

            sld.Value = int.Parse(txb.Text.ToString());
        }
    }
}