using System.Text.Json;
using HL7Forge.Core.Models;

namespace HL7Forge.Core;

public class PersonStore
{
    private readonly List<Person> _people = new();

    public static PersonStore Load(string baseDir, string version)
    {
        var path = Path.Combine(baseDir, "Profiles", version, "people.json");
        var store = new PersonStore();
        if (File.Exists(path))
        {
            var json = File.ReadAllText(path);
            var list = JsonSerializer.Deserialize<List<Person>>(json);
            if (list != null) store._people.AddRange(list);
        }
        return store;
    }

    public bool HasPeople => _people.Count > 0;

    public Person GetBySeed(int seed)
    {
        if (_people.Count == 0) throw new InvalidOperationException("No people loaded");
        var idx = Math.Abs(seed) % _people.Count;
        return _people[idx];
    }
}
