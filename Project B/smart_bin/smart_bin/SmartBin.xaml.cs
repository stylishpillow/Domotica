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

        private async void OpenBin(object sender, EventArgs e)
        {
            // 1e parameter is het ipaddress van de arduino 2e is poort 80 als het goed is heeft arduino dat ook en 3e is het bericht wat je verstuurd
            //als je force stuurt krijg je "gelukt" terug en anders verstuurd hij het verstuurde bericht terug 
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
                //status.Text = client.ask(ipaddress, 80, "status");
            }
        }
    }
}