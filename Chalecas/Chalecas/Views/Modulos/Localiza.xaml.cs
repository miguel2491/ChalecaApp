using Chalecas.Models;
using Chalecas.Servicios;
using Chalecas.SQLiteDB;
using Newtonsoft.Json;
using Plugin.Geolocator;
using Plugin.Toast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace Chalecas.Views.Modulos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Localiza : ContentPage
    {
        public Usuario user;
        private UserDB userdb;

        public Localiza()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            //--------------------------------------------------
            userdb = new UserDB();
            var user_exist = userdb.GetMembers().ToList();
            var id_usr = user_exist[0].id_user;
            var ussr = user_exist[0].nombre;
            //--------------------------------------------------
            var pos = await CrossGeolocator.Current.GetPositionAsync();

            Mapx.MoveToRegion(
                MapSpan.FromCenterAndRadius(
            new Position(pos.Latitude, pos.Longitude), Distance.FromMiles(1)));


            var pin = new Pin
            {
                Type = PinType.Place,
                Position = new Position(pos.Latitude, pos.Longitude),
                Label = ussr,
                Address = "  usted se encuentra aqui",

            };
            Mapx.Pins.Add(pin);
            Mapx.IsVisible = true;
            ApiRest apires = new ApiRest();
            var responsed = await apires.GetChalecas();
            if (responsed.IsSuccessStatusCode)
            {
                string contentd = await responsed.Content.ReadAsStringAsync();
                var userResultd = JsonConvert.DeserializeObject<List<Usuario>>(contentd);

                listUser.Items.Clear();
                foreach (var item in userResultd)
                {
                    if (ussr != item.nombre)
                        listUser.Items.Add(item.nombre);
                }
            }
            //----------------------------------------
        }

        private async void btnUbicar_Clicked(object sender, EventArgs e)
        {
            //Extraer ID select
            ApiRest apirest = new ApiRest();
            int selectedIndex = listUser.SelectedIndex;
            var chaleca = "";
            if (selectedIndex != -1)
            {
                chaleca = listUser.Items[selectedIndex];
            }
            var response = await apirest.GetPosUser(chaleca);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var userResultd = JsonConvert.DeserializeObject<List<LocalizaModel>>(content);
                var lat = 0.00;
                var lon = 0.00;
                Mapx.Pins.Clear();

                Polyline polyline = new Polyline
                {
                    StrokeColor = Xamarin.Forms.Color.Red,
                    StrokeWidth = 12,
                };

                foreach (var item in userResultd)
                {
                    lat = Convert.ToDouble(item.latitud);
                    lon = Convert.ToDouble(item.longitud);
                    var pin = new Pin
                    {
                        Type = PinType.Place,
                        Position = new Position(lat, lon),
                        Label = "Último registro",
                        Address = " 00:00:00",
                    };
                    Mapx.Pins.Add(pin);
                    //----------
                    polyline.Geopath.Add(new Position(lat,lon));
                }
                //Mapx.Pins.Add(pin);
                Mapx.MapElements.Add(polyline);
                //------------------------------------------------------
                // Instantiate a Circle
                Circle circle = new Circle
                {
                    Center = new Position(lat, lon),
                    Radius = new Distance(550),
                    StrokeColor = Color.FromHex("#88FF0000"),
                    StrokeWidth = 8,
                    FillColor = Color.FromHex("#88FFC0CB")
                };
                Mapx.MapElements.Add(circle);
                //-------------------------------------------------------
                Mapx.MoveToRegion(
                MapSpan.FromCenterAndRadius(
                    new Position(lat, lon), Distance.FromMiles(1)));
                var pos = await CrossGeolocator.Current.GetPositionAsync();
                //Map map = new Map { };
                
                CrossToastPopUp.Current.ShowToastSuccess("Pintaria ruta");
            }
            else
            {
                CrossToastPopUp.Current.ShowToastError("Sin Localizar");
            }
        }

        CancellationTokenSource cts;
        async Task GetCurrentLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                cts = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, cts.Token);

                if (location != null)
                {
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
                Console.WriteLine("Sin acceso a GPS");
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
        }

        protected override void OnDisappearing()
        {
            if (cts != null && !cts.IsCancellationRequested)
                cts.Cancel();
            base.OnDisappearing();
        }
        //---------------------------------------------------------------------------------------------------------------
    }
}