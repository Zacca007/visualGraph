namespace VisualGraph
{
    partial class SelezioneAlgoritmo
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
            cbAlgorithm = new ComboBox();
            lbAlgorithm = new Label();
            cbEndNode = new ComboBox();
            lbStartNode = new Label();
            lbEndNode = new Label();
            graphBindingSource = new BindingSource(components);
            cbStartNode = new ComboBox();
            cbExecution = new Button();
            ((System.ComponentModel.ISupportInitialize)graphBindingSource).BeginInit();
            SuspendLayout();
            // 
            // cbAlgorithm
            // 
            cbAlgorithm.Font = new Font("Segoe UI", 16F);
            cbAlgorithm.FormattingEnabled = true;
            cbAlgorithm.Items.AddRange(new object[] { "dijkstra" });
            cbAlgorithm.Location = new Point(365, 14);
            cbAlgorithm.Name = "cbAlgorithm";
            cbAlgorithm.Size = new Size(165, 38);
            cbAlgorithm.TabIndex = 0;
            cbAlgorithm.SelectedIndexChanged += cbAlgorithm_SelectedIndexChanged;
            // 
            // lbAlgorithm
            // 
            lbAlgorithm.AutoSize = true;
            lbAlgorithm.Font = new Font("Segoe UI", 12F);
            lbAlgorithm.Location = new Point(191, 24);
            lbAlgorithm.Name = "lbAlgorithm";
            lbAlgorithm.Size = new Size(168, 21);
            lbAlgorithm.TabIndex = 1;
            lbAlgorithm.Text = "seleziona un algoritmo";
            // 
            // cbEndNode
            // 
            cbEndNode.FormattingEnabled = true;
            cbEndNode.Location = new Point(409, 145);
            cbEndNode.Name = "cbEndNode";
            cbEndNode.Size = new Size(121, 23);
            cbEndNode.TabIndex = 3;
            cbEndNode.SelectedIndexChanged += cbEndNode_SelectedIndexChanged;
            // 
            // lbStartNode
            // 
            lbStartNode.AutoSize = true;
            lbStartNode.Font = new Font("Segoe UI", 12F);
            lbStartNode.Location = new Point(191, 111);
            lbStartNode.Name = "lbStartNode";
            lbStartNode.Size = new Size(127, 21);
            lbStartNode.TabIndex = 4;
            lbStartNode.Text = "nodo di partenza";
            // 
            // lbEndNode
            // 
            lbEndNode.AutoSize = true;
            lbEndNode.Font = new Font("Segoe UI", 12F);
            lbEndNode.Location = new Point(422, 111);
            lbEndNode.Name = "lbEndNode";
            lbEndNode.Size = new Size(108, 21);
            lbEndNode.TabIndex = 5;
            lbEndNode.Text = "nodo di arrivo";
            // 
            // graphBindingSource
            // 
            graphBindingSource.DataSource = typeof(CsGraph.Graph);
            // 
            // cbStartNode
            // 
            cbStartNode.FormattingEnabled = true;
            cbStartNode.Location = new Point(191, 145);
            cbStartNode.Name = "cbStartNode";
            cbStartNode.Size = new Size(127, 23);
            cbStartNode.TabIndex = 6;
            cbStartNode.SelectedIndexChanged += cbStartNode_SelectedIndexChanged;
            // 
            // cbExecution
            // 
            cbExecution.Font = new Font("Segoe UI", 16F);
            cbExecution.Location = new Point(267, 371);
            cbExecution.Name = "cbExecution";
            cbExecution.Size = new Size(196, 38);
            cbExecution.TabIndex = 7;
            cbExecution.Text = "esegui algoritmo";
            cbExecution.UseVisualStyleBackColor = true;
            cbExecution.Click += cbExecution_Click;
            // 
            // SelezioneAlgoritmo
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(750, 450);
            Controls.Add(cbExecution);
            Controls.Add(cbStartNode);
            Controls.Add(lbEndNode);
            Controls.Add(lbStartNode);
            Controls.Add(cbEndNode);
            Controls.Add(lbAlgorithm);
            Controls.Add(cbAlgorithm);
            Name = "SelezioneAlgoritmo";
            Text = "SelezioneAlgoritmo";
            ((System.ComponentModel.ISupportInitialize)graphBindingSource).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox cbAlgorithm;
        private Label lbAlgorithm;
        private ComboBox cbEndNode;
        private Label lbStartNode;
        private Label lbEndNode;
        private BindingSource graphBindingSource;
        private ComboBox cbStartNode;
        private Button cbExecution;
    }
}