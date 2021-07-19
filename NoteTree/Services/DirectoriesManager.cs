using NoteTree.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace NoteTree.Services
{
    public static class DirectoriesManager
    {
        public static ObservableCollection<TreeElementModel> GetDirectories(TreeElementModel folderElement)
        {
            var tree = new ObservableCollection<TreeElementModel>();
            var directories = Directory.GetDirectories(folderElement.Path);
            var files = Directory.GetFiles(folderElement.Path, "*.ntn");

            foreach (var folder in directories)
            {
                var name = Path.GetFileName(folder);
                var newFolderElement = new TreeElementModel()
                {
                    Path = folder,
                    Label = name,
                    Type = TreeElementTypeEnum.Folder,
                    Children = new ObservableCollection<TreeElementModel>(),
                    Parent = folderElement
                };

                tree.Add(newFolderElement);
            }

            foreach (var note in files)
            {
                var name = Path.GetFileNameWithoutExtension(note);
                var newNoteElement = new TreeElementModel()
                {
                    Path = note,
                    Label = name,
                    Type = TreeElementTypeEnum.Note,
                    Parent = folderElement
                };

                tree.Add(newNoteElement);
            }

            return tree;
        }

        public static ObservableCollection<TreeElementModel> GetDirectories(string path)
        {
            var tree = new ObservableCollection<TreeElementModel>();
            var directories = Directory.GetDirectories(path);
            var files = Directory.GetFiles(path, "*.ntn");

            foreach (var folder in directories)
            {
                var name = Path.GetFileName(folder);
                var newFolderElement = new TreeElementModel()
                {
                    Path = folder,
                    Label = name,
                    Type = TreeElementTypeEnum.Folder,
                    Children = new ObservableCollection<TreeElementModel>()
                };

                tree.Add(newFolderElement);
            }

            foreach (var note in files)
            {
                var name = Path.GetFileNameWithoutExtension(note);
                var newNoteElement = new TreeElementModel()
                {
                    Path = note,
                    Label = name,
                    Type = TreeElementTypeEnum.Note
                };

                tree.Add(newNoteElement);
            }

            return tree;
        }

        public static void UpdateFolder(TreeElementModel folderElement)
        {
            folderElement.Children = GetDirectories(folderElement);
        }
    }
}
