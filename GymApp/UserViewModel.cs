using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GymApp
{
    public class UserViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<User> Users { get; set; } = new ObservableCollection<User>();

        public int UserCount => Users.Count;

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
        private string businessError;

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

        public string BusinessError
        {
            get => businessError;
            set { businessError = value; OnPropertyChanged(); }
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
        public Command<User> DeleteUserCommand { get; }

        public UserViewModel()
        {
            AddUserCommand = new Command(AddUser);
            EditUserCommand = new Command<User>(StartEditUser);
            SaveEditCommand = new Command(SaveEdit);
            CancelEditCommand = new Command(CancelEdit);
            DeleteUserCommand = new Command<User>(DeleteUser);

            
            Users.Add(new User { FirstName = "Debug", LastName = "User", Email = "debug@example.com", DateOfBirth = DateTime.Today.AddYears(-30) });

            
            System.Diagnostics.Debug.WriteLine("UserViewModel initialized");
            System.Diagnostics.Debug.WriteLine($"Users count: {Users.Count}");

            Users.CollectionChanged += (s, e) =>
            {
                OnPropertyChanged(nameof(UserCount));
            };
        }

        private async void DeleteUser(User user)
        {
            if (user == null)
                return;

            System.Diagnostics.Debug.WriteLine($"Attempting to delete user: {user.FirstName} {user.LastName}");

            
            var currentPage = Application.Current.MainPage;

            
            bool confirmed = await currentPage.DisplayAlert(
                "Potvrda brisanja",
                $"Jeste li sigurni da želite obrisati korisnika {user.FirstName} {user.LastName}?",
                "Da",
                "Ne");

            if (confirmed)
            {
                
                Users.Remove(user);

                
                OnPropertyChanged(nameof(Users));
                System.Diagnostics.Debug.WriteLine($"User deleted. Updated Users count: {Users.Count}");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("User deletion cancelled");
            }
        }

        private void ValidateFirstName()
        {
            FirstNameError = string.IsNullOrWhiteSpace(FirstName) ? "Ime je obavezno" : string.Empty;

            if (string.IsNullOrEmpty(FirstNameError))
            {
                FirstNameError = FirstName.Length > 20 ? "Ime ne može biti duže od 20 karaktera." : string.Empty;
            }

            AddUserCommand.ChangeCanExecute();
            SaveEditCommand.ChangeCanExecute();
            System.Diagnostics.Debug.WriteLine($"FirstName validated: '{FirstName}', Error: '{FirstNameError}'");
        }

        private void ValidateLastName()
        {
            LastNameError = string.IsNullOrWhiteSpace(LastName) ? "Prezime je obavezno" : string.Empty;
            AddUserCommand.ChangeCanExecute();
            SaveEditCommand.ChangeCanExecute();
            System.Diagnostics.Debug.WriteLine($"LastName validated: '{LastName}', Error: '{LastNameError}'");
        }

        private void ValidateEmail()
        {
            EmailError = string.IsNullOrEmpty(Email) ? string.Empty :
                         !IsValidEmail(Email) ? "Email nije valjan" : string.Empty;
            AddUserCommand.ChangeCanExecute();
            SaveEditCommand.ChangeCanExecute();
            System.Diagnostics.Debug.WriteLine($"Email validated: '{Email}', Error: '{EmailError}'");
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void ValidateDateOfBirth()
        {
            var today = DateTime.Today;
            if (DateOfBirth > today.AddYears(-18))
            {
                DateOfBirthError = "Datum mora biti punoljetna";
            }
            else if (DateOfBirth.Year < 1900)
            {
                DateOfBirthError = "Datum rođenja nije valjan";
            }
            else
            {
                DateOfBirthError = string.Empty;
            }

            AddUserCommand.ChangeCanExecute();
            SaveEditCommand.ChangeCanExecute();
            System.Diagnostics.Debug.WriteLine($"DateOfBirth validated: '{DateOfBirth:d}', Error: '{DateOfBirthError}'");
        }

        private void ValidateBusinessRules()
        {

            bool postoji = Users.Any(u =>
            u.FirstName.Equals(FirstName, StringComparison.OrdinalIgnoreCase) &&
            u.LastName.Equals(LastName, StringComparison.OrdinalIgnoreCase));

            if (postoji)
            {
                BusinessError = "Osoba sa ovim imenom i prezimenom vec postoji.";
            }


            AddUserCommand.ChangeCanExecute();
            SaveEditCommand.ChangeCanExecute();
            System.Diagnostics.Debug.WriteLine($" Error: '{BusinessError}'");
        }

        private bool CanAddUser()
        {
            bool canAdd = !string.IsNullOrWhiteSpace(FirstName) &&
                   !string.IsNullOrWhiteSpace(LastName) &&
                   string.IsNullOrEmpty(FirstNameError) &&
                   string.IsNullOrEmpty(LastNameError) &&
                   string.IsNullOrEmpty(EmailError) &&
                   string.IsNullOrEmpty(DateOfBirthError);

            System.Diagnostics.Debug.WriteLine($"CanAddUser: {canAdd}");
            System.Diagnostics.Debug.WriteLine($"FirstName: '{FirstName}', Error: '{FirstNameError}'");
            System.Diagnostics.Debug.WriteLine($"LastName: '{LastName}', Error: '{LastNameError}'");
            System.Diagnostics.Debug.WriteLine($"Email: '{Email}', Error: '{EmailError}'");
            System.Diagnostics.Debug.WriteLine($"DateOfBirth: '{DateOfBirth:d}', Error: '{DateOfBirthError}'");

            return canAdd;
        }

        private bool CanSaveEdit()
        {
            bool canSave = !string.IsNullOrWhiteSpace(FirstName) &&
                   !string.IsNullOrWhiteSpace(LastName) &&
                   string.IsNullOrEmpty(FirstNameError) &&
                   string.IsNullOrEmpty(LastNameError) &&
                   string.IsNullOrEmpty(EmailError) &&
                   string.IsNullOrEmpty(DateOfBirthError);

            System.Diagnostics.Debug.WriteLine($"CanSaveEdit: {canSave}");
            return canSave;
        }

        private async void AddUser()
        {
            System.Diagnostics.Debug.WriteLine("AddUser method called");

            if (ValidateAll())
            {
                System.Diagnostics.Debug.WriteLine("Validation passed, creating new user");

                var newUser = new User
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    Email = Email,
                    DateOfBirth = DateOfBirth
                };

                System.Diagnostics.Debug.WriteLine($"Adding user: {newUser.FirstName} {newUser.LastName}");
                Users.Add(newUser);

                // Force UI update
                OnPropertyChanged(nameof(Users));
                System.Diagnostics.Debug.WriteLine($"New Users count: {Users.Count}");

                ClearFields();

                try
                {
                    System.Diagnostics.Debug.WriteLine("Attempting navigation to Pregled_korisnika");
                    // Match the route in AppShell.xaml
                    await Shell.Current.GoToAsync("//Pregled_korisnika");
                    System.Diagnostics.Debug.WriteLine("Navigation successful");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Navigation error: {ex.Message}");
                    System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");

                    // Try alternative navigation
                    try
                    {
                        System.Diagnostics.Debug.WriteLine("Trying to navigate with GoToAsync Shell route");
                        await Shell.Current.GoToAsync($"///{nameof(PregledKorisnikaPage)}");
                    }
                    catch (Exception ex2)
                    {
                        System.Diagnostics.Debug.WriteLine($"Second navigation attempt failed: {ex2.Message}");
                    }
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Validation failed");
            }
        }

        private async void StartEditUser(User user)
        {
            System.Diagnostics.Debug.WriteLine($"StartEditUser called with user: {user?.FirstName}");

            if (user == null)
                return;

            SelectedUser = user;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            DateOfBirth = user.DateOfBirth;
            IsEditing = true;
            System.Diagnostics.Debug.WriteLine("IsEditing set to true");

            try
            {
                System.Diagnostics.Debug.WriteLine("Attempting navigation to Unos_korisnika");
                await Shell.Current.GoToAsync("//Unos_korisnika");
                System.Diagnostics.Debug.WriteLine("Navigation successful");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Navigation error: {ex.Message}");

                // Try alternative navigation
                try
                {
                    System.Diagnostics.Debug.WriteLine("Trying to navigate with GoToAsync Shell route");
                    await Shell.Current.GoToAsync($"///{nameof(UnosKorisnikaPage)}");
                }
                catch (Exception ex2)
                {
                    System.Diagnostics.Debug.WriteLine($"Second navigation attempt failed: {ex2.Message}");
                }
            }
        }

        private async void SaveEdit()
        {
            System.Diagnostics.Debug.WriteLine("SaveEdit method called");

            if (ValidateAll() && SelectedUser != null)
            {
                System.Diagnostics.Debug.WriteLine("Validation passed, updating user");

                SelectedUser.FirstName = FirstName;
                SelectedUser.LastName = LastName;
                SelectedUser.Email = Email;
                SelectedUser.DateOfBirth = DateOfBirth;

                // Force UI update
                var index = Users.IndexOf(SelectedUser);
                if (index >= 0)
                {
                    Users[index] = SelectedUser;
                    System.Diagnostics.Debug.WriteLine($"Updated user at index {index}");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("User not found in collection");
                }

                OnPropertyChanged(nameof(Users));

                ClearFields();
                IsEditing = false;
                SelectedUser = null;

                try
                {
                    System.Diagnostics.Debug.WriteLine("Attempting navigation to Pregled_korisnika");
                    await Shell.Current.GoToAsync("//Pregled_korisnika");
                    System.Diagnostics.Debug.WriteLine("Navigation successful");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Navigation error: {ex.Message}");

                    // Try alternative navigation
                    try
                    {
                        System.Diagnostics.Debug.WriteLine("Trying to navigate with GoToAsync Shell route");
                        await Shell.Current.GoToAsync($"///{nameof(PregledKorisnikaPage)}");
                    }
                    catch (Exception ex2)
                    {
                        System.Diagnostics.Debug.WriteLine($"Second navigation attempt failed: {ex2.Message}");
                    }
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Validation failed or SelectedUser is null");
            }
        }

        private async void CancelEdit()
        {
            System.Diagnostics.Debug.WriteLine("CancelEdit method called");

            ClearFields();
            IsEditing = false;
            SelectedUser = null;

            try
            {
                System.Diagnostics.Debug.WriteLine("Attempting navigation to Pregled_korisnika");
                await Shell.Current.GoToAsync("//Pregled_korisnika");
                System.Diagnostics.Debug.WriteLine("Navigation successful");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Navigation error: {ex.Message}");

                // Try alternative navigation
                try
                {
                    System.Diagnostics.Debug.WriteLine("Trying to navigate with GoToAsync Shell route");
                    await Shell.Current.GoToAsync($"///{nameof(PregledKorisnikaPage)}");
                }
                catch (Exception ex2)
                {
                    System.Diagnostics.Debug.WriteLine($"Second navigation attempt failed: {ex2.Message}");
                }
            }
        }

        private void ClearFields()
        {
            System.Diagnostics.Debug.WriteLine("ClearFields called");
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
            DateOfBirth = DateTime.Today.AddYears(-18);
            FirstNameError = string.Empty;
            LastNameError = string.Empty;
            EmailError = string.Empty;
            DateOfBirthError = string.Empty;
        }

        private bool ValidateAll()
        {
            System.Diagnostics.Debug.WriteLine("ValidateAll called");

            ValidateFirstName();
            ValidateLastName();
            ValidateEmail();

            ValidateDateOfBirth();
            ValidateBusinessRules();

            bool isValid = string.IsNullOrEmpty(FirstNameError) &&
                   string.IsNullOrEmpty(LastNameError) &&
                   string.IsNullOrEmpty(EmailError) &&
                   string.IsNullOrEmpty(DateOfBirthError);

            System.Diagnostics.Debug.WriteLine($"ValidateAll result: {isValid}");
            return isValid;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string name = null)
        {
            System.Diagnostics.Debug.WriteLine($"PropertyChanged: {name}");
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            System.Diagnostics.Debug.WriteLine($"NotifyPropertyChanged: {propertyName}");
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}