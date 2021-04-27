
namespace EWKT.Visualizer
{
    partial class MainForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtEWKT = new System.Windows.Forms.TextBox();
            this.btnEWKT = new System.Windows.Forms.Button();
            this.chkShowLabels = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.txtEWKT, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnEWKT, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.chkShowLabels, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1089, 545);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // txtEWKT
            // 
            this.txtEWKT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEWKT.Location = new System.Drawing.Point(4, 5);
            this.txtEWKT.Margin = new System.Windows.Forms.Padding(4);
            this.txtEWKT.Multiline = true;
            this.txtEWKT.Name = "txtEWKT";
            this.tableLayoutPanel1.SetRowSpan(this.txtEWKT, 2);
            this.txtEWKT.Size = new System.Drawing.Size(985, 110);
            this.txtEWKT.TabIndex = 0;
            this.txtEWKT.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtEWKT_KeyDown);
            // 
            // btnEWKT
            // 
            this.btnEWKT.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEWKT.Location = new System.Drawing.Point(997, 4);
            this.btnEWKT.Margin = new System.Windows.Forms.Padding(4);
            this.btnEWKT.Name = "btnEWKT";
            this.btnEWKT.Size = new System.Drawing.Size(88, 89);
            this.btnEWKT.TabIndex = 1;
            this.btnEWKT.Text = "Update";
            this.btnEWKT.UseVisualStyleBackColor = true;
            this.btnEWKT.Click += new System.EventHandler(this.btnEWKT_Click);
            // 
            // chkShowLabels
            // 
            this.chkShowLabels.AutoSize = true;
            this.chkShowLabels.Checked = true;
            this.chkShowLabels.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowLabels.Location = new System.Drawing.Point(996, 100);
            this.chkShowLabels.Name = "chkShowLabels";
            this.chkShowLabels.Size = new System.Drawing.Size(59, 17);
            this.chkShowLabels.TabIndex = 2;
            this.chkShowLabels.Text = "Labels";
            this.chkShowLabels.UseVisualStyleBackColor = true;
            this.chkShowLabels.CheckedChanged += new System.EventHandler(this.chkShowLabels_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1089, 545);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EWKT Visualizer";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox txtEWKT;
        private System.Windows.Forms.Button btnEWKT;
        private System.Windows.Forms.CheckBox chkShowLabels;
    }
}

