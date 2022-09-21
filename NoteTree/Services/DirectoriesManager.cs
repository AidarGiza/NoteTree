using NoteTree.Interfaces;
using NoteTree.Models;
using System.Collections.ObjectModel;
using System.IO;

namespace NoteTree.Services
{
    public static class DirectoriesManager
    {
        public static ObservableCollection<ITreeElement> GetDirectories(IFileStructureElement folderElement)
        {
            var tree = new ObservableCollection<ITreeElement>();
            var directories = Directory.GetDirectories(folderElement.Path);
            var files = Directory.GetFiles(folderElement.Path, "*.ntn");

            foreach (var folder in directories)
            {
                var name = Path.GetFileName(folder);
                var newFolderElement = new TreeElementModel()
                {
                    Path = folder,
                    Name = name,
                    Type = TreeElementTypeEnum.Folder,
                    Children = new ObservableCollection<ITreeElement>(),
                    Parent = folderElement as ITreeElement
                };

                tree.Add(newFolderElement);
            }

            foreach (var note in files)
            {
                var name = Path.GetFileNameWithoutExtension(note);
                var newNoteElement = new TreeElementModel()
                {
                    Path = note,
                    Name = name,
                    Type = TreeElementTypeEnum.Note,
                    Parent = folderElement as ITreeElement
                };

                tree.Add(newNoteElement);
            }

            return tree;
        }

        public static ObservableCollection<ITreeElement> GetDirectories(string path)
        {
            var tree = new ObservableCollection<ITreeElement>();
            var directories = Directory.GetDirectories(path);
            var files = Directory.GetFiles(path, "*.ntn");

            foreach (var folder in directories)
            {
                var name = Path.GetFileName(folder);
                var newFolderElement = new TreeElementModel()
                {
                    Path = folder,
                    Name = name,
                    Type = TreeElementTypeEnum.Folder,
                    Children = new ObservableCollection<ITreeElement>()
                };

                tree.Add(newFolderElement);
            }

            foreach (var note in files)
            {
                var name = Path.GetFileNameWithoutExtension(note);
                var newNoteElement = new TreeElementModel()
                {
                    Path = note,
                    Name = name,
                    Type = TreeElementTypeEnum.Note
                };

                tree.Add(newNoteElement);
            }

            return tree;
        }

        public static void UpdateFolder(IParentElement<ITreeElement> folderElement)
        {
            folderElement.Children = GetDirectories(folderElement as IFileStructureElement);
        }
    }
}
