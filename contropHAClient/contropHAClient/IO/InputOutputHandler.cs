using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace contropHAClient.IO
{
    class InputOutputHandler
    {
        public string read(NetworkStream stream)
        {
            try
            {
                byte[] buffer = new byte[100];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                return Encoding.ASCII.GetString(buffer, 0, bytesRead);
            }
            catch (IOException ex)
            {
                Console.WriteLine("Error while reading: " + ex.Message);
                return null;
            }
        }
        public void write(NetworkStream stream, string message)
        {
            try
            {
                byte[] sendBytes = Encoding.ASCII.GetBytes(message);
                stream.Write(sendBytes, 0, sendBytes.Length);
            }
            catch (IOException ex)
            {
                Console.WriteLine("Error while writing: " + ex.Message);
            }
        }
    }
}
