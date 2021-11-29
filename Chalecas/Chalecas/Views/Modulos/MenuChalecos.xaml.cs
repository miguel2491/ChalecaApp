using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Plugin.Connectivity;
using Plugin.SimpleAudioPlayer;
using Plugin.Toast;
using Xamarin.Essentials;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Chalecas.Views.Modulos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuChalecos : Xamarin.Forms.TabbedPage
    {
        ISimpleAudioPlayer player;
        public MenuChalecos()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            var stream = GetStreamFromFile("Wendi.mp3");
            player = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
            player.Load(stream);
            On<Android>().SetToolbarPlacement(ToolbarPlacement.Top);
            MessagingCenter.Subscribe<string>(this, "MensajeFirebase", (value) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    CrossToastPopUp.Current.ShowToastError("Llamar " + value);
                    InitCall();
                    var duration = TimeSpan.FromSeconds(5);
                    Vibration.Vibrate(duration);
                    TimbreO to = new TimbreO();
                    to.getEfectImg();
                    to.showBtn();
                });
            });
        }

        private void InitCall()
        {
            player.Play();
        }

        Stream GetStreamFromFile(string filename)
        {
            var assembly = typeof(App).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream("Chalecas." + filename);
            return stream;
        }

        protected override async void OnAppearing()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                await DisplayAlert("Error", "Por favor activa tus datos o conectate a una red", "ok");
                System.Environment.Exit(0);
            }
        }

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await this.DisplayAlert("Aviso", "¿Realmente quieres salir?", "Si", "No");
                if (result)
                {
                    System.Environment.Exit(0);
                }
            });
            return true;
        }
        //-------------------------------------------------------------------------------------------------
    }
}