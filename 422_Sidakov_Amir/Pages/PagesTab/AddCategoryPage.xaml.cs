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
    /// Добавление категории: 
    /// Создание или изменение категорий.
    /// </summary>
    public partial class AddCategoryPage : Page
    {
        private Category _currentCategory = new Category();

        public AddCategoryPage(Category selectedCategory)
        {
            InitializeComponent();
            if (selectedCategory != null)
                _currentCategory = selectedCategory;

            DataContext = _currentCategory;

            this.Title = "Category_Add_Page";

        }

        public Sidakov_DB_PaymentEntities1 GetContext()
        {
            using (var context = new Sidakov_DB_PaymentEntities1())
            {
                return context;
            }
        }

        private void ButtonSaveCategory_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_currentCategory.Name))
                errors.AppendLine("Укажите название категории!");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            try
            {
                using (var context = new Sidakov_DB_PaymentEntities1())
                {
                    if (_currentCategory.ID == 0)
                    {
                        context.Category.Add(_currentCategory);
                    }
                    else
                    {
                        context.Entry(_currentCategory).State = System.Data.Entity.EntityState.Modified;
                    }

                    context.SaveChanges();
                }

                MessageBox.Show("Данные успешно сохранены!");

                NavigationService?.Navigate(new CategoryTabPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}");
            }
        }

        private void ButtonClean_Click(object sender, RoutedEventArgs e)
        {
            TBCategoryName.Text = "";
        }
    }
}
