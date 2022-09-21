using System.Collections.ObjectModel;

namespace NoteTree.Interfaces
{
    public interface IParentElement<T>
    {
        public ObservableCollection<T> Children { get; set; }
    }
}
