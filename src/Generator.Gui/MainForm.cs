using System.Reflection;
using System.Text.Json;
using HL7Forge.Core;

namespace HL7Forge.Gui
{
    public class MainForm : Form
    {
        //Tooltips
        private readonly ToolTip seedToolTip = new();

        // Top controls
        private readonly ComboBox cmbTrigger = new() { DropDownStyle = ComboBoxStyle.DropDownList, Left = 180, Top = 12, Width = 180 };
        private readonly ComboBox cmbVersion = new() { DropDownStyle = ComboBoxStyle.DropDownList, Left = 12, Top = 12, Width = 160 };
        private readonly NumericUpDown numCount = new() { Left = 365, Top = 12, Width = 60, Minimum = 1, Maximum = 100000, Value = 10 };
        private readonly NumericUpDown numSeedA = new() { Left = 430, Top = 12, Width = 60, Minimum = 0, Maximum = int.MaxValue, Value = 1234 };
        private readonly TextBox txtOut = new() { Left = 12, Top = 44, Width = 360, Text = "out" };
        private readonly Button btnBrowse = new() { Left = 378, Top = 44, Width = 82, Text = "Browse..." };
        private readonly Button btnGenerate = new() { Left = 495, Top = 12, Width = 65, Text = "Generate" };

        // Preview/Diff
        private readonly GroupBox grpPreview = new() { Left = 12, Top = 110, Width = 542, Height = 280, Text = "Preview / Diff" };
        private readonly Label lblSeg = new() { Left = 10, Top = 20, Width = 55, Text = "Segment:" };
        private readonly ComboBox cmbSegment = new() { Left = 70, Top = 16, Width = 140, DropDownStyle = ComboBoxStyle.DropDownList };
        private readonly CheckBox chkLive = new() { Left = 220, Top = 18, Width = 60, Text = "Live" };
        private readonly Button btnPreview = new() { Left = 285, Top = 16, Width = 90, Text = "Preview" };
        private readonly Label lblSeedA = new() { Left = 10, Top = 50, Width = 60, Text = "Seed A:" };
        private readonly NumericUpDown numSeedPreviewA = new() { Left = 70, Top = 46, Width = 100, Minimum = 0, Maximum = int.MaxValue, Value = 1234 };
        private readonly Label lblSeedB = new() { Left = 180, Top = 50, Width = 60, Text = "Seed B:" };
        private readonly NumericUpDown numSeedPreviewB = new() { Left = 240, Top = 46, Width = 100, Minimum = 0, Maximum = int.MaxValue, Value = 4321 };
        private readonly Button btnDiff = new() { Left = 350, Top = 44, Width = 90, Text = "Diff A vs B" };
        private readonly Label lblStatus = new() { Left = 450, Top = 48, Width = 80, Text = "" };

        private readonly Label lblA = new() { Left = 10, Top = 76, Width = 260, Text = "A (selected segment from Seed A)" };
        private readonly Label lblB = new() { Left = 276, Top = 76, Width = 260, Text = "B (selected segment from Seed B)" };
        private readonly TextBox txtPreviewA = new() { Left = 10, Top = 92, Width = 260, Height = 170, Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Vertical };
        private readonly TextBox txtPreviewB = new() { Left = 276, Top = 92, Width = 260, Height = 170, Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Vertical };

        // Cohort controls
        private readonly GroupBox grpCohort = new() { Left = 12, Top = 400, Width = 542, Height = 185, Text = "Per-Patient Batch (Cohort)" };
        private readonly Label lblTriggers = new() { Left = 10, Top = 20, Width = 130, Text = "Triggers (comma):" };
        private readonly TextBox txtTriggers = new() { Left = 140, Top = 18, Width = 388, Text = "ADT^A01,ADT^A31,ADT^A03" };
        private readonly Label lblPatients = new() { Left = 10, Top = 50, Width = 110, Text = "# Patients:" };
        private readonly NumericUpDown numPatients = new() { Left = 140, Top = 48, Width = 80, Minimum = 1, Maximum = 100000, Value = 10 };
        private readonly Label lblBaseSeed = new() { Left = 230, Top = 50, Width = 70, Text = "Base Seed:" };
        private readonly NumericUpDown numBaseSeed = new() { Left = 310, Top = 48, Width = 100, Minimum = 0, Maximum = int.MaxValue, Value = 1000 };
        private readonly Label lblSeqStart = new() { Left = 10, Top = 80, Width = 110, Text = "Seq Start:" };
        private readonly NumericUpDown numSeqStart = new() { Left = 140, Top = 78, Width = 80, Minimum = 1, Maximum = int.MaxValue, Value = 1 };
        private readonly Label lblPerTrig = new() { Left = 230, Top = 80, Width = 130, Text = "Per-trigger count:" };
        private readonly NumericUpDown numPerTrig = new() { Left = 360, Top = 78, Width = 80, Minimum = 1, Maximum = 1000, Value = 1 };
        private readonly CheckBox chkSeqPerPatient = new() { Left = 10, Top = 110, Width = 200, Text = "Reset seq for each patient", Checked = true };
        private readonly Label lblPattern = new() { Left = 10, Top = 135, Width = 120, Text = "Filename pattern:" };
        private readonly TextBox txtPattern = new() { Left = 140, Top = 132, Width = 388, Text = "${padleft:3:seed}_${profile.trigger}_${padleft:6:seq}.hl7" };
        private readonly Button btnCohort = new() { Left = 444, Top = 78, Width = 65, Text = "Generate" };
        private readonly Button btnPreviewNames = new() { Left = 140, Top = 156, Width = 140, Text = "Preview Names" };
        private readonly Button btnOpenOut = new() { Left = 290, Top = 156, Width = 238, Text = "Open Output Folder" };

        // Log panel
        private readonly GroupBox grpLog = new() { Left = 12, Top = 600, Width = 542, Height = 140, Text = "Log" };
        private readonly RichTextBox rtbLog = new() { Left = 10, Top = 18, Width = 522, Height = 90, ReadOnly = true, DetectUrls = false };
        private readonly ProgressBar prg = new() { Left = 10, Top = 112, Width = 522, Height = 16, Minimum = 0, Maximum = 100, Value = 0 };

        private readonly FolderBrowserDialog dlg = new();

        public MainForm()
        {
            Text = "HL7Forge - Dummy Message Generator";
            Width = 580; Height = 790; FormBorderStyle = FormBorderStyle.FixedDialog; MaximizeBox = false;

            seedToolTip.SetToolTip(numSeedA, "Random seed. Use same value to regenerate identical dummy data.");

            // Top row
            Controls.AddRange([cmbVersion, cmbTrigger, numCount, numSeedA, txtOut, btnBrowse, btnGenerate]);

            // Preview layout
            cmbSegment.Items.AddRange(["PID", "PV1", "PV2", "NK1", "MRG", "DG1", "AL1", "PR1", "PRD", "ROL", "RF1", "ORC", "OBR", "OBX", "SPM", "NTE", "MSH", "EVN", "MSA", "ERR", "LOC"]);
            cmbSegment.SelectedIndex = 0;

            grpPreview.Controls.Add(lblSeg);
            grpPreview.Controls.Add(cmbSegment);
            grpPreview.Controls.Add(chkLive);
            grpPreview.Controls.Add(btnPreview);
            grpPreview.Controls.Add(lblSeedA);
            grpPreview.Controls.Add(numSeedPreviewA);
            grpPreview.Controls.Add(lblSeedB);
            grpPreview.Controls.Add(numSeedPreviewB);
            grpPreview.Controls.Add(btnDiff);
            grpPreview.Controls.Add(lblStatus);
            grpPreview.Controls.Add(lblA);
            grpPreview.Controls.Add(lblB);
            grpPreview.Controls.Add(txtPreviewA);
            grpPreview.Controls.Add(txtPreviewB);
            Controls.Add(grpPreview);

            // Cohort layout
            grpCohort.Controls.Add(lblTriggers);
            grpCohort.Controls.Add(txtTriggers);
            grpCohort.Controls.Add(lblPatients);
            grpCohort.Controls.Add(numPatients);
            grpCohort.Controls.Add(lblBaseSeed);
            grpCohort.Controls.Add(numBaseSeed);
            grpCohort.Controls.Add(lblSeqStart);
            grpCohort.Controls.Add(numSeqStart);
            grpCohort.Controls.Add(lblPerTrig);
            grpCohort.Controls.Add(numPerTrig);
            grpCohort.Controls.Add(chkSeqPerPatient);
            grpCohort.Controls.Add(lblPattern);
            grpCohort.Controls.Add(txtPattern);
            grpCohort.Controls.Add(btnCohort);
            grpCohort.Controls.Add(btnPreviewNames);
            grpCohort.Controls.Add(btnOpenOut);
            Controls.Add(grpCohort);

            // Log layout
            grpLog.Controls.Add(rtbLog);
            grpLog.Controls.Add(prg);
            Controls.Add(grpLog);

            // Events
            btnBrowse.Click += (s, e) => { if (dlg.ShowDialog() == DialogResult.OK) { txtOut.Text = dlg.SelectedPath; } };
            btnGenerate.Click += async (s, e) => await Generate();
            btnPreview.Click += (s, e) => RefreshPreviews(true);
            btnDiff.Click += (s, e) => RefreshPreviews(false);
            cmbTrigger.SelectedIndexChanged += (s, e) => MaybeLivePreview();
            cmbVersion.SelectedIndexChanged += (s, e) => { ReloadTriggersForSelectedVersion(); MaybeLivePreview(); };
            numSeedA.ValueChanged += (s, e) => MaybeLivePreview();
            numSeedPreviewA.ValueChanged += (s, e) => MaybeLivePreview();
            cmbSegment.SelectedIndexChanged += (s, e) => MaybeLivePreview();
            chkLive.CheckedChanged += (s, e) => MaybeLivePreview();
            btnCohort.Click += async (s, e) => await GenerateCohort();
            btnPreviewNames.Click += (s, e) => PreviewFilenames();
            btnOpenOut.Click += (s, e) => OpenOutputFolder();

            // Populate versions and triggers dynamically
            LoadVersions();
            //ReloadTriggersForSelectedVersion();

            // Sync preview A seed with main seed
            numSeedPreviewA.Value = numSeedA.Value;
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

        private void MaybeLivePreview()
        {
            if (chkLive.Checked)
            {
                RefreshPreviews(true);
            }
        }

        private static string[] BuildAndSplit(string trigger, string version, int seedA, int seedVisit)
        {
            var policy = new SafePiPolicy();
            var consts = ConstantsStore.Load(AppContext.BaseDirectory, version);
            policy.Apply(consts);
            var faker = new DataFaker(policy, consts);

            string profilesDir = Path.Combine(AppContext.BaseDirectory, "Profiles", version);
            string profilePath = Path.Combine(profilesDir, trigger.Replace("^", "_") + ".json");
            string profileJson = File.Exists(profilePath) ? File.ReadAllText(profilePath) : "{}";
            var factory = new SegmentFactory(policy, faker, profileJson);
            string msg = factory.BuildMessage(trigger, version, seedA, seedVisit, 1);
            string[] lines = msg.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);
            return lines;
        }

        private static string ExtractSegment(string[] lines, string seg)
        {
            string wanted = seg.ToUpperInvariant() + "|";
            var hits = lines.Where(l => l.StartsWith(wanted, StringComparison.OrdinalIgnoreCase)).ToList();
            return hits.Count == 0 ? "(no segment present in current profile)" : string.Join(Environment.NewLine, hits);
        }

        private void RefreshPreviews(bool singleOnly)
        {
            try
            {
                lblStatus.Text = "Generating...";
                string trigger = (string)cmbTrigger.SelectedItem!;
                string version = (string)cmbVersion.SelectedItem!;
                string seg = (string)cmbSegment.SelectedItem!;

                int seedA = (int)numSeedPreviewA.Value;
                string[] linesA = BuildAndSplit(trigger, version, seedA, seedA + 1000);
                string segA = ExtractSegment(linesA, seg);
                txtPreviewA.Font = new Font(FontFamily.GenericMonospace, 9.0f);
                txtPreviewA.Text = segA;

                if (!singleOnly)
                {
                    int seedB = (int)numSeedPreviewB.Value;
                    string[] linesB = BuildAndSplit(trigger, version, seedB, seedB + 1000);
                    string segB = ExtractSegment(linesB, seg);
                    txtPreviewB.Font = new Font(FontFamily.GenericMonospace, 9.0f);
                    txtPreviewB.Text = segB;
                    lblStatus.Text = (segA == segB) ? "Identical" : "Different";
                }
                else
                {
                    txtPreviewB.Text = string.Empty;
                    lblStatus.Text = "OK";
                }
            }
            catch (Exception ex)
            {
                txtPreviewA.Text = "Error: " + ex.Message;
                txtPreviewB.Text = string.Empty;
                lblStatus.Text = "Error";
            }
        }

        private async Task Generate()
        {
            string trigger = (string)cmbTrigger.SelectedItem!;
            string version = (string)cmbVersion.SelectedItem!;
            int count = (int)numCount.Value;
            int seedA = (int)numSeedA.Value;
            string outDir = txtOut.Text.Trim();
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
