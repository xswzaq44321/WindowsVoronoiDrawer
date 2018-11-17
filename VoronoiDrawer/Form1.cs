using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;

namespace VoronoiDrawer
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
			blueBrush = new SolidBrush(Color.Blue);
			orangeBrush = new SolidBrush(Color.Orange);
			int penWidth = (int)numericUpDown_line_size.Value;
			int circleR = (int)numericUpDown_point_size.Value;
			blackPen = new Pen(Color.Black, penWidth);
			redPen = new Pen(Color.Red, penWidth);
			greenPen = new Pen(Color.Green, penWidth);
			circleSize = new Size(circleR, circleR);
			pictureBox.AllowDrop = true;
			timer1.Interval = (int)numericUpDown_timer.Value;
		}

		Bitmap bmp;
		Graphics g;
		Pen blackPen, redPen, greenPen;
		Brush blueBrush, orangeBrush;
		Size circleSize;
		VoronoiStruct.Voronoi vmap;
		VoronoiStruct.Line sweepLine = new VoronoiStruct.Line(0, 0, 0, 0);

		void readMap(string filePath, out VoronoiStruct.Voronoi map)
		{
			StreamReader sr = new StreamReader(filePath);
			string json = sr.ReadToEnd();
			map = JsonConvert.DeserializeObject<VoronoiStruct.Voronoi>(json);
		}

		void drawPoint(Brush brush, VoronoiStruct.Point pos)
		{
			// this func is just a short hand for writting FillEllipse
			Point pos1 = new Point(pos.x - circleSize.Width / 2, pos.y - circleSize.Height / 2);
			g.FillEllipse(brush, new RectangleF(pos1, circleSize));
		}
		void drawPoint(Brush brush, Point pos)
		{
			Point pos1 = new Point(pos.X - circleSize.Width / 2, pos.Y - circleSize.Height / 2);
			g.FillEllipse(brush, new RectangleF(pos1, circleSize));
		}

		void drawLine(Pen pen, VoronoiStruct.Point pos1, VoronoiStruct.Point pos2)
		{
			g.DrawLine(pen, pos1.x, pos1.y, pos2.x, pos2.y);
		}
		void drawLine(Pen pen, VoronoiStruct.Line line)
		{
			drawLine(pen, line.a, line.b);
		}
		void drawLine(Pen pen, Point pos1, Point pos2)
		{
			g.DrawLine(pen, pos1, pos2);
		}

		void drawVoronoi(VoronoiStruct.Voronoi map)
		{
			if (map == null)
				return;
			bmp = new Bitmap(map.width, map.height);
			g = Graphics.FromImage(bmp);
			g.Clear(Color.White);
			pictureBox.Image = bmp;
			foreach (var item in map.polygons)
			{
				drawPoint(blueBrush, item.focus);
			}
			foreach (var item in map.polygons)
			{
				foreach (var item2 in item.edges)
				{
					drawLine(blackPen, item2.line);
				}
			}
			pictureBox.Invalidate();
		}

		void performFortuneStep(object sender, EventArgs e)
		{
			// end perform
			if (sweepLine.a.x > vmap.width * 2)
			{
				timer1.Stop();
				drawVoronoi(vmap);
				return;
			}
			++sweepLine.a.x;
			++sweepLine.b.x;
			Bitmap bar = new Bitmap(bmp);
			pictureBox.Image = bar;
			g = Graphics.FromImage(bar);
			drawLine(redPen, sweepLine);
			List<VoronoiStruct.Parabola> parabolas = new List<VoronoiStruct.Parabola>();
			for (int i = 0; i < vmap.polygons.Count; i++)
			{
				parabolas.Add(getParabola(vmap.polygons[i].focus, i));
			}
			if (checkBox_visualize_voronoi.Checked)
			{
				foreach (var item in parabolas)
				{
					foreach (var item2 in parabolas)
					{
						item.dealIntersect(item2);
					}
					if (item.points.Count > 1)
					{
						g.DrawCurve(greenPen, item.points.ToArray());
					}
				}
			}
			pictureBox.Invalidate();
		}

		void performFortune()
		{
			if (vmap == null)
			{
				return;
			}
			sweepLine.a = new VoronoiStruct.Point(0, 0);
			sweepLine.b = new VoronoiStruct.Point(0, vmap.height);
			timer1.Start();
		}

		VoronoiStruct.Parabola getParabola(VoronoiStruct.Point focus, int id)
		{
			return new VoronoiStruct.Parabola(focus, sweepLine.a.x, new Rectangle(0, 0, vmap.width, vmap.height), id);
		}

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				readMap(openFileDialog1.FileName, out vmap);
				drawVoronoi(vmap);
			}
		}

		private void clearMapToolStripMenuItem_Click(object sender, EventArgs e)
		{
			g.Clear(Color.White);
			pictureBox.Invalidate();
		}

		private void pictureBox_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				e.Effect = DragDropEffects.Copy;
			}
		}

		private void pictureBox_DragDrop(object sender, DragEventArgs e)
		{
			string[] path = (string[])e.Data.GetData(DataFormats.FileDrop, false);
			if (Path.GetExtension(path[0]) == ".json")
			{
				readMap(path[0], out vmap);
				drawVoronoi(vmap);
			}
		}

		private void outputImageToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (saveFileDialog1.ShowDialog() == DialogResult.OK)
			{
				FileStream fs = (FileStream)saveFileDialog1.OpenFile();
				switch (saveFileDialog1.FilterIndex)
				{
					case 1:
						pictureBox.Image.Save(fs,
							System.Drawing.Imaging.ImageFormat.Jpeg);
						break;
					case 2:
						pictureBox.Image.Save(fs,
							System.Drawing.Imaging.ImageFormat.Bmp);
						break;
					case 3:
						pictureBox.Image.Save(fs,
							System.Drawing.Imaging.ImageFormat.Gif);
						break;
					case 4:
						pictureBox.Image.Save(fs,
							System.Drawing.Imaging.ImageFormat.Png);
						break;
				}
			}
		}

		private void numericUpDown_point_size_ValueChanged(object sender, EventArgs e)
		{
			circleSize = new Size((int)numericUpDown_point_size.Value, (int)numericUpDown_point_size.Value);
			drawVoronoi(vmap);
		}

		private void numericUpDown_timer_ValueChanged(object sender, EventArgs e)
		{
			timer1.Interval = (int)numericUpDown_timer.Value;
		}

		private void button_pause_voronoi_Click(object sender, EventArgs e)
		{
			timer1.Stop();
		}

		private void button_continue_voronoi_Click(object sender, EventArgs e)
		{
			timer1.Start();
		}

		private void button_stop_perform_voronoi_Click(object sender, EventArgs e)
		{
			timer1.Stop();
			drawVoronoi(vmap);
		}

		private void button_perform_voronoi_Click(object sender, EventArgs e)
		{
			performFortune();
		}

		private void numericUpDown_line_size_ValueChanged(object sender, EventArgs e)
		{
			int size = (int)numericUpDown_line_size.Value;
			blackPen.Width = size;
			greenPen.Width = size;
			redPen.Width = size;
			drawVoronoi(vmap);
		}
	}
}
