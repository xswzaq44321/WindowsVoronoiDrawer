namespace VoronoiDrawer
{
	partial class Form1
	{
		/// <summary>
		/// 設計工具所需的變數。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 清除任何使用中的資源。
		/// </summary>
		/// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form 設計工具產生的程式碼

		/// <summary>
		/// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
		/// 這個方法的內容。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.pictureBox = new System.Windows.Forms.PictureBox();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.clearMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.outputImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.label4 = new System.Windows.Forms.Label();
			this.button_stop_perform_voronoi = new System.Windows.Forms.Button();
			this.checkBox_visualize_voronoi = new System.Windows.Forms.CheckBox();
			this.button_perform_voronoi = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.numericUpDown_timer = new System.Windows.Forms.NumericUpDown();
			this.numericUpDown_point_size = new System.Windows.Forms.NumericUpDown();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.numericUpDown_line_size = new System.Windows.Forms.NumericUpDown();
			this.button_pause_voronoi = new System.Windows.Forms.Button();
			this.button_continue_voronoi = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
			this.menuStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown_timer)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown_point_size)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown_line_size)).BeginInit();
			this.SuspendLayout();
			// 
			// pictureBox
			// 
			this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pictureBox.Location = new System.Drawing.Point(11, 26);
			this.pictureBox.Margin = new System.Windows.Forms.Padding(2);
			this.pictureBox.Name = "pictureBox";
			this.pictureBox.Size = new System.Drawing.Size(400, 400);
			this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox.TabIndex = 0;
			this.pictureBox.TabStop = false;
			this.pictureBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.pictureBox_DragDrop);
			this.pictureBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.pictureBox_DragEnter);
			// 
			// menuStrip1
			// 
			this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 1, 0, 1);
			this.menuStrip1.Size = new System.Drawing.Size(671, 24);
			this.menuStrip1.TabIndex = 1;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.clearMapToolStripMenuItem,
            this.outputImageToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(61, 22);
			this.fileToolStripMenuItem.Text = "Control";
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
			this.openToolStripMenuItem.Text = "Open new map";
			this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
			// 
			// clearMapToolStripMenuItem
			// 
			this.clearMapToolStripMenuItem.Name = "clearMapToolStripMenuItem";
			this.clearMapToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
			this.clearMapToolStripMenuItem.Text = "Clear map";
			this.clearMapToolStripMenuItem.Click += new System.EventHandler(this.clearMapToolStripMenuItem_Click);
			// 
			// outputImageToolStripMenuItem
			// 
			this.outputImageToolStripMenuItem.Name = "outputImageToolStripMenuItem";
			this.outputImageToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
			this.outputImageToolStripMenuItem.Text = "Save Image";
			this.outputImageToolStripMenuItem.Click += new System.EventHandler(this.outputImageToolStripMenuItem_Click);
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "openFileDialog1";
			this.openFileDialog1.Filter = "Vornoi Map (*.json) | *.json";
			// 
			// saveFileDialog1
			// 
			this.saveFileDialog1.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif|Png Image|*.png";
			// 
			// timer1
			// 
			this.timer1.Tick += new System.EventHandler(this.performFortuneStep);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label4.Location = new System.Drawing.Point(416, 174);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(137, 21);
			this.label4.TabIndex = 24;
			this.label4.Text = "Perform Voronoi";
			// 
			// button_stop_perform_voronoi
			// 
			this.button_stop_perform_voronoi.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.button_stop_perform_voronoi.Location = new System.Drawing.Point(542, 224);
			this.button_stop_perform_voronoi.Name = "button_stop_perform_voronoi";
			this.button_stop_perform_voronoi.Size = new System.Drawing.Size(120, 31);
			this.button_stop_perform_voronoi.TabIndex = 23;
			this.button_stop_perform_voronoi.Text = "stop step";
			this.button_stop_perform_voronoi.UseVisualStyleBackColor = true;
			this.button_stop_perform_voronoi.Click += new System.EventHandler(this.button_stop_perform_voronoi_Click);
			// 
			// checkBox_visualize_voronoi
			// 
			this.checkBox_visualize_voronoi.AutoSize = true;
			this.checkBox_visualize_voronoi.Location = new System.Drawing.Point(416, 198);
			this.checkBox_visualize_voronoi.Name = "checkBox_visualize_voronoi";
			this.checkBox_visualize_voronoi.Size = new System.Drawing.Size(64, 16);
			this.checkBox_visualize_voronoi.TabIndex = 22;
			this.checkBox_visualize_voronoi.Text = "visualize";
			this.checkBox_visualize_voronoi.UseVisualStyleBackColor = true;
			// 
			// button_perform_voronoi
			// 
			this.button_perform_voronoi.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.button_perform_voronoi.Location = new System.Drawing.Point(416, 224);
			this.button_perform_voronoi.Name = "button_perform_voronoi";
			this.button_perform_voronoi.Size = new System.Drawing.Size(120, 31);
			this.button_perform_voronoi.TabIndex = 21;
			this.button_perform_voronoi.Text = "perform step";
			this.button_perform_voronoi.UseVisualStyleBackColor = true;
			this.button_perform_voronoi.Click += new System.EventHandler(this.button_perform_voronoi_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label1.Location = new System.Drawing.Point(416, 26);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(83, 21);
			this.label1.TabIndex = 15;
			this.label1.Text = "point size";
			// 
			// numericUpDown_timer
			// 
			this.numericUpDown_timer.Location = new System.Drawing.Point(420, 148);
			this.numericUpDown_timer.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.numericUpDown_timer.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numericUpDown_timer.Name = "numericUpDown_timer";
			this.numericUpDown_timer.Size = new System.Drawing.Size(120, 22);
			this.numericUpDown_timer.TabIndex = 20;
			this.numericUpDown_timer.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.numericUpDown_timer.ValueChanged += new System.EventHandler(this.numericUpDown_timer_ValueChanged);
			// 
			// numericUpDown_point_size
			// 
			this.numericUpDown_point_size.Location = new System.Drawing.Point(420, 50);
			this.numericUpDown_point_size.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
			this.numericUpDown_point_size.Name = "numericUpDown_point_size";
			this.numericUpDown_point_size.Size = new System.Drawing.Size(120, 22);
			this.numericUpDown_point_size.TabIndex = 16;
			this.numericUpDown_point_size.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
			this.numericUpDown_point_size.ValueChanged += new System.EventHandler(this.numericUpDown_point_size_ValueChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label3.Location = new System.Drawing.Point(416, 124);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(111, 21);
			this.label3.TabIndex = 19;
			this.label3.Text = "timer interval";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label2.Location = new System.Drawing.Point(416, 75);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(70, 21);
			this.label2.TabIndex = 17;
			this.label2.Text = "line size";
			// 
			// numericUpDown_line_size
			// 
			this.numericUpDown_line_size.Location = new System.Drawing.Point(420, 99);
			this.numericUpDown_line_size.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numericUpDown_line_size.Name = "numericUpDown_line_size";
			this.numericUpDown_line_size.Size = new System.Drawing.Size(120, 22);
			this.numericUpDown_line_size.TabIndex = 18;
			this.numericUpDown_line_size.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
			this.numericUpDown_line_size.ValueChanged += new System.EventHandler(this.numericUpDown_line_size_ValueChanged);
			// 
			// button_pause_voronoi
			// 
			this.button_pause_voronoi.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.button_pause_voronoi.Location = new System.Drawing.Point(542, 261);
			this.button_pause_voronoi.Name = "button_pause_voronoi";
			this.button_pause_voronoi.Size = new System.Drawing.Size(120, 31);
			this.button_pause_voronoi.TabIndex = 25;
			this.button_pause_voronoi.Text = "pause step";
			this.button_pause_voronoi.UseVisualStyleBackColor = true;
			this.button_pause_voronoi.Click += new System.EventHandler(this.button_pause_voronoi_Click);
			// 
			// button_continue_voronoi
			// 
			this.button_continue_voronoi.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.button_continue_voronoi.Location = new System.Drawing.Point(416, 261);
			this.button_continue_voronoi.Name = "button_continue_voronoi";
			this.button_continue_voronoi.Size = new System.Drawing.Size(120, 31);
			this.button_continue_voronoi.TabIndex = 26;
			this.button_continue_voronoi.Text = "continue step";
			this.button_continue_voronoi.UseVisualStyleBackColor = true;
			this.button_continue_voronoi.Click += new System.EventHandler(this.button_continue_voronoi_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(671, 437);
			this.Controls.Add(this.button_continue_voronoi);
			this.Controls.Add(this.button_pause_voronoi);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.button_stop_perform_voronoi);
			this.Controls.Add(this.checkBox_visualize_voronoi);
			this.Controls.Add(this.button_perform_voronoi);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.numericUpDown_timer);
			this.Controls.Add(this.numericUpDown_point_size);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.numericUpDown_line_size);
			this.Controls.Add(this.pictureBox);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Margin = new System.Windows.Forms.Padding(2);
			this.Name = "Form1";
			this.Text = "Form1";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown_timer)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown_point_size)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown_line_size)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pictureBox;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem clearMapToolStripMenuItem;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.ToolStripMenuItem outputImageToolStripMenuItem;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button button_stop_perform_voronoi;
		private System.Windows.Forms.CheckBox checkBox_visualize_voronoi;
		private System.Windows.Forms.Button button_perform_voronoi;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown numericUpDown_timer;
		private System.Windows.Forms.NumericUpDown numericUpDown_point_size;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.NumericUpDown numericUpDown_line_size;
		private System.Windows.Forms.Button button_pause_voronoi;
		private System.Windows.Forms.Button button_continue_voronoi;
	}
}

