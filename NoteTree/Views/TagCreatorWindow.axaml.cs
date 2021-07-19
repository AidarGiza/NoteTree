using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace NoteTree.Views
{
    public class TagCreatorWindow : Window
    {
        public bool Result = false;
        public TagCreatorWindow()
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
