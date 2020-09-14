using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace EditorConfigMetadataGenerator
{
    public static class CommandLineUtils
    {
        private const string DefaultDataLocation = @"C:\w\EditorConfigMetadataGenerator\EditorConfigMetadataGenerator";

        public static Regex? GetRuleRegex(string[] args)
        {
            if (!args.Any())
            {
                Console.WriteLine("Usage: EditorConfigMetadataGenerator <RuleKind> <WorkingDirectoryPath>");
                return null;
            }

            return args[0] switch
            {
                "formatting" => RuleRegexes.FormattingRuleRegex,
                "language" => RuleRegexes.LanguageRuleRegex,
                _ => null
            };
        }

        public static string GetDataLocation(string[] args)
        {
            if (args.Length >= 2)
            {
                Console.WriteLine($"Using data location from argument ({args[0]})");
                return args[1];
            }

            Console.WriteLine($"No data location passed. Using default data location {DefaultDataLocation})");
            return DefaultDataLocation;
        }
    }
}
