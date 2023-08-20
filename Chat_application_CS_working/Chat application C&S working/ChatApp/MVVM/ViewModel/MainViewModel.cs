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
namespace ChatClient.MVVM.ViewModel
{
    class MainViewModel
    {
        public ObservableCollection<UserModel> Users { get; set; }
        public ObservableCollection<string> Messages { get; set; }

        //Property of type RelayCommand called ConnectToServerCommand:
        public RelayCommand ConnectToServerCommand { get; set; }
        public RelayCommand SendMessageCommand { get; set; }

        public string Username { get; set; }
        public string Message { get; set; }

        //Create a private instance of the Server object called _server:
        private Server _server;

        //Main View Model constructor:
        public MainViewModel()
        {
            Users = new ObservableCollection<UserModel>();
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

            //THIS LINE OF CODE IS GIVING ISSUES:
            SendMessageCommand = new RelayCommand(o => _server.SendMessageToServer(Message), o => !string.IsNullOrEmpty(Message));
        }

        //Action 3: user disconnected
        private void RemoveUser()
        {
            var uid = _server.PacketReader.ReadMessage();
            var user = Users.Where(x => x.UID == uid).FirstOrDefault();
            Application.Current.Dispatcher.Invoke(() => Users.Remove(user));
        }

        //Action 2: user sent a message:
        private void MessageReceived()
        {
            var msg = _server.PacketReader.ReadMessage();
            Application.Current.Dispatcher.Invoke(() => Messages.Add(msg));
        }

        //Action 1: user connected
        private void UserConnected()
        {
            //Create a new instance of the UserModel class called user:
            var user = new UserModel
            {
                Username = _server.PacketReader.ReadMessage(),
                UID = _server.PacketReader.ReadMessage(),
            };

            if (!Users.Any(x => x.UID == user.UID))
            {
                Application.Current.Dispatcher.Invoke(() => Users.Add(user));
            }
        }



    }
}
