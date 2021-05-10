using FParsec;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using MathNet.Symbolics;
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
        SeriesCollection seriesCollection;
        LineSeries functionLine;
        ScatterSeries scatterSeries;
        public MainWindow()
        {
            InitializeComponent();
            seriesCollection = new SeriesCollection();
            functionLine = new LineSeries { Values = new ChartValues<ObservablePoint>(), PointGeometrySize = 0, Title = "f(x)" };
            scatterSeries = new ScatterSeries { Values = new ChartValues<ObservablePoint>(), Title = "Точки", MinPointShapeDiameter = 7, MaxPointShapeDiameter = 20};
            Chart.Series = seriesCollection;
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
                        LabelResult.Content = integral.MethodMonteKarlo(Convert.ToDouble(UdCountPoints.Text));
                        break;
                    case 5:
                        List<Point> randPoints;
                        LabelResult.Content = integral.MethodMonteKarloGeometrical(Convert.ToDouble(UdCountPoints.Text), out randPoints);
                        seriesCollection.Clear();
                        scatterSeries.Values.Clear();
                        functionLine.Values.Clear();
                        List<Point> points = GeneratePoints();
                        for (int i = 0; i < points.Count; i++) functionLine.Values.Add(new ObservablePoint(points[i].X, points[i].Y));
                        for (int i = 0; i < randPoints.Count; i++) scatterSeries.Values.Add(new ObservablePoint(randPoints[i].X, randPoints[i].Y));


                        seriesCollection.Add(functionLine);
                        seriesCollection.Add(scatterSeries);
                        break;
                    case 6:
                        LabelResult.Content = integral.MethodGauss(3);
                        break;
                    case 7:
                        LabelResult.Content = integral.MethodChebyshev(3);
                        break;
                }
            }
            catch (FormatException ex) { MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private List<Point> GeneratePoints() 
        {
            SymbolicExpression expression = SymbolicExpression.Parse(TextBoxInputFunc.Text);
            Dictionary<string, FloatingPoint> variable = new Dictionary<string, FloatingPoint>();
            variable.Add("x", 0);

            List<Point> points = new List<Point>();
            decimal step = 0;
            decimal b = Convert.ToDecimal(TextBoxInputB.Text);
            while (step <= b) 
            {
                variable["x"] = (double)step;
                points.Add(new Point((double)step, expression.Evaluate(variable).RealValue));
                step += (decimal)0.1;
            }
            //for (int i = 0; i < count; i++)
            //{
            //    variable["x"] = (double)step;
            //    points.Add(new Point((double)step, expression.Evaluate(variable).RealValue));
            //    step += (decimal)0.1;
            //}

            return points;
        }
    }
}
