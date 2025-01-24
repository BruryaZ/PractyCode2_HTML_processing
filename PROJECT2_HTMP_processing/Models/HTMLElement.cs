using PROJECT2_HTMP_processing.Entities;
using PROJECT2_HTMP_processing.Models;
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
        public List<string> Attributes { get; set; }
        public List<string> Classes { get; set; }
        public string InnerHTML { get; set; }

        public List<HTMLElement> Children { get; set; }
        public HTMLElement Parent { get; set; }
        public HTMLElement()
        {
            Attributes = new List<string>();
            Classes = new List<string>();
            Parent = null;
            Children = new List<HTMLElement>();
            InnerHTML = "";
        }
        public override string ToString()
        {
            string elem = $"Id: " + Id + " Name: " + Name + " Attributes: ";
            foreach (var at in Attributes)
                elem += at + ", ";
            elem += " Classes: ";
            foreach (var cl in Classes)
                elem += cl + ", ";
            elem += " InnerHTML: " + InnerHTML + " Parent: " + Parent;
            return elem;
        }
        public HTMLElement(string id, string name, string innerHTML) : base()
        {
            Id = id;
            Name = name;
            InnerHTML = innerHTML;
        }

        public IEnumerable<HTMLElement> Descendants()
        {
            Queue<HTMLElement> queue = new Queue<HTMLElement>();
            queue.Enqueue(this);
            HTMLElement tmp = null;

            while (queue.Count > 0)
            {
                tmp = queue.Dequeue();
                yield return tmp;

                foreach (var child in tmp.Children)
                {
                    queue.Enqueue(child);
                }
            }
        }

        public IEnumerable<HTMLElement> Ancestors()
        {
            HTMLElement tmp = this;
            while (tmp.Parent != null)
            {
                tmp = tmp.Parent;
                yield return tmp;
            }
        }

        public List<HTMLElement> FindElements(Selector element, HTMLElement current, HashSet<HTMLElement> resaults)
        {
            var descendants = current.Descendants().ToList();
            foreach (var des in descendants)
            {
                if(element == null && current == null)
                    resaults.Add(des);

                if (IsTheSameSelector(element, des))
                {
                    // father
                    FindElements(element.Parent, des.Parent, resaults);
                }
            }

            return resaults.ToList();
        }

        public bool IsTheSameSelector(Selector selector, HTMLElement element)
        {
            if (selector == null && element == null)
                return true;

            if (selector == null || element == null)
                return false;

            if (element.Name != selector.TagName)
                return false;

            if (!selector.Classes.All(c => element.Classes.Contains(c)))
                return false;

            if (element.Id != selector.Id)
                return false;

            return true;
        }
    }
}
// The last space start the current selector
//"div #mydiv .class-name”
