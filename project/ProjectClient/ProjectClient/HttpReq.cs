using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;

namespace ProjectClient
{
    class HttpReq
    {
        private HttpWebResponse webResponse;

        /// <summary>
        /// Connect to website and set webresponse 
        /// if servo is set set servo value in url
        /// </summary>
        /// <param name="servo"></param>
        private void ConnectToWebsite(int servo = -1)
        {
            string url = "http://192.168.2.17/"; // url van arduino
            if (servo > -1)
            {
                url += "?p7=" + servo; // plak get request achter url
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);    //http request
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            webResponse = response;// set webrequest 

        }
        /// <summary>
        /// get all the html from the webpage
        /// </summary>
        /// <returns>all the html from the website</returns>
        private string GetHtml()
        { 
            StreamReader sr = new StreamReader(webResponse.GetResponseStream());

            string html = sr.ReadToEnd();   
            sr.Close(); 
            return html; 
        }
        /// <summary>
        /// Connects to website and get all the html. 
        /// search for the <b> tag and get value after the tag (servo value)
        /// </summary>
        /// <returns>servo value</returns>
        public string GetServoValue()
        {
            ConnectToWebsite();
            string htmlPage = GetHtml();
            string ServoValue = htmlPage.Substring(htmlPage.IndexOf("<b>") + 3, 3);

            return ServoValue;
        }

        /// <summary>
        /// Connects to website and get all the html. 
        /// search for the <i> tag and get value after the tag (Sensor value)
        /// </summary>
        /// <returns>Sensor value</returns>
        public string GetSensorValue()
        {
            ConnectToWebsite();
            string htmlPage = GetHtml();
            string sensorValue = htmlPage.Substring(htmlPage.IndexOf("<i>") + 3, 3);

            return sensorValue;
        }

        /// <summary>
        /// Set the servo value in ConnectToWebsite (for the get request)
        /// </summary>
        /// <param name="servoValue"></param>
        public void SetServoValue(int servoValue)
        {
            ConnectToWebsite(servoValue);
        }
       
    }
}
