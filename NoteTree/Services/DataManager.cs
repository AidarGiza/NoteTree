using NoteTree.Interfaces;
using NoteTree.Models;
using System.Collections.ObjectModel;

namespace NoteTree.Services
{
    public class DataManager
    {
        public ObservableCollection<ITreeElement> TreeElements { get; set; }

        public TagsCollectionModel AllTags { get; set; }

        public ObservableCollection<NoteModelBase> AllNotes { get; set; }
        
        public DataManager()
        {
            TreeElements = new ObservableCollection<ITreeElement>();
        }
    }
}
