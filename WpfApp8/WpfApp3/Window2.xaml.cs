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
using System.Windows.Shapes;
using System.IO;

namespace WpfApp3
{
    /// <summary>
    /// Логика взаимодействия для Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        public Window2()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "Image|*.png;*.jpg;*.jpeg";

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                string filename = dialog.FileName;

                
                ProgressBar.Visibility = Visibility.Visible;
                ProgressBar.Value = 0;

                await Task.Run(() =>
                {
                    
                    for (int i = 0; i <= 100; i++)
                    {
                        Dispatcher.Invoke(() => ProgressBar.Value = i); 
                        System.Threading.Thread.Sleep(10); 
                    }
                });

                
                ShowerImage.Source = new BitmapImage(new Uri(filename));

                
                ProgressBar.Visibility = Visibility.Hidden;
            }
        }

        private void ProgressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow w = new MainWindow();
            w.Show();
            this.Close();
        }
    }
}

