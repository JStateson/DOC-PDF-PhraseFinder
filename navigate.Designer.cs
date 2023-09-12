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
            label1 = new Label();
            btnExpand = new Button();
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
            btnNext.Location = new Point(12, 12);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(136, 23);
            btnNext.TabIndex = 1;
            btnNext.Text = "Next on page";
            btnNext.UseVisualStyleBackColor = true;
            btnNext.Visible = false;
            btnNext.Click += btnNext_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(170, 12);
            label1.Name = "label1";
            label1.Size = new Size(135, 30);
            label1.TabIndex = 2;
            label1.Text = "Red indicates more than\r\none on same page";
            // 
            // btnExpand
            // 
            btnExpand.Location = new Point(328, 12);
            btnExpand.Name = "btnExpand";
            btnExpand.Size = new Size(97, 23);
            btnExpand.TabIndex = 3;
            btnExpand.Text = "Expand all";
            btnExpand.UseVisualStyleBackColor = true;
            btnExpand.Click += btnExpand_Click;
            // 
            // navigate
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(437, 450);
            Controls.Add(btnExpand);
            Controls.Add(label1);
            Controls.Add(btnNext);
            Controls.Add(tvPhrases);
            Name = "navigate";
            Text = "navigate";
            Deactivate += navigate_Deactivate;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TreeView tvPhrases;
        private Button btnNext;
        private Label label1;
        private Button btnExpand;
    }
}