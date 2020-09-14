using System.Text.RegularExpressions;

namespace EditorConfigMetadataGenerator
{
    public static class RuleRegexes
    {
        public static readonly Regex FormattingRuleRegex = new Regex(@"<h[45] id=""(\w|_)+"">(?<ruleName>(\w|_)+)</h[45]>(
<p>(?<ruleDocumentation>.+)</p>)?
<table>
<thead>
<tr>
<th>Property</th>
<th>Value</th>
</tr>
</thead>
<tbody>
<tr>
<td><strong>Rule name</strong></td>
<td>(\w|_)+</td>
</tr>
<tr>
<td><strong>Applicable languages</strong></td>
<td>.+</td>
</tr>(
<tr>
<td><strong>Introduced version</strong></td>
<td>.+</td>
</tr>)?
<tr>
<td><strong>Values</strong></td>
<td>(?<ruleValues>.+)</td>
</tr>
</tbody>
</table>");

        public static readonly Regex LanguageRuleRegex = new Regex(@"<h4 id=""(\w|_)+"">(?<ruleName>(\w|_)+)</h4>
<table>
<thead>
<tr>
<th>Property</th>
<th>Value</th>
</tr>
</thead>
<tbody>
<tr>
<td><strong>Rule name</strong></td>
<td>(\w|_)+</td>
</tr>
<tr>
<td><strong>Rule ID</strong></td>
<td>.+</td>
</tr>
<tr>
<td><strong>Applicable languages</strong></td>
<td>.*</td>
</tr>
<tr>
<td><strong>Values</strong></td>
<td>(?<ruleValues>.+)</td>
</tr>
<tr>
<td><strong>Visual Studio default</strong></td>
<td><code>.+</code></td>
</tr>
</tbody>
</table>");
    }
}
