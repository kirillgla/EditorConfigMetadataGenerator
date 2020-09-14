using System.IO;

namespace EditorConfigMetadataGenerator
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            var parsingMode = CommandLineUtils.GetParsingMode(args);
            if (parsingMode == null) return;
            string dataLocation = CommandLineUtils.GetDataLocation(args);

            string text = File.ReadAllText($@"{dataLocation}\Data.html");
            using var writer = new StreamWriter($@"{dataLocation}\Output.json");

            var generator = new MetadataGenerator(parsingMode, text, writer);
            generator.Generate();
        }
    }
}
