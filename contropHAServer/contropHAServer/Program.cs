using contropHAServer.IO;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace contropHAServer
{
    class Program
    {

        static void Main(string[] args)
        {
            Random rnd = new Random();
            double temperature = Math.Round(rnd.NextDouble() * 100,2);//temperature is set randomly
            double defauleTemp = temperature;
             object lockObj = new object();
            bool isConnected = false;
            InputOutputHandler inputOutputHandler = new InputOutputHandler();
            try
            {
                IPAddress ipAd = IPAddress.Parse("127.0.0.1"); // use local m/c IP address, and use the same at the client
                TcpListener server = new TcpListener(ipAd, 8001);//Initializes the Listener
                server.Start();//Start Listeneting at the specified port
 
                Console.WriteLine("The server is running at port 8001...");
                Console.WriteLine("Waiting for a connection.....");

              waitForSocket:  Socket s = server.AcceptSocket();
                Console.WriteLine("Connection accepted from " + s.RemoteEndPoint);//get connection request
                isConnected = true;
                Console.WriteLine("Recieved from client"+$"{inputOutputHandler.read(s)}");

                inputOutputHandler.write(s,"The string was recieved by the server.");//sent acknowledgment
                Console.WriteLine("Sent Acknowledgement");

                if(inputOutputHandler.read(s)== "Acknowledgement accepted")
                {
                    Console.WriteLine("Recieved from client Acknowledgement accepted");
                    while (isConnected)
                    {
                        string[] dataReceived = inputOutputHandler.read(s).Split("|");
                        switch (dataReceived[0])
                        {
                            case "GETtemp"://get the current temperature
                                inputOutputHandler.write(s, temperature.ToString());
                                break;

                            case "POSTtemp"://change temperature and update the client that the temperature changed
                                lock(lockObj)//critical section 
                                {
                                    temperature = (Convert.ToDouble(dataReceived[1]));
                                }
                                inputOutputHandler.write(s, "TemperatureUpdated"+temperature.ToString());
                                break;

                            case "CloseConnection"://close client socket and return to original temperature
                                s.Close();
                                isConnected = false;
                                Console.WriteLine("Client closed the connection");
                                lock (lockObj)//critical section 
                                {
                                    temperature = defauleTemp;
                                }
                                break;
                        }
                    }
              
                }

                goto waitForSocket;//wait for another connection to be open

            }
            catch (Exception e)
            {
                Console.WriteLine("Error..... " + e.StackTrace);
            }
        }

    }
}
