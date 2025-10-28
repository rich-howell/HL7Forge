using System.Text.Json;
using HL7Forge.Core.Models;

namespace HL7Forge.Core
{
    public static class SegmentPatcher
    {
        public static string Apply(string segmentLine, JsonElement? map, Person? person, JsonElement? profileRoot, JsonElement? constRoot, int seed, int seq)
        {
            if (map == null || map.Value.ValueKind != JsonValueKind.Object) return segmentLine;
            if (!map.Value.TryGetProperty("segment", out var segEl)) return segmentLine;

            var segName = segmentLine.Split('|')[0];
            if (!string.Equals(segEl.GetString(), segName, StringComparison.OrdinalIgnoreCase)) return segmentLine;

            if (!map.Value.TryGetProperty("overrides", out var overrides) || overrides.ValueKind != JsonValueKind.Object) return segmentLine;

            var fields = segmentLine.Split('|').ToList();
            foreach (var prop in overrides.EnumerateObject())
            {
                if (!int.TryParse(prop.Name, out int fieldIndex)) continue;
                var tmpl = prop.Value.GetString() ?? string.Empty;
                var rendered = TemplateEngine.Render(tmpl, person, profileRoot, constRoot, seed, seq);
                while (fields.Count <= fieldIndex) fields.Add(string.Empty);
                fields[fieldIndex] = rendered;
            }
            return string.Join('|', fields);
        }
    }
}
