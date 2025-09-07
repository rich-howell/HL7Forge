using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace HL7Forge.Core
{
    public class SegmentFactory
    {
        private readonly SafePiPolicy _policy;
        private readonly DataFaker _faker;
        private readonly JsonDocument _profile;
        private readonly PersonStore _people;
        private readonly ConstantsStore _constants;
        private readonly Dictionary<string, JsonDocument> _maps = new();
        private int _currentSeed = 0;
        private int _currentSeq = 0;

        public SegmentFactory(SafePiPolicy policy, DataFaker faker, string profileJson)
        {
            _policy = policy;
            _faker = faker;
            _profile = JsonDocument.Parse(string.IsNullOrWhiteSpace(profileJson) ? "{}" : profileJson);
            var version = _profile.RootElement.TryGetProperty("version", out var vEl) ? (vEl.GetString() ?? "2.5.1") : "2.5.1";
            _people = PersonStore.Load(AppContext.BaseDirectory, version);
            _constants = ConstantsStore.Load(AppContext.BaseDirectory, version);

            // Load maps lazily: if there is a maps section in profile, ingest it; also load from disk folder maps/<SEG>.map.json
            if (_profile.RootElement.TryGetProperty("maps", out var mapsEl) && mapsEl.ValueKind == JsonValueKind.Object)
            {
                foreach (var prop in mapsEl.EnumerateObject())
                {
                    _maps[prop.Name.ToUpperInvariant()] = JsonDocument.Parse(prop.Value.GetRawText());
                }
            }
            // Disk-based maps
            var mapsPath = System.IO.Path.Combine(AppContext.BaseDirectory, "Profiles", version, "maps");
            if (System.IO.Directory.Exists(mapsPath))
            {
                foreach (var file in System.IO.Directory.EnumerateFiles(mapsPath, "*.json"))
                {
                    try
                    {
                        var jd = JsonDocument.Parse(System.IO.File.ReadAllText(file));
                        if (jd.RootElement.TryGetProperty("segment", out var segEl))
                        {
                            var segName = (segEl.GetString() ?? "").ToUpperInvariant();
                            if (!string.IsNullOrEmpty(segName)) _maps[segName] = jd;
                        }
                    } catch {}
                }
            }
        }

        public string BuildMessage(string trigger, string version, int patientSeed, int visitSeed, int seq)
        {
            var sb = new StringBuilder();
            var order = GetOrder(trigger, version);
            int idx = 0;

            foreach (var item in order)
            {
                var segName = item.Name.ToUpperInvariant();
                int repeat = item.Repeat;

                for (int r=0; r<repeat; r++)
                {
                    _currentSeed = patientSeed + idx + r;
                    _currentSeq = seq;

                    var line = BuildByName(segName, trigger, version, patientSeed + idx + r, visitSeed + idx + r);
                    if (!string.IsNullOrEmpty(line))
                    {
                        line = PatchIfMapped(line);
                        sb.Append(line);
                        if (!line.EndsWith("\r")) sb.Append("\r");
                    }
                }
                idx++;
            }
            return sb.ToString();
        }

        private (string Name,int Repeat)[] GetOrder(string trigger, string version)
        {
            var list = new List<(string,int)>();
            if (_profile.RootElement.TryGetProperty("order", out var orderEl) && orderEl.ValueKind == JsonValueKind.Array)
            {
                foreach (var node in orderEl.EnumerateArray())
                {
                    if (node.ValueKind == JsonValueKind.String)
                    {
                        list.Add((node.GetString() ?? "", 1));
                    }
                    else if (node.ValueKind == JsonValueKind.Object)
                    {
                        var seg = node.TryGetProperty("segment", out var sEl) ? sEl.GetString() ?? "" : "";
                        int rep = 1;
                        if (node.TryGetProperty("repeat", out var rEl))
                        {
                            if (rEl.ValueKind == JsonValueKind.Number) rep = rEl.GetInt32();
                            else if (rEl.ValueKind == JsonValueKind.String)
                            {
                                var txt = rEl.GetString() ?? "1";
                                if (txt.Contains("-"))
                                {
                                    var parts = txt.Split('-');
                                    int min = int.Parse(parts[0]); int max = int.Parse(parts[1]);
                                    var rng = new Random(_currentSeed ^ 0xC0FFEE);
                                    rep = rng.Next(min, max+1);
                                }
                                else int.TryParse(txt, out rep);
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(seg)) list.Add((seg, Math.Max(1, rep)));
                    }
                }
            }

            if (list.Count == 0)
            {
                // Reasonable default for ADT_A01 if profile lacks order
                list.AddRange(new [] { ("MSH",1),("EVN",1),("PID",1),("PD1",1),("PV1",1),("PV2",1),("NK1",1) });
            }
            return list.ToArray();
        }

        private string PatchIfMapped(string segLine)
        {
            var seg = segLine.Split('|')[0].ToUpperInvariant();
            _maps.TryGetValue(seg, out var mapDoc);
            var person = _people.HasPeople ? _people.GetBySeed(_currentSeed) : null;
            return SegmentPatcher.Apply(segLine, mapDoc?.RootElement, person, _profile.RootElement, _constants.Root, _currentSeed, _currentSeq);
        }

        private string BuildByName(string seg, string trigger, string version, int patientSeed, int visitSeed)
        {
            // Fast path for MSH: include trigger/version and default apps/facilities.
            switch (seg)
            {
                case "MSH": return BuildMSH(trigger, version);
                case "EVN": return "EVN|" + DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                case "PID": return "PID|";
                case "PD1": return "PD1|";
                case "PV1": return "PV1|";
                case "PV2": return "PV2|";
                case "NK1": return "NK1|";
                case "MRG": return "MRG|";
                case "DG1": return "DG1|";
                case "AL1": return "AL1|";
                case "PR1": return "PR1|";
                case "PRD": return "PRD|";
                case "ROL": return "ROL|";
                case "RF1": return "RF1|";
                case "ORC": return "ORC|";
                case "OBR": return "OBR|";
                case "OBX": return "OBX|";
                case "SPM": return "SPM|";
                case "NTE": return "NTE|";
                case "MSA": return "MSA|";
                case "ERR": return "ERR|";
                case "LOC": return "LOC|";
                default:
                    return seg + "|";
            }
        }

        private string BuildMSH(string trigger, string version)
        {
            // MSH-1 |, MSH-2 encoding ^~\&, then sending/receiving apps/facilities from policy/constants
            string enc = "^~\\&";
            string dtm = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            string msgType = trigger; // e.g., ADT^A01
            string controlId = (_currentSeq > 0) ? _currentSeq.ToString() : new Random(_currentSeed ^ 0xBADC0DE).Next(1, 99999999).ToString();
            var sb = new StringBuilder();
            sb.Append("MSH|").Append(enc).Append('|');
            sb.Append(_policy.DefaultSendingApplication).Append('|');
            sb.Append(_policy.DefaultSendingFacility).Append('|');
            sb.Append(_policy.DefaultReceivingApplication).Append('|');
            sb.Append(_policy.DefaultReceivingFacility).Append('|');
            sb.Append(dtm).Append("||"); // security empty, message type next
            sb.Append(msgType).Append('|');
            sb.Append(controlId).Append('|');
            sb.Append(version);
            return sb.ToString();
        }
    }
}
