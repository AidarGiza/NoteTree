using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace NoteTree.Views.NoteTypesViews
{
    public class CornellNoteView : UserControl
    {
        public CornellNoteView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
