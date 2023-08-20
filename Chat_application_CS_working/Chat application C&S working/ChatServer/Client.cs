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

            //Start the process by off-load it to a different thread:
            Task.Run(() => Process());

        }

        //Function to process all of the packets that is received:
        void Process()
        {
            //Infinite loop that will keep running while it is true:
            while (true)
            {
                //Try catch loop for exception handling:
                try
                {
                    //Read the opcode sent:
                    var opcode = _packetReader.ReadByte();
        
                    switch (opcode)
                    {
                        case 5: //opcode = 5 then display the message on the console for the server and in the client with the date, time, name of the user and the message that they sent:
                            var msg = _packetReader.ReadMessage();
                            Console.WriteLine($"[{DateTime.Now}]: {Username}: {msg}");
                            Program.BroadcastMessage($"[{DateTime.Now}]: [{Username}]: {msg}");
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception) //Can be used to specify network exceptions or if a user connects and disconnects from the server:
                {
                    //Console.WriteLine($"[{UID.ToString()}]: Disconnected!");
                    Console.WriteLine($"[{DateTime.Now}]: {Username} has left the chat!");
                    Program.BroadcastDisconnect(UID.ToString());
                    ClientSocket.Close(); //Close the client socket object:
                    break;
                }
            }
        }
    }
}
