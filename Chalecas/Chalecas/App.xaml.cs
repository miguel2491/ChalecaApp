using Chalecas.Views.Login;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Chalecas
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //MainPage = new MainPage();
            MainPage = new NavigationPage(new SplashScreen());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
