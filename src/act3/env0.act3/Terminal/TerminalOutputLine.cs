namespace Env0.Act3.Terminal
{
    public class TerminalOutputLine
    {
        public string Text { get; set; }
        public OutputType Type { get; set; }

        public TerminalOutputLine(string text, OutputType type)
        {
            Text = text;
            Type = type;
        }
    }
}
