using Newtonsoft.Json;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;

namespace NoteTree.Models
{
    public class NoteModelBase : ReactiveObject
    {
        [JsonProperty("name")]
        public string Name
        {
            get => name;
            set => this.RaiseAndSetIfChanged(ref name, value, nameof(Name));
        }
        private string name;

        [JsonProperty("path")]
        public string Path
        {
            get => path;
            set => this.RaiseAndSetIfChanged(ref path, value, nameof(Path));
        }
        private string path;

        [JsonProperty("date")]
        public DateTime Date
        {
            get => date;
            set => this.RaiseAndSetIfChanged(ref date, value, nameof(Date));
        }
        private DateTime date;

        [JsonProperty("tags")]
        public ObservableCollection<TagModel> Tags
        {
            get => tags;
            set => this.RaiseAndSetIfChanged(ref tags, value, nameof(Tags));
        }
        private ObservableCollection<TagModel> tags;

        public NoteModelBase()
        {
            Tags = new ObservableCollection<TagModel>();
        }

        public void DeleteTag(TagModel tag)
        {
            Tags.Remove(tag);
        }
    }
}