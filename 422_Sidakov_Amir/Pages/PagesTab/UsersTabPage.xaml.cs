using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
    /// Логика взаимодействия для UsersTabPage.xaml
    /// </summary>
    public partial class UsersTabPage : Page
    {
        public UsersTabPage()
        {
            InitializeComponent();
            LoadUsers();

        }

        private void LoadUsers()
        {
            try
            {
                using (var context = new Sidakov_DB_PaymentEntities())
                {
                    var users = context.User.ToList();
                    this.IsVisibleChanged += Page_IsVisibleChanged;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                try
                {
                    DataGridUser.ItemsSource = null;

                    using (var context = new Sidakov_DB_PaymentEntities())
                    {
                        context.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                        DataGridUser.ItemsSource = context.User.ToList();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                DataGridUser.ItemsSource = null;
            }
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AddUserPage(null));
        }

        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            var usersForRemoving = DataGridUser.SelectedItems.Cast<User>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить записи в количестве " +
                $"{usersForRemoving.Count()} элементов ? ", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {

                    using (var context = new Sidakov_DB_PaymentEntities())
                    {
                        var userIds = usersForRemoving.Select(u => u.ID).ToList();

                        var usersToDelete = context.User.Where(u => userIds.Contains(u.ID)).ToList();

                        context.User.RemoveRange(usersToDelete);
                        context.SaveChanges();
                    }

                    MessageBox.Show("Данные успешно удалены!");

                    RefreshDataGrid();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddUserPage((sender as Button).DataContext as User));

        }

        private void RefreshDataGrid()
        {
            try
            {
                using (var context = new Sidakov_DB_PaymentEntities())
                {
                    DataGridUser.ItemsSource = context.User.ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления данных: {ex.Message}", "Ошибка",
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
