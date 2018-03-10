namespace KaraokePlayer
{
    partial class QueueForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QueueForm));
            this.materialListBox1 = new MaterialSkin.Controls.MaterialListBox();
            this.SuspendLayout();
            // 
            // materialListBox1
            // 
            this.materialListBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.materialListBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.materialListBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.materialListBox1.Depth = 0;
            this.materialListBox1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.materialListBox1.Font = new System.Drawing.Font("Roboto", 24F);
            this.materialListBox1.FormattingEnabled = true;
            this.materialListBox1.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.materialListBox1.Location = new System.Drawing.Point(12, 77);
            this.materialListBox1.MouseLocation = new System.Drawing.Point(-1, -1);
            this.materialListBox1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialListBox1.Name = "materialListBox1";
            this.materialListBox1.Size = new System.Drawing.Size(261, 311);
            this.materialListBox1.TabIndex = 6;
            this.materialListBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.materialListBox1_KeyDown);
            this.materialListBox1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.materialListBox1_MouseDoubleClick);
            // 
            // QueueForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(285, 399);
            this.ControlBox = false;
            this.Controls.Add(this.materialListBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "QueueForm";
            this.ShowInTaskbar = false;
            this.Text = "Queue";
            this.ResumeLayout(false);

        }

        #endregion

        private MaterialSkin.Controls.MaterialListBox materialListBox1;
    }
}