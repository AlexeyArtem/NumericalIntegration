using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MathNet.Symbolics;
using NumericsMethodsLibrary;

namespace NumericalIntegration
{
    enum BorderMethodRectangle 
    {
        Left,
        Middle,
        Right
    }
    
    class DefineIntergral
    {
        private double a, b;
        private string funcString;
        private SymbolicExpression funcExpression;
        private Dictionary<string, FloatingPoint> variable;
        private Random random;

        public DefineIntergral(string function, double a, double b) 
        {
            funcString = function;
            funcExpression = SymbolicExpression.Parse(function);
            variable = new Dictionary<string, FloatingPoint>();
            variable.Add("x", 0);
            random = new Random();
            
            //Нужно ли проверять, чтобы нижняя граница была меньше верхней?
            this.a = a;
            this.b = b;
            
        }

        private double GetFunctionValue(double x)
        {
            variable["x"] = x;
            return funcExpression.Evaluate(variable).RealValue;
        }


        public double MethodRectangle(BorderMethodRectangle border, double h)
        {
            double result = 0;
            for (double i = a; i <= b; i += h)
            {
                result += GetFunctionValue(i) * h;
            }

            return result;
        }

        public double MethodTrapezoid(double h)
        {
            double result = 0;
            for (double i = a; i <= b; i += h)
            {
                if (i == b) break;
                result += (GetFunctionValue(i) + GetFunctionValue(i + h)) * h;
            }
            result *= 0.5;

            return result;
        }

        public double MethodSplains(double h)
        {
            double sum1 = 0;
            double sum2 = 0;
            List<Point> points = new List<Point>();
            for (double i = a; i < b; i += h)
            {
                points.Add(new Point(i, GetFunctionValue(i)));
            }
            Derivative derivative = new Derivative(points);
            for (double i = a; i <= b; i += h)
            {
                if (i == b) break;
                sum1 += (GetFunctionValue(i) + GetFunctionValue(i + h)) * h;
                sum2 += derivative.CubicInterpolationMethod(3).DerivativePoints.IndexOf(new Point(i, GetFunctionValue(i + h))) * Math.Pow(h, 3);
            }
            sum1 *= 0.5;
            sum2 *= 1 / 12;
            double result = sum1 - sum2;

            return result;
        }

        public double MethodParabol(double h)
        {
            double sumOdd = 0;
            for (double i = a + h; i < b; i += 2 * h)
            {
                sumOdd += GetFunctionValue(i);
            }

            double sumEven = 0;
            for (double i = a + 2 * h; i < b; i += 2 * h)
            {
                sumEven += GetFunctionValue(i);
            }

            double result = (h / 3) * (GetFunctionValue(a) + 4 * sumOdd + 2 * sumEven + GetFunctionValue(b));

            return result;
        }

        public double MethodMonteKarlo(double n)
        {
            Random random = new Random();

            double length = b - a;
            double sum = 0;
            for (int i = 0; i < n; i++)
            {
                sum += GetFunctionValue(a + random.Next(0, (int)length) + random.NextDouble());
            }

            double result = ((b - a) / n) * sum;

            return result;
        }

        public double MethodMonteKarloGeometrical(double n)
        {
            //Определение экстремумов по Y
            double yMin = 0;
            double yMax = 0;
            double valueFunc;
            for (double i = a; i < b; i += 0.001)
            {
                valueFunc = GetFunctionValue(i);
                if (valueFunc > yMax) yMax = valueFunc;
                else if (valueFunc < yMin) yMin = valueFunc;
            }

            //Генерация случайных точек
            double xRand, yRand;
            List<Point> points = new List<Point>();
            while (points.Count < n)
            {
                xRand = random.Next(Convert.ToInt32(a), Convert.ToInt32(b)) + random.NextDouble();
                yRand = random.Next(Convert.ToInt32(yMin), Convert.ToInt32(yMax)) + random.NextDouble();
                if (xRand < a || xRand > b) continue;
                if (yRand < yMin || yRand > yMax) continue;

                points.Add(new Point(xRand, yRand));
            }

            //Определение количества точек, попавших в функцию
            int k = 0;
            for (int i = 0; i < points.Count; i++)
            {
                double value = GetFunctionValue(points[i].X);
                if ((points[i].Y > 0 && points[i].Y <= value) || (points[i].Y < 0 && points[i].Y >= value)) k++;
            }

            double area = (b - a) * (yMax - yMin);
            double result = (k / n) * area;

            return result;
        }

        public double MethodGauss(int n)
        {
            double result = 0;
            for (int i = 0; i < n; i++)
            {
                result += GetCoefficientA(i, n) * GetFunctionValue(GetArgumentX(i, n));
            }

            result *= (b - a) / 2;
            return result;
        }

        private double GetArgumentX(int i, int n)
        {
            double x;
            x = ((a +b) / 2) + ((b - a) / 2) * GetRootLegendrePolynomial(n, i, 1);
            return x;
        }

        private double GetCoefficientA(int i, int n)
        {
            double a;
            a = 2 / ((1 - GetRootLegendrePolynomial(n, i, 2)) * Math.Pow(GetDerivativeLegendrePolynomial(GetRootLegendrePolynomial(n, i, 1), n), 2));
            return a;
        }

        private double GetRootLegendrePolynomial(int n, int i, int k) 
        {
            double t, tCurrent;
            if(k == 0) return Math.Cos(Math.PI * (4 * i - 1) / (4 * n + 2));
            else
            {
                tCurrent = GetRootLegendrePolynomial(n, i, k-1);
                t = tCurrent - (GetLegendrePolynomial(tCurrent, n) / GetDerivativeLegendrePolynomial(tCurrent, n));
            }
            return t;
        }

        private double GetLegendrePolynomial(double t, int k)
        {
            double polinomial;
            if (k == 0) return 1;
            else if (k == 1) return t;
            //else if(k + 1 == n) 
            else polinomial = ((2 * k + 1)/(k + 1)) * t * GetLegendrePolynomial(t, k - 1) - (k / (k + 1)) * GetLegendrePolynomial(t, k - 2);
            return polinomial;
        }

        private double GetDerivativeLegendrePolynomial(double t, int n)
        {
            return (n / (1 - t * t)) * (GetLegendrePolynomial(t, n - 1) - t * GetLegendrePolynomial(t, n));
        }
    }
}
