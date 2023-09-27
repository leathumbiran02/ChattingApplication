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
using System.Data.SqlClient;
using System.Data;
using ChatClient.MVVM.Core;
using ChatClient.MVVM.Model;
using ChatClient.Net;
using System.Collections.ObjectModel;

namespace ChatApp
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        //Function to navigate to the login page:
        private void Go_to_login(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            loginWindow.Owner = this; // Set the register window as the owner of the login window
            loginWindow.Show();
            this.Hide();
        }

        //Function to handle when a user creates an account:
        private void Submit_btn_click(object sender, RoutedEventArgs e)
        {
            //If any field is empty, display an error message and return:
            if (AreRequiredFieldsEmpty())
            {
                MessageBox.Show("Please fill in all required fields.");
                return;
            }

            SqlConnection sqlCon = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB; Initial Catalog=ChattingAppDB; Integrated Security=True"); //Connection string for SQL Server:

            try
            {
                if (sqlCon.State == ConnectionState.Closed) //If the connection to the database is closed execute the following:
                {
                    sqlCon.Open(); //Open the SQL connection:
                    String query = "INSERT INTO users_table(username,full_name,email,password) VALUES(@username,@full_name,@email,@password)"; //Create a query to insert a user:
                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.Parameters.AddWithValue("@username", username_txt.Text);
                    sqlCmd.Parameters.AddWithValue("@full_name", full_name_txt.Text);
                    sqlCmd.Parameters.AddWithValue("@email", email_address_txt.Text);
                    sqlCmd.Parameters.AddWithValue("@password", password_txt.Password);

                    int rowsAffected = sqlCmd.ExecuteNonQuery();

                    if (rowsAffected > 0) //If a row was affected, display a message and navigate to the login page:
                    {
                        MessageBox.Show("Thank you for registering with us!");

                        LoginWindow loginWindow = new LoginWindow();
                        loginWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        loginWindow.Owner = this; // Set the register window as the owner of the login window
                        loginWindow.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Registration failed! Please try again.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlCon.Close(); //Close the sql connection:
            }
        }

        //Function to check if any of the fields are empty in the form:
        private bool AreRequiredFieldsEmpty()
        {
            return string.IsNullOrEmpty(username_txt.Text) ||
                   string.IsNullOrEmpty(full_name_txt.Text) ||
                   string.IsNullOrEmpty(email_address_txt.Text) ||
                   string.IsNullOrEmpty(password_txt.Password);
        }
    }
}
