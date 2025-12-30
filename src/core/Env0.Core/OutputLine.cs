namespace Env0.Core;

public sealed class OutputLine
{
    public OutputLine(OutputType type, string text, bool newLine = true)
    {
        Type = type;
        Text = text;
        NewLine = newLine;
    }

    public OutputType Type { get; }
    public string Text { get; }
    public bool NewLine { get; }
}
