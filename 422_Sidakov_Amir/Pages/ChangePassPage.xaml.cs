using _422_Sidakov_Amir.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace _422_Sidakov_Amir.Pages
{
    /// <summary>
    /// Логика взаимодействия для ChangePassPage.xaml
    /// </summary>
    public partial class ChangePassPage : Page
    {
        public ChangePassPage()
        {
            InitializeComponent();
        }

        public static string GetHash(String password)
        {
            using (var hash = SHA1.Create())
                return
                string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x =>
                x.ToString("X2")));
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(passBxOld.Password) ||
                string.IsNullOrEmpty(passBxNew.Password) ||
                string.IsNullOrEmpty(passBxConfirm.Password) ||
                string.IsNullOrEmpty(txtbxLogin.Text))
            {
                MessageBox.Show("Все поля обязательны к заполнению!");
                return;
            }

            string hashedPass = GetHash(passBxOld.Password);

            using (var context = new Sidakov_DB_PaymentEntities1())
            {
                var user = context.User.FirstOrDefault(u => u.Login == txtbxLogin.Text && u.Password == hashedPass);

                if (user == null)
                {
                    MessageBox.Show("Текущий пароль/Логин неверный!");
                    return;
                }

                if (passBxNew.Password.Length >= 6)
                {
                    bool en = true;
                    bool number = false;

                    for (int i = 0; i < passBxNew.Password.Length; i++)
                    {
                        if (passBxNew.Password[i] >= '0' && passBxNew.Password[i] <= '9')
                            number = true;
                        else if (!((passBxNew.Password[i] >= 'A' && passBxNew.Password[i] <= 'Z') ||
                            (passBxNew.Password[i] >= 'a' && passBxNew.Password[i] <= 'z')))
                            en = true;
                    }

                    if (!en)
                        MessageBox.Show("Используйте только английскую расскладку!");
                    else if (!number)
                        MessageBox.Show("Добавьте хотябы одну цифру!");

                    if (en && number)
                    {
                        if (passBxNew.Password != passBxConfirm.Password)
                        {
                            MessageBox.Show("Пароли не совпадают!");
                        }
                        else
                        {

                            user.Password = GetHash(passBxNew.Password);
                            context.SaveChanges();
                            MessageBox.Show("Пароль успешно изменен!");
                            NavigationService?.Navigate(new AuthPage());
                        }
                    }
                }
                else
                    MessageBox.Show("Пароль слишком короткий, должно быть минимум 6 символов!");
            }
        }

        private void txtbxLogin_TextChanged(object sender, TextChangedEventArgs e)
        {
            lblLoginHint.Visibility = string.IsNullOrEmpty(txtbxLogin.Text) ? Visibility.Visible : Visibility.Hidden;
        }

        private void passBxOld_PasswordChanged(object sender, RoutedEventArgs e)
        {
            lblOldPassHint.Visibility = string.IsNullOrEmpty(passBxOld.Password) ? Visibility.Visible : Visibility.Hidden;
        }

        private void passBxNew_PasswordChanged(object sender, RoutedEventArgs e)
        {
            lblNewPassHint.Visibility = string.IsNullOrEmpty(passBxNew.Password) ? Visibility.Visible : Visibility.Hidden;
        }

        private void passBxConfirm_PasswordChanged(object sender, RoutedEventArgs e)
        {
            lblConfirmPassHint.Visibility = string.IsNullOrEmpty(passBxConfirm.Password) ? Visibility.Visible : Visibility.Hidden;
        }

        private void lblLoginHint_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            txtbxLogin.Focus();
        }

        private void lblOldPassHint_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            passBxOld.Focus();
        }

        private void lblNewPassHint_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            passBxNew.Focus();
        }

        private void lblConfirmPassHint_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            passBxConfirm.Focus();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }
    }
}
