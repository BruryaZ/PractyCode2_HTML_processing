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
            Atributes = new List<string>();
            Classes = new List<string>();
        }
        public override string ToString()
        {
            string elem = $"Id: " + Id + " Name: " + Name + " Atributes: ";
            //foreach (var at in Atributes)
            //    elem += at + ", ";
            //elem += " Classes: ";
            //foreach (var cl in Classes)
            //    elem += cl + ", ";
            //elem += " InnerHTML: " + InnerHTML;
            return elem;
        }
        public HTMLElement(string id, string name, string innerHTML, HTMLElement child, HTMLElement parent):base()
        {
            Id = id;
            Name = name;
            InnerHTML = innerHTML;
            Child = child;
            Parent = parent;
        }
    }
}
