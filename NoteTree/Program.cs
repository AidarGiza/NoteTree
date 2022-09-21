using Avalonia;
using Avalonia.ReactiveUI;
using System;

namespace NoteTree
{
    class Program
    {
        //public static string[] _args;
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        //public static void Main(string[] args)
        //{
        //    _args = args;
        //    BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        //}



        [STAThread]
        public static void Main(string[] args) => BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);


        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI();

        public static void AppMain(Application app, string[] args)
        {
            

            


            //App.MainWindow = new MainWindow
            //{
            //    DataContext = new MainWindowViewModel(),
            //};
            //(App.MainWindow.DataContext as MainWindowViewModel).UserControl = App.MainWindow;
            //if (App.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            //{
            //    desktop.MainWindow = App.MainWindow;
            //}
        }

        
    }
}
