using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace NoteTree.Views.NoteTypesViews
{
    public class TextNoteView : UserControl
    {
        public TextNoteView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
