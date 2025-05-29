namespace VisualGraph
{
    partial class Home
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
            lbHeader = new Label();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            btnImportGraph = new Button();
            btnAlgorithmChoice = new Button();
            SuspendLayout();
            // 
            // lbHeader
            // 
            lbHeader.AutoSize = true;
            lbHeader.Font = new Font("Segoe UI", 18F);
            lbHeader.Location = new Point(319, 9);
            lbHeader.Name = "lbHeader";
            lbHeader.Size = new Size(292, 32);
            lbHeader.TabIndex = 0;
            lbHeader.Text = "VISUALIZZATORE DI GRAFI";
            // 
            // btnImportGraph
            // 
            btnImportGraph.Font = new Font("Segoe UI", 16F);
            btnImportGraph.Location = new Point(12, 6);
            btnImportGraph.Name = "btnImportGraph";
            btnImportGraph.Size = new Size(171, 41);
            btnImportGraph.TabIndex = 1;
            btnImportGraph.Text = "importa grafo";
            btnImportGraph.UseVisualStyleBackColor = true;
            btnImportGraph.Click += btnImportGraph_Click;
            // 
            // btnAlgorithmChoice
            // 
            btnAlgorithmChoice.Font = new Font("Segoe UI", 16F);
            btnAlgorithmChoice.Location = new Point(344, 448);
            btnAlgorithmChoice.Name = "btnAlgorithmChoice";
            btnAlgorithmChoice.Size = new Size(220, 48);
            btnAlgorithmChoice.TabIndex = 2;
            btnAlgorithmChoice.Text = "seleziona algoritmo";
            btnAlgorithmChoice.UseVisualStyleBackColor = true;
            btnAlgorithmChoice.Click += btnAlgorithmChoice_Click;
            // 
            // Home
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(951, 508);
            Controls.Add(btnAlgorithmChoice);
            Controls.Add(btnImportGraph);
            Controls.Add(lbHeader);
            Name = "Home";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lbHeader;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Button btnImportGraph;
        private Button btnAlgorithmChoice;
    }
}
