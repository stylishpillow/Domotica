using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace smart_bin
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {

        Client client = new Client();
        public MainPage()
        {
            InitializeComponent();
        }

        private void OpenBin(object sender, EventArgs e)
        {
            // 1e parameter is het ipaddress van de arduino 2e is poort 80 als het goed is heeft arduino dat ook en 3e is het bericht wat je verstuurd
            //als je force stuurt krijg je "gelukt" terug en anders verstuurd hij het verstuurde bericht terug 
            string ip = ipaddress.Text;
            status.Text = client.ask(ip, 80, "open");
        }
    }
}
