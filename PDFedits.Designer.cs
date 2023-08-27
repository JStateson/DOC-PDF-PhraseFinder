namespace PDF_PhraseFinder
{
    partial class PDFedits
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
            dgv_edits = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgv_edits).BeginInit();
            SuspendLayout();
            // 
            // dgv_edits
            // 
            dgv_edits.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv_edits.Location = new Point(12, 12);
            dgv_edits.Name = "dgv_edits";
            dgv_edits.ReadOnly = true;
            dgv_edits.RowTemplate.Height = 25;
            dgv_edits.Size = new Size(351, 264);
            dgv_edits.TabIndex = 0;
            // 
            // PDFedits
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(375, 300);
            Controls.Add(dgv_edits);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "PDFedits";
            Text = "PDFedits";
            ((System.ComponentModel.ISupportInitialize)dgv_edits).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dgv_edits;
    }
}