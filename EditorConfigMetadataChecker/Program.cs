using System;
using System.IO;
using System.Linq;
using EditorConfigMetadataGenerator;

namespace EditorConfigMetadataChecker
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            string dataLocation = CommandLineUtils.GetDataLocation(args, 0);
            string output = File.ReadAllText(@$"{dataLocation}\Output.json");
            var rules = File.ReadAllLines($@"{dataLocation}\Goal.txt");
            var notFoundList = (
                from rule in rules
                where !output.Contains(rule)
                select rule
            ).ToList();
            if (!notFoundList.Any())
            {
                Console.WriteLine("All rules are present!");
            }
            else
            {
                Console.WriteLine($"{notFoundList.Count} rules are missing:");
                foreach (string notFound in notFoundList)
                {
                    Console.WriteLine(notFound);
                }
            }
        }
    }
}
