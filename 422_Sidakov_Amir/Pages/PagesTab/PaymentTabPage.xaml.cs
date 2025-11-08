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
    /// Управление платежами: 
    /// Просмотр, добавление и редактирование платежей.
    /// </summary>
    public partial class PaymentTabPage : Page
    {
        public PaymentTabPage()
        {
            InitializeComponent();

            DataGridPayment.ItemsSource = GetContext().Payment.ToList();
            this.IsVisibleChanged += Page_IsVisibleChanged;

            this.Title = "Payment_Tab_Page";

        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                GetContext().ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                DataGridPayment.ItemsSource = GetContext().Payment.ToList();
            }
        }
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AddPaymentPage(null));
        }


        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            var paymentForRemoving = DataGridPayment.SelectedItems.Cast<Payment>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить записи в количестве {paymentForRemoving.Count()} элементов?",
                "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    var context = GetContext();

                    var paymentIds = paymentForRemoving.Select(c => c.ID).ToList();
                    var paymentiesToRemove = context.Payment.Where(c => paymentIds.Contains(c.ID)).ToList();

                    context.Payment.RemoveRange(paymentiesToRemove);
                    context.SaveChanges();

                    DataGridPayment.ItemsSource = context.Payment.ToList();

                    MessageBox.Show("Данные успешно удалены!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            var selectedPayment = (sender as Button).DataContext as Payment;
            NavigationService.Navigate(new AddPaymentPage(selectedPayment?.ID));
        }


        private static Sidakov_DB_PaymentEntities1 _context;
        public static Sidakov_DB_PaymentEntities1 GetContext()
        {
            if (_context == null)
                _context = new Sidakov_DB_PaymentEntities1();
            return _context;
        }

    }
}