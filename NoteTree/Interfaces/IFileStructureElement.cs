using System.Collections.ObjectModel;

namespace NoteTree.Interfaces
{
    public interface IFileStructureElement
    {
        public string Path { get; set; }

        public string Name { get; set; }
    }
}
