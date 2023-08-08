using contropHAClient.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace contropHAClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static TcpClient client;
        static NetworkStream stream;
        static InputOutputHandler inputOutputHandler;
        bool isConnected = false;
        static CancellationTokenSource cancellationTokenSource; 
        public MainWindow()
        {
            InitializeComponent();
            inputOutputHandler = new InputOutputHandler();
           
        }


        private async void setNewTemp(object sender, RoutedEventArgs e)
        {
            double temperatue = Convert.ToDouble(temperatureTextBox.Text);
            double duration = Convert.ToDouble(durationTextBox.Text);
            double oldTemperatur = Convert.ToDouble(currentTempTextBox.Text);
            await SetTemperatureWithTimer(temperatue, duration, oldTemperatur);

        }

        // Change temperature and wait for the specified duration
        //Then Revert to the old temperature and update UI
        private async Task SetTemperatureWithTimer(double newTemperature, double duration,double currentTemperature)
        {

            inputOutputHandler.write(stream, "POSTtemp" + $"|{newTemperature}");
            try
            {
                await Task.Delay((int)duration * 1000, cancellationTokenSource.Token);

            }
            catch
            {
                return;
            }

            inputOutputHandler.write(stream, "POSTtemp" + $"|{currentTemperature}");

        }

        //Stop the connection with the server 
        private void stopConnection(object sender, RoutedEventArgs e)
        {
            inputOutputHandler.write(stream, "CloseConnection");
            connectionLabel.Foreground = Brushes.Red;
            connectionLabel.Content = "Disconnect";
            cancellationTokenSource.Cancel();
            ResetAllBoxes();
            isConnected = false;

        }

        // Establish a connection to the server using the same IP address and port.Obtain a stream.
        // If we receive an acknowledgment from the server confirming the connection attempt,
        // we send the acknowledgment back in accordance with the TCP/IP protocol.
        // Once a connection is established, retrieve the current temperature,
        // enable components that were previously disabled, and initiate a background task
        // to monitor updates in temperature.
        private void connect(object sender, RoutedEventArgs e)
        {
            try
            {
                client = new TcpClient();
                client.Connect("127.0.0.1", 8001);
                stream = client.GetStream();
                inputOutputHandler.write(stream, "Establish connection");
                string s = inputOutputHandler.read(stream);
                if (s == "The string was recieved by the server.")//connection was established and it is safe to send messages
                {
                    inputOutputHandler.write(stream, "Acknowledgement accepted");
                    cancellationTokenSource = new CancellationTokenSource();
                    isConnected = true;
                    connectionLabel.Foreground = Brushes.Green;
                    connectionLabel.Content = "Connected";
                    EnabledAllComponents();
                    DisplayCurrentTemp();
                    // Start a background task to listen for updates
                    Task.Run(() => ListenForUpdates());
                }
            }
            catch
            {
                MessageBox.Show("No connection to the server.");
                return;
            }

 

        }

        //Display the current temperature. reads it from the server.
        private void DisplayCurrentTemp()
        {
            inputOutputHandler.write(stream, "GETtemp");
            string dataReceived = inputOutputHandler.read(stream);
            currentTempTextBox.Text = dataReceived;
        }

        //while disconnect reset all boxes and and disable all buttons except the connect button
        private void ResetAllBoxes()
        {
            currentTempTextBox.Text = "";
            durationTextBox.Text = "";
            temperatureTextBox.Text = "";
            connectButton.IsEnabled = true;
            durationTextBox.IsEnabled = false;
            temperatureTextBox.IsEnabled = false;
            diconnectButton.IsEnabled = false;
            setTempButton.IsEnabled = false;
        }

        //Once connected, allow the usage of this application by enable buttons.
        private void EnabledAllComponents()
        {
            durationTextBox.IsEnabled = true;
            temperatureTextBox.IsEnabled = true;
            connectButton.IsEnabled = false;
            diconnectButton.IsEnabled = true;
            setTempButton.IsEnabled = true;
        }

        private void ListenForUpdates()
        {
            while (isConnected) // Continue listening as long as connected
            {
                string message = inputOutputHandler.read(stream); 
                ProcessMessage(message); // Handle the received message
            }
        }

        private void ProcessMessage(string message)
        {
            if (message.StartsWith("TemperatureUpdated"))
            {
                double newTemperature = double.Parse(message.Substring("TemperatureUpdated".Length));
                Dispatcher.Invoke(() => UpdateUIWithNewTemperature(newTemperature));//updates the ui
            }
        }

        private void UpdateUIWithNewTemperature(double temperature)
        {
            // Update the UI with the new temperature value
            currentTempTextBox.Text = temperature.ToString();
        }
    }
}
