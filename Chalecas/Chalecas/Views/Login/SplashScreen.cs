using Chalecas.Models;
using Chalecas.SQLiteDB;
using Chalecas.Views.Modulos;
using System;
using System.Linq;
using Xamarin.Forms;


namespace Chalecas.Views.Login
{
    class SplashScreen : ContentPage
    {
        Image splashImage;

        public SplashScreen()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            var sub = new AbsoluteLayout();
            splashImage = new Image
            {
                Source = "ik.png",
                WidthRequest = 280,
                HeightRequest = 250,
                Opacity = 0
            };

            AbsoluteLayout.SetLayoutFlags(splashImage,
                AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(splashImage,
                new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            sub.Children.Add(splashImage);

            this.BackgroundColor = Color.FromHex("#400101");
            //this.BackgroundImageSource = "fondo2.png";
            this.Content = sub;
        }
        public Usuario user;
        public UserDB userDataBase;
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await splashImage.FadeTo(1, 150, null);
            await splashImage.ScaleTo(1, 1000);
            await splashImage.ScaleTo(0.6, 1500, Easing.BounceOut);
            await splashImage.FadeTo(0, 270, null);
            userDataBase = new UserDB();
            var user_exista = userDataBase.GetMembers().ToList();
            var status = 0;
            var rol = "";
            if (user_exista.Count == 0)
            {
                status = 0;
                rol = "";
            }
            else
            {
                status = user_exista[0].status;
                rol = user_exista[0].rol;
                var user_exist = userDataBase.GetMembers();
                int RowCount = 0;
                int usercount = user_exist.Count();
                RowCount = Convert.ToInt32(usercount);
            }

            if (status == 1)
            {
                if (rol == "Sindica")
                {
                    Application.Current.MainPage = new NavigationPage(new Menu());
                }
                else
                {
                    Application.Current.MainPage = new NavigationPage(new MenuChalecos());
                }
                
            }
            else
            {
                Application.Current.MainPage = new NavigationPage(new Login());
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
    }
}
