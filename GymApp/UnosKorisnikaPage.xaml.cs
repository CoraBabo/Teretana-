using System;

namespace GymApp;

public partial class UnosKorisnikaPage : ContentPage
{
    public UnosKorisnikaPage()
    {
        InitializeComponent();

        BindingContext = App.SharedViewModel;
    }
}