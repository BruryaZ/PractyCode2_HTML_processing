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

IEnumerable<IEnumerable<string>> HTMLLis;
foreach (var line in HTMLlines)
    HTMLLis.Add(new Regex("([^\\s]*?)=\"(.*?)\"").Matches(HTMLlines));

foreach (var line in HTMLlines)
{
    Console.WriteLine(line);
}

// custing to object