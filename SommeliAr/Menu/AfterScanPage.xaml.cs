using SommeliAr.Models;
using SommeliAr.ViewModels;
using SommeliAr.Views;
using Xamarin.Forms;

namespace SommeliAr.Menu
{
    public partial class AfterScanPage : ContentPage
    {
        private async void OnPageSelected()
        {
            MyResultListViewModel res = new MyResultListViewModel();
            await res.GetInfoAsync();
            BindingContext = res;
        }

        public AfterScanPage()
        {
            InitializeComponent();
            OnPageSelected();
            if (Device.RuntimePlatform == Device.Android)
            {
                var navigationPage = Application.Current.MainPage as NavigationPage;
                navigationPage.BarBackgroundColor = Color.Transparent;
            }
        }

        private async void OnItemSelected(object sender, ItemTappedEventArgs e)
        {
            var mydetails = e.Item as MyWineModel;
            await Navigation.PushAsync(new MyListPageDetail(mydetails.Name, mydetails.Description, mydetails.SensorialNotes, mydetails.ProductionArea, mydetails.Dishes, mydetails.Image, mydetails.Rating));
        }
    }
}