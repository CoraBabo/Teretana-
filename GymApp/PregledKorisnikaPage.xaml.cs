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

            if (BindingContext is UserViewModel viewModel)
            {
                var collectionView = this.FindByName<CollectionView>("UsersCollectionView");
                if (collectionView != null)
                {
                    collectionView.ItemsSource = null;
                    collectionView.ItemsSource = viewModel.Users;

                    // Force refresh
                    viewModel.NotifyPropertyChanged(nameof(UserViewModel.Users));
                }
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

        private void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is User user)
            {
                var viewModel = (UserViewModel)BindingContext;
                viewModel.DeleteUserCommand.Execute(user);
            }
        }
    }
}