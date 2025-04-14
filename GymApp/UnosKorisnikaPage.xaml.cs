using System;

namespace GymApp;

public partial class UnosKorisnikaPage : ContentPage
{
    private UserViewModel _viewModel;

    public UnosKorisnikaPage()
    {
        InitializeComponent();

        _viewModel = App.SharedViewModel;
        BindingContext = _viewModel;

        // Debug output
        System.Diagnostics.Debug.WriteLine("UnosKorisnikaPage initialized");
        System.Diagnostics.Debug.WriteLine($"BindingContext is null? {BindingContext == null}");
        System.Diagnostics.Debug.WriteLine($"IsEditing: {_viewModel.IsEditing}");
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        System.Diagnostics.Debug.WriteLine("UnosKorisnikaPage OnAppearing");

        // Force refresh of commands and properties
        if (_viewModel != null)
        {
            _viewModel.AddUserCommand.ChangeCanExecute();
            _viewModel.SaveEditCommand.ChangeCanExecute();

            System.Diagnostics.Debug.WriteLine($"ViewModel FirstName: '{_viewModel.FirstName}'");
            System.Diagnostics.Debug.WriteLine($"ViewModel LastName: '{_viewModel.LastName}'");
            System.Diagnostics.Debug.WriteLine($"ViewModel IsEditing: {_viewModel.IsEditing}");
        }
    }

    private async void OnDebugButtonClicked(object sender, EventArgs e)
    {
        System.Diagnostics.Debug.WriteLine("Debug button clicked");

        if (_viewModel != null)
        {
            // Manually populate some test data
            _viewModel.FirstName = "Test";
            _viewModel.LastName = "User";
            _viewModel.Email = "test@example.com";

            
            try
            {
                await Shell.Current.GoToAsync("//Pregled_korisnika");
                System.Diagnostics.Debug.WriteLine("Navigation successful");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Navigation error: {ex.Message}");
                await DisplayAlert("Error", "Navigation failed: " + ex.Message, "OK");
            }
        }
        else
        {
            System.Diagnostics.Debug.WriteLine("ViewModel is null");
            await DisplayAlert("Error", "ViewModel is null", "OK");
        }
    }
}