using Alprog2.Pages;

namespace Alprog2
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(DataNumerikPage), typeof(DataNumerikPage));
            Routing.RegisterRoute(nameof(GrafikPage), typeof(GrafikPage));
            Routing.RegisterRoute(nameof(DatabasePage), typeof(DatabasePage));
            Routing.RegisterRoute(nameof(ComPortPage), typeof(ComPortPage));

        }
    }
}
