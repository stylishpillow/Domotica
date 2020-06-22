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

        private async void SetIP(object sender, EventArgs e)
        {
            string ip_address = ipaddress_1.Text+".";
            ip_address += ipaddress_2.Text+".";
            ip_address += ipaddress_3.Text+".";
            ip_address += ipaddress_4.Text;

            string checkIP = client.ask(ip_address, 80, "checkIP");
            if(checkIP == "correct")
            {
                await Navigation.PushModalAsync(new SmartBin(ip_address));
            } else
            {
               await DisplayAlert("Fout!", "Het ingevulde IP adres is incorrect", "sluiten");

            }

        }
    }
}
