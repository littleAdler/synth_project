namespace Synth_1
{
    partial class Synth1
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
            this.Button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Button1
            // 
            this.Button1.Location = new System.Drawing.Point(357, 206);
            this.Button1.Name = "Button1";
            this.Button1.Size = new System.Drawing.Size(75, 23);
            this.Button1.TabIndex = 0;
            this.Button1.Text = "Button1";
            this.Button1.UseMnemonic = false;
            this.Button1.UseVisualStyleBackColor = true;
            this.Button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // Synth1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.Button1);
            this.KeyPreview = true;
            this.Name = "Synth1";
            this.Text = "Synth1";
            this.Load += new System.EventHandler(this.Synth1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Synth1_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Button1;
    }
}

