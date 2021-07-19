using System;
using System.Collections.Generic;
using System.Text;
using Avalonia.Controls;
using ReactiveUI;

namespace NoteTree.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        public ContentControl UserControl { get; set; }
    }
}
