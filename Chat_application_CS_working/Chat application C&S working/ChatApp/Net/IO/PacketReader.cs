using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace ChatClient.Net.IO
{
    //PacketReader class that inherits from a BinaryReader:
    class PacketReader : BinaryReader
    {
        private NetworkStream _ns;

        //PacketReader constructor that takes NetworkStream (ns) as its parameter, it then passes ns into a base class:
        public PacketReader(NetworkStream ns) : base(ns)
        {
            _ns = ns;
        }

        //ReadMessage function to read messages:
        public string ReadMessage()
        {
            //Temporary buffer:
            byte[] msgBuffer;
            //Length is read int 32 which is the size of the actual length of the message that is being sent:
            var length = ReadInt32();
            //msgBuffer will read whatever length we have read from the client:
            msgBuffer = new byte[length];
            //_ns.Read lets us read into the message buffer which starts at an offset of 0 and we want to read the length of the actual packet:
            _ns.Read(msgBuffer, 0, length);

            //We get the message in the form of a string:
            var msg = Encoding.ASCII.GetString(msgBuffer);

            //Return the message:
            return msg;
        }

    }
}
