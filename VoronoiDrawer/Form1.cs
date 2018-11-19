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

		void readMap(string filePath, out VoronoiStruct.Voronoi vmap)
		{
			StreamReader sr = new StreamReader(filePath);
			string json = sr.ReadToEnd();
			var map = JsonConvert.DeserializeObject<VoronoiStruct.Voronoi>(json);
			foreach (var poly in map.polygons)
			{
				poly.setRegin(new Rectangle(0, 0, map.width, map.height));
			}
			vmap = map;
			sr.Close();
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

		bool onEdge(VoronoiStruct.Edge o)
		{
			if (o.line.a.x == 0 || o.line.a.y == 0 || o.line.b.x == 0 || o.line.b.y == 0 ||
				o.line.a.x == vmap.width - 1 || o.line.a.y == vmap.width - 1 || o.line.b.x == vmap.width - 1 || o.line.b.y == vmap.width - 1)
			{
				return true;
			}
			return false;
		}

		void optimizeVmap()
		{
			foreach (var poly in vmap.polygons)
			{
				poly.optimize();
				for (int i = 0; i < poly.edges.Count; i++)
				{
					string json = JsonConvert.SerializeObject(poly);
					System.Diagnostics.Debug.WriteLine(json);
					VoronoiStruct.Polygon[] tri = new VoronoiStruct.Polygon[3];
					VoronoiStruct.Edge a = poly.edges[i];
					VoronoiStruct.Edge b = poly.edges[(i + 1) % poly.edges.Count];
					if(poly.edges.Count > 2 && onEdge(a) && onEdge(b))
					{
						continue;
					}
					HashSet<int> IDs = new HashSet<int>();
					IDs.Add(a.parentID[0]);
					IDs.Add(a.parentID[1]);
					IDs.Add(b.parentID[0]);
					IDs.Add(b.parentID[1]);
					if (IDs.Count != 3)
						System.Diagnostics.Debug.WriteLine("Error, hashset count incorrect");
					tri[0] = vmap.polygons[IDs.ElementAt(0)];
					tri[1] = vmap.polygons[IDs.ElementAt(1)];
					tri[2] = vmap.polygons[IDs.ElementAt(2)];

					double[,] X = new double[3, 3];
					double[,] Y = new double[3, 3];
					double[,] D = new double[3, 3];
					for (int k = 0; k < 3; k++)
					{
						X[0, k] = (Math.Pow(tri[k].focus.x, 2) + Math.Pow(tri[k].focus.y, 2));
						X[1, k] = tri[k].focus.y;
						X[2, k] = 1;
						Y[0, k] = tri[k].focus.x;
						Y[1, k] = (Math.Pow(tri[k].focus.x, 2) + Math.Pow(tri[k].focus.y, 2));
						Y[2, k] = 1;
						D[0, k] = tri[k].focus.x;
						D[1, k] = tri[k].focus.y;
						D[2, k] = 1;
					}
					double DX = det(X);
					double DY = det(Y);
					double DD = det(D);
					PointF center = new PointF((float)(DX / (2 * DD)), (float)(DY / (2 * DD)));
					if (distance(a.line.a, center) < distance(a.line.b, center))
					{
						a.line.a = new VoronoiStruct.Point((int)center.X, (int)center.Y);
					}
					else
					{
						a.line.b = new VoronoiStruct.Point((int)center.X, (int)center.Y);
					}
					if (distance(b.line.a, center) < distance(b.line.b, center))
					{
						b.line.a = new VoronoiStruct.Point((int)center.X, (int)center.Y);
					}
					else
					{
						b.line.b = new VoronoiStruct.Point((int)center.X, (int)center.Y);
					}
				}
			}
		}

		double det(double[,] m)
		{
			return (m[0, 0] * m[1, 1] * m[2, 2] + m[0, 1] * m[1, 2] * m[2, 0] + m[0, 2] * m[1, 0] * m[2, 1])
				- (m[0, 2] * m[1, 1] * m[2, 0] + m[0, 1] * m[1, 0] * m[2, 2] + m[0, 0] * m[1, 2] * m[2, 1]);
		}

		void performFortuneStep(object sender, EventArgs e)
		{
			// end perform
			if (sweepLine.a.x > vmap.width * 2)
			{
				timer1.Stop();
				optimizeVmap();
				drawVoronoi(vmap);
				return;
			}
			if (checkBox_visualize_voronoi.Checked)
				drawVoronoi(vmap);
			Bitmap bar = new Bitmap(bmp);
			pictureBox.Image = bar;
			g = Graphics.FromImage(bar);
			var parabolas = oneFortuneStep();
			foreach (var item in parabolas)
			{
				var intersections = item.getIntersections();
				if (item.points.Count > 1 && checkBox_visualize_voronoi.Checked)
				{
					g.DrawCurve(greenPen, item.points.ToArray());
					foreach (var item3 in intersections)
					{
						foreach (var item4 in item3.points)
						{
							drawPoint(orangeBrush, Point.Round(item4));
						}
					}
				}
			}
			if (checkBox_visualize_voronoi.Checked)
				drawLine(redPen, sweepLine);
			pictureBox.Invalidate();
		}

		List<VoronoiStruct.Parabola> oneFortuneStep()
		{
			++sweepLine.a.x;
			++sweepLine.b.x;
			List<VoronoiStruct.Parabola> parabolas = new List<VoronoiStruct.Parabola>();
			for (int i = 0; i < vmap.polygons.Count; i++)
			{
				if (!vmap.polygons[i].isEnclosed())
					parabolas.Add(getParabola(vmap.polygons[i].focus, i));
			}
			foreach (var item in parabolas)
			{
				foreach (var item2 in parabolas)
				{
					item.dealIntersect(item2);
				}
				if (item.points.Count == 0)
				{
					vmap.polygons[item.id].setEnclosed(true);
				}
				var intersections = item.getIntersections();
				foreach (var item2 in intersections)
				{
					for (int i = 0; i < 2; i++)
					{
						VoronoiStruct.Edge edge = null;
						foreach (var item3 in vmap.polygons[item2.parentID[i]].edges)
						{
							if (item3.parentID.Contains(item2.parentID[0]) &&
								item3.parentID.Contains(item2.parentID[1]))
							{
								edge = item3;
							}
						}
						if (edge == null)
						{
							edge = new VoronoiStruct.Edge(item2.parentID[0], item2.parentID[1]);
							if (item2.points.Length == 1)
							{
								edge.line.a = new VoronoiStruct.Point((int)item2.points[0].X, (int)item2.points[0].Y);
								edge.line.b = new VoronoiStruct.Point((int)item2.points[0].X, (int)item2.points[0].Y);
							}
							else
							{
								edge.line.a = new VoronoiStruct.Point((int)item2.points[0].X, (int)item2.points[0].Y);
								edge.line.b = new VoronoiStruct.Point((int)item2.points[1].X, (int)item2.points[1].Y);
							}
							vmap.polygons[item2.parentID[i]].edges.Add(edge);
							vmap.polygons[item2.parentID[i]].neighborID.Add(item2.parentID[1 - i]);
						}
						else
						{
							if (item2.points.Length == 1)
							{
								if (distance(edge.line.a, item2.points[0]) < distance(edge.line.b, item2.points[0]))
								{
									edge.line.a = new VoronoiStruct.Point((int)item2.points[0].X, (int)item2.points[0].Y);
								}
								else
								{
									edge.line.b = new VoronoiStruct.Point((int)item2.points[0].X, (int)item2.points[0].Y);
								}
							}
							else
							{
								edge.line.a = new VoronoiStruct.Point((int)item2.points[0].X, (int)item2.points[0].Y);
								edge.line.b = new VoronoiStruct.Point((int)item2.points[1].X, (int)item2.points[1].Y);
							}
						}
					}
				}
			}
			return parabolas;
		}

		double distance(VoronoiStruct.Point a, PointF b)
		{
			return Math.Sqrt(Math.Pow(a.x - b.X, 2) + Math.Pow(a.y - b.Y, 2));
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
			if (g != null)
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
			if (path != null && Path.GetExtension(path[0]) == ".json")
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

		private void button_run_Click(object sender, EventArgs e)
		{
			if (vmap == null)
				return;
			sweepLine.a = new VoronoiStruct.Point(0, 0);
			sweepLine.b = new VoronoiStruct.Point(0, vmap.height);
			progressBar_frotune.Maximum = vmap.width * 2;
			progressBar_frotune.Value = 0;
			while (sweepLine.a.x <= vmap.width * 2)
			{
				progressBar_frotune.Value = (int)sweepLine.a.x;
				oneFortuneStep();
			}
			optimizeVmap();
			drawVoronoi(vmap);
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
