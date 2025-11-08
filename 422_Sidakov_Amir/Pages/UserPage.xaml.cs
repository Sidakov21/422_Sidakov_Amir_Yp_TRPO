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
    /// Страница пользователя: 
    /// Показывает список пользователей с фильтрацией и сортировкой.
    /// </summary>
    public partial class UserPage : Page
    {
        public UserPage()
        {
            InitializeComponent();
            LoadUsers();
            this.Title = "User_Page";
        }

        private void LoadUsers()
        {
            try
            {
                using (var context = new Sidakov_DB_PaymentEntities1())
                {
                    var users = context.User.ToList();
                    ListUser.ItemsSource = users;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
        }

        private void fioFilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateUsers();
        }

        private void sortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateUsers();
        }

        private void clearFiltersButton_Click_1(object sender, RoutedEventArgs e)
        {
            fioFilterTextBox.Text = "";
            sortComboBox.SelectedIndex = 0;
            onlyAdminCheckBox.IsChecked = false;
        }

        private void onlyAdminCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            UpdateUsers();
        }

        private void onlyAdminCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateUsers();
        }

        private void UpdateUsers()
        {
            if (!IsInitialized)
            {
                return;
            }

            try
            {
                using (var context = new Sidakov_DB_PaymentEntities1())
                {
                    var users = context.User.ToList();

                    // Фильтрация по фамилии 
                    if (!string.IsNullOrWhiteSpace(fioFilterTextBox.Text))
                    {
                        users = users.Where(x =>
                            x.FIO.ToLower().Contains(fioFilterTextBox.Text.ToLower())).ToList();
                    }

                    // Фильтрация по роли
                    if (onlyAdminCheckBox.IsChecked == true)
                    {
                        users = users.Where(x => x.Role == "admin").ToList();
                    }

                    // Сортировка
                    users = (sortComboBox.SelectedIndex == 0)
                        ? users.OrderBy(x => x.FIO).ToList()
                        : users.OrderByDescending(x => x.FIO).ToList();

                    ListUser.ItemsSource = users;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления данных: {ex.Message}");
            }
        }

    }
}
