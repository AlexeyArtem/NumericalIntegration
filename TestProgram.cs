using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Symbolics;


namespace Test
{
    enum BorderMethodRectangle
    {
        Left,
        Right,
        Middle
    }
    class Program
    {
        static void Main(string[] args)
        {
            double a = 0;
            double b = 10;
            double h = 0.01;
            double n = 1000000;

            Console.WriteLine("Введите функцию:");
            string function = Console.ReadLine();

            //double result = 0;
            //for (double i = a; i <= b; i += h)
            //{
            //    if (i == b) break;
            //    result += (MathParse(function, i) + MathParse(function, i + h)) * h;
            //}
            //result *= 0.5;

            Console.WriteLine("Результат: " + MethodParabol(function, a, b, h));
            Console.WriteLine("Результат: " + MethodRectangle(BorderMethodRectangle.Left, function, a, b, h));
            Console.WriteLine("Результат: " + MethodTrapezoid(function, a, b, h));
            Console.WriteLine("Результат: " + MethodMonteKarlo(function, a, b, n));
            Console.Read();
        }

        static double MathParse(string function, double x)
        {
            SymbolicExpression expression = SymbolicExpression.Parse(function);
            Dictionary<string, FloatingPoint> variables = new Dictionary<string, FloatingPoint>();
            variables.Add("x", x);

            return expression.Evaluate(variables).RealValue;
        }

        
        static double MethodRectangle(BorderMethodRectangle border, string function, double a, double b, double h)
        {
            double result = 0;
            for (double i = a; i <= b; i += h)
            {
                result += MathParse(function, i) * h;
            }

            return result;
        }

        static double MethodTrapezoid(string function, double a, double b, double h)
        {
            double result = 0;
            for (double i = a; i <= b; i += h)
            {
                if (i == b) break;
                result += (MathParse(function, i) + MathParse(function, i + h)) * h;
            }
            result *= 0.5;

            return result;
        }
        static double MethodParabol(string function, double a, double b, double h)
        {
            double sumOdd = 0;
            for (double i = a + h; i < b; i += 2*h)
            {
                sumOdd += MathParse(function, i);
            }

            double sumEven = 0;
            for (double i = a + 2 * h; i < b; i += 2 * h)
            {
                sumEven += MathParse(function, i);
            }

            double result = (h / 3) * (MathParse(function, a) + 4 * sumOdd + 2 * sumEven + MathParse(function, b));

            return result;
        }
        static double MethodMonteKarlo(string function, double a, double b, double n)
        {
            Random random = new Random();

            double length = b - a;
            double sum = 0;
            for (int i = 0; i < n; i++)
            {
                sum += MathParse(function, a + random.Next(0, (int)length - 1) + random.NextDouble());
            }

            double result = ((b - a) / n) * sum;

            return result;
        }
    }
}
