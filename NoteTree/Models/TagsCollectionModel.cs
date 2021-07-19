using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace NoteTree.Models
{
    public class TagsCollectionModel
    {
        [JsonProperty("tags_collection")]
        public ObservableCollection<TagModel> Tags { get; set; }

        public TagsCollectionModel()
        {
            Tags = new ObservableCollection<TagModel>();
        }

        public static TagsCollectionModel LoadCollection(string path)
        {
            TagsCollectionModel tagsCollectionModel = null;

            if (File.Exists($"{path}/.notetags"))
            {
                using (StreamReader reader = new StreamReader($"{path}/.notetags"))
                {
                    string textFromFile = reader.ReadToEnd();
                    byte[] data = Convert.FromBase64String(textFromFile);
                    MemoryStream ms = new MemoryStream(data);
                    using (BsonDataReader bsonReader = new BsonDataReader(ms))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        tagsCollectionModel = serializer.Deserialize<TagsCollectionModel>(bsonReader);
                    }
                }
            }
            if (tagsCollectionModel == null)
            {
                tagsCollectionModel = new TagsCollectionModel();
                tagsCollectionModel.SaveCollection(path);
            }
            SetParents(tagsCollectionModel.Tags);
            return tagsCollectionModel;
        }

        public void SaveCollection(string path)
        {
            MemoryStream ms = new MemoryStream();
            using (BsonDataWriter writer = new BsonDataWriter(ms))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(writer, this);
            }
            string data = Convert.ToBase64String(ms.ToArray());
            StreamWriter sw = new StreamWriter($"{path}/.notetags");
            sw.Write(data);
            sw.Close();
        }

        public static void SetParents(ObservableCollection<TagModel> tagsCollection)
        {
            foreach (var tag in tagsCollection)
            {
                if (tag.Children != null && tag.Children.Count > 0)
                {
                    foreach (var subtag in tag.Children)
                    {
                        subtag.Parent = tag;
                    }
                    SetParents(tag.Children);
                }
            }
        }
    }
}
