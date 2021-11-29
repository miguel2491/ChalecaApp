using Plugin.Toast;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Chalecas
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            MessagingCenter.Subscribe<string>(this, "MensajeFirebase", (value) =>
              {
                  Device.BeginInvokeOnMainThread(() =>
                  {
                      CrossToastPopUp.Current.ShowToastError("Llamar " + value);
                  });
              });
        }
    }
}
