using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SemaphoreThreads
{
    public partial class MainWindow : Window
    {
        private readonly SemaphoreSlim sem = new SemaphoreSlim(0); 
        private int val = 0;      
        private int threadNo = 0;   

        public MainWindow()
        {
            InitializeComponent();
            txtScroll.Text = val.ToString();
        }
         
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            val++;
            sem.Release();
            txtScroll.Text = val.ToString();
        }
         
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (val == 0) 
                return;

            if (sem.Wait(0))
            {
                val--;
                txtScroll.Text = val.ToString();
            }
            else
            {
                MessageBox.Show("There is a not enogh place", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
         
        private void createBtn_Click(object sender, RoutedEventArgs e)
        {
            threadNo++;
            listbox3.Items.Add($"Thread {threadNo}");
        }

        private void listbox3_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = listbox3.SelectedItem;
            if (item == null) 
                return;

            listbox3.Items.Remove(item);
            listbox2.Items.Add(item);

            var th = new Thread(() =>
            {
                sem.Wait();

                Dispatcher.Invoke(() =>
                {
                    listbox2.Items.Remove(item);
                    listbox1.Items.Add(item);
                });
            });
            th.IsBackground = true;
                th.Start();
        }

        private void listbox1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = listbox1.SelectedItem;

            if (item == null) 
                return;

            listbox1.Items.Remove(item);

            sem.Release();
        }
    }
}