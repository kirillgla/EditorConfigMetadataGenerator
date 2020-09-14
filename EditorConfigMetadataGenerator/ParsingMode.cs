using System.IO;
using System.Text.RegularExpressions;

namespace EditorConfigMetadataGenerator
{
    public abstract class ParsingMode
    {
        private static readonly Regex TagRegex = new Regex("</?[^<>]+>");

        protected static readonly Regex RuleFirstValueRegex =
            new Regex(@"^<code>(?<valueName>(\w)+)</code>( -)?(?<documentation>([^<]|<[^b]|<b[^r]|<br[^>])*)(<br><br>)?");

        protected static readonly Regex RuleValueRegex =
            new Regex(@"<br><br><code>(?<valueName>(\w)+)</code>( -)?(?<documentation>([^<]|<[^b]|<b[^r]|<br[^>])*)");

        public static string EscapeDocumentation(string documentation) => TagRegex.Replace(documentation, "");
        public abstract Regex RuleRegex { get; }

        protected void WriteValue(StreamWriter writer, Match valueMatch)
        {
            string documentation = EscapeDocumentation(valueMatch.Groups["documentation"].Value.TrimStart());
            if (string.IsNullOrEmpty(documentation))
            {
                writer.WriteLine($"\"{valueMatch.Groups["valueName"].Value}\"");
                return;
            }

            writer.WriteLine($"{BaseValueOffset}{{");
            writer.WriteLine($"{BaseValueOffset}  \"type\": \"constant\",");
            writer.WriteLine($"{BaseValueOffset}  \"value\": \"{valueMatch.Groups["valueName"].Value}\",");
            writer.WriteLine($"{BaseValueOffset}  \"documentation\": \"{documentation}\"");
            writer.Write($"{BaseValueOffset}}}");
        }

        protected abstract string BaseValueOffset { get; }
        public abstract bool ShouldSkipRule(string optionName);
        public abstract void WriteValues(Match optionMatch, StreamWriter writer);
    }
}
