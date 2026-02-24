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
    /// Логика взаимодействия для Page3.xaml
    /// </summary>
    public partial class Page3 : Page
    {
        private List<(double X, double Y)> _points = new List<(double, double)>();

        public Page3() => InitializeComponent();

        private double ComputeY(double x, double a, double b)
        {
            double arg = a - b * x;
            if (Math.Abs(Math.Cos(arg)) < 1e-9)
                return double.NaN;
            return 0.1 * a * Math.Pow(x, 3) * Math.Tan(arg);
        }

        private void BtnCalculate3_Click(object sender, RoutedEventArgs e)
        {
            ErrorText3.Text = "";

            if (!TryParse(TbX0.Text, "x₀", out double x0)) return;
            if (!TryParse(TbXk.Text, "xₖ", out double xk)) return;
            if (!TryParse(TbDx.Text, "dx", out double dx)) return;
            if (!TryParse(TbA.Text, "a", out double a)) return;
            if (!TryParse(TbB.Text, "b", out double b)) return;
            if (!TryParse(TbXDisplay.Text, "x", out double x0input)) return;

            if (Math.Abs(dx) < 1e-10)
            {
                ErrorText3.Text = "Ошибка: dx не может быть равен нулю.";
                return;
            }

            _points = new List<(double, double)>();
            var sb = new StringBuilder();

            double x = x0input; 
            for (int i = 0; i < 10000; i++)
            {
                bool beyond = dx > 0 ? x > xk + 1e-10 : x < xk - 1e-10;
                if (beyond) break;

                double y = ComputeY(x, a, b);

                if (!double.IsNaN(y) && !double.IsInfinity(y))
                {
                    sb.AppendLine(y.ToString("G7", CultureInfo.InvariantCulture));
                    _points.Add((x, y));
                }

                x += dx;
            }

            TbResults3.Text = sb.ToString();
            DrawChart();
        }

        private void BtnClear3_Click(object sender, RoutedEventArgs e)
        {
            TbX0.Clear();
            TbXk.Clear();
            TbDx.Clear();
            TbA.Clear();
            TbB.Clear();
            TbXDisplay.Clear();
            TbResults3.Clear();
            _points = new List<(double, double)>();
            ChartCanvas.Children.Clear();
            AxisCanvas.Children.Clear();
            ErrorText3.Text = "";
        }

        private void ChartCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_points.Count > 0)
                DrawChart();
        }

        private void DrawChart()
        {
            ChartCanvas.Children.Clear();
            AxisCanvas.Children.Clear();

            if (_points.Count < 2) return;

            double canvasW = ChartCanvas.ActualWidth;
            double canvasH = ChartCanvas.ActualHeight;
            if (canvasW <= 0 || canvasH <= 0) return;

            double minX = double.MaxValue, maxX = double.MinValue;
            double minY = double.MaxValue, maxY = double.MinValue;
            foreach (var p in _points)
            {
                if (p.X < minX) minX = p.X;
                if (p.X > maxX) maxX = p.X;
                if (p.Y < minY) minY = p.Y;
                if (p.Y > maxY) maxY = p.Y;
            }

            double rangeX = maxX - minX;
            double rangeY = maxY - minY;
            if (Math.Abs(rangeX) < 1e-10) rangeX = 1;
            if (Math.Abs(rangeY) < 1e-10) rangeY = 1;

            double leftMargin = 44;
            double topMargin = 20;

            double ToSX(double dx) => (dx - minX) / rangeX * canvasW;
            double ToSY(double dy) => canvasH - (dy - minY) / rangeY * canvasH;

            for (int i = 0; i <= 5; i++)
            {
                double frac = (double)i / 5;

                AxisCanvas.Children.Add(new Line
                {
                    X1 = leftMargin + frac * canvasW,
                    Y1 = topMargin,
                    X2 = leftMargin + frac * canvasW,
                    Y2 = topMargin + canvasH,
                    Stroke = Brushes.LightGray,
                    StrokeThickness = 1
                });
                AxisCanvas.Children.Add(new Line
                {
                    X1 = leftMargin,
                    Y1 = topMargin + frac * canvasH,
                    X2 = leftMargin + canvasW,
                    Y2 = topMargin + frac * canvasH,
                    Stroke = Brushes.LightGray,
                    StrokeThickness = 1
                });
            }

            var rect = new Rectangle
            {
                Width = canvasW,
                Height = canvasH,
                Stroke = Brushes.Black,
                StrokeThickness = 1,
                Fill = Brushes.Transparent
            };
            Canvas.SetLeft(rect, leftMargin);
            Canvas.SetTop(rect, topMargin);
            AxisCanvas.Children.Add(rect);

            int tickX = Math.Min(_points.Count, 6);
            for (int i = 0; i <= tickX; i++)
            {
                double frac = (double)i / tickX;
                double screenX = leftMargin + frac * canvasW;
                double screenY = topMargin + canvasH;

                var tb = new TextBlock
                {
                    Text = (minX + frac * rangeX).ToString("G4", CultureInfo.InvariantCulture),
                    FontSize = 10,
                    Foreground = Brushes.Black
                };
                Canvas.SetLeft(tb, screenX - 12);
                Canvas.SetTop(tb, screenY + 4);
                AxisCanvas.Children.Add(tb);
                AxisCanvas.Children.Add(new Line
                {
                    X1 = screenX,
                    Y1 = screenY,
                    X2 = screenX,
                    Y2 = screenY + 4,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                });
            }

            for (int i = 0; i <= 5; i++)
            {
                double frac = (double)i / 5;
                double screenX = leftMargin;
                double screenY = topMargin + canvasH - frac * canvasH;

                var tb = new TextBlock
                {
                    Text = (minY + frac * rangeY).ToString("G4", CultureInfo.InvariantCulture),
                    FontSize = 10,
                    Foreground = Brushes.Black
                };
                Canvas.SetLeft(tb, screenX - 40);
                Canvas.SetTop(tb, screenY - 7);
                AxisCanvas.Children.Add(tb);
                AxisCanvas.Children.Add(new Line
                {
                    X1 = screenX - 4,
                    Y1 = screenY,
                    X2 = screenX,
                    Y2 = screenY,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                });
            }

            var poly = new Polyline
            {
                Stroke = Brushes.SteelBlue,
                StrokeThickness = 2,
                StrokeLineJoin = PenLineJoin.Round
            };
            foreach (var p in _points)
                poly.Points.Add(new Point(ToSX(p.X), ToSY(p.Y)));
            ChartCanvas.Children.Add(poly);
        }

        private bool TryParse(string text, string name, out double val)
        {
            val = 0;
            if (string.IsNullOrWhiteSpace(text))
            {
                ErrorText3.Text = $"Ошибка: поле «{name}» не заполнено.";
                return false;
            }
            if (!double.TryParse(text.Replace(',', '.'),
                NumberStyles.Float, CultureInfo.InvariantCulture, out val))
            {
                ErrorText3.Text = $"Ошибка: поле «{name}» содержит недопустимое значение.";
                return false;
            }
            return true;
        }
    }
}
