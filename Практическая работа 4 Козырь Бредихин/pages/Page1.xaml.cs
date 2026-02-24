using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace Практическая_работа_4_Козырь_Бредихин.pages
{
    /// <summary>
    /// Логика взаимодействия для Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        public Page1() => InitializeComponent();

        private void BtnCalculate1_Click(object sender, RoutedEventArgs e)
        {
            ErrorText1.Text = "";

            if (!TryParse(TbX1.Text, "x", out double x)) return;
            if (!TryParse(TbY1.Text, "y", out double y)) return;
            if (!TryParse(TbZ1.Text, "z", out double z)) return;

            double inner = x - (2.0 * y) / (1.0 + x * x * y * y);
            if (Math.Abs(inner) < 1e-10)
            {
                ErrorText1.Text = "Ошибка: |x − 2y/(1+x²y²)| = 0, деление невозможно.";
                return;
            }

            double absY = Math.Abs(y);
            if (x < 0 && Math.Abs(absY - Math.Round(absY)) > 1e-10)
            {
                ErrorText1.Text = "Ошибка: x < 0 и |y| не целое — x^|y| не определён в вещественных числах.";
                return;
            }

            if (Math.Abs(z) < 1e-10)
            {
                ErrorText1.Text = "Ошибка: z не должен быть равен нулю.";
                return;
            }

            try
            {
                double numerator = 1.0 + Math.Pow(Math.Sin(x + y), 2);

                double denominator = Math.Abs(inner);

                double xPowAbsY = Math.Pow(x, absY);

                double cosPart = Math.Pow(Math.Cos(Math.Atan(1.0 / z)), 2);

                double v = (numerator / denominator) * xPowAbsY + cosPart;

                TbResult1.Text = v.ToString("G10", CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                ErrorText1.Text = "Ошибка вычисления: " + ex.Message;
            }
        }

        private void BtnClear1_Click(object sender, RoutedEventArgs e)
        {
            TbX1.Clear();
            TbY1.Clear();
            TbZ1.Clear();
            TbResult1.Clear();
            ErrorText1.Text = "";
        }

        private bool TryParse(string text, string name, out double val)
        {
            val = 0;
            if (string.IsNullOrWhiteSpace(text))
            {
                ErrorText1.Text = $"Ошибка: поле «{name}» не заполнено.";
                return false;
            }
            if (!double.TryParse(text.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out val))
            {
                ErrorText1.Text = $"Ошибка: поле «{name}» содержит недопустимое значение.";
                return false;
            }
            return true;
        }
    }
}