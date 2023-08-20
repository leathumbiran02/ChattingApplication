using System;
using System.IO;
using System.Text;

namespace ChatClient.Net.IO
{
    /*The PacketBuilder class allows us to append data to a memory stream
     * which we will be able to use to get the bytes to send to the server:
    */
    class PacketBuilder
    {
        //Import a MemoryStream with namespace _ms:
        MemoryStream _ms;

        //PacketBuilder constructor:
        public PacketBuilder()
        {
            //Instantiate to a new MemoryStream:
            _ms = new MemoryStream();
        }

        /*Three functions:
         * 1.) WriteOpCode - used as a flag for the server to interpret a packet 
         * in different ways depending on its opcode. For example opcode 1 could mean
         * that the server reads a string message.
         * 2.) WriteMessage
         * 3.) GetPacketBytes
         */

        //WriteOpCode function that passes in the parameter opcode of type byte:
        public void WriteOpCode(byte opcode)
        {
            //Write a byte:
            _ms.WriteByte(opcode);
        }

        //WriteMessage function to send messages and not just usernames:
        public void WriteMessage(string msg)
        {
            //Create a new variable of type var called msgLength, and store the length of the message received:
            var msgLength = msg.Length;
            //Get the bytes from the msgLength and append it to the memory stream:
            _ms.Write(BitConverter.GetBytes(msgLength));
            _ms.Write(Encoding.ASCII.GetBytes(msg));
        }

        public byte[] GetPacketBytes()
        {
            return _ms.ToArray();
        }
    }
}
