using System.Reflection;
using System.Text.Json;
using HL7Forge.Core;

namespace HL7Forge.Gui
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            Icon = Properties.Resources.hl7_forge_icon;

            seedToolTip.SetToolTip(numSeedA, "Random seed. Use same value to regenerate identical dummy data.");

            // Events
            btnBrowse.Click += (s, e) => { if (dlg.ShowDialog() == DialogResult.OK) { txtOut.Text = dlg.SelectedPath; } };
            btnGenerate.Click += async (s, e) => await Generate();

            cmbVersion.SelectedIndexChanged += (s, e) => { ReloadTriggersForSelectedVersion(); };

            btnCohort.Click += async (s, e) => await GenerateCohort();
            btnPreviewNames.Click += (s, e) => PreviewFilenames();
            btnOpenOut.Click += (s, e) => OpenOutputFolder();

            LoadVersions();
            //ReloadTriggersForSelectedVersion();

        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            var asm = Assembly.GetExecutingAssembly();
            using Stream? s = asm.GetManifestResourceStream("Generator.Gui.assets.app.ico");
            if (s != null)
            {
                Icon = new Icon(s);
            }
        }

        private static string ProfilesRoot() =>
            // Files are copied to output; we look there.
            Path.Combine(AppContext.BaseDirectory, "Profiles");

        private void LoadVersions()
        {
            try
            {
                string root = ProfilesRoot();
                if (!Directory.Exists(root))
                {
                    AppendLog("Profiles folder not found in output. Ensure csproj CopyToOutput is set.", Color.DarkRed);
                    return;
                }
                var versions = Directory.GetDirectories(root).Select(Path.GetFileName).OrderBy(v => v).ToList();
                cmbVersion.Items.Clear();
                foreach (string? v in versions)
                {
                    _ = cmbVersion.Items.Add(v);
                }

                if (cmbVersion.Items.Count > 0)
                {
                    cmbVersion.SelectedIndex = cmbVersion.Items.Count - 1; // latest by name
                }
            }
            catch (Exception ex)
            {
                AppendLog("Error loading versions: " + ex.Message, Color.DarkRed);
            }
        }

        private void ReloadTriggersForSelectedVersion()
        {
            try
            {
                cmbTrigger.Items.Clear();
                string? version = cmbVersion.SelectedItem as string;
                if (string.IsNullOrWhiteSpace(version))
                {
                    return;
                }

                string dir = Path.Combine(ProfilesRoot(), version);
                if (!Directory.Exists(dir))
                {
                    return;
                }

                var files = Directory.GetFiles(dir, "*.json", SearchOption.TopDirectoryOnly)
                                     .Where(p =>
                                     {
                                         string name = Path.GetFileName(p);
                                         if (name.Equals("constants.json", StringComparison.OrdinalIgnoreCase))
                                         {
                                             return false;
                                         }

                                         if (name.Equals("people.json", StringComparison.OrdinalIgnoreCase))
                                         {
                                             return false;
                                         }

                                         return name.EndsWith(".map.json", StringComparison.OrdinalIgnoreCase) ? false : name.Contains('_');
                                     })
                                     .ToList();
                var triggers = new List<(string trigger, string file)>();

                foreach (string? file in files)
                {
                    // Avoid maps folder
                    string name = Path.GetFileName(file);
                    if (string.Equals(name, "constants.json", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    if (name.EndsWith(".map.json", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    try
                    {
                        using var doc = JsonDocument.Parse(File.ReadAllText(file));
                        if (doc.RootElement.TryGetProperty("trigger", out JsonElement tEl))
                        {
                            string? trig = tEl.GetString();
                            if (!string.IsNullOrWhiteSpace(trig))
                            {
                                triggers.Add((trig!, file));
                            }

                            continue;
                        }
                    }
                    catch { /* ignore a bad file */ }

                    // Fallback from filename: ADT_A01.json -> ADT^A01
                    string stem = Path.GetFileNameWithoutExtension(file);
                    triggers.Add((stem.Replace('_', '^'), file));
                }

                foreach (string? t in triggers.Select(t => t.trigger).Distinct().OrderBy(s => s))
                {
                    _ = cmbTrigger.Items.Add(t);
                }

                if (cmbTrigger.Items.Count > 0)
                {
                    cmbTrigger.SelectedIndex = 0;
                }

                txtTriggers.Text = string.Join(",", cmbTrigger.Items.Cast<string>());
                AppendLog($"Loaded version {version} with {cmbTrigger.Items.Count} trigger(s).", Color.Gray);
            }
            catch (Exception ex)
            {
                AppendLog("Error loading triggers: " + ex.Message, Color.DarkRed);
            }
        }

        private async Task Generate()
        {
            string trigger = (string)cmbTrigger.SelectedItem!;
            string version = (string)cmbVersion.SelectedItem!;
            int count = (int)numCount.Value;
            int seedA = (int)numSeedA.Value;
            string outDir = txtOut.Text.Trim();

            if (string.IsNullOrWhiteSpace(outDir))
            {
                outDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                outDir = Path.Combine(outDir, "Hl7Forge");
                txtOut.Text = outDir;
                AppendLog("Output Directory Not Specificed, defaulting to fallack", Color.Gray);
            }

            _ = Directory.CreateDirectory(outDir);

            var policy = new SafePiPolicy();
            var consts = ConstantsStore.Load(AppContext.BaseDirectory, version);
            policy.Apply(consts);
            var faker = new DataFaker(policy, consts);
            string profilesDir = Path.Combine(AppContext.BaseDirectory, "Profiles", version);
            string profilePath = Path.Combine(profilesDir, trigger.Replace("^", "_") + ".json");
            string profileJson = File.Exists(profilePath) ? File.ReadAllText(profilePath) : "{}";
            var factory = new SegmentFactory(policy, faker, profileJson);

            prg.Value = 0; prg.Maximum = count; prg.Step = 1;
            AppendLog($"Generating {count} {trigger} message(s) for {version}...", Color.Gray);

            for (int i = 0; i < count; i++)
            {
                string msg = factory.BuildMessage(trigger, version, seedA + i, seedA + 1000 + i, i + 1);
                string file = Path.Combine(outDir, $"{trigger.Replace('^', '_')}_{i + 1:000}.hl7");
                await File.WriteAllTextAsync(file, msg);
                AppendLog($"Wrote: {file}", Color.Black);
                if (prg.Value < prg.Maximum)
                {
                    prg.Value += 1;
                }
            }

            AppendLog($"Done. Generated {count} message(s) in: {Path.GetFullPath(outDir)}", Color.Green);
            _ = MessageBox.Show($"Generated {count} message(s) in:\n{Path.GetFullPath(outDir)}", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async Task GenerateCohort()
        {
            string version = (string)cmbVersion.SelectedItem!;
            string outDir = txtOut.Text.Trim();
            _ = Directory.CreateDirectory(outDir);

            var policy = new SafePiPolicy();
            var consts = ConstantsStore.Load(AppContext.BaseDirectory, version);
            policy.Apply(consts);
            var faker = new DataFaker(policy, consts);
            string profilesDir = Path.Combine(AppContext.BaseDirectory, "Profiles", version);

            var triggers = txtTriggers.Text.Split([','], StringSplitOptions.RemoveEmptyEntries).Select(t => t.Trim()).ToList();
            int patients = (int)numPatients.Value;
            int baseSeed = (int)numBaseSeed.Value;
            int seqStart = (int)numSeqStart.Value;
            int perTrig = (int)numPerTrig.Value;
            bool resetPerPatient = chkSeqPerPatient.Checked;
            string pattern = txtPattern.Text;

            int total = patients * triggers.Count * perTrig;
            prg.Value = 0; prg.Maximum = Math.Max(1, total); prg.Step = 1;
            AppendLog($"Cohort: patients={patients}, triggers={triggers.Count}, perTrig={perTrig} → total {total} messages", Color.Gray);

            int globalSeq = seqStart;

            for (int p = 0; p < patients; p++)
            {
                int patientSeed = baseSeed + (p * 100);
                int visitSeed = patientSeed + 1000;
                int seq = resetPerPatient ? seqStart : globalSeq;

                foreach (string? trigger in triggers)
                {
                    string profilePath = Path.Combine(profilesDir, trigger.Replace("^", "_") + ".json");
                    if (!File.Exists(profilePath))
                    {
                        continue;
                    }

                    string profileJson = File.ReadAllText(profilePath);
                    var factory = new SegmentFactory(policy, faker, profileJson);
                    using var profDoc = JsonDocument.Parse(profileJson);

                    for (int c = 0; c < perTrig; c++)
                    {
                        string msg = factory.BuildMessage(trigger, version, patientSeed, visitSeed, seq);

                        // Render filename
                        string templ = pattern.Replace("${patient}", (p + 1).ToString("000"));
                        string fname = TemplateEngine.Render(templ, null, profDoc.RootElement, consts.Root, patientSeed, seq);
                        if (string.IsNullOrWhiteSpace(fname))
                        {
                            fname = $"{p + 1:000}_{trigger.Replace('^', '_')}_{seq:000000}.hl7";
                        }

                        foreach (char bad in Path.GetInvalidFileNameChars())
                        {
                            fname = fname.Replace(bad, '_');
                        }

                        string file = Path.Combine(outDir, fname);
                        await File.WriteAllTextAsync(file, msg);
                        AppendLog($"Wrote: {file}", Color.Black);
                        if (prg.Value < prg.Maximum)
                        {
                            prg.Value += 1;
                        }

                        seq++;
                        if (!resetPerPatient)
                        {
                            globalSeq = seq;
                        }
                    }
                }
            }

            AppendLog($"Done. Cohort in: {Path.GetFullPath(outDir)}", Color.Green);
            _ = MessageBox.Show($"Cohort generated in:\n{Path.GetFullPath(outDir)}", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void PreviewFilenames()
        {
            try
            {
                string version = (string)cmbVersion.SelectedItem!;
                var consts = ConstantsStore.Load(AppContext.BaseDirectory, version);

                var triggers = txtTriggers.Text.Split([','], StringSplitOptions.RemoveEmptyEntries).Select(t => t.Trim()).ToList();
                int baseSeed = (int)numBaseSeed.Value;
                int seqStart = (int)numSeqStart.Value;
                bool resetPerPatient = chkSeqPerPatient.Checked;
                int perTrig = (int)numPerTrig.Value;
                string pattern = txtPattern.Text;

                string sampleTrigger = triggers.Count > 0 ? triggers.First() : "ADT^A01";
                string profileSamplePath = Path.Combine(AppContext.BaseDirectory, "Profiles", version, sampleTrigger.Replace("^", "_") + ".json");
                string profileJson = File.Exists(profileSamplePath) ? File.ReadAllText(profileSamplePath) : "{\"trigger\":\"" + sampleTrigger + "\",\"version\":\"" + version + "\"}";
                using var profDoc = JsonDocument.Parse(profileJson);

                var examples = new System.Text.StringBuilder();
                int patients = 1;
                int seq = seqStart;
                for (int p = 0; p < patients; p++)
                {
                    foreach (string? trigger in triggers)
                    {
                        for (int c = 0; c < perTrig; c++)
                        {
                            string templ = pattern.Replace("${patient}", (p + 1).ToString("000"));
                            string fname = TemplateEngine.Render(templ, null, profDoc.RootElement, consts.Root, baseSeed + (p * 100), seq);
                            if (string.IsNullOrWhiteSpace(fname))
                            {
                                fname = $"{p + 1:000}_{trigger.Replace('^', '_')}_{seq:000000}.hl7";
                            }

                            foreach (char bad in Path.GetInvalidFileNameChars())
                            {
                                fname = fname.Replace(bad, '_');
                            }

                            _ = examples.AppendLine(fname);
                            seq++;
                        }
                    }
                    if (resetPerPatient)
                    {
                        seq = seqStart;
                    }
                }

                _ = MessageBox.Show(examples.ToString(), "Filename Preview (first patient)", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show("Error generating preview: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenOutputFolder()
        {
            try
            {
                string outDir = txtOut.Text.Trim();
                if (!Directory.Exists(outDir))
                {
                    _ = Directory.CreateDirectory(outDir);
                }

                _ = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "explorer.exe",
                    Arguments = Path.GetFullPath(outDir),
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show("Unable to open folder: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AppendLog(string text, Color color)
        {
            try
            {
                rtbLog.SelectionStart = rtbLog.TextLength;
                rtbLog.SelectionLength = 0;
                rtbLog.SelectionColor = color;
                rtbLog.AppendText(text + Environment.NewLine);
                rtbLog.SelectionColor = rtbLog.ForeColor;
                rtbLog.ScrollToCaret();
            }
            catch { }
        }
    }
}
