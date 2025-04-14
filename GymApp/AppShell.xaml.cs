namespace GymApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Register routes with names that match the Shell content routes in AppShell.xaml
            Routing.RegisterRoute("Unos_korisnika", typeof(UnosKorisnikaPage));
            Routing.RegisterRoute("Pregled_korisnika", typeof(PregledKorisnikaPage));
        }
    }
}