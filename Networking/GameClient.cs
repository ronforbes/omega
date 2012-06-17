using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Omega
{
    public class GameClient
    {
        public event ReceiveEventHandler Received;

        Socket socket;
        IPEndPoint ipEndPoint;

        public GameClient(string ipAddress, int port)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            ipEndPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
        }

        public void Send(string format, params object[] args)
        {
            // Send message to server
            SocketAsyncEventArgs sendEventArgs = new SocketAsyncEventArgs();
            byte[] buffer = Encoding.UTF8.GetBytes(string.Format(format, args));
            sendEventArgs.RemoteEndPoint = ipEndPoint;
            sendEventArgs.SetBuffer(buffer, 0, buffer.Length);
            sendEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnSend);
            socket.SendAsync(sendEventArgs);
        }

        void OnSend(object sender, SocketAsyncEventArgs e) { }

        void Receive()
        {
            // Prepare to receive a message from the server
            SocketAsyncEventArgs receiveEventArgs = new SocketAsyncEventArgs();
            receiveEventArgs.RemoteEndPoint = ipEndPoint;
            receiveEventArgs.SetBuffer(new Byte[1024], 0, 1024);
            receiveEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnReceive);
            socket.ReceiveAsync(receiveEventArgs);
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
