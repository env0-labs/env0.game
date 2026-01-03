namespace env0.records.Model;

public sealed class ChoiceDefinition
{
    public required string Id { get; init; }
    public required int Index { get; init; }
    public required string Verb { get; init; }
    public required string Noun { get; init; }
    public required List<string> Aliases { get; init; }

    public List<string>? RequiresAll { get; init; }
    public List<string>? RequiresNone { get; init; }

    public string? DisabledReason { get; init; }

    public required List<EffectDefinition> Effects { get; init; }
    
    public string? ResultText { get; set; }

}

