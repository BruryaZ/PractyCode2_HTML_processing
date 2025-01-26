using PROJECT2_HTMP_processing.Entities;
using PROJECT2_HTMP_processing.Models;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

// ממשק לטעינת HTML מדף והמרתו לאובייקטים בקוד
Console.WriteLine("Enter your URL: ");
string url = Console.ReadLine();
var html = await Load(url);//https://www.youtube.com/watch?v=KqAF4TCEXbI

// filter the spaces
var cleanHTML = new Regex("\\s").Replace(html, " ");

// separate the HTML to lines
IEnumerable<string> HTMLlines = new Regex("<(.*?)>").Split(cleanHTML).Where(h => h.Length > 0 && h[0] != ' ');

// Building the tree
// root - HTML tag
HTMLElement root = new HTMLElement(), tmpElement, currentElement;
string rootLine = "", currentLine = HTMLlines.First(), tagName = " ";
foreach (string line in HTMLlines)
{
    if (line.Contains("html") && (!line.Contains("!DOCTYPE html")))
    {
        rootLine = line;
        break;
    }
}
root.Name = "html";
root = FullFields(rootLine, root);
currentElement = root;
List<string> HTMLLST = HTMLlines.ToList();
HTMLLST.Remove("!DOCTYPE html");
HTMLLST[0] = HTMLLST[0].Replace("html ", "");
root = FullFields(HTMLLST[0], root);
HTMLLST[0] = "";

// building the tree
foreach (string line in HTMLLST)
{
    if (currentLine == "html/")
    {
        //?
        break;
    }

    tagName = GetFirstWord(line);

    // close tag
    if (currentElement != null && tagName.StartsWith('/'))
    {
        currentElement = currentElement.Parent;
    }

    // new tag
    else if(HTMLHelper.HTMLTags.Contains(tagName))
    {
        tmpElement = new HTMLElement();
        tmpElement.Name = tagName;
        tmpElement = FullFields(line, tmpElement);
        currentElement.Children.Add(tmpElement);
        tmpElement.Parent = currentElement;
        currentElement = tmpElement;
    }

    else
    {
        currentElement.InnerHTML += line;
    }
}
root.Name = "!DOCTYPE html";
PrintHtmlAsDocument(root);

// Check FindElement
var selector = Selector.QueryToSelector("head link");

var results = new HashSet<HTMLElement>();

var foundElements = root.FindElements(selector, root, results);
Console.WriteLine("resaults: ");
foreach (var element in foundElements)
{
    Console.WriteLine(element.Name);
}

// Auxilary function 
static string GetFirstWord(string str)
{
    // פיצול המחרוזת לרשימה של מילים
    string[] words = str.Split(' ');

    // החזרת המילה הראשונה
    return words.Length > 0 ? words[0] : string.Empty;
}
void PrintHtmlAsDocument(HTMLElement element, int depth = 0)
{
    // יצירת רווחים לפי העומק
    string indentation = new string(' ', depth * 2);

    // פתיחת התגית
    Console.Write($"{indentation}<{element.Name}");

    // הוספת מאפיינים (Attributes) לתגית הפתיחה
    if (element.Classes.Count > 0)
    {
        Console.Write($" class=\"{string.Join(" ", element.Classes)}\"");
    }

    // הוספת תוכן לתגית הפתיחה אם יש (כמו id, או width)
    foreach (var attribute in element.Attributes)
    {
        Console.Write($" {attribute}");
    }

    // בדיקה אם לתגית יש ילדים או תוכן
    if (element.Children.Count == 0 && string.IsNullOrEmpty(element.InnerHTML))
    {
        // תגית בודדה (self-closing tag)
        Console.WriteLine(" />");
    }
    else
    {
        // תגית רגילה
        Console.WriteLine(">");

        // הדפסת תוכן פנימי אם יש
        if (!string.IsNullOrEmpty(element.InnerHTML))
        {
            Console.WriteLine($"{indentation}  {element.InnerHTML}");
        }

        // הדפסת ילדים
        foreach (var child in element.Children)
        {
            PrintHtmlAsDocument(child, depth + 1);
        }

        // סגירת התגית
        Console.WriteLine($"{indentation}</{element.Name}>");
    }
}
static HTMLElement FullFields(string HTMLline, HTMLElement element)
{
    var matches = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(HTMLline);
    List<string> attributes = new List<string>();

    foreach (Match match in matches)
    {
        // Add the matched attribute to the list
        if (match.Success)
        {
            string attributeName = match.Groups[1].Value; // Get the attribute name
            string attributeValue = match.Groups[2].Value; // Get the attribute value

            if (attributeName == "class")
            {
                // Split the class values and add them to the element's Classes
                foreach (var cl in attributeValue.Split(' '))
                {
                    element.Classes.Add(cl);
                }
            }
            else if (attributeName == "id")
            {
                element.Id = attributeValue; // Set the element's Id
            }
            else
            {
                attributes.Add(match.Value); // Add other attributes
            }
        }
    }

    element.Attributes = attributes;
    return element;
}
static async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}