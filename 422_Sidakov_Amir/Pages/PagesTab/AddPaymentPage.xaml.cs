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

namespace _422_Sidakov_Amir.Pages.PagesTab
{
    /// <summary>
    /// Логика взаимодействия для AddPaymentPage.xaml
    /// </summary>
    public partial class AddPaymentPage : Page
    {
        private Payment _currentPayment = new Payment();
        private Sidakov_DB_PaymentEntities1 _context = new Sidakov_DB_PaymentEntities1();


        public AddPaymentPage(int? paymentId = null)
        {
            InitializeComponent();
            LoadComboBoxData();


            if (paymentId.HasValue)
                _currentPayment = _context.Payment.Find(paymentId.Value);
            else
                _currentPayment = new Payment { Date = DateTime.Today };

            DataContext = _currentPayment;

            this.Title = "Payment_Add_Page";
        }

        private void LoadComboBoxData()
        {
            CBCategory.ItemsSource = _context.Category.ToList();
            CBCategory.DisplayMemberPath = "Name";
            CBCategory.SelectedValuePath = "ID";

            CBUser.ItemsSource = _context.User.ToList();
            CBUser.DisplayMemberPath = "FIO";
            CBUser.SelectedValuePath = "ID";
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentPayment.Date.ToString()))
                errors.AppendLine("Укажите дату!");
            if (string.IsNullOrWhiteSpace(_currentPayment.Num.ToString()))
                errors.AppendLine("Укажите количество!");
            if (string.IsNullOrWhiteSpace(_currentPayment.Price.ToString()))
                errors.AppendLine("Укажите цену");
            if (string.IsNullOrWhiteSpace(_currentPayment.UserID.ToString()))
                errors.AppendLine("Укажите клиента!");
            if
           (string.IsNullOrWhiteSpace(_currentPayment.CategoryID.ToString()))
                errors.AppendLine("Укажите категорию!");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            try
            {
                if (_currentPayment.ID == 0)
                {
                    _context.Payment.Add(_currentPayment);
                }
                else
                {
                    _context.Payment.Attach(_currentPayment);
                    _context.Entry(_currentPayment).State = System.Data.Entity.EntityState.Modified;
                }

                _context.SaveChanges();
                MessageBox.Show("Данные успешно сохранены!");
                NavigationService?.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}");
            }
        }

        private void ButtonClean_Click(object sender, RoutedEventArgs e)
        {
            _currentPayment = new Payment { Date = DateTime.Today };
            DataContext = _currentPayment;

            CBCategory.SelectedIndex = -1;
            CBUser.SelectedIndex = -1;
        }

        private void CBCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CBCategory.SelectedItem is Category category)
                _currentPayment.CategoryID = category.ID;
        }

        private void CBUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CBUser.SelectedItem is User user)
                _currentPayment.UserID = user.ID;
        }
    }
}
