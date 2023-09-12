

namespace DOC_PhraseFinder
{
    partial class PhraseFinderForm
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            MStrip = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            OpenWord = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            settingsToolStripMenuItem = new ToolStripMenuItem();
            ofd = new OpenFileDialog();
            toolTip1 = new ToolTip(components);
            btnAdd = new Button();
            btnRemove = new Button();
            btnSave = new Button();
            nudPage = new NumericUpDown();
            btnNext = new Button();
            dgv_phrases = new DataGridView();
            gbPageCtrl = new GroupBox();
            btnNavigate = new Button();
            tbViewPage = new TextBox();
            btnViewDoc = new Button();
            cbWholeWord = new CheckBox();
            groupBox1 = new GroupBox();
            label2 = new Label();
            label1 = new Label();
            tbNumPages = new TextBox();
            tbTotalMatch = new TextBox();
            tbPdfName = new TextBox();
            groupBox6 = new GroupBox();
            searchPanel = new GroupBox();
            pbarLoading = new ProgressBar();
            btnRunSearch = new Button();
            btnStopScan = new Button();
            groupBox10 = new GroupBox();
            cbIgnoreCase = new CheckBox();
            groupBox9 = new GroupBox();
            btnInvert = new Button();
            btnUncheckall = new Button();
            btnSelectAll = new Button();
            groupBox8 = new GroupBox();
            MStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudPage).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgv_phrases).BeginInit();
            gbPageCtrl.SuspendLayout();
            groupBox1.SuspendLayout();
            groupBox6.SuspendLayout();
            searchPanel.SuspendLayout();
            groupBox10.SuspendLayout();
            groupBox9.SuspendLayout();
            groupBox8.SuspendLayout();
            SuspendLayout();
            // 
            // MStrip
            // 
            MStrip.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, helpToolStripMenuItem, settingsToolStripMenuItem });
            MStrip.Location = new Point(0, 0);
            MStrip.Name = "MStrip";
            MStrip.Size = new Size(811, 24);
            MStrip.TabIndex = 0;
            MStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { OpenWord, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // OpenWord
            // 
            OpenWord.Name = "OpenWord";
            OpenWord.Size = new Size(131, 22);
            OpenWord.Text = "Open DOC";
            OpenWord.Click += OpenWord_Click;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(131, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(44, 20);
            helpToolStripMenuItem.Text = "Help";
            helpToolStripMenuItem.Click += helpToolStripMenuItem_Click;
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new Size(59, 20);
            settingsToolStripMenuItem.Text = "Phrases";
            settingsToolStripMenuItem.Click += settingsToolStripMenuItem_Click;
            // 
            // ofd
            // 
            ofd.FileName = "openFileDialog1";
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(15, 19);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(97, 23);
            btnAdd.TabIndex = 0;
            btnAdd.Text = "Add Phrase";
            toolTip1.SetToolTip(btnAdd, "Adds a dummy  phrase");
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnRemove
            // 
            btnRemove.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnRemove.ForeColor = Color.Red;
            btnRemove.Location = new Point(15, 64);
            btnRemove.Name = "btnRemove";
            btnRemove.Size = new Size(97, 23);
            btnRemove.TabIndex = 1;
            btnRemove.Text = "Delete Phrase";
            toolTip1.SetToolTip(btnRemove, "Delete all checked phrases");
            btnRemove.UseVisualStyleBackColor = true;
            btnRemove.Click += btnRemove_Click;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(15, 107);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(97, 23);
            btnSave.TabIndex = 2;
            btnSave.Text = "Save Phrases";
            toolTip1.SetToolTip(btnSave, "Save in users app data");
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // nudPage
            // 
            nudPage.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            nudPage.Location = new Point(138, 19);
            nudPage.Name = "nudPage";
            nudPage.Size = new Size(16, 33);
            nudPage.TabIndex = 1;
            toolTip1.SetToolTip(nudPage, "changes the page");
            nudPage.Visible = false;
            nudPage.ValueChanged += nudPage_ValueChanged;
            // 
            // btnNext
            // 
            btnNext.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            btnNext.Location = new Point(250, 21);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(98, 29);
            btnNext.TabIndex = 3;
            btnNext.Text = "Next Phrase";
            toolTip1.SetToolTip(btnNext, "next phrase on any found page");
            btnNext.UseVisualStyleBackColor = true;
            btnNext.Visible = false;
            btnNext.Click += btnNext_Click;
            // 
            // dgv_phrases
            // 
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgv_phrases.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgv_phrases.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv_phrases.Location = new Point(295, 116);
            dgv_phrases.Name = "dgv_phrases";
            dgv_phrases.RowTemplate.Height = 25;
            dgv_phrases.Size = new Size(473, 505);
            dgv_phrases.TabIndex = 0;
            toolTip1.SetToolTip(dgv_phrases, "click any row to select a phrase  for page viewing");
            dgv_phrases.Click += dgv_phrases_Click;
            // 
            // gbPageCtrl
            // 
            gbPageCtrl.Controls.Add(btnNavigate);
            gbPageCtrl.Controls.Add(btnNext);
            gbPageCtrl.Controls.Add(tbViewPage);
            gbPageCtrl.Controls.Add(nudPage);
            gbPageCtrl.Controls.Add(btnViewDoc);
            gbPageCtrl.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            gbPageCtrl.Location = new Point(295, 22);
            gbPageCtrl.Name = "gbPageCtrl";
            gbPageCtrl.Size = new Size(473, 69);
            gbPageCtrl.TabIndex = 6;
            gbPageCtrl.TabStop = false;
            gbPageCtrl.Text = "Page View Control (page/word)";
            toolTip1.SetToolTip(gbPageCtrl, "Display the next or the previous\r\npage that had the phrase");
            gbPageCtrl.Visible = false;
            // 
            // btnNavigate
            // 
            btnNavigate.Enabled = false;
            btnNavigate.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnNavigate.ForeColor = Color.Green;
            btnNavigate.Location = new Point(382, 21);
            btnNavigate.Name = "btnNavigate";
            btnNavigate.Size = new Size(75, 29);
            btnNavigate.TabIndex = 4;
            btnNavigate.Text = "Navigate";
            toolTip1.SetToolTip(btnNavigate, "move this page to corner of screen\r\nbefore clicking button so the document\r\nis more visible");
            btnNavigate.UseVisualStyleBackColor = true;
            btnNavigate.Click += btnNavigate_Click;
            // 
            // tbViewPage
            // 
            tbViewPage.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            tbViewPage.Location = new Point(176, 26);
            tbViewPage.Name = "tbViewPage";
            tbViewPage.Size = new Size(57, 23);
            tbViewPage.TabIndex = 2;
            tbViewPage.Text = "1";
            // 
            // btnViewDoc
            // 
            btnViewDoc.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            btnViewDoc.Location = new Point(19, 22);
            btnViewDoc.Name = "btnViewDoc";
            btnViewDoc.Size = new Size(99, 29);
            btnViewDoc.TabIndex = 0;
            btnViewDoc.Text = "View Doc";
            btnViewDoc.UseVisualStyleBackColor = true;
            btnViewDoc.Click += btnViewDoc_Click;
            // 
            // cbWholeWord
            // 
            cbWholeWord.AutoSize = true;
            cbWholeWord.Checked = true;
            cbWholeWord.CheckState = CheckState.Checked;
            cbWholeWord.Location = new Point(128, 23);
            cbWholeWord.Name = "cbWholeWord";
            cbWholeWord.Size = new Size(92, 19);
            cbWholeWord.TabIndex = 6;
            cbWholeWord.Text = "Whole Word";
            cbWholeWord.UseVisualStyleBackColor = true;
            cbWholeWord.CheckedChanged += cbWholeWord_CheckedChanged;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(tbNumPages);
            groupBox1.Controls.Add(tbTotalMatch);
            groupBox1.Controls.Add(tbPdfName);
            groupBox1.Location = new Point(7, 163);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(230, 122);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 85);
            label2.Name = "label2";
            label2.Size = new Size(117, 15);
            label2.TabIndex = 3;
            label2.Text = "Total Matches Found";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 53);
            label1.Name = "label1";
            label1.Size = new Size(103, 15);
            label1.TabIndex = 2;
            label1.Text = "Total Pages In Doc";
            // 
            // tbNumPages
            // 
            tbNumPages.Location = new Point(126, 50);
            tbNumPages.Name = "tbNumPages";
            tbNumPages.Size = new Size(58, 23);
            tbNumPages.TabIndex = 0;
            // 
            // tbTotalMatch
            // 
            tbTotalMatch.Location = new Point(126, 82);
            tbTotalMatch.Name = "tbTotalMatch";
            tbTotalMatch.Size = new Size(58, 23);
            tbTotalMatch.TabIndex = 1;
            // 
            // tbPdfName
            // 
            tbPdfName.BackColor = SystemColors.ControlLightLight;
            tbPdfName.ForeColor = SystemColors.MenuHighlight;
            tbPdfName.Location = new Point(39, 22);
            tbPdfName.Name = "tbPdfName";
            tbPdfName.ReadOnly = true;
            tbPdfName.Size = new Size(145, 23);
            tbPdfName.TabIndex = 0;
            // 
            // groupBox6
            // 
            groupBox6.Controls.Add(gbPageCtrl);
            groupBox6.Controls.Add(searchPanel);
            groupBox6.Controls.Add(groupBox10);
            groupBox6.Controls.Add(groupBox1);
            groupBox6.Controls.Add(groupBox9);
            groupBox6.Controls.Add(groupBox8);
            groupBox6.Controls.Add(dgv_phrases);
            groupBox6.Location = new Point(12, 42);
            groupBox6.Name = "groupBox6";
            groupBox6.Size = new Size(788, 643);
            groupBox6.TabIndex = 4;
            groupBox6.TabStop = false;
            groupBox6.Text = "Phrase Searching";
            // 
            // searchPanel
            // 
            searchPanel.Controls.Add(pbarLoading);
            searchPanel.Controls.Add(btnRunSearch);
            searchPanel.Controls.Add(btnStopScan);
            searchPanel.Enabled = false;
            searchPanel.ForeColor = SystemColors.ControlText;
            searchPanel.Location = new Point(7, 22);
            searchPanel.Name = "searchPanel";
            searchPanel.Size = new Size(268, 122);
            searchPanel.TabIndex = 8;
            searchPanel.TabStop = false;
            searchPanel.Text = "Search Progress";
            // 
            // pbarLoading
            // 
            pbarLoading.Location = new Point(119, 31);
            pbarLoading.Name = "pbarLoading";
            pbarLoading.Size = new Size(132, 23);
            pbarLoading.TabIndex = 0;
            // 
            // btnRunSearch
            // 
            btnRunSearch.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            btnRunSearch.ForeColor = SystemColors.MenuHighlight;
            btnRunSearch.Location = new Point(7, 25);
            btnRunSearch.Name = "btnRunSearch";
            btnRunSearch.Size = new Size(102, 36);
            btnRunSearch.TabIndex = 3;
            btnRunSearch.Text = "Run Search";
            btnRunSearch.UseVisualStyleBackColor = true;
            btnRunSearch.Click += btnRunSearch_Click;
            // 
            // btnStopScan
            // 
            btnStopScan.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnStopScan.ForeColor = Color.Red;
            btnStopScan.Location = new Point(7, 77);
            btnStopScan.Name = "btnStopScan";
            btnStopScan.Size = new Size(131, 23);
            btnStopScan.TabIndex = 0;
            btnStopScan.Text = "Click to stop";
            btnStopScan.UseVisualStyleBackColor = true;
            btnStopScan.Click += btnStopScan_Click;
            // 
            // groupBox10
            // 
            groupBox10.Controls.Add(cbWholeWord);
            groupBox10.Controls.Add(cbIgnoreCase);
            groupBox10.Location = new Point(7, 300);
            groupBox10.Name = "groupBox10";
            groupBox10.Size = new Size(268, 63);
            groupBox10.TabIndex = 5;
            groupBox10.TabStop = false;
            groupBox10.Text = "Local Settings";
            // 
            // cbIgnoreCase
            // 
            cbIgnoreCase.AutoSize = true;
            cbIgnoreCase.Checked = true;
            cbIgnoreCase.CheckState = CheckState.Checked;
            cbIgnoreCase.Location = new Point(22, 23);
            cbIgnoreCase.Name = "cbIgnoreCase";
            cbIgnoreCase.Size = new Size(88, 19);
            cbIgnoreCase.TabIndex = 5;
            cbIgnoreCase.Text = "Ignore Case";
            cbIgnoreCase.UseVisualStyleBackColor = true;
            cbIgnoreCase.CheckedChanged += cbIgnoreCase_CheckedChanged_1;
            // 
            // groupBox9
            // 
            groupBox9.Controls.Add(btnInvert);
            groupBox9.Controls.Add(btnUncheckall);
            groupBox9.Controls.Add(btnSelectAll);
            groupBox9.Location = new Point(147, 388);
            groupBox9.Name = "groupBox9";
            groupBox9.Size = new Size(127, 146);
            groupBox9.TabIndex = 4;
            groupBox9.TabStop = false;
            groupBox9.Text = "Checkbox Property";
            // 
            // btnInvert
            // 
            btnInvert.Location = new Point(9, 117);
            btnInvert.Name = "btnInvert";
            btnInvert.Size = new Size(104, 23);
            btnInvert.TabIndex = 2;
            btnInvert.Text = "Invert Selection";
            btnInvert.UseVisualStyleBackColor = true;
            btnInvert.Click += btnInvert_Click;
            // 
            // btnUncheckall
            // 
            btnUncheckall.Location = new Point(9, 73);
            btnUncheckall.Name = "btnUncheckall";
            btnUncheckall.Size = new Size(104, 23);
            btnUncheckall.TabIndex = 1;
            btnUncheckall.Text = "Uncheck all";
            btnUncheckall.UseVisualStyleBackColor = true;
            btnUncheckall.Click += btnUncheckall_Click;
            // 
            // btnSelectAll
            // 
            btnSelectAll.Location = new Point(9, 31);
            btnSelectAll.Name = "btnSelectAll";
            btnSelectAll.Size = new Size(104, 23);
            btnSelectAll.TabIndex = 0;
            btnSelectAll.Text = "Check All";
            btnSelectAll.UseVisualStyleBackColor = true;
            btnSelectAll.Click += btnSelectAll_Click;
            // 
            // groupBox8
            // 
            groupBox8.Controls.Add(btnSave);
            groupBox8.Controls.Add(btnRemove);
            groupBox8.Controls.Add(btnAdd);
            groupBox8.Location = new Point(6, 388);
            groupBox8.Name = "groupBox8";
            groupBox8.Size = new Size(127, 146);
            groupBox8.TabIndex = 2;
            groupBox8.TabStop = false;
            groupBox8.Text = "Phrases";
            // 
            // PhraseFinderForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImageLayout = ImageLayout.Zoom;
            ClientSize = new Size(811, 697);
            Controls.Add(groupBox6);
            Controls.Add(MStrip);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            MainMenuStrip = MStrip;
            MaximizeBox = false;
            Name = "PhraseFinderForm";
            Text = "USDA/FNS  -  Phrase Finder for Word Documents";
            FormClosing += PhraseFinderForm_FormClosing;
            MStrip.ResumeLayout(false);
            MStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudPage).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgv_phrases).EndInit();
            gbPageCtrl.ResumeLayout(false);
            gbPageCtrl.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox6.ResumeLayout(false);
            searchPanel.ResumeLayout(false);
            groupBox10.ResumeLayout(false);
            groupBox10.PerformLayout();
            groupBox9.ResumeLayout(false);
            groupBox8.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip MStrip;
        private OpenFileDialog ofd;
        private ToolTip toolTip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private GroupBox groupBox1;
        private TextBox tbPdfName;
        private TextBox tbTotalMatch;
        private TextBox tbNumPages;
        private GroupBox groupBox6;
        private DataGridView dgv_phrases;
        private GroupBox groupBox8;
        private Button btnSave;
        private Button btnRemove;
        private Button btnAdd;
        private GroupBox groupBox9;
        private Button btnInvert;
        private Button btnUncheckall;
        private Button btnSelectAll;
        private GroupBox gbPageCtrl;
        private TextBox tbViewPage;
        private NumericUpDown nudPage;
        private Button btnViewDoc;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private Button btnNext;
        private CheckBox cbWholeWord;
        private CheckBox cbIgnoreCase;
        private GroupBox groupBox10;
        private ToolStripMenuItem OpenWord;
        private GroupBox searchPanel;
        private Button btnStopScan;
        private ProgressBar pbarLoading;
        private Button btnRunSearch;
        private Label label2;
        private Label label1;
        private Button btnNavigate;
    }
}