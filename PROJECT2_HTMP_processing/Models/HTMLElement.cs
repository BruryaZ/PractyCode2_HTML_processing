using PROJECT2_HTMP_processing.Entities;
using PROJECT2_HTMP_processing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
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

        public List<HTMLElement> FindElements(Selector element, HTMLElement current, HashSet<HTMLElement> results)
        {
            if (current == null && element != null)
                return null;

            if (element == null)
                return results.ToList();

            if (IsTheSameSelector(element, current))
            {
                foreach (var des in current.Children)
                {
                    if (FindElements(element, des, results) != null)
                    {
                        results.Add(current);
                        break;
                    }
                }

                if(current.Children.Count == 0)
                    results.Add(current);
            }

            foreach (var des in current.Children)
            {
                FindElements(element, des, results);
            }

            return results.ToList();
        }

        public bool IsTheSameSelector(Selector selector, HTMLElement element)
        {
            if (element == null)
                return true;

            if (selector == null && element == null)
                return true;

            if (selector == null || element == null)
                return false;

            if (selector.TagName != null)
                if (element.Name == null || element.Name != selector.TagName)
                    return false;

            if (selector.Classes.Count > 0 && !selector.Classes.All(c => element.Classes.Contains(c)))
                return false;

            if (selector.Id != null)
                if (element.Id == null || element.Id != selector.Id)
                    return false;

            return true;
        }
    }
}