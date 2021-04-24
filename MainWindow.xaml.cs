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
                DefineIntergral integral = new DefineIntergral(TextBoxInputFunc.Text, a, b);

                switch (CbSelectMethod.SelectedIndex)
                {
                    case 0:
                        LabelResult.Content = integral.MethodRectangle(BorderMethodRectangle.Left, 0.1);
                        break;
                    case 1:
                        LabelResult.Content = integral.MethodTrapezoid(0.1);
                        break;
                    case 2:
                        LabelResult.Content = integral.MethodParabol(0.1);
                        break;
                    case 3:
                        LabelResult.Content = integral.MethodSplains(0.1);
                        break;
                    case 4:
                        LabelResult.Content = integral.MethodMonteKarlo(Convert.ToDouble(TbMonteKarloGeo.Text));
                        break;
                    case 5:
                        LabelResult.Content = integral.MethodMonteKarloGeometrical(Convert.ToDouble(TbMonteKarloGeo.Text));
                        break;
                    case 6:
                        LabelResult.Content = integral.MethodGauss(2);
                        break;
                }
            }
            catch (FormatException ex) { MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
        }
    }
}
