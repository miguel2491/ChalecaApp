using Chalecas.Models;
using Chalecas.Servicios;
using Chalecas.SQLiteDB;
using Newtonsoft.Json;
using Plugin.Geolocator;
using Plugin.Toast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Chalecas.Views.Modulos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TimbreO : ContentPage
    {
        private UserDB userdb;
        public TimbreO()
        {
            InitializeComponent();
            //Get Info User
        }

        protected override async void OnAppearing()
        {
            userdb = new UserDB();
            var user_exist = userdb.GetMembers().ToList();
            var id_usr = Convert.ToString(user_exist[0].id_user);
            //----Send Ubicacion----------------------------------------------
            var pos = await CrossGeolocator.Current.GetPositionAsync();
            var lat = Convert.ToString(pos.Latitude);
            var lon = Convert.ToString(pos.Longitude);
            ApiRest apirest = new ApiRest();
            var responsePos = await apirest.SetPos(id_usr, lat, lon);
            if (responsePos.IsSuccessStatusCode)
            {
                var jsonPos = await responsePos.Content.ReadAsStringAsync();
                var resultPos = JsonConvert.DeserializeObject<Respuesta>(jsonPos);
                var succPos = resultPos.success;
                var failurePos = resultPos.failure;
            }
            //----Enviar Notificacion-----------------------------------------
            var response = await apirest.SendLlamadaSindica(id_usr);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Respuesta>(json);
                var succ = result.success;
                var failure = result.failure;
                //var Resultd = JsonConvert.DeserializeObject<List<Respuesta>>(json);
                if (succ == 1)
                {
                    CrossToastPopUp.Current.ShowToastSuccess("Aviso Enviado");
                }
                if (failure == 1)
                {
                    CrossToastPopUp.Current.ShowToastError("Algo ocurrio mal, vuelve a intentar");
                    //Habilitar Boton para llamar
                }
                showBtn();
            }
            //---------------------------------------------
        }

        public async void getEfectImg()
        {
            await ImageSindi.ScaleTo(1, 1000);
            btnTimbrar.IsVisible = true;
            showBtn();
        }

        public void showBtn()
        {
            btnTimbrar.IsVisible = true;
        }

        private async void btnTimbrar_Clicked(object sender, EventArgs e)
        {
            btnTimbrar.IsVisible = false;
            userdb = new UserDB();
            var user_exist = userdb.GetMembers().ToList();
            var id_usr = Convert.ToString(user_exist[0].id_user);
            ApiRest apirest = new ApiRest();
            //----Enviar Notificacion-----------------------------------------
            var response = await apirest.SendLlamadaSindica(id_usr);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Respuesta>(json);
                var succ = result.success;
                var failure = result.failure;
                //var Resultd = JsonConvert.DeserializeObject<List<Respuesta>>(json);
                if (succ == 1)
                {
                    CrossToastPopUp.Current.ShowToastSuccess("Aviso Enviado");
                }
                if (failure == 1)
                {
                    CrossToastPopUp.Current.ShowToastError("Algo ocurrio mal, vuelve a intentar");
                    //Habilitar Boton para llamar
                }
                showBtn();
                //--------------
                uint transitionTime = 800;
                double displacement = ImageSindi.Width;

                await Task.WhenAll(
                    ImageSindi.FadeTo(0, transitionTime, Easing.Linear),
                    ImageSindi.TranslateTo(-displacement, ImageSindi.Y, transitionTime, Easing.CubicInOut));

                // Changes image source.
                ImageSindi.Source = ImageSource.FromFile("ic_sindica");

                await ImageSindi.TranslateTo(displacement, 0, 0);
                await Task.WhenAll(
                    ImageSindi.FadeTo(1, transitionTime, Easing.Linear),
                    ImageSindi.TranslateTo(0, ImageSindi.Y, transitionTime, Easing.CubicInOut));
                //--------------
            }
            //---------------------------------------------
        }
        //-------------------------------------------------------------
    }
}