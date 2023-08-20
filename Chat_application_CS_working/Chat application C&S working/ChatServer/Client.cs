using ChatServer.Net.IO;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ChatServer
{
    //Client class:
    class Client
    {
        //Username is stored in a string:
        public string Username { get; set; }
        //Globally unique identifier (GUID), this is the ID of the application
        //in case we want to specify which client will be able to do which tasks 
        //later on:
        public Guid UID { get; set; }
        //Keep track of the TCP client socket object here:
        public TcpClient ClientSocket { get; set; }

        //Define an instance of the PacketReader class called _packetReader:
        PacketReader _packetReader;

        public Client(TcpClient client)
        {
            //Set ClientSocket to be client:
            ClientSocket = client;
            //Generate a new user id whenever the client is instantiated:
            UID = Guid.NewGuid();
            //Create a new instance of the PacketReader class and pass in the network stream:
            _packetReader = new PacketReader(ClientSocket.GetStream());

            var opcode = _packetReader.ReadByte();

            Username = _packetReader.ReadMessage();

            //Broadcast this into the server console saying that a user has connected:
            Console.WriteLine($"[{DateTime.Now}]: {Username} has joined the chat!");

            Task.Run(() => Process());

        }

        void Process()
        {
            while (true)
            {
                try
                {
                    var opcode = _packetReader.ReadByte();
                    switch (opcode)
                    {
                        case 5:
                            var msg = _packetReader.ReadMessage();
                            Console.WriteLine($"[{DateTime.Now}]: {Username}: {msg}");
                            Program.BroadcastMessage($"[{DateTime.Now}]: [{Username}]: {msg}");
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception)
                {
                    //Console.WriteLine($"[{UID.ToString()}]: Disconnected!");
                    Console.WriteLine($"[{DateTime.Now}]: {Username} has left the chat!");
                    Program.BroadcastDisconnect(UID.ToString());
                    ClientSocket.Close();
                    break;
                }
            }
        }
    }
}
