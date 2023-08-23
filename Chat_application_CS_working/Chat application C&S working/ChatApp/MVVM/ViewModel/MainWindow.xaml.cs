using System;
using System.Windows;

namespace ChatApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(string username)
        {
            InitializeComponent();
            DataContext = new MainViewModel(username); // Set the DataContext to a new instance of MainViewModel
        }

        private void Logout_btn_click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to log out?", "Logout", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                // Call the Logout method in the ViewModel
                ((MainViewModel)DataContext).Logout();
            }
        }
    }
}
