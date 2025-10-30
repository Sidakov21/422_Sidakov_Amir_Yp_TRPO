using _422_Sidakov_Amir.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace _422_Sidakov_Amir
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new AuthPage());
            this.Title = "Sidakov_Payment";
        }

        private void Windows_Loaded(object sender, RoutedEventArgs e)
        {
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = new TimeSpan(0,0,1);
            timer.IsEnabled = true;

            timer.Tick += (o, t) => { 
                DateTimeNow.Text = DateTime.Now.ToString();
            };

            timer.Start();
        }

        private void Window_Closing(Object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("Вы уверены , что хотите закрыть окно?", "Message", MessageBoxButton.YesNo)
                == System.Windows.MessageBoxResult.No){
                e.Cancel = true;
            }
            else { 
                e.Cancel = false; 
            }

        }

        private bool isTheme = true;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (isTheme)
            {
                var uri = new Uri("Dictionary.xaml", UriKind.Relative);
                ResourceDictionary resourceDic = Application.LoadComponent(uri) as ResourceDictionary;
                Application.Current.Resources.Clear();
                Application.Current.Resources.MergedDictionaries.Add(resourceDic);
            }
            else
            {
                var uri = new Uri("DictionaryWindow.xaml", UriKind.Relative);
                ResourceDictionary resourceDic = Application.LoadComponent(uri) as ResourceDictionary;
                Application.Current.Resources.Clear();
                Application.Current.Resources.MergedDictionaries.Add(resourceDic);
            }

            isTheme = !isTheme;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new AuthPage());
        }
    }
}



