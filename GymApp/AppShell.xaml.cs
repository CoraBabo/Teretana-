namespace GymApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("unoskorisnika", typeof(UnosKorisnikaPage));
            Routing.RegisterRoute("pregledkorisnika", typeof(PregledKorisnikaPage));
        }
    }
}