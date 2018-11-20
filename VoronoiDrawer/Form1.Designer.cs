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
			this.檔案ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.清除地圖ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.匯出圖檔ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.複製jsonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.儲存jsonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.label4 = new System.Windows.Forms.Label();
			this.button_pause_fortune = new System.Windows.Forms.Button();
			this.checkBox_circle_fortune = new System.Windows.Forms.CheckBox();
			this.button_perform_fortune = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.numericUpDown_timer = new System.Windows.Forms.NumericUpDown();
			this.numericUpDown_point_size = new System.Windows.Forms.NumericUpDown();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.numericUpDown_line_size = new System.Windows.Forms.NumericUpDown();
			this.label5 = new System.Windows.Forms.Label();
			this.button_run = new System.Windows.Forms.Button();
			this.progressBar_frotune = new System.Windows.Forms.ProgressBar();
			this.saveFileDialog2 = new System.Windows.Forms.SaveFileDialog();
			this.button_perform_Lloyd = new System.Windows.Forms.Button();
			this.label6 = new System.Windows.Forms.Label();
			this.checkBox_beachLine_fortune = new System.Windows.Forms.CheckBox();
			this.checkBox_voronoi_diagram_fortune = new System.Windows.Forms.CheckBox();
			this.checkBox_delaunay_triangulation_fortune = new System.Windows.Forms.CheckBox();
			this.button_get_result_fortune = new System.Windows.Forms.Button();
			this.button_auto_perform_fortune = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
			this.menuStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown_timer)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown_point_size)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown_line_size)).BeginInit();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// pictureBox
			// 
			this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pictureBox.Location = new System.Drawing.Point(11, 25);
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
            this.檔案ToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 1, 0, 1);
			this.menuStrip1.Size = new System.Drawing.Size(840, 24);
			this.menuStrip1.TabIndex = 1;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// 檔案ToolStripMenuItem
			// 
			this.檔案ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.清除地圖ToolStripMenuItem,
            this.匯出圖檔ToolStripMenuItem,
            this.toolStripSeparator1,
            this.複製jsonToolStripMenuItem,
            this.儲存jsonToolStripMenuItem});
			this.檔案ToolStripMenuItem.Name = "檔案ToolStripMenuItem";
			this.檔案ToolStripMenuItem.Size = new System.Drawing.Size(43, 22);
			this.檔案ToolStripMenuItem.Text = "檔案";
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.openToolStripMenuItem.Text = "開啟地圖";
			this.openToolStripMenuItem.Click += new System.EventHandler(this.開啟ToolStripMenuItem_Click);
			// 
			// 清除地圖ToolStripMenuItem
			// 
			this.清除地圖ToolStripMenuItem.Name = "清除地圖ToolStripMenuItem";
			this.清除地圖ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.清除地圖ToolStripMenuItem.Text = "清除地圖";
			this.清除地圖ToolStripMenuItem.Click += new System.EventHandler(this.清除地圖ToolStripMenuItem_Click);
			// 
			// 匯出圖檔ToolStripMenuItem
			// 
			this.匯出圖檔ToolStripMenuItem.Name = "匯出圖檔ToolStripMenuItem";
			this.匯出圖檔ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.匯出圖檔ToolStripMenuItem.Text = "匯出圖檔";
			this.匯出圖檔ToolStripMenuItem.Click += new System.EventHandler(this.匯出圖檔ToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
			// 
			// 複製jsonToolStripMenuItem
			// 
			this.複製jsonToolStripMenuItem.Name = "複製jsonToolStripMenuItem";
			this.複製jsonToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.複製jsonToolStripMenuItem.Text = "複製json";
			this.複製jsonToolStripMenuItem.Click += new System.EventHandler(this.複製jsonToolStripMenuItem_Click);
			// 
			// 儲存jsonToolStripMenuItem
			// 
			this.儲存jsonToolStripMenuItem.Name = "儲存jsonToolStripMenuItem";
			this.儲存jsonToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.儲存jsonToolStripMenuItem.Text = "儲存json";
			this.儲存jsonToolStripMenuItem.Click += new System.EventHandler(this.儲存jsonToolStripMenuItem_Click);
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
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label4.Location = new System.Drawing.Point(3, 148);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(163, 21);
			this.label4.TabIndex = 24;
			this.label4.Text = "Fortune\'s Algorithm";
			// 
			// button_pause_fortune
			// 
			this.button_pause_fortune.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.button_pause_fortune.Location = new System.Drawing.Point(215, 225);
			this.button_pause_fortune.Name = "button_pause_fortune";
			this.button_pause_fortune.Size = new System.Drawing.Size(100, 31);
			this.button_pause_fortune.TabIndex = 23;
			this.button_pause_fortune.Text = "Pause";
			this.button_pause_fortune.UseVisualStyleBackColor = true;
			this.button_pause_fortune.Click += new System.EventHandler(this.button_pause_fortune_Click);
			// 
			// checkBox_circle_fortune
			// 
			this.checkBox_circle_fortune.AutoSize = true;
			this.checkBox_circle_fortune.Checked = true;
			this.checkBox_circle_fortune.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox_circle_fortune.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.checkBox_circle_fortune.Location = new System.Drawing.Point(3, 172);
			this.checkBox_circle_fortune.Name = "checkBox_circle_fortune";
			this.checkBox_circle_fortune.Size = new System.Drawing.Size(63, 20);
			this.checkBox_circle_fortune.TabIndex = 22;
			this.checkBox_circle_fortune.Text = "Circles";
			this.checkBox_circle_fortune.UseVisualStyleBackColor = true;
			// 
			// button_perform_fortune
			// 
			this.button_perform_fortune.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.button_perform_fortune.Location = new System.Drawing.Point(3, 225);
			this.button_perform_fortune.Name = "button_perform_fortune";
			this.button_perform_fortune.Size = new System.Drawing.Size(100, 31);
			this.button_perform_fortune.TabIndex = 21;
			this.button_perform_fortune.Text = "Perform step";
			this.button_perform_fortune.UseVisualStyleBackColor = true;
			this.button_perform_fortune.Click += new System.EventHandler(this.button_perform_fortune_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label1.Location = new System.Drawing.Point(3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(83, 21);
			this.label1.TabIndex = 15;
			this.label1.Text = "point size";
			// 
			// numericUpDown_timer
			// 
			this.numericUpDown_timer.Location = new System.Drawing.Point(7, 122);
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
			this.numericUpDown_point_size.Location = new System.Drawing.Point(7, 24);
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
			this.label3.Location = new System.Drawing.Point(3, 98);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(111, 21);
			this.label3.TabIndex = 19;
			this.label3.Text = "timer interval";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label2.Location = new System.Drawing.Point(3, 49);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(70, 21);
			this.label2.TabIndex = 17;
			this.label2.Text = "line size";
			// 
			// numericUpDown_line_size
			// 
			this.numericUpDown_line_size.Location = new System.Drawing.Point(7, 73);
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
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label5.Location = new System.Drawing.Point(3, 344);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(117, 21);
			this.label5.TabIndex = 27;
			this.label5.Text = "Just get result";
			// 
			// button_run
			// 
			this.button_run.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.button_run.Location = new System.Drawing.Point(3, 368);
			this.button_run.Name = "button_run";
			this.button_run.Size = new System.Drawing.Size(120, 31);
			this.button_run.TabIndex = 28;
			this.button_run.Text = "Run";
			this.button_run.UseVisualStyleBackColor = true;
			// 
			// progressBar_frotune
			// 
			this.progressBar_frotune.Location = new System.Drawing.Point(129, 368);
			this.progressBar_frotune.Name = "progressBar_frotune";
			this.progressBar_frotune.Size = new System.Drawing.Size(292, 31);
			this.progressBar_frotune.Step = 1;
			this.progressBar_frotune.TabIndex = 29;
			// 
			// saveFileDialog2
			// 
			this.saveFileDialog2.Filter = "Voronoi Map (*.json) | *.json";
			// 
			// button_perform_Lloyd
			// 
			this.button_perform_Lloyd.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.button_perform_Lloyd.Location = new System.Drawing.Point(3, 293);
			this.button_perform_Lloyd.Name = "button_perform_Lloyd";
			this.button_perform_Lloyd.Size = new System.Drawing.Size(120, 31);
			this.button_perform_Lloyd.TabIndex = 30;
			this.button_perform_Lloyd.Text = "Get result";
			this.button_perform_Lloyd.UseVisualStyleBackColor = true;
			this.button_perform_Lloyd.Click += new System.EventHandler(this.button_perform_Lloyd_Click_1);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.label6.Location = new System.Drawing.Point(3, 269);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(144, 21);
			this.label6.TabIndex = 31;
			this.label6.Text = "Lloyd\'s Algorithm";
			// 
			// checkBox_beachLine_fortune
			// 
			this.checkBox_beachLine_fortune.AutoSize = true;
			this.checkBox_beachLine_fortune.Checked = true;
			this.checkBox_beachLine_fortune.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox_beachLine_fortune.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.checkBox_beachLine_fortune.Location = new System.Drawing.Point(167, 172);
			this.checkBox_beachLine_fortune.Name = "checkBox_beachLine_fortune";
			this.checkBox_beachLine_fortune.Size = new System.Drawing.Size(84, 20);
			this.checkBox_beachLine_fortune.TabIndex = 32;
			this.checkBox_beachLine_fortune.Text = "BeachLine";
			this.checkBox_beachLine_fortune.UseVisualStyleBackColor = true;
			// 
			// checkBox_voronoi_diagram_fortune
			// 
			this.checkBox_voronoi_diagram_fortune.AutoSize = true;
			this.checkBox_voronoi_diagram_fortune.Checked = true;
			this.checkBox_voronoi_diagram_fortune.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox_voronoi_diagram_fortune.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.checkBox_voronoi_diagram_fortune.Location = new System.Drawing.Point(167, 199);
			this.checkBox_voronoi_diagram_fortune.Name = "checkBox_voronoi_diagram_fortune";
			this.checkBox_voronoi_diagram_fortune.Size = new System.Drawing.Size(125, 20);
			this.checkBox_voronoi_diagram_fortune.TabIndex = 33;
			this.checkBox_voronoi_diagram_fortune.Text = "Voronoi Diagram";
			this.checkBox_voronoi_diagram_fortune.UseVisualStyleBackColor = true;
			// 
			// checkBox_delaunay_triangulation_fortune
			// 
			this.checkBox_delaunay_triangulation_fortune.AutoSize = true;
			this.checkBox_delaunay_triangulation_fortune.Checked = true;
			this.checkBox_delaunay_triangulation_fortune.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox_delaunay_triangulation_fortune.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.checkBox_delaunay_triangulation_fortune.Location = new System.Drawing.Point(3, 199);
			this.checkBox_delaunay_triangulation_fortune.Name = "checkBox_delaunay_triangulation_fortune";
			this.checkBox_delaunay_triangulation_fortune.Size = new System.Drawing.Size(158, 20);
			this.checkBox_delaunay_triangulation_fortune.TabIndex = 34;
			this.checkBox_delaunay_triangulation_fortune.Text = "Delaunay Triangulation";
			this.checkBox_delaunay_triangulation_fortune.UseVisualStyleBackColor = true;
			// 
			// button_get_result_fortune
			// 
			this.button_get_result_fortune.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.button_get_result_fortune.Location = new System.Drawing.Point(321, 225);
			this.button_get_result_fortune.Name = "button_get_result_fortune";
			this.button_get_result_fortune.Size = new System.Drawing.Size(100, 31);
			this.button_get_result_fortune.TabIndex = 35;
			this.button_get_result_fortune.Text = "Get result";
			this.button_get_result_fortune.UseVisualStyleBackColor = true;
			this.button_get_result_fortune.Click += new System.EventHandler(this.button_get_result_fortune_Click);
			// 
			// button_auto_perform_fortune
			// 
			this.button_auto_perform_fortune.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.button_auto_perform_fortune.Location = new System.Drawing.Point(109, 225);
			this.button_auto_perform_fortune.Name = "button_auto_perform_fortune";
			this.button_auto_perform_fortune.Size = new System.Drawing.Size(100, 31);
			this.button_auto_perform_fortune.TabIndex = 36;
			this.button_auto_perform_fortune.Text = "Auto perform";
			this.button_auto_perform_fortune.UseVisualStyleBackColor = true;
			this.button_auto_perform_fortune.Click += new System.EventHandler(this.button_auto_perform_fortune_Click);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.button_auto_perform_fortune);
			this.panel1.Controls.Add(this.numericUpDown_line_size);
			this.panel1.Controls.Add(this.button_get_result_fortune);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.checkBox_delaunay_triangulation_fortune);
			this.panel1.Controls.Add(this.label3);
			this.panel1.Controls.Add(this.checkBox_voronoi_diagram_fortune);
			this.panel1.Controls.Add(this.numericUpDown_point_size);
			this.panel1.Controls.Add(this.checkBox_beachLine_fortune);
			this.panel1.Controls.Add(this.numericUpDown_timer);
			this.panel1.Controls.Add(this.label6);
			this.panel1.Controls.Add(this.button_perform_fortune);
			this.panel1.Controls.Add(this.button_perform_Lloyd);
			this.panel1.Controls.Add(this.checkBox_circle_fortune);
			this.panel1.Controls.Add(this.progressBar_frotune);
			this.panel1.Controls.Add(this.button_pause_fortune);
			this.panel1.Controls.Add(this.button_run);
			this.panel1.Controls.Add(this.label4);
			this.panel1.Controls.Add(this.label5);
			this.panel1.Location = new System.Drawing.Point(416, 25);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(424, 411);
			this.panel1.TabIndex = 37;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(840, 437);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.pictureBox);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Margin = new System.Windows.Forms.Padding(2);
			this.Name = "Form1";
			this.Text = "Voronoi Drawer";
			this.Resize += new System.EventHandler(this.Form1_Resize);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown_timer)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown_point_size)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown_line_size)).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pictureBox;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem 檔案ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 清除地圖ToolStripMenuItem;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.ToolStripMenuItem 匯出圖檔ToolStripMenuItem;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button button_pause_fortune;
		private System.Windows.Forms.CheckBox checkBox_circle_fortune;
		private System.Windows.Forms.Button button_perform_fortune;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown numericUpDown_timer;
		private System.Windows.Forms.NumericUpDown numericUpDown_point_size;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.NumericUpDown numericUpDown_line_size;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button button_run;
		private System.Windows.Forms.ProgressBar progressBar_frotune;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem 複製jsonToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 儲存jsonToolStripMenuItem;
		private System.Windows.Forms.SaveFileDialog saveFileDialog2;
		private System.Windows.Forms.Button button_perform_Lloyd;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.CheckBox checkBox_beachLine_fortune;
		private System.Windows.Forms.CheckBox checkBox_voronoi_diagram_fortune;
		private System.Windows.Forms.CheckBox checkBox_delaunay_triangulation_fortune;
		private System.Windows.Forms.Button button_get_result_fortune;
		private System.Windows.Forms.Button button_auto_perform_fortune;
		private System.Windows.Forms.Panel panel1;
	}
}

