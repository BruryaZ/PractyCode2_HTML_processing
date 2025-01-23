using PROJECT2_HTMP_processing.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROJECT2_HTMP_processing.Models
{
    public class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; }
        public Selector Parent { get; set; }
        public Selector Child { get; set; }
        public Selector() 
        {
            this.Classes = new List<string>(); 
            this.Parent = null;
            this.Child = null;
        }

        public Selector(string tagName, string id, List<string> classes):base() 
        {
            TagName = tagName;
            Id = id;
            this.Classes = classes;
        }

        public static Selector QueryToSelector(string query)
        {
            Selector root = new Selector(), tmp = null , current = root;
            List<string> conditions ;
            string[] separateQuery = query.Split(' ');

            foreach (string s in separateQuery)
            {
                tmp = new Selector();
                conditions = SeparateString(s);

                foreach (string s2 in conditions)
                {
                    if (HTMLHelper.HTMLTags.Contains(s2))
                    {
                        tmp.TagName = s2;
                    }
                    else if (s2.StartsWith("#"))
                    {
                        tmp.Id = s2.Substring(1); 
                    }
                    else if (s2.StartsWith("."))
                    {
                        tmp.Classes.Add(s2.Substring(1)); 
                    }
                }
            }
            // check parent and child
            current.Child = tmp;
            tmp.Parent = current;
            current = current.Child;
            return root;
        }

        static List<string> SeparateString(string input)
        {
            List<string> result = new List<string>();

            var parts = input.Split(new char[] { '#', '.' }, StringSplitOptions.None);

            result.Add(parts[0]);

            for (int i = 1; i < parts.Length; i++)
            {
                if (input.Contains("#" + parts[i]))
                {
                    result.Add("#" + parts[i]);
                }
                else if (input.Contains("." + parts[i]))
                {
                    result.Add("." + parts[i]);
                }
            }

            return result;
        }
    }
}
