using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using RandomCoffeeMachine.Services;
using RandomCoffeeMachine.ViewModels;
using RandomCoffeeMachine.Views;
using System.IO;

namespace RandomCoffeeMachine
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (Directory.Exists("storage") == false)
            {
                Directory.CreateDirectory("storage");
            }
            var viewModel = LocalStorageService.Load<MainWindowViewModel>("data.json");

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = viewModel
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
