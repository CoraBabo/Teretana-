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
        private DateTime dateOfBirth = DateTime.Today.AddYears(-18);
        private User selectedUser;
        private bool isEditing = false;
        private string firstNameError;
        private string lastNameError;
        private string emailError;
        private string dateOfBirthError;

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

        public DateTime DateOfBirth
        {
            get => dateOfBirth;
            set
            {
                dateOfBirth = value;
                OnPropertyChanged();
                ValidateDateOfBirth();
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

        public string DateOfBirthError
        {
            get => dateOfBirthError;
            set { dateOfBirthError = value; OnPropertyChanged(); }
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

        private void ValidateDateOfBirth()
        {
            var today = DateTime.Today;
            if (DateOfBirth > today)
            {
                DateOfBirthError = "Datum rođenja ne može biti u budućnosti";
            }
            else if (DateOfBirth.Year < 1900)
            {
                DateOfBirthError = "Datum rođenja nije valjan";
            }
            else
            {
                DateOfBirthError = string.Empty;
            }

            (AddUserCommand as Command).ChangeCanExecute();
            (SaveEditCommand as Command).ChangeCanExecute();
        }

        private bool CanAddUser()
        {
            return !string.IsNullOrWhiteSpace(FirstName) &&
                   !string.IsNullOrWhiteSpace(LastName) &&
                   string.IsNullOrEmpty(DateOfBirthError);
        }

        private bool CanSaveEdit()
        {
            return !string.IsNullOrWhiteSpace(FirstName) &&
                   !string.IsNullOrWhiteSpace(LastName) &&
                   string.IsNullOrEmpty(DateOfBirthError);
        }

        private async void AddUser()
        {
            if (ValidateAll())
            {
                Users.Add(new User
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    Email = Email,
                    DateOfBirth = DateOfBirth
                });

                OnPropertyChanged(nameof(Users));
                ClearFields();

                try
                {
                    await Shell.Current.GoToAsync("pregledkorisnika");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Navigation error: {ex.Message}");
                    
                    await Shell.Current.GoToAsync("//Pregled_korisnika");
                }
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
            DateOfBirth = user.DateOfBirth;
            IsEditing = true;

            try
            {
                await Shell.Current.GoToAsync("unoskorisnika");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Navigation error: {ex.Message}");
              
                await Shell.Current.GoToAsync("//Unos_korisnika");
            }
        }

        private async void SaveEdit()
        {
            if (ValidateAll() && SelectedUser != null)
            {
                SelectedUser.FirstName = FirstName;
                SelectedUser.LastName = LastName;
                SelectedUser.Email = Email;
                SelectedUser.DateOfBirth = DateOfBirth;

                
                OnPropertyChanged(nameof(Users));

                ClearFields();
                IsEditing = false;
                SelectedUser = null;

                try
                {
                    await Shell.Current.GoToAsync("pregledkorisnika");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Navigation error: {ex.Message}");
                    
                    await Shell.Current.GoToAsync("//Pregled_korisnika");
                }
            }
        }

        private async void CancelEdit()
        {
            ClearFields();
            IsEditing = false;
            SelectedUser = null;

            try
            {
                await Shell.Current.GoToAsync("pregledkorisnika");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Navigation error: {ex.Message}");
                
                await Shell.Current.GoToAsync("//Pregled_korisnika");
            }
        }

        private void ClearFields()
        {
            FirstName = LastName = Email = string.Empty;
            DateOfBirth = DateTime.Today.AddYears(-18);
            FirstNameError = LastNameError = EmailError = DateOfBirthError = string.Empty;
        }

        private bool ValidateAll()
        {
            ValidateFirstName();
            ValidateLastName();
            ValidateEmail();
            ValidateDateOfBirth();

            return string.IsNullOrEmpty(FirstNameError) &&
                   string.IsNullOrEmpty(LastNameError) &&
                   string.IsNullOrEmpty(EmailError) &&
                   string.IsNullOrEmpty(DateOfBirthError);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}