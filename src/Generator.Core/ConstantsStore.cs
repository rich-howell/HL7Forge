using System.Text.Json;

namespace HL7Forge.Core;

public class ConstantsStore
{
    private readonly JsonDocument? _doc;
    public ConstantsStore(JsonDocument? doc) { _doc = doc; }
    public JsonElement? Root => _doc?.RootElement;

    public static ConstantsStore Load(string baseDir, string version)
    {
        var path = Path.Combine(baseDir, "Profiles", version, "constants.json");
        if (File.Exists(path))
        {
            var json = File.ReadAllText(path);
            var doc = JsonDocument.Parse(json);
            return new ConstantsStore(doc);
        }
        return new ConstantsStore(null);
    }

    public string GetString(string key, string fallback = "")
    {
        if (_doc == null) return fallback;
        if (_doc.RootElement.TryGetProperty(key, out var el) && el.ValueKind == JsonValueKind.String)
            return el.GetString() ?? fallback;
        return fallback;
    }

    public List<string> GetStringArray(string key)
    {
        var list = new List<string>();
        if (_doc == null) return list;
        if (_doc.RootElement.TryGetProperty(key, out var el) && el.ValueKind == JsonValueKind.Array)
        {
            foreach (var it in el.EnumerateArray())
                if (it.ValueKind == JsonValueKind.String) list.Add(it.GetString() ?? "");
        }
        return list;
    }
}
