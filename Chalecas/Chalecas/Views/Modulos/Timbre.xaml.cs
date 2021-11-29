using Chalecas.Models;
using Chalecas.Servicios;
using Newtonsoft.Json;
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
    public partial class Timbre : ContentPage
    {
        private static bool banderaClick;
        public Timbre()
        {
            InitializeComponent();
            banderaClick = true;
        }

        protected override async void OnAppearing()
        {
            //--------------------------------------------------
            ApiRest apirest = new ApiRest();
            var response = await apirest.GetChalecas();
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var userResult = JsonConvert.DeserializeObject<List<Usuario>>(content);
                listView.ItemsSource = userResult;
                listView.ItemSelected += OnClickOpcionSeleccionada;
            }
            //------------------------
        }

        private async void OnClickOpcionSeleccionada(object sender, SelectedItemChangedEventArgs e)
        {
            if (banderaClick)
            {
                var item = e.SelectedItem as Usuario;
                if (item != null)
                {
                    var oSeleccionado = Convert.ToString(item.id);
                    banderaClick = false;
                    //----SendNot------------------------------------------------------------------------------------
                    ApiRest apirest = new ApiRest();
                    var response = await apirest.SendLlamada(oSeleccionado, item.nombre, "Llamada de la sindica");
                    if (response.IsSuccessStatusCode)
                    {
                        CrossToastPopUp.Current.ShowToastSuccess("Mandar a Llamar a " + item.nombre + " " + item.id);
                    }
                    else
                    {
                        CrossToastPopUp.Current.ShowToastError("Ocurrio un problema");
                    }
                    //------------------------------------------------------------------------------------------------
                    await Task.Run(async () =>
                    {
                        await Task.Delay(500);
                        banderaClick = true;
                    });
                }
            } // fin banderaCLick
        }// fin metodo OnClickOpcionSeleccionada
        //-------------------------------------------------------------------------------------
    }
}