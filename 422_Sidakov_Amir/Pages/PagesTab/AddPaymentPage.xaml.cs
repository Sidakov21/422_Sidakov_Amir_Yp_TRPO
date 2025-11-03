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

        public AddPaymentPage(Payment selectedPayment)
        {
            InitializeComponent();

            CBCategory.ItemsSource = GetContext().Category.ToList();
            CBCategory.DisplayMemberPath = "Name";
            CBUser.ItemsSource = GetContext().User.ToList();
            CBUser.DisplayMemberPath = "FIO";

            if (selectedPayment != null)
                _currentPayment = selectedPayment;

            DataContext = _currentPayment;
        }

        public Sidakov_DB_PaymentEntities GetContext()
        {
            using (var context = new Sidakov_DB_PaymentEntities())
            {
                return new Sidakov_DB_PaymentEntities();
            }
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
            if (_currentPayment.ID == 0)
                GetContext().Payment.Add(_currentPayment);
            try
            {
                GetContext().SaveChanges();
                MessageBox.Show("Данные успешно сохранены!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void ButtonClean_Click(object sender, RoutedEventArgs e)
        {
            _currentPayment = new Payment();
            DataContext = _currentPayment;

            CBCategory.SelectedIndex = -1;
            CBUser.SelectedIndex = -1;

            TBPaymentName.Text = "";
            TBAmount.Text = "";
            TBCount.Text = "";
            TBDate.Text = "";
        }
    }
}
