using Chalecas.ViewModels;
using Chalecas.Views;
using Xamarin.Forms;

namespace Chalecas
{
    public partial class MainPage : ContentPage
    {
        private MainViewModel _viewModel;
        public MainPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new MainViewModel();                        
        }

        async void btn1_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new MyPageTwo());
        }
    }
}
