using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Generators;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using NoteTree.Interfaces;
using NoteTree.Models;
using NoteTree.Services;
using NoteTree.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NoteTree.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<ITreeElement> TreeElements => App.DataManager?.TreeElements;
        public ObservableCollection<TagModel> Tags => App.DataManager?.AllTags?.Tags;

        public TreeElementModel SelectedTreeItem
        {
            get => selectedTreeItem;
            set
            {
                //if (value != null)
                //{
                //    if (value.Type == TreeElementTypeEnum.Note)
                //    {
                //        OpenNote(value.Path);
                //    }
                //}
                this.RaiseAndSetIfChanged(ref selectedTreeItem, value, nameof(SelectedTreeItem));
                this.RaisePropertyChanged(nameof(IsAddCommandsVisible));
            }
        }
        private TreeElementModel selectedTreeItem;

        public NoteModelBase SelectedNote
        {
            get => selectedNote;
            set => this.RaiseAndSetIfChanged(ref selectedNote, value, nameof(SelectedNote));
        }
        private NoteModelBase selectedNote;

        public TagModel SelectedTag
        {
            get => selectedTag;
            set => this.RaiseAndSetIfChanged(ref selectedTag, value, nameof(SelectedTag));
        }
        private TagModel selectedTag;

        public bool ShowContextMenu => SelectedTreeItem != null;

        public bool IsAddCommandsVisible => SelectedTreeItem != null && SelectedTreeItem.Type == TreeElementTypeEnum.Folder;

        public TreeView elementsTree;
        public TreeView tagsTree;

        internal void SetUiEvents()
        {
            if (UserControl != null)
            {
                elementsTree = (UserControl as MainWindow).FindControl<TreeView>("ElementsTree");
                tagsTree = (UserControl as MainWindow).FindControl<TreeView>("TagsTree");
                if (elementsTree != null)
                {
                    elementsTree.DoubleTapped += ElementsTree_DoubleTapped;
                    elementsTree.ContextMenu.ContextMenuOpening += ContextMenu_ContextMenuOpening;
                }
                if (tagsTree != null)
                {
                    tagsTree.ContextMenu.ContextMenuOpening += tagsTree_ContextMenuOpening;
                }
            }
        }

        private void tagsTree_ContextMenuOpening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (tagsTree?.ItemContainerGenerator?.Containers != null && tagsTree?.ItemContainerGenerator?.Containers.Count() > 0)
            {
                CheckChilderen(tagsTree.ItemContainerGenerator.Containers, tagsTree);
            }
        }

        private void ContextMenu_ContextMenuOpening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (elementsTree?.ItemContainerGenerator?.Containers != null && elementsTree?.ItemContainerGenerator?.Containers.Count() > 0)
            {
                CheckChilderen(elementsTree.ItemContainerGenerator.Containers, elementsTree);
            }
        }

        public MainWindowViewModel()
        {
            if (App.Config?.RootFolderPath != null)
            {
                App.DataManager.TreeElements = new ObservableCollection<ITreeElement>();
                var name = Path.GetFileName(App.Config.RootFolderPath);

                TreeElementModel rootElement = new TreeElementModel()
                {
                    Path = App.Config.RootFolderPath,
                    Name = name,
                    Children = new ObservableCollection<ITreeElement>()
                };

                var firstLevelElements = DirectoriesManager.GetDirectories(rootElement);

                foreach (var element in firstLevelElements)
                {
                    TreeElements.Add(element as ITreeElement);
                    element.Children = DirectoriesManager.GetDirectories(rootElement);

                }

                //TreeElements.Add(rootFolder);
                //rootFolder.Children = DirectoriesManager.GetDirectories(rootFolder);
                //foreach (var child in rootFolder.Children)
                //{
                //    TreeElements.Add(child);
                //}

                App.DataManager.AllTags = TagsCollectionModel.LoadCollection(App.Config.RootFolderPath);

                LoadEveryNotes();

                this.RaisePropertyChanged(nameof(TreeElements));
            }
        }


        void CheckChilderen(IEnumerable<ItemContainerInfo> list, TreeView tree)
        {
            foreach (var item in list)
            {
                if (item.ContainerControl.IsPointerOver) tree.SelectedItem = item.Item;
                if ((item.ContainerControl as ItemsControl).ItemContainerGenerator.Containers != null && (item.ContainerControl as ItemsControl).ItemContainerGenerator.Containers.Count() > 0) CheckChilderen((item.ContainerControl as ItemsControl).ItemContainerGenerator.Containers, tree);
            }
        }

        private void ElementsTree_DoubleTapped(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (SelectedTreeItem != null)
            {
                if (SelectedTreeItem.Type == TreeElementTypeEnum.Note) OpenNote(SelectedTreeItem.Path);
                else if (SelectedTreeItem.Type == TreeElementTypeEnum.Folder) SelectedTreeItem.IsExpanded = true;
            }
        }

        private void OpenNote(string path)
        {
            //using (StreamReader reader = new StreamReader(path))
            //{
            //    string textFromFile = reader.ReadToEnd();
            //    byte[] data = Convert.FromBase64String(textFromFile);
            //    MemoryStream ms = new MemoryStream(data);
            //    using (BsonDataReader bsonReader = new BsonDataReader(ms))
            //    {
            //        JsonSerializer serializer = new JsonSerializer();
            //        TextNoteModel noteModel = serializer.Deserialize<TextNoteModel>(bsonReader);
            //        if (noteModel == null)
            //        {
            //            noteModel = new TextNoteModel();
            //            noteModel.Date = new DateTime(DateTime.Now.Ticks, DateTimeKind.Utc);
            //        }
            //        noteModel.Path = path;
            //        SelectedNote = noteModel;
            //    }
            //}
            if (App.DataManager?.AllNotes != null)
            {
                SelectedNote = App.DataManager.AllNotes.FirstOrDefault(n => n.Path == path);
            }
        }

        private NoteModelBase ParseNote(string data)
        {
            return new TextNoteModel();
        }

        public async Task<bool> SelectRoot()
        {
            App.DataManager.TreeElements = new ObservableCollection<ITreeElement>();
            OpenFolderDialog openFolderDialog = new OpenFolderDialog();
            openFolderDialog.Title = "Выбор корневой директории";
            var dir = await openFolderDialog.ShowAsync(UserControl as MainWindow);
            if (dir != null)
            {
                var name = Path.GetFileName(dir);

                RootElementModel rootFolder = new RootElementModel()
                {
                    Path = dir,
                    Name = name,
                    Children = new ObservableCollection<ITreeElement>()
                };

                //TreeElements.Add(rootFolder);
                rootFolder.Children = DirectoriesManager.GetDirectories(rootFolder);
                foreach (var child in rootFolder.Children)
                {
                    TreeElements.Add(child as ITreeElement);
                }

                App.Config.RootFolderPath = dir;
                ConfigModel.SaveConfig(App.Config);

                App.DataManager.AllTags = TagsCollectionModel.LoadCollection(dir);

                LoadEveryNotes();

                this.RaisePropertyChanged(nameof(Tags));
                this.RaisePropertyChanged(nameof(TreeElements));

                return true;
            }
            else return false;
        }

        public void LoadEveryNotes()
        {
            App.DataManager.AllNotes = new ObservableCollection<NoteModelBase>();
            if (App.Config.RootFolderPath != null)
            {
                LoadNotesFromDirectory(App.Config.RootFolderPath);
            }
        }

        public void LoadNotesFromDirectory(string path)
        {
            var directories = Directory.GetDirectories(path);
            var files = Directory.GetFiles(path, "*.ntn");

            if (files != null && files.Length > 0)
            {
                foreach (var file in files)
                {
                    try
                    {
                        using (StreamReader reader = new StreamReader(file))
                        {
                            string textFromFile = reader.ReadToEnd();
                            byte[] data = Convert.FromBase64String(textFromFile);
                            using (MemoryStream ms = new MemoryStream(data))
                            {
                                using (BsonDataReader bsonReader = new BsonDataReader(ms))
                                {
                                    JsonSerializer serializer = new JsonSerializer();
                                    NoteModelBase noteModel = serializer.Deserialize<NoteModelBase>(bsonReader);
                                    if (noteModel != null) App.DataManager.AllNotes.Add(noteModel);
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.Write(e);
                    }
                }
            }

            if (directories != null && directories.Length > 0)
            {
                foreach (var dir in directories)
                {
                    LoadNotesFromDirectory(dir);
                }
            }
        }

        public void Exit()
        {
            App.MainWindow.Close();
        }

        public async void CreateMainTag()
        {
            var tag = await CreateTag();
            if (tag != null)
            {
                App.DataManager.AllTags.Tags.Add(tag);
                App.DataManager.AllTags.SaveCollection(App.Config.RootFolderPath);
            }
            this.RaisePropertyChanged(nameof(Tags));
        }

        public async void AddTag()
        {
            if (SelectedTag != null)
            {
                var tag = await CreateTag();
                if (tag != null)
                {
                    if (SelectedTag.Children == null) SelectedTag.Children = new ObservableCollection<TagModel>();
                    SelectedTag.Children.Add(tag);
                    tag.Parent = SelectedTag;
                    App.DataManager.AllTags.SaveCollection(App.Config.RootFolderPath);
                }
                this.RaisePropertyChanged(nameof(Tags));
            }
        }

        public async void EditSelectedTag()
        {
            if (SelectedTag != null)
            {
                TagModel tagToEdit = new TagModel()
                {
                    Name = SelectedTag.Name,
                    FontColorString = SelectedTag.FontColorString,
                    BackgroundColorString = SelectedTag.BackgroundColorString
                };
                TagCreatorWindow tagCreatorWindow = new TagCreatorWindow()
                {
                    DataContext = new TagCreatorViewModel()
                };
                (tagCreatorWindow.DataContext as TagCreatorViewModel).UserControl = tagCreatorWindow;
                (tagCreatorWindow.DataContext as TagCreatorViewModel).TagToEdit = tagToEdit;
                await tagCreatorWindow.ShowDialog((App.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime).MainWindow);
                if (tagCreatorWindow.Result)
                {
                    var notesToEdit = App.DataManager.AllNotes.Where(n => n.Tags.Any(t => t.Name == SelectedTag.Name && t.FontColorString == SelectedTag.FontColorString && t.BackgroundColorString == SelectedTag.BackgroundColorString));
                    foreach (var note in notesToEdit)
                    {
                        var t = note.Tags.FirstOrDefault(t => t.Name == SelectedTag.Name && t.FontColorString == SelectedTag.FontColorString && t.BackgroundColorString == SelectedTag.BackgroundColorString);
                        t.Name = tagToEdit.Name;
                        t.FontColorString = tagToEdit.FontColorString;
                        t.BackgroundColorString = tagToEdit.BackgroundColorString;

                        SaveNote(note);
                    }

                    SelectedTag.Name = tagToEdit.Name;
                    SelectedTag.FontColorString = tagToEdit.FontColorString;
                    SelectedTag.BackgroundColorString = tagToEdit.BackgroundColorString;
                }
            }
        }

        async Task<TagModel> CreateTag()
        {
            TagModel newTag = new TagModel();
            TagCreatorWindow tagCreatorWindow = new TagCreatorWindow()
            {
                DataContext = new TagCreatorViewModel()
            };
            (tagCreatorWindow.DataContext as TagCreatorViewModel).UserControl = tagCreatorWindow;
            (tagCreatorWindow.DataContext as TagCreatorViewModel).TagToEdit = newTag;
            await tagCreatorWindow.ShowDialog(App.MainWindow);
            if (tagCreatorWindow.Result) return newTag;
            else return null;
        }

        void DelSelectedTag()
        {
            if (SelectedTag != null)
            {
                if (SelectedTag.Parent != null)
                {
                    SelectedTag.Parent.Children.Remove(SelectedTag);
                }
                else
                {
                    App.DataManager.AllTags.Tags.Remove(SelectedTag);
                }
                App.DataManager.AllTags.SaveCollection(App.Config.RootFolderPath);

                this.RaisePropertyChanged(nameof(Tags));
            }
        }

        public void AddTagToNote(TagModel tag)
        {
            if (selectedNote != null)
            {
                if (SelectedNote.Tags != null && !SelectedNote.Tags.Any(t => t.Name == tag.Name && t.FontColorString == tag.FontColorString && t.BackgroundColorString == tag.BackgroundColorString))
                {
                    SelectedNote.Tags.Add(tag);
                }
            }
        }

        public void AddFolder()
        {
            int i = 1;
            string name = "New Folder";
            while (Directory.Exists($"{SelectedTreeItem.Path}/{name}"))
            {
                name = $"New Folder {i}";
                i++;
            }
            Directory.CreateDirectory($"{SelectedTreeItem.Path}/{name}");
            var newFolderElement = new TreeElementModel()
            {
                Path = $"{SelectedTreeItem.Path}/{name}",
                Name = name,
                Type = TreeElementTypeEnum.Folder,
                Children = new ObservableCollection<ITreeElement>(),
                Parent = SelectedTreeItem
            };
            SelectedTreeItem.Children.Add(newFolderElement);
            SelectedTreeItem.IsExpanded = true;
        }

        private async Task<bool> CheckRootFolder()
        {
            if (App.Config.RootFolderPath == null)
            {
                var chooseRootFolderResult = await MessageBox.Show("Корневая папка не выбрана.\nВыбрать сейчас?", "Выбрать корневую папку?", MessageBoxButton.YesNo, MessageBoxImage.Question, UserControl as MainWindow);
                if (chooseRootFolderResult == MessageBoxResult.Yes)
                {
                    return await SelectRoot();
                }
                return false;
            }
            return true;
        }

        public async void NewTextNote()
        {
            if (!await CheckRootFolder()) return;

            TreeElementModel folder = SelectedTreeItem.Type == TreeElementTypeEnum.Folder ? SelectedTreeItem : SelectedTreeItem.Parent as TreeElementModel;

            if (folder != null && folder.Type == TreeElementTypeEnum.Folder)
            {
                var textNote = new TextNoteModel();
                textNote.Date = new DateTime(DateTime.Now.Ticks, DateTimeKind.Utc);

                MemoryStream ms = new MemoryStream();
                using (BsonDataWriter writer = new BsonDataWriter(ms))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(writer, textNote);
                }

                string data = Convert.ToBase64String(ms.ToArray());
                int i = 1;
                string name = "New Text Note";
                while (File.Exists($"{folder.Path}/{name}.ntn"))
                {
                    name = $"New Text Note {i}";
                    i++;
                }
                StreamWriter sw = new StreamWriter($"{folder.Path}/{name}.ntn");
                sw.Write(data);
                sw.Close();
                var newNoteElement = new TreeElementModel()
                {
                    Path = $"{folder.Path}/{name}.ntn",
                    Name = name,
                    Type = TreeElementTypeEnum.Note,
                    Parent = folder
                };
                folder.Children.Add(newNoteElement);
                folder.IsExpanded = true;
            }
        }

        public async void NewExtendedTextNote()
        {
            if (!await CheckRootFolder()) return;

        }

        public async void NewCornellNote()
        {
            if (!await CheckRootFolder()) return;

        }

        public void SaveNote(NoteModelBase note)
        {
            MemoryStream ms = new MemoryStream();
            using (BsonDataWriter writer = new BsonDataWriter(ms))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(writer, note);
            }

            string data = Convert.ToBase64String(ms.ToArray());

            StreamWriter sw = new StreamWriter(note.Path);
            sw.Write(data);
            sw.Close();
        }

        public void SaveSelected()
        {
            if (SelectedNote != null)
            {
                SaveNote(SelectedNote);
            }
        }

        public async void DeleteSelected()
        {
            if (SelectedTreeItem != null)
            {
                IParentElement<ITreeElement> nextSelected = null;

                if (SelectedTreeItem.Type == TreeElementTypeEnum.Folder)
                {
                    if (Directory.GetFileSystemEntries(SelectedTreeItem.Path).Length != 0)
                    {
                        var deleteFolderResult = await MessageBox.Show("Папка не пуста, удалить ее со всем содержимым?", "Удалить?", MessageBoxButton.YesNo, MessageBoxImage.Question, UserControl as MainWindow);
                        if (deleteFolderResult == MessageBoxResult.Yes)
                        {
                            Directory.Delete(SelectedTreeItem.Path, true);
                        }
                    }
                    else
                    {
                        Directory.Delete(SelectedTreeItem.Path);
                    }
                }
                else if (SelectedTreeItem.Type == TreeElementTypeEnum.Note)
                {
                    File.Delete(SelectedTreeItem.Path);
                }

                if (SelectedTreeItem.Parent != null)
                {
                    nextSelected = SelectedTreeItem.Parent;
                    nextSelected.Children.Remove(SelectedTreeItem);
                }
                else
                {
                    if (TreeElements.Count > 0)
                    {
                        nextSelected = TreeElements.FirstOrDefault();
                    }
                }
                SelectedTreeItem = nextSelected as TreeElementModel;
            }
            
            
            
            //if (SelectedTreeItem != null)
            //{
            //    DirectoriesManager.UpdateFolder(SelectedTreeItem);
            //}
            this.RaisePropertyChanged(nameof(TreeElements));
        }
    }
}
