using PROJECT2_HTMP_processing.Entities;
using PROJECT2_HTMP_processing.Models;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Xml.Linq;

static async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}

//var html = await Load(" https://learn.malkabruk.co.il/practicode");
var html = await Load("https://studio-chen.co.il/");

// filter the spaces
var cleanHTML = new Regex("\\s").Replace(html, " ");

// separate the HTML to lines
IEnumerable<string> HTMLlines = new Regex("<(.*?)>").Split(cleanHTML).Where(h => h.Length > 0 && h[0] != ' ');

// Building the tree
//root - HTML tag
HTMLElement root = new HTMLElement(), tmpElement, currentElement;
string rootLine = "", currentLine = HTMLlines.First(), tagName = " ";
foreach (string line in HTMLlines)
{
    if (line.Contains("html") && (!line.Contains("!DOCTYPE html")))
    {
        Console.WriteLine(line);
        rootLine = line;
        break;
    }
}
root.Name = "html";
root = FullFields(rootLine, root);
currentElement = root;

// building the tree
foreach (string line in HTMLlines)
{
    if (currentLine != "html/")
    {
        //?
        break;
    }

    tagName = GetFirstWord(line);

    // new tag
    if(HTMLHelper.HTMLTags.Contains(tagName))
    {
        tmpElement = new HTMLElement();
        tmpElement.Name = tagName;
        tmpElement = FullFields(line, tmpElement);
    }
}

// auxilary function 
static string GetFirstWord(string str)
{
    // פיצול המחרוזת לרשימה של מילים
    string[] words = str.Split(' ');

    // החזרת המילה הראשונה
    return words.Length > 0 ? words[0] : string.Empty;
}
static string RemoveFirstWord(string str)
{
    // פיצול המחרוזת לרשימה של מילים
    string[] words = str.Split(' ');

    // אם יש יותר ממילה אחת, מחזירים את המילים מלבד הראשונה
    return words.Length > 1 ? string.Join(" ", words, 1, words.Length - 1) : string.Empty;
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
            if (match.Value.IndexOf("class") != -1)
                foreach (var cl in match.Groups[0].Value.Split(' '))
                    element.Classes.Add(cl);

            else if (match.Value.IndexOf("id") == -1)
                element.Id = match.Value;
            
            else
                attributes.Add(match.Value);
        }
    }

    element.Atributes = attributes;
    return element;
}