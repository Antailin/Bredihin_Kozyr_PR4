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
    /// Логика взаимодействия для Page2.xaml
    /// </summary>
    public partial class Page2 : Page
    {
        public Page2() => InitializeComponent();

        private double F(double x)
        {
            if (RbSh.IsChecked == true) return Math.Sinh(x);
            if (RbX2.IsChecked == true) return x * x;
            return Math.Exp(x);
        }

        private void BtnCalculate2_Click(object sender, RoutedEventArgs e)
        {
            ErrorText2.Text = "";

            if (!TryParse(TbX2.Text, "x", out double x)) return;
            if (!TryParse(TbY2.Text, "y", out double y)) return;

            try
            {
                double fx = F(x);
                double diff = x - y;
                double c;

                if (Math.Abs(diff) < 1e-10)
                {
                    c = fx * fx + y * y + Math.Sin(y);
                }
                else if (diff > 0)
                {
                    c = Math.Pow(fx - y, 2) + Math.Cos(y);
                }
                else
                {
                    if (Math.Abs(Math.Cos(y)) < 1e-10)
                    {
                        ErrorText2.Text = "Ошибка: tg(y) не определён при данном y (cos(y) = 0).";
                        return;
                    }
                    c = Math.Pow(y - fx, 2) + Math.Tan(y);
                }

                TbResult2.Text = c.ToString("G10", CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                ErrorText2.Text = "Ошибка вычисления: " + ex.Message;
            }
        }

        private void BtnClear2_Click(object sender, RoutedEventArgs e)
        {
            TbX2.Clear();
            TbY2.Clear();
            TbResult2.Clear();
            RbSh.IsChecked = true;
            ErrorText2.Text = "";
        }

        private bool TryParse(string text, string name, out double val)
        {
            val = 0;
            if (string.IsNullOrWhiteSpace(text))
            {
                ErrorText2.Text = $"Ошибка: поле «{name}» не заполнено.";
                return false;
            }
            if (!double.TryParse(text.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out val))
            {
                ErrorText2.Text = $"Ошибка: поле «{name}» содержит недопустимое значение.";
                return false;
            }
            return true;
        }
    }
}

