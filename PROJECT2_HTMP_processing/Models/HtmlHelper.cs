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
        private readonly List<string> _HTMLTags = new List<string>();
        private readonly List<string> _HTMLTagsVoid = new List<string>();

        public List<string> HTMLTags => _HTMLTags;
        public List<string> HTMLTagsVoid => _HTMLTagsVoid;
        public HTMLHelper()
        {
            _HTMLTags = JsonSerializer.Deserialize<List<string>>(File.ReadAllText("Files/HTMLTags.json"));
            _HTMLTagsVoid = JsonSerializer.Deserialize<List<string>>(File.ReadAllText("Files/HTMLVoidTag.json"));
        }
    }
}