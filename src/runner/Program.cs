using System;
using System.Diagnostics;
using System.IO;

namespace Env0.Runner
{
    internal static class Program
    {
        private static int Main()
        {
            while (true)
            {
                Console.WriteLine("Select an act to launch:");
                Console.WriteLine("  1) Act 1 (placeholder)");
                Console.WriteLine("  2) Act 2");
                Console.WriteLine("  3) Act 3");
                Console.WriteLine("  4) Act 4 (placeholder)");
                Console.WriteLine("  Q) Quit");
                Console.Write("> ");

                var input = Console.ReadLine();
                if (input == null)
                {
                    return 0;
                }

                input = input.Trim();
                if (input.Equals("q", StringComparison.OrdinalIgnoreCase) ||
                    input.Equals("quit", StringComparison.OrdinalIgnoreCase))
                {
                    return 0;
                }

                switch (input)
                {
                    case "1":
                        Console.WriteLine("Act 1 runner is a placeholder for now.");
                        break;
                    case "2":
                        RunProject(ProjectLaunch.Create(
                            @"src\act2\env0.act2.csproj",
                            "env0.act2.dll",
                            "net8.0"));
                        break;
                    case "3":
                        RunProject(ProjectLaunch.Create(
                            @"src\act3\env0.act3.playground\Env0.Act3.Playground.csproj",
                            "Env0.Act3.Playground.dll",
                            "net8.0"));
                        break;
                    case "4":
                        Console.WriteLine("Act 4 runner is a placeholder for now.");
                        break;
                    default:
                        Console.WriteLine("Unknown option. Please choose 1-4 or Q.");
                        break;
                }

                Console.WriteLine();
            }
        }

        private static void RunProject(ProjectLaunch launch)
        {
            var repoRoot = FindRepoRoot();
            var projectFullPath = Path.Combine(repoRoot, launch.ProjectPath);
            var projectDir = Path.GetDirectoryName(projectFullPath) ?? repoRoot;

            var outputDir = Path.Combine(projectDir, "bin", "Debug", launch.TargetFramework);
            var outputDll = Path.Combine(outputDir, launch.OutputDllName);

            if (!BuildProject(projectFullPath, repoRoot))
            {
                return;
            }

            if (!File.Exists(outputDll))
            {
                Console.WriteLine("Build succeeded but output DLL not found: " + outputDll);
                return;
            }

            var psi = new ProcessStartInfo("dotnet")
            {
                WorkingDirectory = outputDir,
                UseShellExecute = false,
                RedirectStandardOutput = false,
                RedirectStandardError = false,
                RedirectStandardInput = false
            };

            psi.ArgumentList.Add(outputDll);

            Console.WriteLine("Running: dotnet " + outputDll);
            using var process = Process.Start(psi);
            if (process == null)
            {
                Console.WriteLine("Failed to start dotnet.");
                return;
            }
            process.WaitForExit();

            Console.WriteLine("Exit code: " + process.ExitCode);
        }

        private static bool BuildProject(string projectPath, string repoRoot)
        {
            var psi = new ProcessStartInfo("dotnet")
            {
                WorkingDirectory = repoRoot,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            psi.ArgumentList.Add("build");
            psi.ArgumentList.Add(projectPath);
            psi.ArgumentList.Add("-c");
            psi.ArgumentList.Add("Debug");

            Console.WriteLine("Building: dotnet build " + projectPath + " -c Debug");
            using var process = Process.Start(psi);
            if (process == null)
            {
                Console.WriteLine("Failed to start dotnet build.");
                return false;
            }

            process.OutputDataReceived += (_, e) =>
            {
                if (e.Data != null)
                {
                    Console.WriteLine(e.Data);
                }
            };
            process.ErrorDataReceived += (_, e) =>
            {
                if (e.Data != null)
                {
                    Console.Error.WriteLine(e.Data);
                }
            };

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();

            Console.WriteLine("Build exit code: " + process.ExitCode);
            return process.ExitCode == 0;
        }

        private static string FindRepoRoot()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                var slnPath = Path.Combine(dir.FullName, "env0.game.sln");
                if (File.Exists(slnPath))
                {
                    return dir.FullName;
                }

                dir = dir.Parent;
            }

            return Directory.GetCurrentDirectory();
        }

        private readonly struct ProjectLaunch
        {
            private ProjectLaunch(string projectPath, string outputDllName, string targetFramework)
            {
                ProjectPath = projectPath;
                OutputDllName = outputDllName;
                TargetFramework = targetFramework;
            }

            public string ProjectPath { get; }
            public string OutputDllName { get; }
            public string TargetFramework { get; }

            public static ProjectLaunch Create(string projectPath, string outputDllName, string targetFramework)
            {
                return new ProjectLaunch(projectPath, outputDllName, targetFramework);
            }
        }
    }
}
