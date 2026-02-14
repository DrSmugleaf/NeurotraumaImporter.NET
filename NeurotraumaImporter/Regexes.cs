using System.Text.RegularExpressions;

namespace NeurotraumaImporter;

public partial class Regexes
{
    [GeneratedRegex(@"trello.com/b/([^/]+)")]
    public static partial Regex WikiRegex();

    [GeneratedRegex(@"\[https://trello.com/c/.+?/\d+-(.+?)\]\(.+?\)")]
    public static partial Regex LinkRegex();

    [GeneratedRegex("(#{1,}) (.+)")]
    public static partial Regex SpanRegex();
}
