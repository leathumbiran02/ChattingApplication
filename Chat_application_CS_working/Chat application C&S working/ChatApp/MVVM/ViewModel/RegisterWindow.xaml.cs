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

        private void go_to_login(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private void submit_btn_click(object sender, RoutedEventArgs e)
        {
            SqlConnection sqlCon = new SqlConnection(@"Data Source=localhost; Initial Catalog=ChattingAppDB; Integrated Security=True"); //Connection string for SQL Server:

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
                        loginWindow.Show();
                        this.Close();
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
    }
}
