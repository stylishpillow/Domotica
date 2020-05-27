using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;

namespace ProjectClient
{
    class HttpReq
    {
        public string GetHtml(string servo = "")
        {
            string uri = "http://192.168.2.17/"; // url van arduino
            string get = servo; //waarde voor get request
            uri += servo; // plak get request achter url

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);    //http request
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();  //http response
            StreamReader sr = new StreamReader(response.GetResponseStream());   //geen idee maar moeten we gebruiken 

            string html = sr.ReadToEnd();   //lees de webpagina
            sr.Close(); // sluit connectie
            return html; //geef de gehele html pagina terug
        }
    }
}
