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
			purplePen = new Pen(Color.Purple, penWidth);
			circleSize = new Size(circleR, circleR);
			pictureBox.AllowDrop = true;
			timer1.Interval = (int)numericUpDown_timer.Value;
			this.Form1_Resize(null, null);
		}

		Bitmap bmp;
		Graphics g;
		Pen blackPen, redPen, greenPen, purplePen;
		Brush blueBrush, orangeBrush;
		Size circleSize;
		VoronoiStruct.Voronoi vmap;
		VoronoiStruct.SweepLine sweepLine;

		void readMap(string filePath, out VoronoiStruct.Voronoi vmap)
		{
			StreamReader sr = new StreamReader(filePath);
			string json = sr.ReadToEnd();
			initMap(json, out vmap);
			sr.Close();
		}

		void initMap(string json, out VoronoiStruct.Voronoi vmap)
		{
			var map = JsonConvert.DeserializeObject<VoronoiStruct.Voronoi>(json);
			foreach (var poly in map.polygons)
			{
				foreach (var edge in poly.edges)
				{
					edge.deAbstract();
				}
			}
			vmap = map;
			sweepLine = null;
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
			foreach (var poly in map.polygons)
			{
				foreach (var edge in poly.edges)
				{
					if (!edge.isAbstract())
					{
						drawLine(blackPen, edge.line);
					}
				}
			}
			pictureBox.Invalidate();
		}

		void performLloyd()
		{
			VoronoiStruct.Voronoi newMap = new VoronoiStruct.Voronoi(vmap.width, vmap.height);
			Rectangle regin = new Rectangle(0, 0, vmap.width, vmap.height);
			foreach (var poly in vmap.polygons)
			{
				List<VoronoiStruct.Point> points = new List<VoronoiStruct.Point>();
				foreach (var edge in poly.edges)
				{
					VoronoiStruct.Point a = edge.line.a;
					VoronoiStruct.Point b = edge.line.b;
					if (!points.Contains(a))
						points.Add(a);
					if (!points.Contains(b))
						points.Add(b);
				}
				int cx = points.Sum((point) => { return point.x; }) / points.Count;
				int cy = points.Sum((point) => { return point.y; }) / points.Count;
				newMap.polygons.Add(new VoronoiStruct.Polygon(new VoronoiStruct.Point(cx, cy)));
			}
			vmap = newMap;
			drawVoronoi(vmap);
		}

		bool onEdge(VoronoiStruct.Edge o)
		{
			if (o.line.a.x == 0 || o.line.a.y == 0 || o.line.b.x == 0 || o.line.b.y == 0 ||
				o.line.a.x == vmap.width - 1 || o.line.a.y == vmap.width - 1 || o.line.b.x == vmap.width - 1 || o.line.b.y == vmap.width - 1)
			{
				return true;
			}
			return false;
		}

		void drawCurrentStep()
		{
			if (sweepLine.L == double.MaxValue)
				return;
			Bitmap bar = new Bitmap(bmp);
			pictureBox.Image = bar;
			g = Graphics.FromImage(bar);
			if (checkBox_beachLine_fortune.Checked)
			{
				var parabolas = sweepLine.getBeachLinePoints();
				foreach (var points in parabolas)
				{
					if (points.Count <= 1)
						continue;
					g.DrawCurve(greenPen, points.ToArray());
				}
			}
			if (checkBox_circle_fortune.Checked)
			{
				var circles = sweepLine.getCirclePoints();
				foreach (var rect in circles)
				{
					g.DrawEllipse(purplePen, rect);
				}
			}
			g.DrawLine(redPen, new Point((int)sweepLine.L, 0), new Point((int)sweepLine.L, vmap.height));
			pictureBox.Invalidate();
		}

		private void 清除地圖ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (g != null)
				g.Clear(Color.White);
			pictureBox.Invalidate();
		}

		private void 複製jsonToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string json = JsonConvert.SerializeObject(vmap, Formatting.Indented);
			Clipboard.SetText(json);
		}

		private void 儲存jsonToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (saveFileDialog2.ShowDialog() == DialogResult.OK)
			{
				string json = JsonConvert.SerializeObject(vmap, Formatting.Indented);
				System.IO.File.WriteAllText(saveFileDialog2.FileName, json);
			}
		}

		private void 貼上jsonToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string json = Clipboard.GetText();
			initMap(json, out vmap);
			drawVoronoi(vmap);
		}

		private void 開啟ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				readMap(openFileDialog1.FileName, out vmap);
				drawVoronoi(vmap);
			}
		}

		private void 匯出圖檔ToolStripMenuItem_Click(object sender, EventArgs e)
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
			if (path != null && Path.GetExtension(path[0]) == ".json")
			{
				readMap(path[0], out vmap);
				drawVoronoi(vmap);
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

		private void numericUpDown_line_size_ValueChanged(object sender, EventArgs e)
		{
			int size = (int)numericUpDown_line_size.Value;
			blackPen.Width = size;
			greenPen.Width = size;
			redPen.Width = size;
			drawVoronoi(vmap);
		}

		private void Form1_Resize(object sender, EventArgs e)
		{
			pictureBox.Size = new Size((int)(this.Size.Height - 76), (int)(this.Size.Height - 76));
			panel1.Left = pictureBox.Right + pictureBox.Margin.Right + panel1.Margin.Left;
		}

		private void button_pause_fortune_Click(object sender, EventArgs e)
		{
			timer1.Stop();
		}

		private void button_get_result_fortune_Click(object sender, EventArgs e)
		{
			if (vmap == null)
				return;
			if (sweepLine == null)
				sweepLine = new VoronoiStruct.SweepLine(vmap);
			while (sweepLine.nextEvent() != double.MaxValue) ;
			sweepLine.finishEdges();
			drawVoronoi(vmap);
		}

		private void button_perform_Lloyd_Click_1(object sender, EventArgs e)
		{
			performLloyd();
		}

		private void button_perform_fortune_Click(object sender, EventArgs e)
		{
			if (vmap == null)
				return;
			if (sweepLine == null)
				sweepLine = new VoronoiStruct.SweepLine(vmap);
			double L = sweepLine.nextEvent();
			if (L == double.MaxValue)
				sweepLine.finishEdges();
			drawVoronoi(vmap);
			drawCurrentStep();
		}

		private void button_auto_perform_fortune_Click(object sender, EventArgs e)
		{
			if (vmap == null)
				return;
			if (sweepLine == null)
				sweepLine = new VoronoiStruct.SweepLine(vmap);
			timer1.Start();
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			double L = sweepLine.nextEvent();
			if (L == double.MaxValue)
			{
				sweepLine.finishEdges();
				timer1.Stop();
			}
			drawVoronoi(vmap);
			drawCurrentStep();
		}
	}
}
