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

        /// <summary>
        /// Gets the ipaddres from text entries and check if ip is correct 
        /// if the response from the arduino is equal to correct: open new page SmartBin
        /// else: Display an error
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
