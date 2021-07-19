using Newtonsoft.Json;
using ReactiveUI;

namespace NoteTree.Models
{
    public class TextNoteModel : NoteModelBase
    {
        [JsonProperty("content")]
        public string Content
        {
            get => content;
            set => this.RaiseAndSetIfChanged(ref content, value, nameof(Content));
        }
        private string content;
    }
}