using ChatServer.Net.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.IO;

/* 
 We use a packet reader on the client and a packet builder on the server:
 */
namespace ChatServer
{
    class Program
    {
        //Create a new static list of type Client called _users:
        static List<Client> _users;
        //Create a TCP listener called _listener:
        static TcpListener _listener;

        static void Main(string[] args)
        {
            //Used to find the folder that the application is running from:
            //Console.WriteLine($"Working Directory: {Directory.GetCurrentDirectory()}");

            //Create a new instance of the list of type client called _users:
            _users = new List<Client>();
            //Create a new instance of the TCP listener class by passing in the ip address and port number for it to listen on:
            _listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 7891);
            //Start the listener:
            _listener.Start();

            //While this is true execute the following:
            //Once we have accepted the first client and processed the packets,
            //We will accept the next client:
            while (true)
            {
                //Create a new instance of the Client class that accepts the TCP CLient:
                var client = new Client(_listener.AcceptTcpClient());

                //Add new clients:
                _users.Add(client);

                //Broadcast the connection to all clients on the server:
                //This will be used to update their local list of users:
                BroadcastConnection();
            }
        }

        //Broad Cast the connection of all clients to other clients:
        static void BroadcastConnection()
        {
            //foreach user that is connected, we send out a packet to each user to notify them that another user has connected:
            foreach (var user in _users)
            {
                foreach (var usr in _users)
                {
                    //Create a new instance of the PacketBulder class called broadcastPacket:
                    var broadcastPacket = new PacketBuilder();

                    //Building the packet:
                    //This packet will send an op code of 1:
                    broadcastPacket.WriteOpCode(1);
                    //Write a message that passes in a string containing the username of the person:
                    broadcastPacket.WriteMessage(usr.Username);
                    //Write a message that passes in a string containing the UID of the client that connected to the server:
                    broadcastPacket.WriteMessage(usr.UID.ToString());
                    //Send the packet:
                    user.ClientSocket.Client.Send(broadcastPacket.GetPacketBytes());
                }
            }
        }

        //Broadcasts the message to all connected clients:
        public static void BroadcastMessage(string message)
        {
            foreach (var user in _users)
            {
                //Construct a packet using opcode 5 and send it to all connected clients:
                var msgPacket = new PacketBuilder();
                msgPacket.WriteOpCode(5);
                msgPacket.WriteMessage(message);
                user.ClientSocket.Client.Send(msgPacket.GetPacketBytes());
            }
        }

        //Broadcasts a message saying that a client has left the server based on their UID:
        public static void BroadcastDisconnect(string uid)
        {
            var disconnectedUser = _users.Where(x => x.UID.ToString() == uid).FirstOrDefault();
            _users.Remove(disconnectedUser); //Remove the user:

            foreach (var user in _users)
            {
                //Build a packet, specify it's opcode as 10 and send it to all connected clients:
                var broadcastPacket = new PacketBuilder();
                broadcastPacket.WriteOpCode(10);
                broadcastPacket.WriteMessage(uid);
                user.ClientSocket.Client.Send(broadcastPacket.GetPacketBytes());
            }

            //Broadcast the username of the person that left:
            BroadcastMessage($"[{DateTime.Now}]: {disconnectedUser.Username} has left the chat!");
        }
    }
}
