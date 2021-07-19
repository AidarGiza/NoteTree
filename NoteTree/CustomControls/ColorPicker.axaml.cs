using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace NoteTree.CustomControls
{
    public class ColorPicker : UserControl
    {
        public ColorPicker()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
