using System.Text.RegularExpressions;
using System.Text.Json;
using HL7Forge.Core.Models;

namespace HL7Forge.Core
{
    public static class TemplateEngine
    {
        private static readonly Regex Token = new(@"\$\{([^}]+)\}", RegexOptions.Compiled);

        public static string Render(string template, Person? person, JsonElement? profileRoot, JsonElement? constRoot, int seed, int seq)
        {
            if (string.IsNullOrEmpty(template)) return string.Empty;
            return Token.Replace(template, m => Eval(m.Groups[1].Value, person, profileRoot, constRoot, seed, seq));
        }

        private static string Eval(string expr, Person? person, JsonElement? profileRoot, JsonElement? constRoot, int seed, int seq)
        {
            try
            {
                if (expr.StartsWith("now:"))
                {
                    var fmt = expr.Substring(4);
                    return DateTime.UtcNow.ToString(fmt);
                }
                if (expr.StartsWith("rand:"))
                {
                    var parts = expr.Substring(5).Split('-');
                    var min = int.Parse(parts[0]);
                    var max = int.Parse(parts[1]);
                    var rng = new Random(seed ^ 0x5f3759df);
                    return rng.Next(min, max + 1).ToString();
                }
                if (expr == "seed") return seed.ToString();
                if (expr == "seq") return seq.ToString();

                if (expr.StartsWith("upper:"))
                    return Eval(expr.Substring(6), person, profileRoot, constRoot, seed, seq).ToUpperInvariant();
                if (expr.StartsWith("lower:"))
                    return Eval(expr.Substring(6), person, profileRoot, constRoot, seed, seq).ToLowerInvariant();

                if (expr.StartsWith("padleft:"))
                {
                    var rest = expr.Substring(8);
                    var parts = rest.Split(':', 2);
                    var n = int.Parse(parts[0]);
                    var inner = Eval(parts[1], person, profileRoot, constRoot, seed, seq);
                    return inner.PadLeft(n, '0');
                }
                if (expr.StartsWith("padright:"))
                {
                    var rest = expr.Substring(9);
                    var parts = rest.Split(':', 2);
                    var n = int.Parse(parts[0]);
                    var inner = Eval(parts[1], person, profileRoot, constRoot, seed, seq);
                    return inner.PadRight(n, ' ');
                }
                if (expr.StartsWith("substr:"))
                {
                    var rest = expr.Substring(7);
                    var parts = rest.Split(':', 3);
                    var start = int.Parse(parts[0]);
                    var len = int.Parse(parts[1]);
                    var inner = Eval(parts[2], person, profileRoot, constRoot, seed, seq);
                    if (start < 0) start = 0;
                    if (start >= inner.Length) return string.Empty;
                    if (start + len > inner.Length) len = inner.Length - start;
                    return inner.Substring(start, len);
                }

                if (expr.StartsWith("profile.") && profileRoot.HasValue)
                {
                    var tail = expr.Substring(8);
                    object? current = profileRoot.Value;
                    foreach (var part in tail.Split('.'))
                    {
                        if (current is JsonElement je)
                        {
                            if (je.ValueKind == JsonValueKind.Object && je.TryGetProperty(part, out var next))
                                current = next;
                            else return "";
                        }
                        else return "";
                    }
                    if (current is JsonElement res)
                    {
                        return res.ValueKind switch
                        {
                            JsonValueKind.String => res.GetString() ?? "",
                            JsonValueKind.Number => res.GetRawText(),
                            JsonValueKind.True => "true",
                            JsonValueKind.False => "false",
                            _ => res.GetRawText()
                        };
                    }
                }

                if (expr.StartsWith("const.") && constRoot.HasValue)
                {
                    var tail = expr.Substring(6);
                    object? current = constRoot.Value;
                    foreach (var part in tail.Split('.'))
                    {
                        if (current is JsonElement je)
                        {
                            if (je.ValueKind == JsonValueKind.Object && je.TryGetProperty(part, out var next))
                                current = next;
                            else return "";
                        }
                        else return "";
                    }
                    if (current is JsonElement resC)
                    {
                        return resC.ValueKind switch
                        {
                            JsonValueKind.String => resC.GetString() ?? "",
                            JsonValueKind.Number => resC.GetRawText(),
                            JsonValueKind.True => "true",
                            JsonValueKind.False => "false",
                            _ => resC.GetRawText()
                        };
                    }
                }

                if (expr.StartsWith("person."))
                {
                    if (person is null) return "";
                    var tail = expr.Substring(7);

                    if (tail == "identifiers|cx")
                    {
                        var reps = new List<string>();
                        foreach (var id in person.Identifiers)
                            reps.Add(Hl7Composer.JoinComponents(id.Id, "", "", id.AssigningAuthority, id.TypeCode));
                        if (reps.Count == 0 && !string.IsNullOrEmpty(person.MRN))
                            reps.Add(Hl7Composer.JoinComponents(person.MRN, "", "", "DUMMY.FAC", "MR"));
                        return string.Join(Hl7Composer.RepetitionSep, reps);
                    }

                    object current = person;
                    foreach (var part in tail.Split('.'))
                    {
                        var m = Regex.Match(part, @"^(\w+)\[(\d+)\]$");
                        if (m.Success)
                        {
                            var prop = m.Groups[1].Value;
                            var idx = int.Parse(m.Groups[2].Value);
                            current = GetIndexedProperty(current, prop, idx) ?? "";
                        }
                        else
                        {
                            current = GetProperty(current, part) ?? "";
                        }
                    }
                    return current?.ToString() ?? "";
                }
            }
            catch { }
            return "";
        }

        private static object? GetProperty(object obj, string prop)
        {
            var p = obj.GetType().GetProperty(prop, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.IgnoreCase);
            return p?.GetValue(obj);
        }

        private static object? GetIndexedProperty(object obj, string prop, int index)
        {
            var val = GetProperty(obj, prop);
            if (val is System.Collections.IList list)
            {
                if (index >= 0 && index < list.Count) return list[index];
            }
            return null;
        }
    }
}
