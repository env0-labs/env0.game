using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Env0.Core;
using env0.act2;

namespace Env0.Act2.Tests;

public sealed class Act2ModuleTests
{
    [Fact]
    public void Handle_FirstCall_PrintsBootAndStorySelection()
    {
        var module = new Act2Module();
        var session = new SessionState();
        var alpha = "test_act2_alpha.json";
        var beta = "test_act2_beta.json";

        CreateStoryFile(alpha, BuildEndStoryJson("start", "Alpha scene."));
        CreateStoryFile(beta, BuildEndStoryJson("start", "Beta scene."));

        try
        {
            var output = module.Handle(string.Empty, session).ToList();
            var texts = output.Select(line => line.Text).ToList();

            Assert.Contains("env0.act2 booting", texts);
            Assert.Contains("Select a story file to load:", texts);

            var alphaIndex = FindStoryNumber(output, alpha);
            var betaIndex = FindStoryNumber(output, beta);
            Assert.True(alphaIndex < betaIndex);

            var last = output.LastOrDefault();
            Assert.NotNull(last);
            Assert.Equal("> ", last!.Text);
            Assert.False(last.NewLine);
            Assert.False(session.IsComplete);
        }
        finally
        {
            DeleteStoryFile(alpha);
            DeleteStoryFile(beta);
        }
    }

    [Fact]
    public void Handle_InvalidSelection_CompletesSession()
    {
        var module = new Act2Module();
        var session = new SessionState();
        var story = "test_act2_invalid.json";

        CreateStoryFile(story, BuildEndStoryJson("start", "End scene."));

        try
        {
            module.Handle(string.Empty, session).ToList();

            var output = module.Handle("0", session).ToList();
            var texts = output.Select(line => line.Text).ToList();

            Assert.Contains("Invalid selection. Please enter a valid story number.", texts);
            Assert.True(session.IsComplete);
        }
        finally
        {
            DeleteStoryFile(story);
        }
    }

    [Fact]
    public void Handle_InvalidInput_DoesNotCompleteAndRendersScene()
    {
        var module = new Act2Module();
        var session = new SessionState();
        var story = "test_act2_running.json";

        CreateStoryFile(story, BuildRunningStoryJson());

        try
        {
            var selectionOutput = module.Handle(string.Empty, session).ToList();
            var selectionNumber = FindStoryNumber(selectionOutput, story);

            var startOutput = module.Handle(selectionNumber.ToString(), session).ToList();
            Assert.False(session.IsComplete);
            Assert.Contains("Start scene.", startOutput.Select(line => line.Text));

            var invalidOutput = module.Handle("abc", session).ToList();
            var invalidTexts = invalidOutput.Select(line => line.Text).ToList();

            Assert.Contains("Invalid input. Enter a number.", invalidTexts);
            Assert.False(session.IsComplete);
        }
        finally
        {
            DeleteStoryFile(story);
        }
    }

    [Fact]
    public void Handle_EndScene_CompletesSession()
    {
        var module = new Act2Module();
        var session = new SessionState();
        var story = "test_act2_complete.json";

        CreateStoryFile(story, BuildEndStoryJson("start", "End scene."));

        try
        {
            var selectionOutput = module.Handle(string.Empty, session).ToList();
            var selectionNumber = FindStoryNumber(selectionOutput, story);

            var output = module.Handle(selectionNumber.ToString(), session).ToList();
            var texts = output.Select(line => line.Text).ToList();

            Assert.Contains("End scene.", texts);
            Assert.Contains("Game ended.", texts);
            Assert.True(session.IsComplete);
        }
        finally
        {
            DeleteStoryFile(story);
        }
    }

    private static string StoryDirectory => Path.Combine(AppContext.BaseDirectory, "story");

    private static void CreateStoryFile(string fileName, string json)
    {
        Directory.CreateDirectory(StoryDirectory);
        File.WriteAllText(Path.Combine(StoryDirectory, fileName), json);
    }

    private static void DeleteStoryFile(string fileName)
    {
        var path = Path.Combine(StoryDirectory, fileName);
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    private static int FindStoryNumber(IEnumerable<OutputLine> output, string fileName)
    {
        foreach (var line in output)
        {
            var text = line.Text?.Trim();
            if (string.IsNullOrEmpty(text))
            {
                continue;
            }

            if (!text.EndsWith(fileName, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            var dotIndex = text.IndexOf('.');
            if (dotIndex <= 0)
            {
                continue;
            }

            var numberText = text.Substring(0, dotIndex);
            if (int.TryParse(numberText, out var number))
            {
                return number;
            }
        }

        throw new InvalidOperationException($"Story selection for {fileName} not found.");
    }

    private static string BuildEndStoryJson(string startSceneId, string text)
    {
        return $@"{{
  ""StartSceneId"": ""{startSceneId}"",
  ""Scenes"": [
    {{
      ""Id"": ""{startSceneId}"",
      ""Text"": ""{text}"",
      ""IsEnd"": true,
      ""Choices"": []
    }}
  ]
}}";
    }

    private static string BuildRunningStoryJson()
    {
        return @"{
  ""StartSceneId"": ""start"",
  ""Scenes"": [
    {
      ""Id"": ""start"",
      ""Text"": ""Start scene."",
      ""IsEnd"": false,
      ""Choices"": [
        {
          ""Number"": 1,
          ""Text"": ""Go"",
          ""Effects"": [
            { ""Type"": ""GotoScene"", ""Value"": ""end"" }
          ]
        }
      ]
    },
    {
      ""Id"": ""end"",
      ""Text"": ""End scene."",
      ""IsEnd"": true,
      ""Choices"": []
    }
  ]
}";
    }
}
