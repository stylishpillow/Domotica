using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        }

        private void OpenBin(object sender, EventArgs e)
        {
            // 1e parameter is het ipaddress van de arduino 2e is poort 80 als het goed is heeft arduino dat ook en 3e is het bericht wat je verstuurd
            //als je force stuurt krijg je "gelukt" terug en anders verstuurd hij het verstuurde bericht terug 
            string ip = ipaddress.Text;

            if (client.ask(ip, 80, "open") == "Error")
            {
                status.TextColor = Color.Red;
                ipaddress.TextColor = Color.Red; 
            } else
            {
                status.Text = client.ask(ip, 80, "open");
                ipaddress.IsEnabled = false;
            }
        }
    }
}