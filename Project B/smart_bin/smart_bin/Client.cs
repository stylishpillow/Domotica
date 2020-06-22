using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace smart_bin
{
    class Client
    {
        /// <summary>
        /// Open connection with arduino
        /// </summary>
        /// <param name="ipaddress">Server ipaddres</param>
        /// <param name="portnr">Server portnr</param>
        /// <returns>Socket connection</returns>
        public Socket open(string ipaddress, int portnr)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ip = IPAddress.Parse(ipaddress);
            IPEndPoint endpoint = new IPEndPoint(ip, portnr);
            socket.Connect(endpoint);
            return socket;
        }

        /// <summary>
        ///  Send text to arduino
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="text"></param>
        public void write(Socket socket, string text)
        {
            socket.Send(Encoding.ASCII.GetBytes(text));
        }

        /// <summary>
        /// Get response from server (receive message)
        /// </summary>
        /// <param name="socket"></param>
        /// <returns>response from arduino</returns>
        public string read(Socket socket)
        {
            byte[] bytes = new byte[4096];
            int bytesRec = socket.Receive(bytes);
            string text = Encoding.ASCII.GetString(bytes, 0, bytesRec);
            return text;
        }

        /// <summary>
        /// close connection
        /// </summary>
        /// <param name="socket"></param>
        public void close(Socket socket)
        {
            socket.Close();
        }

        /// <summary>
        /// open connection, write message, receive response and close connection
        /// ask response from server
        /// </summary>
        /// <param name="ipaddres">arduino(server) ipaddress</param>
        /// <param name="portnr">portnr (arduino has port nr 80)</param>
        /// <param name="message">message to arduino</param>
        /// <returns>message from arduino</returns>
        public string ask(string ipaddres, int portnr, string message)
        {
            try
            {
                Socket s = open(ipaddres, portnr);
                write(s, message);
                string reply = read(s);
                close(s);
                return reply;
            } catch(Exception)
            {
                return "Error";
            }
        }
    }
}
