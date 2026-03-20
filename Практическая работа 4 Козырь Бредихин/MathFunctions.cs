
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Практическая_работа_4_Козырь_Бредихин
{
    /// <summary>
    /// Содержит чистые математические функции из Практической работы №4,
    /// вынесенные из code-behind в отдельный класс для обеспечения тестируемости.
    /// </summary>
    public static class MathFunctions
    {
        // Страница 1: 

        public static double ComputePage1(double x, double y, double z)
        {
            if (Math.Abs(z) < 1e-10)
                throw new ArgumentException("z не должен быть равен нулю.", nameof(z));
            double inner = x - (2.0 * y) / (1.0 + x * x * y * y);
            if (Math.Abs(inner) < 1e-10)
                throw new ArgumentException(
                    "|x − 2y/(1+x²y²)| = 0, деление невозможно.", nameof(x));
            double absY = Math.Abs(y);
            if (x < 0 && Math.Abs(absY - Math.Round(absY)) > 1e-10)
                throw new ArgumentException(
                    "x < 0 и |y| не целое — x^|y| не определён в вещественных числах.", nameof(x));
            double numerator = 1.0 + Math.Pow(Math.Sin(x + y), 2);
            double denominator = Math.Abs(inner);
            double xPowAbsY = Math.Pow(x, absY);
            double cosPart = Math.Pow(Math.Cos(Math.Atan(1.0 / z)), 2);

            return (numerator / denominator) * xPowAbsY + cosPart;
        }

        // Страница 2: кусочная функция
        public static double FPage2(string mode, double x)
        {
            if (mode == "sinh") return Math.Sinh(x);
            if (mode == "x2") return x * x;
            return Math.Exp(x);
        }
        public static double ComputePage2(string mode, double x, double y)
        {
            double fx = FPage2(mode, x);
            double diff = x - y;

            if (Math.Abs(diff) < 1e-10)
                return fx * fx + y * y + Math.Sin(y);

            if (diff > 0)
                return Math.Pow(fx - y, 2) + Math.Cos(y);

            if (Math.Abs(Math.Cos(y)) < 1e-10)
                throw new ArgumentException(
                    "tg(y) не определён при данном y (cos(y) = 0).", nameof(y));

            return Math.Pow(y - fx, 2) + Math.Tan(y);
        }

        // Страница 3: 
        public static double ComputePage3(double x, double a, double b)
        {
            double arg = a - b * x;
            if (Math.Abs(Math.Cos(arg)) < 1e-9)
                return double.NaN;

            return 0.1 * a * Math.Pow(x, 3) * Math.Tan(arg);
        }
    }
}
