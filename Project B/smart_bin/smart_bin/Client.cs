using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace smart_bin
{
    class Client
    {
        public Socket open(string ipaddress, int portnr)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ip = IPAddress.Parse(ipaddress);
            IPEndPoint endpoint = new IPEndPoint(ip, portnr);
            socket.Connect(endpoint);
            return socket;
        }

        public void write(Socket socket, string text)
        {
            socket.Send(Encoding.ASCII.GetBytes(text));
        }
        public string read(Socket socket)
        {
            byte[] bytes = new byte[4096];
            int bytesRec = socket.Receive(bytes);
            string text = Encoding.ASCII.GetString(bytes, 0, bytesRec);
            return text;
        }

        public void close(Socket socket)
        {
            socket.Close();
        }

        public string ask(string ipaddres, int portnr, string message)
        {
            Socket s = open(ipaddres, portnr);
            write(s, message);
            string reply = read(s);
            close(s);
            return reply;
        }



    }
}
