using ChatClient.Net.IO;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.IO;

namespace ChatClient.Net
{
    class Server
    {
        //Create TCP client:
        TcpClient _client;

        //Create an instance of the PacketReader class:
        public PacketReader PacketReader;

        //EVENTS:
        //Action 1: user connects
        public event Action connectedEvent;
        //Action 2: user sends message
        public event Action msgReceivedEvent;
        //Action 3: user disconnects
        public event Action userDisconnectEvent;

        //Create a boolean to track whether the packets are being read or not:
        private bool _isReadingPackets = false;

        //Server constructor:
        public Server()
        {
            //Create a new instance of the TCP client:
            _client = new TcpClient();
        }

        //Function that will connect to the server by passing in a user's username:
        public void ConnectToServer(string username)
        {
            //If the client is not connected to the server execute the following:
            if (!_client.Connected)
            {
                _client.Connect("127.0.0.1", 7891); //Connect to the server on ip 127.0.0.1 and port number 7891:

                //Use packet builder to send data to the server:
                PacketReader = new PacketReader(_client.GetStream());

                //If the string is not null or empty (it has a value such as the username) execute the following:
                if (!string.IsNullOrEmpty(username))
                {
                    //Create a instance of the PacketBuilder class called connectPacket:
                    var connectPacket = new PacketBuilder();
                    connectPacket.WriteOpCode(0);
                    connectPacket.WriteMessage(username);
                    //Send the packet:
                    _client.Client.Send(connectPacket.GetPacketBytes());
                }

                //Read/Interpret the packets by calling the StartReadingPackets function:
                StartReadingPackets();
            }
        }

        //Function to read/interpret the packets:
        //This is being offloaded to another thread, as if we don't this
        //can deadlock the application:
        private void ReadPackets()
        {
            //Create a new task and run it:
            Task.Run(() =>
            {
                //infinite loop, will run as long as it's true:
                while (_isReadingPackets)
                {
                    try
                    {
                        //Read the opcode:
                        var opcode = PacketReader.ReadByte();
                        switch (opcode)
                        {
                            //the ? is used to check if its not null:
                            case 1: //opcode = 1 then connect the user:
                                connectedEvent?.Invoke();
                                break;

                            case 5: //opcode = 5 then user sent and received messages:
                                msgReceivedEvent?.Invoke();
                                break;

                            case 10: //opcode = 10 then user disconnected:
                                userDisconnectEvent?.Invoke();
                                break;

                            default: //Default behaviour if the opcode is not one of the cases above:
                                Console.WriteLine("Waiting for an opcode...");
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        //Save the exception and runtime errors to a file called exception_log.txt:
                        logException(ex);
                    }
                }
            });
        }

        //Function to send messages to the server by passing in the message as a parameter:
        public void SendMessageToServer(string message)
        {
            try
            {
                //Create a new instance of the PacketBuilder class called messagePacket:
                var messagePacket = new PacketBuilder();
                //Specify it's opcode as 5:
                messagePacket.WriteOpCode(5);
                //Write the message by passing in the messge that was entered:
                messagePacket.WriteMessage(message);
                //Send the packet:
                _client.Client.Send(messagePacket.GetPacketBytes());
            }
            catch (Exception ex) //Save the exception and runtime errors to a file called exception_log.txt:
            {
                logException(ex);
            }
        }

        //Function to save the exceptions and runtime errors from the server to a text file:
        static void logException(Exception ex)
        {
            //File path for the log file:
            string logFilePath = "exception_log.txt";

            //Format to store the messages in:
            string logEntry = $"[{DateTime.Now}] - Exception: {ex.Message}\n{ex.StackTrace}\n";

            //Appending the new entries to the text file:
            using (StreamWriter writer = File.AppendText(logFilePath))
            {
                writer.WriteLine(logEntry);
            }
        }

        //Function to test for exceptions and to see if they are being saved to a text file:
        public void ThrowExceptionForTesting()
        {
            throw new Exception("This is a test exception.");
        }

        //Function to disconnect a client from the server:
        public void Disconnect()
        {
            if (_client.Connected)//If the client is connected, close the socket connection:
            {
                StopReadingPackets(); //Stop reading the packets before disconnecting the user:
                _client.Close(); //Close the socket connection:
            }
        }

        //Function to start reading packets:
        public void StartReadingPackets()
        {
            if (!_isReadingPackets)
            {
                _isReadingPackets = true;
                ReadPackets();
            }
        }

        //Function to stop reading packets:
        public void StopReadingPackets()
        {
            _isReadingPackets = false;
        }
    }
}
