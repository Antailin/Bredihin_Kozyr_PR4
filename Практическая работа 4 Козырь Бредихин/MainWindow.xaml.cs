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
using Практическая_работа_4_Козырь_Бредихин.pages;

namespace Практическая_работа_4_Козырь_Бредихин
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Page1 _page1 = new Page1();
        private readonly Page2 _page2 = new Page2();
        private readonly Page3 _page3 = new Page3();

        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(_page1);
        }

        private void BtnPage1_Click(object sender, RoutedEventArgs e)
            => MainFrame.Navigate(_page1);

        private void BtnPage2_Click(object sender, RoutedEventArgs e)
            => MainFrame.Navigate(_page2);

        private void BtnPage3_Click(object sender, RoutedEventArgs e)
            => MainFrame.Navigate(_page3); 

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var result = MessageBox.Show(
                "Вы действительно хотите выйти из приложения?",
                "Подтверждение выхода",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.No)
                e.Cancel = true;
        }
    }
}


