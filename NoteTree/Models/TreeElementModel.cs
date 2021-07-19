using DynamicData;
using NoteTree.Services;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Linq;

namespace NoteTree.Models
{
    public class TreeElementModel : ReactiveObject
    {
        public bool IsExpanded
        {
            get => isExpanded;
            set
            {
                if (value)
                {
                    foreach (var child in Children.Where(e => e.Type == TreeElementTypeEnum.Folder))
                    {
                        if (child.Children.Count == 0)
                        {
                            child.Children.AddRange(DirectoriesManager.GetDirectories(child));
                        }
                    }
                }
                this.RaiseAndSetIfChanged(ref isExpanded, value, nameof(IsExpanded));
                this.RaisePropertyChanged(nameof(IconPath));
            }
        }
        private bool isExpanded;

        public TreeElementTypeEnum Type { get; set; }

        public string Path { get; set; }

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

        public string Label { get; set; }

        public ObservableCollection<TreeElementModel> Children { get; set; }

        public TreeElementModel Parent { get; set; }

    }
}
