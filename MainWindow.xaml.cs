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

namespace NumericalIntegration
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtFindSolution_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxInputA.Text == string.Empty || TextBoxInputB.Text == string.Empty)
            {
                MessageBox.Show("Введите границы интегрирования.", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (TextBoxInputFunc.Text == string.Empty)
            {
                MessageBox.Show("Введите подинтегральную функцию.", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                double a = Convert.ToDouble(TextBoxInputA.Text);
                double b = Convert.ToDouble(TextBoxInputB.Text);
                DefineIntergral intergral = new DefineIntergral(TextBoxInputFunc.Text, a, b);
                //LabelResult.Content = intergral.MethodRectangle(BorderMethodRectangle.Left, 0.1);
                //LabelResult.Content = intergral.MethodMonteKarloGeometrical(100000);
                //LabelResult.Content = intergral.MethodMonteKarlo(1000000);
                LabelResult.Content = intergral.MethodGauss(1);
            }
            catch (FormatException ex) { MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
        }
    }
}
