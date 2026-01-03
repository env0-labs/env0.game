using env0.records.Model;

namespace env0.records.Engine;

public sealed class InputRouter
{
    private static readonly HashSet<string> HelpCommands = new(StringComparer.OrdinalIgnoreCase)
    {
        "help",
        "options"
    };

    public bool IsHelpCommand(string input)
    {
        var normalized = ChoiceInputNormalizer.Normalize(input);
        return HelpCommands.Contains(normalized);
    }

    public bool TryResolve(string input, SceneDefinition scene, out ChoiceDefinition? choice)
    {
        choice = null;

        if (string.IsNullOrWhiteSpace(input))
            return false;

        var trimmed = input.Trim();
        if (IsDigitsOnly(trimmed) && int.TryParse(trimmed, out var index))
        {
            choice = scene.Choices.FirstOrDefault(c => c.Index == index);
            return choice != null;
        }

        var normalized = ChoiceInputNormalizer.Normalize(trimmed);
        if (string.IsNullOrWhiteSpace(normalized))
            return false;

        var aliasMap = BuildAliasMap(scene);
        return aliasMap.TryGetValue(normalized, out choice);
    }

    private static bool IsDigitsOnly(string input)
    {
        foreach (var ch in input)
        {
            if (!char.IsDigit(ch))
                return false;
        }

        return input.Length > 0;
    }

    private static Dictionary<string, ChoiceDefinition> BuildAliasMap(SceneDefinition scene)
    {
        var map = new Dictionary<string, ChoiceDefinition>(StringComparer.OrdinalIgnoreCase);

        foreach (var choice in scene.Choices)
        {
            foreach (var alias in choice.Aliases)
            {
                var normalized = ChoiceInputNormalizer.Normalize(alias);
                if (string.IsNullOrWhiteSpace(normalized))
                    continue;

                if (!map.ContainsKey(normalized))
                    map[normalized] = choice;
            }
        }

        return map;
    }
}
