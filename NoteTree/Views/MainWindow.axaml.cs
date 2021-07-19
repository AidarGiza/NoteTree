using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using NoteTree.ViewModels;

namespace NoteTree.Views
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            DataContextChanged += MainWindow_DataContextChanged;
        }

        private void MainWindow_DataContextChanged(object sender, System.EventArgs e)
        {
            if (DataContext is MainWindowViewModel)
            {
                (DataContext as MainWindowViewModel).UserControl = this;
                (DataContext as MainWindowViewModel).SetUiEvents();
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
