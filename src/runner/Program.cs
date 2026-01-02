using System;
using System.Collections.Generic;
using Env0.Terminal;
using Env0.Core;
using env0.maintenance;
using env0.records;

namespace Env0.Runner
{
    internal static class Program
    {
        private static int Main()
        {
            RunWithRouting(new MaintenanceModule());
            return 0;
        }

        private static void RunWithRouting(IContextModule module)
        {
            var next = RunModule(module);
            while (next != ContextRoute.None)
            {
                var routedModule = CreateModule(next);
                next = RunModule(routedModule);
            }
        }

        private static ContextRoute RunModule(IContextModule module)
        {
            var originalDirectory = Environment.CurrentDirectory;
            Environment.CurrentDirectory = AppContext.BaseDirectory;
            var session = new SessionState { NextContext = ContextRoute.None };
            PrintOutput(module.Handle(string.Empty, session));

            while (!session.IsComplete)
            {
                var input = Console.ReadLine();
                if (input == null)
                {
                    session.IsComplete = true;
                    break;
                }

                PrintOutput(module.Handle(input, session));
            }

            Environment.CurrentDirectory = originalDirectory;
            return session.NextContext;
        }

        private static IContextModule CreateModule(ContextRoute route)
        {
            return route switch
            {
                ContextRoute.Maintenance => new MaintenanceModule(),
                ContextRoute.Records => new RecordsModule(),
                ContextRoute.Terminal => new TerminalModule(),
                _ => throw new InvalidOperationException($"Unknown route: {route}")
            };
        }

        private static void PrintOutput(IEnumerable<OutputLine> lines)
        {
            if (lines == null)
            {
                return;
            }

            foreach (var line in lines)
            {
                var text = line.Text ?? string.Empty;
                if (line.NewLine)
                {
                    Console.WriteLine(text);
                }
                else
                {
                    Console.Write(text);
                }
            }
        }
    }
}



