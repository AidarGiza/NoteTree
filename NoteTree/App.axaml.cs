using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using NoteTree.Models;
using NoteTree.Services;
using NoteTree.ViewModels;
using NoteTree.Views;
using System.Drawing;
using System.IO;

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
            Config = ConfigModel.LoadConfig();
            if (Config == null) Config = new ConfigModel();
            DataManager = new DataManager();

            LoadImgResources();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel()
                };
                (desktop.MainWindow.DataContext as MainWindowViewModel).UserControl = desktop.MainWindow;
            }

            base.OnFrameworkInitializationCompleted();
        }

        private static void LoadImgResources()
        {
#if DEBUG
            string img_dir = @"D:\Projects\CS\NoteTree\NoteTree\bin\Debug\netcoreapp3.1\Assets\icons";
#else
            string img_dir = "Assets/icons";
#endif
            DirectoryInfo dir = new DirectoryInfo(img_dir);
            var filesEnumerator = dir.EnumerateFiles().GetEnumerator();
            ResourceDictionary dict = new ResourceDictionary();
            while (filesEnumerator.MoveNext())
            {
                FileInfo info = filesEnumerator.Current;
                Bitmap _img = new Bitmap(info.FullName);
                dict.Add(info.Name, _img);
            }
            Current.Resources.MergedDictionaries.Add(dict);
        }
    }
}
