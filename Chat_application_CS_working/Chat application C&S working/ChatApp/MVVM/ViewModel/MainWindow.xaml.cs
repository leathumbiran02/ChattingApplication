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
    }
}
