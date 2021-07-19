using NoteTree.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace NoteTree.Services
{
    public class DataManager
    {
        public ObservableCollection<TreeElementModel> TreeElements;

        public TagsCollectionModel AllTags;

        public ObservableCollection<NoteModelBase> AllNotes;
        
        public DataManager()
        {
            TreeElements = new ObservableCollection<TreeElementModel>();
        }
    }
}
