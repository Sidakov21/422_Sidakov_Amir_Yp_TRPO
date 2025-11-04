using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
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

namespace _422_Sidakov_Amir.Pages.PagesTab
{
    /// <summary>
    /// Логика взаимодействия для AddUserPage.xaml
    /// </summary>
    public partial class AddUserPage : Page
    {
        private User _currentUser = new User();
        public AddUserPage(User selectedUser)
        {
            InitializeComponent();

            if (selectedUser != null)
                _currentUser = selectedUser;
            DataContext = _currentUser;
        }

        public Sidakov_DB_PaymentEntities1 GetContext()
        {
            using (var context = new Sidakov_DB_PaymentEntities1())
            {
                return new Sidakov_DB_PaymentEntities1();
            }
        }

        public static string GetHash(String password)
        {
            using (var hash = SHA1.Create())
                return
                string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x =>
                x.ToString("X2")));
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_currentUser.Login))
                errors.AppendLine("Укажите логин!");

            if (string.IsNullOrWhiteSpace(_currentUser.Password))
                errors.AppendLine("Укажите пароль!");
            
            if ((_currentUser.Role == null) || (cmbRole.Text == ""))
                errors.AppendLine("Выберите роль!");
            else
                _currentUser.Role = cmbRole.Text;

            if (string.IsNullOrWhiteSpace(_currentUser.FIO))
                errors.AppendLine("Укажите ФИО");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            try
            {
                using (var context = new Sidakov_DB_PaymentEntities1())
                {
                    _currentUser.Password = GetHash(_currentUser.Password);

                    if (_currentUser.ID == 0)
                    {
                        context.User.Add(_currentUser);
                    }
                    else
                    {
                        context.Entry(_currentUser).State = System.Data.Entity.EntityState.Modified;
                    }

                    context.SaveChanges();
                }

                MessageBox.Show("Данные успешно сохранены!");

                NavigationService?.Navigate(new UsersTabPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}");
            }

        }

        private void ButtonClean_Click(object sender, RoutedEventArgs e)
        {
            TBLogin.Text = "";
            TBPass.Text = "";
            cmbRole.Items.Clear();
            TBFio.Text = "";
            TBPhoto.Text = "";
        }
    }
}
