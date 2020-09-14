using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace EditorConfigMetadataGenerator
{
    public sealed class LanguageConventionsParsingMode : ParsingMode
    {
        private static IEnumerable<string> IgnoredRules { get; } = new List<string>
        {
            "csharp_preferred_modifier_order",
            "visual_basic_preferred_modifier_order",
            "visual_basic_style_unused_value_expression_statement_preference",
            "visual_basic_style_unused_value_assignment_preference"
        };

        private bool HasDefinedSeverity { get; set; }
        public override Regex RuleRegex => RuleRegexes.LanguageRuleRegex;
        protected override string BaseValueOffset => "          ";

        // Different from others, easier to handle manually
        public override bool ShouldSkipRule(string optionName) => IgnoredRules.Any(optionName.Equals);

        public override void WriteValues(Match optionMatch, StreamWriter writer)
        {
            writer.WriteLine("      \"type\": \"pair\",");
            writer.WriteLine("      \"first\": {");
            writer.WriteLine("        \"type\": \"union\",");
            writer.WriteLine("        \"values\": [");

            string rawRuleValues = optionMatch.Groups["ruleValues"].Value;
            WriteValue(writer, RuleFirstValueRegex.Match(rawRuleValues));
            foreach (Match? valueMatch in RuleValueRegex.Matches(rawRuleValues))
            {
                writer.WriteLine(",");
                WriteValue(writer, valueMatch!);
            }

            writer.WriteLine();
            writer.WriteLine("        ]");
            writer.WriteLine("      },");
            writer.Write("      \"second\": ");
            WriteSeverity(writer);
        }

        private void WriteSeverity(StreamWriter writer)
        {
            if (!HasDefinedSeverity)
            {
                writer.WriteLine(@"{
        ""type"": ""union"",
        ""type_alias"": ""severity"",
        ""values"": [
          {
            ""type"": ""constant"",
            ""value"": ""none"",
            ""documentation"": ""Do not show anything to the user when this rule is violated. Code generation features generate code in this style, however. Rules with none severity never appear in the Quick Actions and Refactorings menu. In most cases, this is considered \""disabled\"" or \""ignored\"".""
          },
          {
            ""type"": ""constant"",
            ""value"": ""silent"",
            ""documentation"": ""Do not show anything to the user when this rule is violated. Code generation features generate code in this style, however. Rules with silent severity participate in cleanup and appear in the Quick Actions and Refactorings menu.""
          },
          {
            ""type"": ""constant"",
            ""value"": ""suggestion"",
            ""documentation"": ""When this style rule is violated, show it to the user as a suggestion. Suggestions appear as three gray dots under the first two characters.""
          },
          {
            ""type"": ""constant"",
            ""value"": ""warning"",
            ""documentation"": ""When this style rule is violated, show a compiler warning.""
          },
          {
            ""type"": ""constant"",
            ""value"": ""error"",
            ""documentation"": ""When this style rule is violated, show a compiler error.""
          }
        ]
      }");
                HasDefinedSeverity = true;
            }
            else
            {
                writer.WriteLine("\"severity\"");
            }
        }
    }
}
