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
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        //Create an observable collection of type UserModel called Users:
        public ObservableCollection<UserModel> Users { get; set; }
        public ObservableCollection<string> Messages { get; set; }

        //Property of type RelayCommand called ConnectToServerCommand:
        public RelayCommand ConnectToServerCommand { get; set; }
        //Property of type RelayComand called SendMessageCommand:
        public RelayCommand SendMessageCommand { get; set; }

        //Properties for Username and Message:
        public string Username { get; set; }
        public string Message { get; set; }

        //Create a private instance of the Server object called _server:
        private Server _server;

        public LoginWindow()
        {
            InitializeComponent();

            //Create an instance of the observable collection of type UserModel called Users:
            Users = new ObservableCollection<UserModel>();

            //Create an instance of the observable collection of type string called Messages:
            Messages = new ObservableCollection<string>();

            //Create a new instance of the imported server object and call it _server:
            _server = new Server();

            //EVENTS:
            // += is used to attach an event handler:
            //Action 1: user connected
            _server.connectedEvent += UserConnected;

            //Create a new instance of the ConnectToServerCommand where the object (username) goes into _server.ConnectToServer:
            //A relay command is used to disable the connect button if the username is empty:
            ConnectToServerCommand = new RelayCommand(o => _server.ConnectToServer(Username), o => !string.IsNullOrEmpty(Username));

            //THIS LINE OF CODE IS GIVING ISSUES, THE TEXT BOX IS NOT CLEARING AFTER SENDING A MESSAGE:
            SendMessageCommand = new RelayCommand(o => _server.SendMessageToServer(Message), o => !string.IsNullOrEmpty(Message));
        }

        private void Submit_btn_click(object sender, RoutedEventArgs e)
        {
            SqlConnection sqlCon = new SqlConnection(@"Data Source=localhost; Initial Catalog=ChattingAppDB; Integrated Security=True"); //Connection string for SQL Server:

            try{
                if(sqlCon.State == ConnectionState.Closed) //If the connection to the database is closed execute the following:
                {
                    sqlCon.Open(); //Open the SQL connection:
                    String query = "SELECT COUNT(1) FROM users_table WHERE username=@username AND password=@password"; //Create a query to count a user with a matching username and password:
                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon); 
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.Parameters.AddWithValue("@username", username_txt.Text);
                    sqlCmd.Parameters.AddWithValue("@password", password_txt.Password);
                    int count = Convert.ToInt32(sqlCmd.ExecuteScalar()); //Get the count and convert to int 32:

                    if(count == 1) //If count is equal to 1, open the chatting application window:
                    {
                        MessageBox.Show("You are being logged in...");

                        MainWindow mainWindow = new MainWindow(username_txt.Text);
                        mainWindow.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Invalid username or password! Please try again.");
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlCon.Close(); //Close the sql connection:
            }
        }

        private void UserConnected()
        {
            //Create a new instance of the UserModel class called user:
            var user = new UserModel
            {
                //Read the new user's username and UID using PacketReader:
                Username = _server.PacketReader.ReadMessage(),
                UID = _server.PacketReader.ReadMessage(),
            };

            //Append the new user to the collection of existing users on the server:
            if (!Users.Any(x => x.UID == user.UID))
            {
                //Use dispatcher to add the user to the collection:
                Application.Current.Dispatcher.Invoke(() => Users.Add(user));
            }
        }
    }
}
