namespace env0.records.Engine;

public static class ChoiceInputNormalizer
{
    private static readonly HashSet<string> Determiners = new(StringComparer.OrdinalIgnoreCase)
    {
        "the",
        "a",
        "an"
    };

    public static string Normalize(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        var tokens = input
            .Trim()
            .ToLowerInvariant()
            .Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (tokens.Length == 0)
            return string.Empty;

        var filtered = new List<string>(tokens.Length);
        foreach (var token in tokens)
        {
            if (!Determiners.Contains(token))
                filtered.Add(token);
        }

        return filtered.Count == 0 ? string.Empty : string.Join(' ', filtered);
    }
}
