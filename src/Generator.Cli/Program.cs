// <copyright file="Program.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Generator.Core;

string trigger = args.Length > 0 ? args[0] : "ADT^A01";
string version = args.Length > 1 ? args[1] : "2.5.1";
int count = args.Length > 2 ? int.Parse(args[2]) : 1;
int seed = args.Length > 3 ? int.Parse(args[3]) : 1234;
string outDir = args.Length > 4 ? args[4] : "out";

Directory.CreateDirectory(outDir);

var policy = new SafePiPolicy();
var consts = ConstantsStore.Load(AppContext.BaseDirectory, version);
policy.Apply(consts);
var faker = new DataFaker(policy, consts);
string profilePath = Path.Combine(AppContext.BaseDirectory, "Profiles", version, "ADT_A01.json");
string profileJson = File.Exists(profilePath) ? File.ReadAllText(profilePath) : "{}";
var factory = new SegmentFactory(policy, faker, profileJson);

for (int i = 0; i < count; i++)
{
    string msg = factory.BuildMessage(trigger, version, seed + i, seed + 1000 + i, i + 1);
    string file = Path.Combine(outDir, $"{trigger.Replace('^', '_')}_{i + 1:000}.hl7");
    await File.WriteAllTextAsync(file, msg).ConfigureAwait(false);
}

Console.WriteLine($"Generated {count} message(s) of {trigger} to {Path.GetFullPath(outDir)}");
