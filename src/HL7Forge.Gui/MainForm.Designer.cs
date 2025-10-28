namespace HL7Forge.Gui
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            seedToolTip = new ToolTip(components);
            cmbVersion = new ComboBox();
            cmbTrigger = new ComboBox();
            numCount = new NumericUpDown();
            numSeedA = new NumericUpDown();
            btnGenerate = new Button();
            txtOut = new TextBox();
            btnBrowse = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            lblStatus = new Label();
            grpCohort = new GroupBox();
            btnPreviewNames = new Button();
            txtPattern = new TextBox();
            lblPattern = new Label();
            chkSeqPerPatient = new CheckBox();
            btnCohort = new Button();
            numPerTrig = new NumericUpDown();
            numSeqStart = new NumericUpDown();
            lblPerTrig = new Label();
            lblSeqStart = new Label();
            numBaseSeed = new NumericUpDown();
            lblBaseSeed = new Label();
            numPatients = new NumericUpDown();
            lblPatients = new Label();
            txtTriggers = new TextBox();
            lblTriggers = new Label();
            btnOpenOut = new Button();
            grpLog = new GroupBox();
            rtbLog = new RichTextBox();
            prg = new ProgressBar();
            dlg = new FolderBrowserDialog();
            ((System.ComponentModel.ISupportInitialize)numCount).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numSeedA).BeginInit();
            grpCohort.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numPerTrig).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numSeqStart).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numBaseSeed).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numPatients).BeginInit();
            grpLog.SuspendLayout();
            SuspendLayout();
            // 
            // cmbVersion
            // 
            cmbVersion.FormattingEnabled = true;
            cmbVersion.Location = new Point(23, 25);
            cmbVersion.Name = "cmbVersion";
            cmbVersion.Size = new Size(121, 23);
            cmbVersion.TabIndex = 0;
            // 
            // cmbTrigger
            // 
            cmbTrigger.FormattingEnabled = true;
            cmbTrigger.Items.AddRange(new object[] { "ADT^A01", "ADT^A31", "ADT^A03" });
            cmbTrigger.Location = new Point(150, 25);
            cmbTrigger.Name = "cmbTrigger";
            cmbTrigger.Size = new Size(170, 23);
            cmbTrigger.TabIndex = 1;
            // 
            // numCount
            // 
            numCount.Location = new Point(326, 25);
            numCount.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numCount.Name = "numCount";
            numCount.Size = new Size(71, 23);
            numCount.TabIndex = 2;
            numCount.Tag = "100000";
            numCount.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // numSeedA
            // 
            numSeedA.Location = new Point(403, 25);
            numSeedA.Name = "numSeedA";
            numSeedA.Size = new Size(68, 23);
            numSeedA.TabIndex = 3;
            // 
            // btnGenerate
            // 
            btnGenerate.Location = new Point(465, 54);
            btnGenerate.Name = "btnGenerate";
            btnGenerate.Size = new Size(75, 23);
            btnGenerate.TabIndex = 4;
            btnGenerate.Text = "Generate";
            btnGenerate.UseVisualStyleBackColor = true;
            // 
            // txtOut
            // 
            txtOut.Location = new Point(23, 54);
            txtOut.Name = "txtOut";
            txtOut.Size = new Size(355, 23);
            txtOut.TabIndex = 5;
            // 
            // btnBrowse
            // 
            btnBrowse.Location = new Point(384, 54);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new Size(75, 23);
            btnBrowse.TabIndex = 6;
            btnBrowse.Text = "Browse...";
            btnBrowse.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(24, 7);
            label1.Name = "label1";
            label1.Size = new Size(45, 15);
            label1.TabIndex = 7;
            label1.Text = "Version";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(150, 7);
            label2.Name = "label2";
            label2.Size = new Size(43, 15);
            label2.TabIndex = 8;
            label2.Text = "Trigger";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(326, 7);
            label3.Name = "label3";
            label3.Size = new Size(40, 15);
            label3.TabIndex = 9;
            label3.Text = "Count";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(403, 7);
            label4.Name = "label4";
            label4.Size = new Size(32, 15);
            label4.TabIndex = 10;
            label4.Text = "Seed";
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(12, 569);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(0, 15);
            lblStatus.TabIndex = 12;
            // 
            // grpCohort
            // 
            grpCohort.Controls.Add(btnPreviewNames);
            grpCohort.Controls.Add(txtPattern);
            grpCohort.Controls.Add(lblPattern);
            grpCohort.Controls.Add(chkSeqPerPatient);
            grpCohort.Controls.Add(btnCohort);
            grpCohort.Controls.Add(numPerTrig);
            grpCohort.Controls.Add(numSeqStart);
            grpCohort.Controls.Add(lblPerTrig);
            grpCohort.Controls.Add(lblSeqStart);
            grpCohort.Controls.Add(numBaseSeed);
            grpCohort.Controls.Add(lblBaseSeed);
            grpCohort.Controls.Add(numPatients);
            grpCohort.Controls.Add(lblPatients);
            grpCohort.Controls.Add(txtTriggers);
            grpCohort.Controls.Add(lblTriggers);
            grpCohort.Location = new Point(12, 112);
            grpCohort.Name = "grpCohort";
            grpCohort.Size = new Size(528, 209);
            grpCohort.TabIndex = 13;
            grpCohort.TabStop = false;
            grpCohort.Text = "Per-Patient Batch (cohort)";
            // 
            // btnPreviewNames
            // 
            btnPreviewNames.Location = new Point(134, 175);
            btnPreviewNames.Name = "btnPreviewNames";
            btnPreviewNames.Size = new Size(103, 23);
            btnPreviewNames.TabIndex = 14;
            btnPreviewNames.Text = "Preview Names";
            btnPreviewNames.UseVisualStyleBackColor = true;
            // 
            // txtPattern
            // 
            txtPattern.Location = new Point(134, 146);
            txtPattern.Name = "txtPattern";
            txtPattern.Size = new Size(388, 23);
            txtPattern.TabIndex = 13;
            txtPattern.Text = "${padleft:3:seed}_${profile.trigger}_${padleft:6:seq}.hl7";
            // 
            // lblPattern
            // 
            lblPattern.AutoSize = true;
            lblPattern.Location = new Point(16, 149);
            lblPattern.Name = "lblPattern";
            lblPattern.Size = new Size(99, 15);
            lblPattern.TabIndex = 12;
            lblPattern.Text = "Filename pattern:";
            // 
            // chkSeqPerPatient
            // 
            chkSeqPerPatient.AutoSize = true;
            chkSeqPerPatient.Location = new Point(16, 121);
            chkSeqPerPatient.Name = "chkSeqPerPatient";
            chkSeqPerPatient.Size = new Size(193, 19);
            chkSeqPerPatient.TabIndex = 11;
            chkSeqPerPatient.Text = "Reset sequence for each patient";
            chkSeqPerPatient.UseVisualStyleBackColor = true;
            // 
            // btnCohort
            // 
            btnCohort.Location = new Point(243, 175);
            btnCohort.Name = "btnCohort";
            btnCohort.Size = new Size(181, 23);
            btnCohort.TabIndex = 10;
            btnCohort.Text = "Generate Per Patient Batch";
            btnCohort.UseVisualStyleBackColor = true;
            // 
            // numPerTrig
            // 
            numPerTrig.Location = new Point(300, 91);
            numPerTrig.Name = "numPerTrig";
            numPerTrig.Size = new Size(89, 23);
            numPerTrig.TabIndex = 9;
            // 
            // numSeqStart
            // 
            numSeqStart.Location = new Point(84, 91);
            numSeqStart.Name = "numSeqStart";
            numSeqStart.Size = new Size(85, 23);
            numSeqStart.TabIndex = 8;
            // 
            // lblPerTrig
            // 
            lblPerTrig.AutoSize = true;
            lblPerTrig.Location = new Point(185, 94);
            lblPerTrig.Name = "lblPerTrig";
            lblPerTrig.Size = new Size(101, 15);
            lblPerTrig.TabIndex = 7;
            lblPerTrig.Text = "Per-trigger count:";
            // 
            // lblSeqStart
            // 
            lblSeqStart.AutoSize = true;
            lblSeqStart.Location = new Point(16, 90);
            lblSeqStart.Name = "lblSeqStart";
            lblSeqStart.Size = new Size(56, 15);
            lblSeqStart.TabIndex = 6;
            lblSeqStart.Text = "Seq Start:";
            // 
            // numBaseSeed
            // 
            numBaseSeed.Location = new Point(300, 60);
            numBaseSeed.Name = "numBaseSeed";
            numBaseSeed.Size = new Size(85, 23);
            numBaseSeed.TabIndex = 5;
            // 
            // lblBaseSeed
            // 
            lblBaseSeed.AutoSize = true;
            lblBaseSeed.Location = new Point(185, 60);
            lblBaseSeed.Name = "lblBaseSeed";
            lblBaseSeed.Size = new Size(62, 15);
            lblBaseSeed.TabIndex = 4;
            lblBaseSeed.Text = "Base Seed:";
            // 
            // numPatients
            // 
            numPatients.Location = new Point(84, 60);
            numPatients.Name = "numPatients";
            numPatients.Size = new Size(85, 23);
            numPatients.TabIndex = 3;
            // 
            // lblPatients
            // 
            lblPatients.AutoSize = true;
            lblPatients.Location = new Point(16, 60);
            lblPatients.Name = "lblPatients";
            lblPatients.Size = new Size(62, 15);
            lblPatients.TabIndex = 2;
            lblPatients.Text = "# Patients:";
            // 
            // txtTriggers
            // 
            txtTriggers.Location = new Point(70, 26);
            txtTriggers.Name = "txtTriggers";
            txtTriggers.Size = new Size(452, 23);
            txtTriggers.TabIndex = 1;
            // 
            // lblTriggers
            // 
            lblTriggers.AutoSize = true;
            lblTriggers.Location = new Point(16, 29);
            lblTriggers.Name = "lblTriggers";
            lblTriggers.Size = new Size(48, 15);
            lblTriggers.TabIndex = 0;
            lblTriggers.Text = "Triggers";
            // 
            // btnOpenOut
            // 
            btnOpenOut.Location = new Point(384, 83);
            btnOpenOut.Name = "btnOpenOut";
            btnOpenOut.Size = new Size(156, 23);
            btnOpenOut.TabIndex = 15;
            btnOpenOut.Text = "Open Output Folder";
            btnOpenOut.UseVisualStyleBackColor = true;
            // 
            // grpLog
            // 
            grpLog.Controls.Add(rtbLog);
            grpLog.Controls.Add(prg);
            grpLog.Location = new Point(12, 327);
            grpLog.Name = "grpLog";
            grpLog.Size = new Size(528, 236);
            grpLog.TabIndex = 14;
            grpLog.TabStop = false;
            grpLog.Text = "Log";
            // 
            // rtbLog
            // 
            rtbLog.Location = new Point(7, 19);
            rtbLog.Name = "rtbLog";
            rtbLog.Size = new Size(515, 179);
            rtbLog.TabIndex = 0;
            rtbLog.Text = "";
            // 
            // prg
            // 
            prg.Location = new Point(7, 204);
            prg.Name = "prg";
            prg.Size = new Size(515, 19);
            prg.TabIndex = 15;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(552, 593);
            Controls.Add(btnOpenOut);
            Controls.Add(grpLog);
            Controls.Add(grpCohort);
            Controls.Add(lblStatus);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnBrowse);
            Controls.Add(txtOut);
            Controls.Add(btnGenerate);
            Controls.Add(numSeedA);
            Controls.Add(numCount);
            Controls.Add(cmbTrigger);
            Controls.Add(cmbVersion);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "MainForm";
            Text = "HL7Forge - Dummy Message Generator";
            ((System.ComponentModel.ISupportInitialize)numCount).EndInit();
            ((System.ComponentModel.ISupportInitialize)numSeedA).EndInit();
            grpCohort.ResumeLayout(false);
            grpCohort.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numPerTrig).EndInit();
            ((System.ComponentModel.ISupportInitialize)numSeqStart).EndInit();
            ((System.ComponentModel.ISupportInitialize)numBaseSeed).EndInit();
            ((System.ComponentModel.ISupportInitialize)numPatients).EndInit();
            grpLog.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ToolTip seedToolTip;
        private ComboBox cmbVersion;
        private ComboBox cmbTrigger;
        private NumericUpDown numCount;
        private NumericUpDown numSeedA;
        private Button btnGenerate;
        private TextBox txtOut;
        private Button btnBrowse;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label lblStatus;
        private GroupBox grpCohort;
        private TextBox txtTriggers;
        private Label lblTriggers;
        private Label lblPatients;
        private NumericUpDown numPatients;
        private Label lblBaseSeed;
        private NumericUpDown numPerTrig;
        private NumericUpDown numSeqStart;
        private Label lblPerTrig;
        private Label lblSeqStart;
        private NumericUpDown numBaseSeed;
        private Button btnCohort;
        private CheckBox chkSeqPerPatient;
        private Label lblPattern;
        private TextBox txtPattern;
        private Button btnOpenOut;
        private Button btnPreviewNames;
        private GroupBox grpLog;
        private RichTextBox rtbLog;
        private ProgressBar prg;
        private FolderBrowserDialog dlg;
    }
}
