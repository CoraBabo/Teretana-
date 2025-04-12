using Microsoft.Maui.Controls;

namespace GymApp
{
    public partial class PregledKorisnikaPage : ContentPage
    {
        public PregledKorisnikaPage()
        {
            InitializeComponent();
            BindingContext = App.SharedViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Force UI update
            if (BindingContext is UserViewModel viewModel)
            {
                // Option 1: Refresh the entire CollectionView
                var collectionView = this.FindByName<CollectionView>("UsersCollectionView");
                if (collectionView != null)
                {
                    collectionView.ItemsSource = null;
                    collectionView.ItemsSource = viewModel.Users;
                }

                // Option 2: Force property change event for Users collection
                viewModel.NotifyPropertyChanged(nameof(UserViewModel.Users));
            }
        }

        private void OnEditButtonClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is User user)
            {
                var viewModel = (UserViewModel)BindingContext;
                viewModel.EditUserCommand.Execute(user);
            }
        }
    }
}