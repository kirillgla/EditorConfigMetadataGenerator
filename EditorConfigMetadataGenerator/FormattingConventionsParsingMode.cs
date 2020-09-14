using System.IO;
using System.Text.RegularExpressions;

namespace EditorConfigMetadataGenerator
{
    public sealed class FormattingConventionsParsingMode : ParsingMode
    {
        public override Regex RuleRegex => RuleRegexes.FormattingRuleRegex;

        // csharp_new_line_before_open_brace is quite complex and is better dealt with manually
        public override bool ShouldSkipRule(string optionName) =>
            optionName.Equals("csharp_new_line_before_open_brace");

        protected override string BaseValueOffset => "        ";

        public override void WriteValues(Match optionMatch, StreamWriter writer)
        {
            writer.WriteLine("      \"type\": \"union\",");
            writer.WriteLine("      \"values\": [");
            string rawRuleValues = optionMatch.Groups["ruleValues"].Value;
            WriteValue(writer, RuleFirstValueRegex.Match(rawRuleValues));
            foreach (Match? valueMatch in RuleValueRegex.Matches(rawRuleValues))
            {
                writer.WriteLine(",");
                WriteValue(writer, valueMatch!);
            }

            writer.WriteLine();
            writer.WriteLine("      ]");
        }

    }
}
