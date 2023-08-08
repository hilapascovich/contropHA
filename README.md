# contropHA
## ControlHA Client-Server Application
The ControlHA Client-Server Application is a software solution that allows users to control and monitor temperature settings remotely.
It consists of two main components: a client application and a server application. 
The client application communicates with the server over a Ethernet(TCP/IP) connection to set and monitor temperature values.

### Features
#### Temperature Control:
The client application enables users to set a new temperature value along with a duration for which the new temperature should be maintained. After the specified duration, the temperature reverts to the previous value.

#### Connection Management:
The client application can establish a connection with the server application over a TCP/IP connection. Once the connection is established, the client can send temperature control commands and receive updates from the server.

#### Real-Time Temperature Display:
The client application displays the current temperature received from the server in real-time, providing users with up-to-date information.

### Getting Started
#### Server Setup:
Compile and run the server application (contropHAServer project) on the server machine. The server listens for incoming client connections and manages temperature updates.

#### Client Setup: 
Compile and run the client application (contropHAClient project) on the client machine. The client can establish a connection to the server, set temperature values, and receive temperature updates.

#### Temperature Control:
Use the client application to set a new temperature value and duration. The application will send the temperature control command to the server, and the server will update the temperature accordingly.

#### Real-Time Monitoring:
The client application displays the current temperature received from the server. The temperature updates are shown in real-time on the user interface.
#### Stopping the Application
The "Stop" function is a crucial feature that terminates all running tasks and ensures a clean exit from the ControlHA Client-Server Application. 
### Project Structure
#### contropHAServer: 
Contains the server application code. It listens for client connections, handles temperature updates, and manages communication with clients.

#### contropHAClient: 
Contains the client application code. It allows users to set temperature values, establish connections, and monitor temperature updates.

#### IO: 
Contains the InputOutputHandler class used for reading and writing data over network streams.

### Technologies Used
Programming Language: C#
Communication Protocol: TCP/IP
User Interface: WPF (Windows Presentation Foundation)

### Assumptions
The following assumptions were made in the development of this application:

##### Temperature Reversion:
After the specified time duration for a temperature change has elapsed, it is assumed that the temperature reverts to its original value.

##### Server Notifications: 
It is assumed that the server notifies the client about any temperature changes. Whenever a temperature change occurs, the server informs the client so that the displayed temperature remains up-to-date.

##### Initial Server Temperature: 
Additionally, it is assumed that the server starts with an initial temperature value. This value serves as the starting point for temperature adjustments and updates.
