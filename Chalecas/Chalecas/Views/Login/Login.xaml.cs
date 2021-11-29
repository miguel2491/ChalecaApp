using Chalecas.Models;
using Chalecas.Servicios;
using Chalecas.SQLiteDB;
using Chalecas.Views.Modulos;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Chalecas.Views.Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {

        public Usuario user;
        private UserDB userdb;
        public Login()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                await DisplayAlert("Error", "Por favor activa tus datos o conectate a una red", "ok");
                btnIniciarSesion.IsVisible = false;
            }
            else
            {
                btnIniciarSesion.IsVisible = true;
            }
        }

        private async void btnIniciarSesion_Clicked(object sender, EventArgs e)
        {
            try 
            {
                btnIniciarSesion.IsVisible = false;
                activityLogin.IsRunning = true;

                userdb = new UserDB();
                var user_exist = userdb.GetMembers().ToList();
                
                var user = username.Text;
                var pass = password.Text;
                var token = user_exist[0].token;
                //----------Api-------------------
                ApiRest apirest = new ApiRest();
                var response = await apirest.GetLogin(user, pass, token);
                if (response.IsSuccessStatusCode)
                {
                    HttpContent resp_content = response.Content;
                    var json = resp_content.ReadAsStringAsync();
                    var uResult = JsonConvert.DeserializeObject<List<Usuario>>(await json);
                    var rol = uResult[0].rol;

                    var idUser = uResult[0].id;
                    var nombre = uResult[0].nombre;
                    var username = uResult[0].username;
                    var password = uResult[0].password;
                    var token_id = uResult[0].token;
                    var status = 1;

                    userdb.UpdateMember(1, idUser, nombre, username, password, rol, status);

                    if (user == "wendi")
                    {
                        Application.Current.MainPage = new NavigationPage(new Menu());
                    }
                    else if (user.Length == 0)
                    {
                        //CrossToastPopUp.Current.ShowToastError("Debes ingresar un usuario");
                    }
                    else
                    {
                        Application.Current.MainPage = new NavigationPage(new MenuChalecos());
                    }
                }
                else
                { 
                    
                }
                //--------------------------------
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error EX", "Error : "+ ex.ToString(), "OK");
            }
            
            activityLogin.IsRunning = false;
            btnIniciarSesion.IsVisible = true;
        }
//---------------------------------------------------------------------------------------------------------------------------------------------------
    }
}