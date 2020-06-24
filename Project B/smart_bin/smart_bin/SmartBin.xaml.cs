using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace smart_bin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SmartBin : ContentPage
    {
        private string ipaddress;
        
        Client client = new Client();
        public SmartBin(string ip)
        {
            InitializeComponent();
            this.ipaddress = ip;           
            ReadStatus(ip);
        }

        /// <summary>
        /// If the button "openBtn" is clicked disable the button and send "open" to the arduino 
        /// if the response is equal to Error: change the text color to red and Show "Er is iets mis gegaan"
        /// else: change text to: arduino response ("open") and change the text color to green
        /// wait 5 seconds and send "close" to the arduino. The response should be "gesloten" 
        /// enable the button "OpenBtn"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OpenBin(object sender, EventArgs e)
        {
            OpenBtn.IsEnabled = false;
            string open = client.ask(ipaddress, 80, "open");

            if (open == "Error")
            {
                status.TextColor = Color.Red;
                status.Text = "Er is iets mis gegaan";
            } else
            {
                status.Text = open;
                status.TextColor = Color.LimeGreen;
            }
            await Task.Delay(5000);
            status.Text = client.ask(ipaddress, 80, "close");
            status.TextColor = Color.Red;
            OpenBtn.IsEnabled = true;
        }

        /// <summary>
        /// check the arduino status every second and change the label if the status is changed
        /// </summary>
        /// <param name="ip"></param>
        private async void ReadStatus(string ip)
        {
            while (true)
            {
                string binStatus = client.ask(ip, 80, "status");
                status.Text = binStatus;
                if (binStatus == "Geopend")
                {
                    OpenBtn.IsEnabled = false;
                    status.TextColor = Color.LimeGreen;
                }
                else if (binStatus == "Gesloten")
                {
                    OpenBtn.IsEnabled = true;
                    status.TextColor = Color.Red;
                }

                await Task.Delay(1000);
            }
        }
    }
}