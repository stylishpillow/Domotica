using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace ProjectClient
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        HttpReq httpreq = new HttpReq(); //maak nieuw objectje
        public MainPage()
        {
            InitializeComponent();

            //htmlData.Text = httpreq.GetHtml();
            string test = httpreq.GetHtml(); // roep de functie aan om de htmlpagina op te halen
            string teststring = test.Substring(test.IndexOf("<b>")+3, 3); // haal de sensor waarde van de analogesensor uit de webpagina ( meer om te testen even nouuu)
            htmlData.Text = teststring; // flikker de waarde naar je tellie 

            servo.Value = Convert.ToInt32(teststring);

            string sensorValue = test.Substring(test.IndexOf("<i>")+3, 3);// haalt de sensor waarde op uit de webpagina
            htmlData2.Text = sensorValue;// zet de waarde in de app

        }

        private void sendValue_Clicked(object sender, EventArgs e)
        {
            int servoVal = Convert.ToInt32(servo.Value);
            string request = httpreq.GetHtml(servoVal);
            string sensorValue = request.Substring(request.IndexOf("<b>") + 3, 3);
            htmlData.Text = sensorValue;
        }

        private void getValue_Clicked(object sender, EventArgs e)
        {
            string test = httpreq.GetHtml();
            string sensorValue = test.Substring(test.IndexOf("<i>") + 3, 3);
            htmlData2.Text = sensorValue;
        }
    }
}
