using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace PROJECT2_HTMP_processing.Models
{
    public class HTMLHelper
    {
        private static readonly HTMLHelper instance = new HTMLHelper();
        private static List<string> _HTMLTags = JsonSerializer.Deserialize<List<string>>(File.ReadAllText("Files/HTMLTags.json"));
        private static List<string> _HTMLTagsVoid = JsonSerializer.Deserialize<List<string>>(File.ReadAllText("Files/HTMLVoidTag.json"));

        public static HTMLHelper Instance => instance;
        public static List<string> HTMLTags => _HTMLTags;
        public static List<string> HTMLTagsVoid => _HTMLTagsVoid;

        private HTMLHelper()
        {
            try
            {
                _HTMLTags = JsonSerializer.Deserialize<List<string>>(File.ReadAllText("Files/HTMLTags.json"));
                _HTMLTagsVoid = JsonSerializer.Deserialize<List<string>>(File.ReadAllText("Files/HTMLVoidTag.json"));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading files: {ex.Message}");
            }
        }
    }
}