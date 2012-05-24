using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace Omega
{
    public class GameClient
    {
        public event ReceiveEventHandler Received;

        Socket socket;

        public GameClient()
        {
            // Initialize socket
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Connect(string ipAddress, int port)
        {
            IPAddress serverIPAddress = IPAddress.Parse(ipAddress);
            IPEndPoint serverIPEndPoint = new IPEndPoint(serverIPAddress, port);
            EndPoint serverEndPoint = (EndPoint)serverIPEndPoint;

            // Connect to server
            SocketAsyncEventArgs e = new SocketAsyncEventArgs();
            e.RemoteEndPoint = serverEndPoint;
            e.Completed += new EventHandler<SocketAsyncEventArgs>(OnConnect);
            socket.ConnectAsync(e);
        }

        void OnConnect(object state, SocketAsyncEventArgs e) 
        { 
            Receive();
        }

        public void Send(string format, params object[] args)
        {
            if (socket.Connected)
            {
                // Send message to server
                SocketAsyncEventArgs sendEventArgs = new SocketAsyncEventArgs();
                byte[] buffer = Encoding.UTF8.GetBytes(string.Format(format, args));
                sendEventArgs.RemoteEndPoint = socket.RemoteEndPoint;
                sendEventArgs.SetBuffer(buffer, 0, buffer.Length);
                sendEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnSend);
                socket.SendAsync(sendEventArgs);
            }
        }

        void OnSend(object sender, SocketAsyncEventArgs e) { }

        void Receive()
        {
            if (socket.Connected)
            {
                // Prepare to receive a message from the server
                SocketAsyncEventArgs receiveEventArgs = new SocketAsyncEventArgs();
                receiveEventArgs.RemoteEndPoint = socket.RemoteEndPoint;
                receiveEventArgs.SetBuffer(new Byte[1024], 0, 1024);
                receiveEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnReceive);
                socket.ReceiveAsync(receiveEventArgs);
            }
        }

        void OnReceive(object sender, SocketAsyncEventArgs e)
        {
            // Receive message from server as a string
            string message = Encoding.UTF8.GetString(e.Buffer, 0, e.BytesTransferred).TrimEnd('\0');

            if (Received != null)
            {
                // Invoke the delegate
                ReceiveEventArgs receiveEventArgs = new ReceiveEventArgs(message);
                Received(this, receiveEventArgs);
            }

            Receive();
        }

        public void Disconnect()
        {
            socket.Close();
        }
    }

    public delegate void ReceiveEventHandler(object sender, ReceiveEventArgs e);

    public class ReceiveEventArgs : EventArgs
    {
        public string Message;

        public ReceiveEventArgs(string message)
        {
            Message = message;
        }
    }
}
