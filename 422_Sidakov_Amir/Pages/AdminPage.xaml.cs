using _422_Sidakov_Amir.Pages.PagesTab;
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

namespace _422_Sidakov_Amir.Pages
{
    /// <summary>
    ///Страница администратора: 
    ///Навигация по разделам(пользователи, категории, платежи, диаграммы).
    /// </summary>
    public partial class AdminPage : Page
    {
        public AdminPage()
        {
            InitializeComponent();
            this.Title = "Admin_Page";
        }

        private void BtnTab1_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new UsersTabPage());
        }

        private void BtnTab2_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new CategoryTabPage());
        }

        private void BtnTab3_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new PaymentTabPage());
        }

        private void BtnTab4_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new DiagrammPage());
        }
    }
}
