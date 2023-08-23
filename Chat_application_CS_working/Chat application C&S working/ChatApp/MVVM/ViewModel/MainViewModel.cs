using ChatClient.MVVM.Core;
using ChatClient.MVVM.Model;
using ChatClient.Net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

//View model for the client to connect to the server:
namespace ChatApp
{
    public class MainViewModel
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

        //Main View Model constructor:
        public MainViewModel(string username)
        {
            Username = username; //Store the logged in Username in username:

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
            //Action 2: user sent message
            _server.msgReceivedEvent += MessageReceived;
            //Action 3: user disconnected
            _server.userDisconnectEvent += RemoveUser;

            //Create a new instance of the ConnectToServerCommand where the object (username) goes into _server.ConnectToServer:
            //A relay command is used to disable the connect button if the username is empty:
            ConnectToServerCommand = new RelayCommand(o => _server.ConnectToServer(Username), o => !string.IsNullOrEmpty(Username));

            //THIS LINE OF CODE IS GIVING ISSUES, THE TEXT BOX IS NOT CLEARING AFTER SENDING A MESSAGE:
            SendMessageCommand = new RelayCommand(o => _server.SendMessageToServer(Message), o => !string.IsNullOrEmpty(Message));
        }

        //Action 3: user disconnected
        private void RemoveUser()
        {
            //Find the UID and then remove the user from the list of connected list of clients:
            var uid = _server.PacketReader.ReadMessage();
            var user = Users.Where(x => x.UID == uid).FirstOrDefault();
            Application.Current.Dispatcher.Invoke(() => Users.Remove(user));
        }

        //Action 2: user sent a message:
        private void MessageReceived()
        {
            //Read the data that was sent:
            var msg = _server.PacketReader.ReadMessage();
            //Append the message to the messages collection:
            Application.Current.Dispatcher.Invoke(() => Messages.Add(msg));
        }

        //Action 1: user connected
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

        public void Logout()
        {
            //Disconnect from the server and perform cleanup:
            _server.Disconnect();

            // Close all windows except the main window
            foreach (Window window in Application.Current.Windows)
            {
                if (window != Application.Current.MainWindow)
                {
                    window.Close();
                }
            }

            // After performing necessary actions, navigate to the RegisterWindow
            var registerWindow = new RegisterWindow();
            registerWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            registerWindow.Show();
        }
    }
}
