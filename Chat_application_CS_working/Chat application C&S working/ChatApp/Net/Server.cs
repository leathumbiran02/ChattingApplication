using ChatClient.Net.IO;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ChatClient.Net
{
    class Server
    {
        //Create TCP client:
        TcpClient _client;
        public PacketReader PacketReader;

        public event Action connectedEvent;
        public event Action msgReceivedEvent;
        public event Action userDisconnectEvent;

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
                PacketReader = new PacketReader(_client.GetStream());

                if (!string.IsNullOrEmpty(username))
                {
                    var connectPacket = new PacketBuilder();
                    connectPacket.WriteOpCode(0);
                    connectPacket.WriteMessage(username);
                    _client.Client.Send(connectPacket.GetPacketBytes());
                }
                ReadPackets();
            }
        }

        private void ReadPackets()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    var opcode = PacketReader.ReadByte();
                    switch (opcode)
                    {
                        case 1:
                            connectedEvent?.Invoke();
                            break;

                        case 5:
                            msgReceivedEvent?.Invoke();
                            break;

                        case 10:
                            userDisconnectEvent?.Invoke();
                            break;

                        default:
                            Console.WriteLine("ah yes..");
                            break;
                    }
                }
            });
        }

        public void SendMessageToServer(string message)
        {
            var messagePacket = new PacketBuilder();
            messagePacket.WriteOpCode(5);
            messagePacket.WriteMessage(message);
            _client.Client.Send(messagePacket.GetPacketBytes());
        }
    }
}
