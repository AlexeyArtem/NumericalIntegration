using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MathNet.Symbolics;

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
                sum += GetFunctionValue(a + random.Next(0, (int)length - 1) + random.NextDouble());
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
                xRand = random.Next((int)a, (int)b) + random.NextDouble();
                yRand = random.Next((int)yMin, (int)yMax) + random.NextDouble();
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
    }
}
