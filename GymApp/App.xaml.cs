namespace GymApp
{
    public partial class App : Application
    {
        public static UserViewModel SharedViewModel { get; private set; }

        public App()
        {
            InitializeComponent();

            SharedViewModel = new UserViewModel();

            MainPage = new AppShell();
        }

    }
}