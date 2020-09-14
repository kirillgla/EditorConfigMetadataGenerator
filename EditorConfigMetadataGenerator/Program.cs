using System;
using System.IO;
using System.Text.RegularExpressions;

namespace EditorConfigMetadataGenerator
{
    internal static class Program
    {
        private static readonly Regex TagRegex = new Regex("</?[^<>]+>");

        private static readonly Regex RuleFirstValueRegex =
            new Regex(@"^<code>(?<valueName>(\w)+)</code> -(?<documentation>([^<]|<[^b]|<b[^r]|<br[^>])*)<br><br>");

        private static readonly Regex RuleValueRegex =
            new Regex(@"<br><br><code>(?<valueName>(\w)+)</code> -(?<documentation>([^<]|<[^b]|<b[^r]|<br[^>])*)");

        static void Main(string[] args)
        {
            var ruleRegex = CommandLineUtils.GetRuleRegex(args);
            if (ruleRegex == null) return;
            string dataLocation = CommandLineUtils.GetDataLocation(args);

            string text = File.ReadAllText($@"{dataLocation}\Data.html");
            using var writer = new StreamWriter($@"{dataLocation}\Output.json");
            var optionsMatches = ruleRegex.Matches(text);

            Console.WriteLine($"Total rules found: {optionsMatches.Count}");

            writer.WriteLine("[");
            int currentIndex = 0;
            foreach (Match? optionMatch in optionsMatches)
            {
                // csharp_new_line_before_open_brace is already described in roslyn.json
                if (optionMatch!.Groups["ruleName"].Value.Equals("csharp_new_line_before_open_brace"))
                {
                    currentIndex += 1;
                    continue;
                }
                writer.WriteLine("  {");
                writer.WriteLine("    \"type\": \"option\",");
                WriteKey(writer, optionMatch);
                writer.WriteLine("    \"value\": {");
                writer.WriteLine("      \"type\": \"union\",");
                writer.WriteLine("      \"values\": [");
                WriteValues(optionMatch, writer);
                writer.WriteLine("      ]");
                writer.WriteLine("    }");
                writer.Write("  }");
                writer.WriteLine(currentIndex == optionsMatches.Count - 1 ? "" : ",");
                currentIndex += 1;
            }

            writer.WriteLine("]");
        }

        private static void WriteValues(Match optionMatch, StreamWriter writer)
        {
            string rawRuleValues = optionMatch.Groups["ruleValues"].Value;
            WriteValue(writer, RuleFirstValueRegex.Match(rawRuleValues));
            foreach (Match? valueMatch in RuleValueRegex.Matches(rawRuleValues))
            {
                writer.WriteLine(",");
                WriteValue(writer, valueMatch!);
            }

            writer.WriteLine();
        }

        private static void WriteValue(StreamWriter writer, Match valueMatch)
        {
            string documentation = EscapeDocumentation(valueMatch.Groups["documentation"].Value.TrimStart());
            if (string.IsNullOrEmpty(documentation))
            {
                writer.WriteLine($"\"{valueMatch.Groups["valueName"].Value}\"");
                return;
            }

            writer.WriteLine("        {");
            writer.WriteLine("          \"type\": \"constant\",");
            writer.WriteLine($"          \"value\": \"{valueMatch.Groups["valueName"].Value}\",");
            writer.WriteLine($"          \"documentation\": \"{documentation}\"");
            writer.Write("        }");
        }

        private static void WriteKey(StreamWriter writer, Match optionMatch)
        {
            if (!optionMatch.Groups["ruleDocumentation"].Success)
            {
                writer.WriteLine($"    \"key\": \"{optionMatch.Groups["ruleName"]}\",");
                return;
            }

            writer.WriteLine("    \"key\": {");
            writer.WriteLine("        \"type\": \"constant\",");
            writer.WriteLine($"        \"value\": \"{optionMatch.Groups["ruleName"]}\",");
            writer.WriteLine(
                $"        \"documentation\": \"{EscapeDocumentation(optionMatch.Groups["ruleDocumentation"].Value)}\"");
            writer.WriteLine("    },");
        }

        private static string EscapeDocumentation(string documentation) => TagRegex.Replace(documentation, "");
    }
}
