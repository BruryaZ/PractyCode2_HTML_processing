using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROJECT2_HTMP_processing.Entities
{
    public class HTMLElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Atributes { get; set; }
        public List<string> Classes { get; set; }
        public string InnerHTML { get; set; }

        public HTMLElement Child { get; set; }
        public HTMLElement Parent { get; set; }
        public HTMLElement()
        {
        }

        public HTMLElement(string id, string name, List<string> atributes, List<string> classes, string innerHTML, HTMLElement child, HTMLElement parent)
        {
            Id = id;
            Name = name;
            Atributes = atributes;
            Classes = classes;
            InnerHTML = innerHTML;
            Child = child;
            Parent = parent;
        }
    }
}
