using NoteTree.Models;
using NoteTree.Views;
using ReactiveUI;

namespace NoteTree.ViewModels
{
    public class TagCreatorViewModel : ViewModelBase
    {
        public TagModel TagToEdit
        {
            get => tagToEdit;
            set => this.RaiseAndSetIfChanged(ref tagToEdit, value, nameof(TagToEdit));
        }
        private TagModel tagToEdit;

        public void Save()
        {
            (UserControl as TagCreatorWindow).Result = true;
            (UserControl as TagCreatorWindow).Close();
        }

        public void Cancel()
        {
            (UserControl as TagCreatorWindow).Result = false;
            (UserControl as TagCreatorWindow).Close();
        }
    }
}
