using System;

namespace Env0.Act3.Tests
{
    internal static class TestOutput
    {
        private static readonly bool Verbose =
            string.Equals(Environment.GetEnvironmentVariable("ENV0_TEST_VERBOSE"), "1", StringComparison.OrdinalIgnoreCase);

        public static void WriteLine(string message)
        {
            if (Verbose)
            {
                Console.WriteLine(message);
            }
        }
    }
}
