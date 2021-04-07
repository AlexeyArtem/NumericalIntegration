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

        public DefineIntergral(string function, double a, double b) 
        {
            funcString = function;
            funcExpression = SymbolicExpression.Parse(function);
            variable = new Dictionary<string, FloatingPoint>();
            variable.Add("x", 0);
            
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

        double MethodMonteKarloGeometrical(double n)
        {
            double yMin = 0;
            double yMax = 0;
            Random random = new Random();
            for (double i = a; i < b; i += 0.001)
            {
                double valueFunction = MathParse(funcString, i);
                
                if (valueFunction > yMax) yMax = valueFunction;
                else if (valueFunction < yMin) yMin = valueFunction;
            }


            int k = 0;
            double xRand, yRand;
            List<Point> points = new List<Point>();
            while (points.Count < n)
            {
                xRand = random.Next((int)a, (int)b) + random.NextDouble();
                yRand = random.Next((int)yMin, (int)yMax) + random.NextDouble();
            }



            for (int i = 0; i < n; i++)
            {
                xRand = random.Next((int)a, (int)b) + random.NextDouble();
                yRand = random.Next((int)yMin, (int)yMax) + random.NextDouble();
                //yRand = random.Next(-1, 1) + random.NextDouble();

                //else yRand = random.Next((int)yMax, (int)yMin) + random.NextDouble() - 1;
                double valueFunction = MathParse(funcString, xRand);

                //if (valueFunction > yRand) k++;
                if ((valueFunction > 0) && (yRand < valueFunction)) k++;
                else if ((valueFunction < 0) && (yRand > valueFunction)) k++;
            }

            double area = (b - a) * (yMax - yMin);
            double result = (k / n) * area;

            return result;
        }

        //private List<Point> GenerateRandomValues(double xMin, double xMax, double yMin, double yMax, int countPoints)
        //{
        //    List<Point> points = new List<Point>();
        //    Random random = new Random();

        //    double xRand;
        //    double yRand;

        //    do
        //    {
        //        xRand = random.Next((int)xMin, (int)xMax) + random.NextDouble();
        //        yRand = random.Next((int)yMin, (int)yMax) + random.NextDouble();

        //        if (xRand < 0) if (xRand < xMin) continue;
        //        if (xRand > 0) if (xRand > xMax) continue;

        //        if (yRand < 0) if (yRand < yMin) continue;
        //        if (yRand > 0) if (yRand > yMax) continue;

        //        points.Add(new Point(xRand, yRand));
        //    }
        //    while (points.Count != countPoints);

        //    return points;
        //}
    }
}
