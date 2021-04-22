using SommeliAr.ViewModels;
using SommeliAr.Views;
using SommeliAr.Views.Menu;
using Xamarin.Forms;

namespace SommeliAr.Menu
{
    public partial class MyHomePage : ContentPage
        
    {
        public MyHomePage()
        {
            InitializeComponent();
            //BindingContext = new MyUsersViewModel();
        }

        //colore violetto chiaro
        public Color myColor = Color.FromHex("#8b52ff");

        void Favourites_btn_Clicked(System.Object sender, System.EventArgs e)
        {
            History_btn.TextColor = Color.Black;
            History_btn.FontSize = 20;

            Favourites_btn.TextColor = myColor;
            Favourites_btn.FontSize = 30;

            eticehtta_prova.Text = "PREFERITI";
            eticehtta_prova.FontSize = 50;
        }

        void History_btn_Clicked(System.Object sender, System.EventArgs e)
        {
            Favourites_btn.TextColor = Color.Black;
            Favourites_btn.FontSize = 20;

            History_btn.TextColor = myColor;
            History_btn.FontSize = 30;

            eticehtta_prova.Text = "HISTORY";
            eticehtta_prova.FontSize = 50;


        }
    }
}
