using System.Text.RegularExpressions;

namespace NeurotraumaImporter;

public partial class Regexes
{
    [GeneratedRegex(@"\[https://trello.com/c/.+?/\d+-(.+?)\]\(.+?\)")]
    public static partial Regex LinkRegex();

    [GeneratedRegex("(#{1,}) (.+)")]
    public static partial Regex SpanRegex();
}
