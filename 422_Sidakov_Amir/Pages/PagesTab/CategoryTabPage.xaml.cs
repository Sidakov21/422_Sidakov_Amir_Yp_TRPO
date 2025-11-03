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
    /// Логика взаимодействия для CategoryTabPage.xaml
    /// </summary>
    public partial class CategoryTabPage : Page
    {
        public CategoryTabPage()
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
                    var categories = context.Category.ToList();
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
                    DataGridCategory.ItemsSource = null;

                    using (var context = new Sidakov_DB_PaymentEntities())
                    {
                        context.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                        DataGridCategory.ItemsSource = context.Category.ToList();
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
                DataGridCategory.ItemsSource = null;
            }
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AddCategoryPage(null));
        }

        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            var categoryForRemoving = DataGridCategory.SelectedItems.Cast<Category>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить записи в количестве " +
                $"{categoryForRemoving.Count()} элементов ? ", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {

                    using (var context = new Sidakov_DB_PaymentEntities())
                    {
                        context.Category.RemoveRange(categoryForRemoving);
                        context.SaveChanges();

                        DataGridCategory.ItemsSource = context.Category.ToList();
                    }
                    
                    
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
            NavigationService.Navigate(new PagesTab.AddCategoryPage((sender as Button).DataContext as Category));
        }
    }
}
