using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Logging.Serilog;
using Avalonia.Media.Imaging;
using Avalonia.ReactiveUI;
using NoteTree.ViewModels;
using NoteTree.Views;
using System.IO;

namespace NoteTree
{
    class Program
    {
        public static string[] _args;
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        public static void Main(string[] args)
        {
            _args = args;
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }
        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToDebug()
                .UseReactiveUI();

        public static void AppMain(Application app, string[] args)
        {
            App.Config = Models.ConfigModel.LoadConfig();
            if (App.Config == null) App.Config = new Models.ConfigModel();
            App.DataManager = new Services.DataManager();

            
            loadImgResources();


            App.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
            };
            (App.MainWindow.DataContext as MainWindowViewModel).UserControl = App.MainWindow;
            if (App.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = App.MainWindow;
            }
        }

        private static void loadImgResources()
        {
            string img_dir = "Assets/icons";
            DirectoryInfo dir = new DirectoryInfo(img_dir);
            var filesEnumerator = dir.EnumerateFiles().GetEnumerator();
            ResourceDictionary dict = new ResourceDictionary();
            while (filesEnumerator.MoveNext())
            {
                FileInfo info = filesEnumerator.Current;
                Bitmap _img = new Bitmap(info.FullName);
                dict.Add(info.Name, _img);
            }
            App.Current.Resources.MergedDictionaries.Add(dict);
        }
    }
}
