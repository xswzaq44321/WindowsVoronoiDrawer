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
			BlackPen = new Pen(Color.Black, 3);
			circleSize = new Size(7, 7);
			pictureBox.AllowDrop = true;

			readMap("test.json", out vmap);
			drawVoronoi(vmap);
		}

		Bitmap bmp;
		Graphics g;
		Pen BlackPen;
		Brush blueBrush;
		Size circleSize;
		VoronoiStruct.Voronoi vmap;

		void readMap(string filePath, out VoronoiStruct.Voronoi map)
		{
			StreamReader sr = new StreamReader(filePath);
			string json = sr.ReadToEnd();
			map = JsonConvert.DeserializeObject<VoronoiStruct.Voronoi>(json);
		}

		void drawPoint(VoronoiStruct.Point pos)
		{
			// this func is just a short hand for writting FillEllipse
			Point pos1 = new Point(pos.x, pos.y);
			g.FillEllipse(blueBrush, new RectangleF(pos1, circleSize));
		}
		void drawPoint(Point pos)
		{
			g.FillEllipse(blueBrush, new RectangleF(pos, circleSize));
		}

		void drawLine(VoronoiStruct.Point pos1, VoronoiStruct.Point pos2)
		{
			g.DrawLine(BlackPen, pos1.x, pos1.y, pos2.x, pos2.y);
		}
		void drawLine(Point pos1, Point pos2)
		{
			g.DrawLine(BlackPen, pos1, pos2);
		}

		void drawVoronoi(VoronoiStruct.Voronoi map)
		{
			bmp = new Bitmap(map.width, map.height);
			g = Graphics.FromImage(bmp);
			g.Clear(Color.White);
			pictureBox.Image = bmp;
			foreach (var item in map.points)
			{
				drawPoint(item);
			}
			foreach (var item in map.lines)
			{
				drawLine(item.a, item.b); ;
			}
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
	}
}
