namespace DOC_PhraseFinder
{
    partial class navigate
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
            tvPhrases = new TreeView();
            btnNext = new Button();
            SuspendLayout();
            // 
            // tvPhrases
            // 
            tvPhrases.Location = new Point(12, 54);
            tvPhrases.Name = "tvPhrases";
            tvPhrases.Size = new Size(413, 384);
            tvPhrases.TabIndex = 0;
            tvPhrases.NodeMouseDoubleClick += tvPhrases_NodeMouseDoubleClick;
            // 
            // btnNext
            // 
            btnNext.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnNext.ForeColor = Color.Green;
            btnNext.Location = new Point(98, 12);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(136, 23);
            btnNext.TabIndex = 1;
            btnNext.Text = "Next on page";
            btnNext.UseVisualStyleBackColor = true;
            btnNext.Visible = false;
            btnNext.Click += btnNext_Click;
            // 
            // navigate
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(437, 450);
            Controls.Add(btnNext);
            Controls.Add(tvPhrases);
            Name = "navigate";
            Text = "navigate";
            ResumeLayout(false);
        }

        #endregion

        private TreeView tvPhrases;
        private Button btnNext;
    }
}