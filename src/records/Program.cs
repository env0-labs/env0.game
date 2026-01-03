using System.Text.Json;
using System.Text.Json.Serialization;
using env0.records.Engine;
using env0.records.Model;
using env0.records.Runtime;

Console.WriteLine("env0.records booting");
Console.WriteLine();

// ------------------------------------------------------------------
// Select story JSON (preloader)
// ------------------------------------------------------------------
var storyDirectory = Path.Combine(AppContext.BaseDirectory, "story");

if (!Directory.Exists(storyDirectory))
    throw new InvalidOperationException($"Story directory not found: {storyDirectory}");

var availableStories = Directory
    .GetFiles(storyDirectory, "*.json", SearchOption.TopDirectoryOnly)
    .OrderBy(Path.GetFileName)
    .ToList();

if (availableStories.Count == 0)
    throw new InvalidOperationException($"No story JSON files found in {storyDirectory}.");

var storyPath = availableStories[0];

if (!File.Exists(storyPath))
    throw new InvalidOperationException($"Story file not found: {storyPath}");

var json = File.ReadAllText(storyPath);

var story = JsonSerializer.Deserialize<StoryDefinition>(
    json,
    new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true,
        Converters = { new JsonStringEnumConverter() }
    }
) ?? throw new InvalidOperationException("Story file could not be parsed.");



// ------------------------------------------------------------------
// Engine setup
// ------------------------------------------------------------------
var repo = new SceneRepository(story);
var state = new GameState(repo.StartSceneId);
var evaluator = new ChoiceEvaluator();
var executor = new EffectExecutor();
var inputRouter = new InputRouter();
var showNumericOptions = false;

// ------------------------------------------------------------------
// Main loop
// ------------------------------------------------------------------
while (true)
{
    var scene = repo.Get(state.CurrentSceneId);

    Console.WriteLine(scene.Text);
    Console.WriteLine();

    // End scene: render text only, no choices
    if (scene.IsEnd)
        break;

    var orderedChoices = scene.Choices.OrderBy(c => c.Index).ToList();
    var availableChoices = new List<ChoiceDefinition>();
    foreach (var choice in orderedChoices)
    {
        if (evaluator.IsEnabled(choice, state, out _))
            availableChoices.Add(choice);
    }

    if (showNumericOptions)
    {
        Console.WriteLine("Available:");
        foreach (var choice in availableChoices)
        {
            var line = $"[{choice.Index}] {choice.Verb} {choice.Noun}";
            Console.WriteLine(line);
        }
    }

    Console.WriteLine();
    Console.Write("> ");

    var input = Console.ReadLine();
    Console.WriteLine();

    if (inputRouter.IsHelpCommand(input ?? string.Empty))
    {
        showNumericOptions = true;
        Console.WriteLine();
        continue;
    }

    if (!inputRouter.TryResolve(input ?? string.Empty, scene, out var selectedChoice) || selectedChoice is null)
    {
        Console.WriteLine("Input not recognized.");
        Console.WriteLine("Use 'options' or 'help' to display numbered choices.");
        Console.WriteLine();
        continue;
    }

    var isEnabled = evaluator.IsEnabled(selectedChoice, state, out var disabledReason);
    if (!isEnabled)
    {
        Console.WriteLine(disabledReason ?? "That option is not available.");
        Console.WriteLine();
        continue;
    }

    // Execute effects (mutates state and may change scene)
    executor.Execute(selectedChoice.Effects, state);

    if (!string.IsNullOrWhiteSpace(selectedChoice.ResultText))
    {
        Console.WriteLine(selectedChoice.ResultText);
        Console.WriteLine();
    }

}

// ------------------------------------------------------------------
// End
// ------------------------------------------------------------------
Console.WriteLine("Game ended.");

