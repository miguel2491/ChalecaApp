using System;
using System.Collections.Generic;
using Chalecas.ViewModels;
using Xamarin.Forms;

namespace Chalecas.Views
{
    public partial class MyPageTwo : ContentPage
    {
        public MyPageTwo()
        {
            InitializeComponent();
            BindingContext = new MainViewModel();
        }
    }
}
