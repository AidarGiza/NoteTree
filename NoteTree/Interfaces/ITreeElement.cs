using NoteTree.Models;

namespace NoteTree.Interfaces
{
    public interface ITreeElement : IParentElement<ITreeElement>
    {
        public bool IsExpanded { get; set; }

        public TreeElementTypeEnum Type { get; set; }
        
        public string IconPath { get; }

        public string Lable { get; }

        public IParentElement<ITreeElement> Parent { get; set; }
    }
}
