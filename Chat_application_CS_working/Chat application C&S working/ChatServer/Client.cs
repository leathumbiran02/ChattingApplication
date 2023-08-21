using ChatServer.Net.IO;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.IO;

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
            try
            {
                while (ClientSocket.Connected)
                {
                    var opcode = _packetReader.ReadByte();

                    switch (opcode)
                    {
                        case 5:
                            var msg = _packetReader.ReadMessage();

                            logMessage($"{Username}: {msg}");

                            Console.WriteLine($"[{DateTime.Now}]: {Username}: {msg}");
                            Program.BroadcastMessage($"[{DateTime.Now}]: [{Username}]: {msg}");

                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                // Disconnect and clean up after the loop ends
                Console.WriteLine($"[{DateTime.Now}]: {Username} has left the chat!");
                Program.BroadcastDisconnect(UID.ToString());
                ClientSocket.Close();
            }
        }

        static void logMessage(string message)
        {
            string logFilePath = "conversation_log.txt";

            string logEntry = $"[{DateTime.Now}] {message}";

            using (StreamWriter writer = File.AppendText(logFilePath))
            {
                writer.WriteLine(logEntry);
            }
        }

    }
}
