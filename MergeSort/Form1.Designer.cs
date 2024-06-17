namespace MergeSort
{
    partial class Form1
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
            this.skiaView = new SkiaSharp.Views.Desktop.SKGLControl();
            this.SuspendLayout();
            // 
            // skiaView
            // 
            this.skiaView.BackColor = System.Drawing.Color.Black;
            this.skiaView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.skiaView.Location = new System.Drawing.Point(0, 0);
            this.skiaView.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.skiaView.Name = "skiaView";
            this.skiaView.Size = new System.Drawing.Size(800, 450);
            this.skiaView.TabIndex = 0;
            this.skiaView.VSync = false;
            this.skiaView.PaintSurface += new System.EventHandler<SkiaSharp.Views.Desktop.SKPaintGLSurfaceEventArgs>(this.skiaView_PaintSurface);
            this.skiaView.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.skiaView_KeyPress);
            this.skiaView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.skiaView_MouseDown);
            this.skiaView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.skiaView_MouseMove);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.skiaView);
            this.Name = "Form1";
            this.Text = "Merge-Sort-Visualisierung";
            this.ResumeLayout(false);

        }

        #endregion

        private SkiaSharp.Views.Desktop.SKGLControl skiaView;
    }
}

