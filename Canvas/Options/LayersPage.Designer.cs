namespace Canvas.Options
{
	partial class LayersPage
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
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(44, 60);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(336, 148);
			this.label1.TabIndex = 0;
			this.label1.Text = "I am still working on this.For now adding and removing layers must be done manual" +
				"ly by modifying the cadxml (xml) file directly.";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// LayersPage
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(424, 268);
			this.Controls.Add(this.label1);
			this.Name = "LayersPage";
			this.Text = "LayersPage";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label label1;
	}
}