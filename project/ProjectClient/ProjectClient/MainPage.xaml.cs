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

            servoValue.Text = httpreq.GetServoValue();

            servo.Value = Convert.ToInt32(httpreq.GetServoValue());

            sensorValue.Text = httpreq.GetSensorValue();


        }
        /// <summary>
        /// if button is clicked get the value from the slider and set the servo value 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sendValue_Clicked(object sender, EventArgs e)
        {
            int servoVal = Convert.ToInt32(servo.Value);

            
            httpreq.SetServoValue(servoVal);

            servoValue.Text = httpreq.GetServoValue();

        }

        /// <summary>
        /// if the button is clicked get the value from the website and set the label
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void getValue_Clicked(object sender, EventArgs e)
        {
            sensorValue.Text = httpreq.GetSensorValue();
        }
    }
}
