using Microsoft.Maui.Controls;

namespace GymApp
{
    public partial class PregledKorisnikaPage : ContentPage
    {
        private UserViewModel _viewModel;

        public PregledKorisnikaPage()
        {
            InitializeComponent();
            _viewModel = App.SharedViewModel;
            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is UserViewModel viewModel)
            {
                var collectionView = this.FindByName<CollectionView>("UsersCollectionView");
                if (collectionView != null)
                {
                    
                    viewModel.NotifyPropertyChanged(nameof(UserViewModel.FilteredUsers));
                    System.Diagnostics.Debug.WriteLine("Collection view refreshed on PregledKorisnikaPage");
                }

                
                if (!string.IsNullOrEmpty(viewModel.SearchText))
                {
                    viewModel.SearchText = string.Empty;
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

        private void OnCopyButtonClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is User user)
            {
                var viewModel = (UserViewModel)BindingContext;
                viewModel.CopyUserCommand.Execute(user);
            }
        }

        private void OnSearchButtonClicked(object sender, EventArgs e)
        {
            var searchEntry = this.FindByName<Entry>("SearchEntry");
            if (searchEntry != null)
            {
                var viewModel = (UserViewModel)BindingContext;
                viewModel.SearchUserCommand.Execute(searchEntry.Text);
                System.Diagnostics.Debug.WriteLine($"Search initiated for text: '{searchEntry.Text}'");
            }
        }
    }
}