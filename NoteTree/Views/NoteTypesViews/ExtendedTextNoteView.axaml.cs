using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace NoteTree.Views.NoteTypesViews
{
    public class ExtendedTextNoteView : UserControl
    {
        public ExtendedTextNoteView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
