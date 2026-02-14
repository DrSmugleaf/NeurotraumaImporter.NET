using System.Globalization;
using System.Text;
using Manatee.Trello;
using NeurotraumaImporter.NET;

var auth = new TrelloAuthorization
{
    AppKey = Environment.GetEnvironmentVariable("TRELLO_KEY"),
    UserToken = Environment.GetEnvironmentVariable("TRELLO_TOKEN")
};

var listNames = new[]
{
    "Symptoms", "Head/Brain", "Lungs", "Heart", "Torso", "Extremities", "Bones", "Any bodypart", "Blood", "Surgery",
    "Procedures", "Items"
};

Console.WriteLine("Paste the link to Neurotrauma's Trello wiki and press Enter:");
if (Console.ReadLine() is not { } link)
    return;

var boardIdMatch = Regexes.WikiRegex().Match(link);
if (!boardIdMatch.Success)
{
    Console.WriteLine($"No board id found in link {link}");
    return;
}

Console.Clear();
var board = new Board(boardIdMatch.Groups[1].Value, auth);
await board.Refresh();

var cardText = new StringBuilder();
var linkRegex = Regexes.LinkRegex();
var spanRegex = Regexes.SpanRegex();
foreach (var list in board.Lists.Where(l => listNames.Contains(l.Name)).OrderBy(l => listNames.IndexOf(l.Name)))
{
    cardText.AppendLine($"# {list.Name}");

    await list.Cards.Refresh();
    foreach (var card in list.Cards.OrderBy(c => c.Name))
    {
        var image = card.Attachments.Any() ? $"<img src=\"{card.Attachments[0].Url}\" height=32 width=32 class=\"valign-middle\">" : string.Empty;
        var name = $"## {image} {card.Name} ^{NameToTag(card.Name)}";
        var description = card.Description
            .Trim()
            .Replace("---", string.Empty)
            .Replace("\n\n", "\n");

        description = linkRegex.Replace(description, match =>
        {
            var link = match.Groups[1].Value.Replace("-", " ");
            link = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(link);
            return $"[[#^{NameToTag(link)}|{link}]]";
        });

        description = spanRegex.Replace(description,
            match => $"\n<span style=\"font-size: {20 - (match.Groups[1].Length - 1) * 2}px\">{match.Groups[2].Value}</span>");

        cardText.AppendLine($"{name}\n{description}\n---");
    }
}

Console.WriteLine(cardText);
return;

string NameToTag(string name)
{
    return name
        .Replace(" ", string.Empty)
        .Replace("&", string.Empty)
        .Replace("(", string.Empty)
        .Replace(")", string.Empty)
        .Replace("/", string.Empty);
}
