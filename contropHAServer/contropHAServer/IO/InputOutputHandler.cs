using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace contropHAServer.IO
{
    class InputOutputHandler
    {
        public string read(Socket stream)
        {
            byte[] buffer = new byte[100];
            int bytesReceived = stream.Receive(buffer);
            return Encoding.ASCII.GetString(buffer, 0, bytesReceived);
        }
        public void write(Socket stream, string s)
        {
            ASCIIEncoding asen = new ASCIIEncoding();
            byte[] sendBytes = asen.GetBytes(s);
            stream.Send(sendBytes);
        }
    }
}
