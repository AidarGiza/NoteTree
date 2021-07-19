using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using NoteTree.Services;

namespace NoteTree.Views
{
    public class MessageBoxView : Window
    {
        public MessageBoxResult Result { get; set; }

        public MessageBoxView()
        {
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
