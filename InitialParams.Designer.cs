namespace PDF_PhraseFinder
{
    partial class InitialParams
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            groupBox2 = new GroupBox();
            groupBox1 = new GroupBox();
            btnUnChk = new Button();
            bltnChkAll = new Button();
            textBox1 = new TextBox();
            btnChkErr = new Button();
            label1 = new Label();
            btnCancel = new Button();
            btnApply = new Button();
            btnClear = new Button();
            tbPhrases = new TextBox();
            toolTip1 = new ToolTip(components);
            groupBox2.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(groupBox1);
            groupBox2.Controls.Add(textBox1);
            groupBox2.Controls.Add(btnChkErr);
            groupBox2.Controls.Add(label1);
            groupBox2.Controls.Add(btnCancel);
            groupBox2.Controls.Add(btnApply);
            groupBox2.Controls.Add(btnClear);
            groupBox2.Controls.Add(tbPhrases);
            groupBox2.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point);
            groupBox2.Location = new Point(31, 12);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(695, 482);
            groupBox2.TabIndex = 0;
            groupBox2.TabStop = false;
            groupBox2.Text = "Default Phrases";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnUnChk);
            groupBox1.Controls.Add(bltnChkAll);
            groupBox1.ForeColor = Color.Coral;
            groupBox1.Location = new Point(501, 176);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(178, 96);
            groupBox1.TabIndex = 11;
            groupBox1.TabStop = false;
            groupBox1.Text = "Adds :0 or :1  if missing";
            // 
            // btnUnChk
            // 
            btnUnChk.ForeColor = Color.Coral;
            btnUnChk.Location = new Point(38, 62);
            btnUnChk.Name = "btnUnChk";
            btnUnChk.Size = new Size(89, 23);
            btnUnChk.TabIndex = 10;
            btnUnChk.Text = "Un-check All";
            toolTip1.SetToolTip(btnUnChk, "Ensure \"0:' in front of all phases");
            btnUnChk.UseVisualStyleBackColor = true;
            btnUnChk.Click += btnUnChk_Click;
            // 
            // bltnChkAll
            // 
            bltnChkAll.ForeColor = SystemColors.Highlight;
            bltnChkAll.Location = new Point(38, 22);
            bltnChkAll.Name = "bltnChkAll";
            bltnChkAll.Size = new Size(89, 24);
            bltnChkAll.TabIndex = 9;
            bltnChkAll.Text = "Check All";
            toolTip1.SetToolTip(bltnChkAll, "Ensure \"1:' in front of all phases ");
            bltnChkAll.UseVisualStyleBackColor = true;
            bltnChkAll.Click += bltnChkAll_Click;
            // 
            // textBox1
            // 
            textBox1.BackColor = SystemColors.Info;
            textBox1.Location = new Point(46, 31);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(354, 84);
            textBox1.TabIndex = 8;
            textBox1.Text = "Use the clipboard (copy / paste)  and notepad\r\n to save / restore phrases\r\n\r\nPREFIX PHRASES WITH 1: OR 0:  TO INDICATE CHECKED\r\nOR NOT TO BE CHECKED";
            // 
            // btnChkErr
            // 
            btnChkErr.Location = new Point(501, 295);
            btnChkErr.Name = "btnChkErr";
            btnChkErr.Size = new Size(102, 23);
            btnChkErr.TabIndex = 7;
            btnChkErr.Text = "Verify Syntax";
            toolTip1.SetToolTip(btnChkErr, "Must be 3 characters or more");
            btnChkErr.UseVisualStyleBackColor = true;
            btnChkErr.Click += btnChkErr_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = SystemColors.Info;
            label1.Location = new Point(501, 341);
            label1.Name = "label1";
            label1.Size = new Size(179, 90);
            label1.TabIndex = 5;
            label1.Text = "Be sure to click the \r\n\"Save Phrases\"when\r\nyou return to the main form\r\nDashs, slash, underscore and\r\nall puncuation are ignored.\r\nDo not use them in your phrases";
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(553, 130);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnApply
            // 
            btnApply.Location = new Point(553, 91);
            btnApply.Name = "btnApply";
            btnApply.Size = new Size(75, 24);
            btnApply.TabIndex = 2;
            btnApply.Text = "Apply";
            toolTip1.SetToolTip(btnApply, "Copy phrases into table\r\nand saves them in windows\r\ndefault settings");
            btnApply.UseVisualStyleBackColor = true;
            btnApply.Click += btnApply_Click;
            // 
            // btnClear
            // 
            btnClear.Location = new Point(553, 53);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(75, 23);
            btnClear.TabIndex = 1;
            btnClear.Text = "Clear";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            // 
            // tbPhrases
            // 
            tbPhrases.Location = new Point(6, 150);
            tbPhrases.Multiline = true;
            tbPhrases.Name = "tbPhrases";
            tbPhrases.ScrollBars = ScrollBars.Both;
            tbPhrases.Size = new Size(460, 281);
            tbPhrases.TabIndex = 0;
            // 
            // InitialParams
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(753, 519);
            Controls.Add(groupBox2);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "InitialParams";
            Text = "InitialParams";
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private GroupBox groupBox2;
        private TextBox tbPhrases;
        private Button btnApply;
        private ToolTip toolTip1;
        private Button btnClear;
        private Button btnCancel;
        private Label label1;
        private Button btnChkErr;
        private TextBox textBox1;
        private Button btnUnChk;
        private Button bltnChkAll;
        private GroupBox groupBox1;
    }
}