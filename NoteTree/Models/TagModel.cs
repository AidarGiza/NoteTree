using Newtonsoft.Json;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;

namespace NoteTree.Models
{
    public class TagModel : ReactiveObject
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("background_color")]
        public string BackgroundColorString
        {
            get
            {
                if (BackgroundColor != null) return BackgroundColor.GetARGB();
                else return "#ffffffff";
            }
            set => BackgroundColor = ArgbColorModel.Parse(value);
        }

        [JsonProperty("font_color")]
        public string FontColorString 
        {
            get
            {
                if (FontColor != null) return FontColor.GetARGB();
                else return "#ffffffff";
            }
            set => FontColor = ArgbColorModel.Parse(value);
        }

        [JsonProperty("children")]
        public ObservableCollection<TagModel> Children { get; set; }

        public ArgbColorModel BackgroundColor
        {
            get => backgroundColor;
            set => this.RaiseAndSetIfChanged(ref backgroundColor, value, nameof(BackgroundColor));
        }
        private ArgbColorModel backgroundColor;

        public ArgbColorModel FontColor
        {
            get => fontColor;
            set => this.RaiseAndSetIfChanged(ref fontColor, value, nameof(FontColor));
        }
        private ArgbColorModel fontColor;

        public TagModel Parent { get; set; }

        public bool IsExpanded { get; set; }

        public TagModel()
        {
            Children = new ObservableCollection<TagModel>();

            Random rnd = new Random();

            BackgroundColor = new ArgbColorModel()
            {
                A = 255,
                R = (byte)rnd.Next(0, 255),
                G = (byte)rnd.Next(0, 255),
                B = (byte)rnd.Next(0, 255),
            };
            FontColor = new ArgbColorModel()
            {
                A = 255
            };
        }
    }
}
