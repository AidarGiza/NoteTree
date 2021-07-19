using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using NoteTree.Models;
using NoteTree.Services;

namespace NoteTree
{
    public class App : Application
    {
        public static DataManager DataManager;
        public static ConfigModel Config;

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public static Window MainWindow
        {
            get => (Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
            set => (Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime).MainWindow = value;
        }

        public override void OnFrameworkInitializationCompleted()
        {
            //if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            //{
            //    desktop.MainWindow = new MainWindow
            //    {
            //        DataContext = new MainWindowViewModel(),
            //    };
            //    (desktop.MainWindow.DataContext as MainWindowViewModel).UserControl = desktop.MainWindow;
            //}

            base.OnFrameworkInitializationCompleted();

            Program.AppMain(this, Program._args);
        }
    }
}
