using CarWorkshop.WPF.Infra;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace CarWorkshop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ServiceProvider ServiceProvider;
        public App()
        {
            var serviceCollection = new ServiceCollection();
            App.ServiceProvider = DIConfig.Configure(serviceCollection);
        }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = App.ServiceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }
    }
}
