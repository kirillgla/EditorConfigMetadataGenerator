using System;
using System.IO;
using System.Text.RegularExpressions;

namespace EditorConfigMetadataGenerator
{
    public sealed class MetadataGenerator
    {
        private ParsingMode ParsingMode { get; }
        private string Text { get; }
        private StreamWriter Writer { get; }

        public MetadataGenerator(ParsingMode parsingMode, string text, StreamWriter writer)
        {
            ParsingMode = parsingMode;
            Text = text;
            Writer = writer;
        }

        public void Generate()
        {
            var optionsMatches = ParsingMode.RuleRegex.Matches(Text);

            Console.WriteLine($"Total rules found: {optionsMatches.Count}");

            Writer.WriteLine("[");
            int currentIndex = 0;
            foreach (Match? optionMatch in optionsMatches)
            {
                string optionName = optionMatch!.Groups["ruleName"].Value;
                if (ParsingMode.ShouldSkipRule(optionName))
                {
                    Console.WriteLine($"Skipping {optionName}");
                    currentIndex += 1;
                    continue;
                }

                Writer.WriteLine("  {");
                Writer.WriteLine("    \"type\": \"option\",");
                WriteKey(optionMatch);
                Writer.WriteLine("    \"value\": {");
                ParsingMode.WriteValues(optionMatch, Writer);
                Writer.WriteLine("    }");
                Writer.Write("  }");
                Writer.WriteLine(currentIndex == optionsMatches.Count - 1 ? "" : ",");
                currentIndex += 1;
            }

            Writer.WriteLine("]");
        }
        
        private void WriteKey(Match optionMatch)
        {
            if (!optionMatch.Groups["ruleDocumentation"].Success)
            {
                Writer.WriteLine($"    \"key\": \"{optionMatch.Groups["ruleName"]}\",");
                return;
            }

            Writer.WriteLine("    \"key\": {");
            Writer.WriteLine("        \"type\": \"constant\",");
            Writer.WriteLine($"        \"value\": \"{optionMatch.Groups["ruleName"]}\",");
            Writer.WriteLine(
                $"        \"documentation\": \"{ParsingMode.EscapeDocumentation(optionMatch.Groups["ruleDocumentation"].Value)}\"");
            Writer.WriteLine("    },");
        }

    }
}
