using System;
using System.Collections.Generic;
using SommeliAr.ViewModels;
using Xamarin.Forms;

namespace SommeliAr.Views
{
    public partial class UsersListPage : ContentPage
    {
        public UsersListPage()
        {
            InitializeComponent();
            BindingContext = new MyUsersViewModel();
        }
    }
}
