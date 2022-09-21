using DynamicData;
using NoteTree.Interfaces;
using NoteTree.Services;
using ReactiveUI;
using System;
using System.Linq;

namespace NoteTree.Models
{
    public class TreeElementModel : RootElementModel, IDisposable, ITreeElement
    {
        private bool disposed = false;

        public bool IsExpanded
        {
            get => isExpanded;
            set
            {
                if (value)
                {
                    foreach (var child in Children.Where(e => (e as ITreeElement).Type == TreeElementTypeEnum.Folder))
                    {
                        if (child.Children.Count == 0)
                        {
                            child.Children.AddRange(DirectoriesManager.GetDirectories(child as IFileStructureElement));
                        }
                    }
                }
                this.RaiseAndSetIfChanged(ref isExpanded, value, nameof(IsExpanded));
                this.RaisePropertyChanged(nameof(IconPath));
            }
        }
        private bool isExpanded;

        public TreeElementTypeEnum Type { get; set; }

        public string Lable => Name;


        public string IconPath
        {
            get
            {
                if (Type == TreeElementTypeEnum.Folder)
                {
                    if (IsExpanded) return "icon_folder_open16.png";
                    else return "icon_folder16.png";
                }
                else if (Type == TreeElementTypeEnum.Note) return "icon_note16.png";
                else return "";
            }
        }

        public IParentElement<ITreeElement> Parent { get; set; }

        public void Dispose()
        {
            // освобождаем неуправляемые ресурсы
            Dispose(true);
            // подавляем финализацию
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;
            if (disposing)
            {
                // Освобождаем управляемые ресурсы
            }
            // освобождаем неуправляемые объекты
            disposed = true;
        }

        // Деструктор
        ~TreeElementModel()
        {
            Dispose(false);
        }
    }
}
