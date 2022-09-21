using NoteTree.Interfaces;
using ReactiveUI;
using System.Collections.ObjectModel;

namespace NoteTree.Models
{
    public class RootElementModel : ReactiveObject, IFileStructureElement, IParentElement<ITreeElement>
    {
        public string Path { get; set; }
        public string Name { get; set; }
        public ObservableCollection<ITreeElement> Children { get; set; }
    }
}
