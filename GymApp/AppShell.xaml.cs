namespace GymApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // These routes are for deeper navigation if needed
            Routing.RegisterRoute("unoskorisnika", typeof(UnosKorisnikaPage));
            Routing.RegisterRoute("pregledkorisnika", typeof(PregledKorisnikaPage));
        }
    }
}