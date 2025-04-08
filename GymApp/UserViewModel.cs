using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using GymApp;
namespace GymApp;
public class UserViewModel : INotifyPropertyChanged
{
    public ObservableCollection<User> Users { get; set; } = new ObservableCollection<User>();

    private string firstName;
    private string lastName;
    private string email;

    public string FirstName
    {
        get => firstName;
        set { firstName = value; OnPropertyChanged(); }
    }

    public string LastName
    {
        get => lastName;
        set { lastName = value; OnPropertyChanged(); }
    }

    public string Email
    {
        get => email;
        set { email = value; OnPropertyChanged(); }
    }

    public Command AddUserCommand { get; }

    public UserViewModel()
    {
        AddUserCommand = new Command(AddUser);
    }

    private void AddUser()
    {
        if (!string.IsNullOrWhiteSpace(FirstName) && !string.IsNullOrWhiteSpace(LastName))
        {
            Users.Add(new User { FirstName = FirstName, LastName = LastName, Email = Email });
            FirstName = LastName = Email = string.Empty;
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    void OnPropertyChanged([CallerMemberName] string name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
