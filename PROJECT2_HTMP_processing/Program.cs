using PROJECT2_HTMP_processing.Models;
using System.Text.RegularExpressions;

static async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}

var html = await Load("https://studio-chen.co.il/");

// fillter the spaces
var cleanHTML = new Regex("\\s").Replace(html, "");

// separate the HTML to lines
IEnumerable<string> HTMLlines = new Regex("<(.*?)>").Split(cleanHTML).Where(h => h.Length > 0);

// separate the deatails
List<List<string>> HTMLList = new List<List<string>>();


foreach (var line in HTMLlines)
{
    var matches = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(line);
    List<string> attributes = new List<string>();

    foreach (Match match in matches)
    {
        // Add the matched attribute to the list
        attributes.Add(match.Value);
    }

    // Add the attributes list to HTMLList
    HTMLList.Add(attributes);
}

//foreach (var line in HTMLList)
//{
//    foreach (var match in line)
//    {
//        Console.Write(match);
//    }
//    Console.WriteLine();
// custing to object
//}
HTMLHelper hTMLHelper = new HTMLHelper();
foreach (string line in hTMLHelper.HTMLTagsVoid)
    Console.WriteLine(line);