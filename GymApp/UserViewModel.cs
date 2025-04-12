using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GymApp
{
    public class UserViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<User> Users { get; set; } = new ObservableCollection<User>();

        private string firstName;
        private string lastName;
        private string email;
        private User selectedUser;
        private bool isEditing = false;
        private string firstNameError;
        private string lastNameError;
        private string emailError;

        public string FirstName
        {
            get => firstName;
            set
            {
                firstName = value;
                OnPropertyChanged();
                ValidateFirstName();
            }
        }

        public string LastName
        {
            get => lastName;
            set
            {
                lastName = value;
                OnPropertyChanged();
                ValidateLastName();
            }
        }

        public string Email
        {
            get => email;
            set
            {
                email = value;
                OnPropertyChanged();
                ValidateEmail();
            }
        }

        public string FirstNameError
        {
            get => firstNameError;
            set { firstNameError = value; OnPropertyChanged(); }
        }

        public string LastNameError
        {
            get => lastNameError;
            set { lastNameError = value; OnPropertyChanged(); }
        }

        public string EmailError
        {
            get => emailError;
            set { emailError = value; OnPropertyChanged(); }
        }

        public bool IsEditing
        {
            get => isEditing;
            set { isEditing = value; OnPropertyChanged(); }
        }

        public User SelectedUser
        {
            get => selectedUser;
            set { selectedUser = value; OnPropertyChanged(); }
        }

        public Command AddUserCommand { get; }
        public Command<User> EditUserCommand { get; }
        public Command SaveEditCommand { get; }
        public Command CancelEditCommand { get; }

        public UserViewModel()
        {
            AddUserCommand = new Command(AddUser, CanAddUser);
            EditUserCommand = new Command<User>(StartEditUser);
            SaveEditCommand = new Command(SaveEdit, CanSaveEdit);
            CancelEditCommand = new Command(CancelEdit);
        }

        private void ValidateFirstName()
        {
            FirstNameError = string.IsNullOrWhiteSpace(FirstName) ? "Ime je obavezno" : string.Empty;
            (AddUserCommand as Command).ChangeCanExecute();
            (SaveEditCommand as Command).ChangeCanExecute();
        }

        private void ValidateLastName()
        {
            LastNameError = string.IsNullOrWhiteSpace(LastName) ? "Prezime je obavezno" : string.Empty;
            (AddUserCommand as Command).ChangeCanExecute();
            (SaveEditCommand as Command).ChangeCanExecute();
        }

        private void ValidateEmail()
        {
            EmailError = string.Empty;
            (AddUserCommand as Command).ChangeCanExecute();
            (SaveEditCommand as Command).ChangeCanExecute();
        }

        private bool CanAddUser()
        {
            return !string.IsNullOrWhiteSpace(FirstName) && !string.IsNullOrWhiteSpace(LastName);
        }

        private bool CanSaveEdit()
        {
            return !string.IsNullOrWhiteSpace(FirstName) && !string.IsNullOrWhiteSpace(LastName);
        }

        private async void AddUser()
        {
            if (ValidateAll())
            {
                Users.Add(new User { FirstName = FirstName, LastName = LastName, Email = Email });
                ClearFields();
                await Shell.Current.GoToAsync("//Pregled_korisnika");
            }
        }

        private async void StartEditUser(User user)
        {
            if (user == null)
                return;

            SelectedUser = user;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            IsEditing = true;

            try
            {
                // Use the registered route name
                await Shell.Current.GoToAsync("//Unos_korisnika");
            }
            catch (Exception ex)
            {
                // Debug the error
                System.Diagnostics.Debug.WriteLine($"Navigation error: {ex.Message}");
            }
        }

        private async void SaveEdit()
        {
            if (ValidateAll() && SelectedUser != null)
            {
                // Option 1: Update the properties which will trigger property change notifications
                SelectedUser.FirstName = FirstName;
                SelectedUser.LastName = LastName;
                SelectedUser.Email = Email;

                // Option 2: Force refresh by removing and re-adding
                int index = Users.IndexOf(SelectedUser);
                if (index != -1)
                {
                    Users.Remove(SelectedUser);
                    Users.Insert(index, SelectedUser);
                }

                // Force UI refresh by triggering collection changed event
                var tempCollection = new ObservableCollection<User>(Users);
                Users.Clear();
                foreach (var u in tempCollection)
                {
                    Users.Add(u);
                }

                ClearFields();
                IsEditing = false;
                SelectedUser = null;

                // Use the registered route name
                await Shell.Current.GoToAsync("//Pregled_korisnika");
            }
        }

        private async void CancelEdit()
        {
            ClearFields();
            IsEditing = false;
            SelectedUser = null;

            // Use the registered route name
            await Shell.Current.GoToAsync("//Pregled_korisnika");
        }

        private void ClearFields()
        {
            FirstName = LastName = Email = string.Empty;
            FirstNameError = LastNameError = EmailError = string.Empty;
        }

        private bool ValidateAll()
        {
            ValidateFirstName();
            ValidateLastName();
            ValidateEmail();

            return string.IsNullOrEmpty(FirstNameError) &&
                   string.IsNullOrEmpty(LastNameError) &&
                   string.IsNullOrEmpty(EmailError);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        // Add a public method to notify property changes
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}