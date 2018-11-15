using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoronoiStruct
{
	class Voronoi
	{
		public Voronoi(int width, int height)
		{
			this.width = width;
			this.height = height;
		}
		public int width, height;
		public List<Point> points = new List<Point>();
		public List<Line> lines = new List<Line>();
	}

	struct Line
	{
		public Line(Point a, Point b)
		{
			this.a = a;
			this.b = b;
		}
		public Line(int ax, int ay, int bx, int by)
		{
			this.a = new Point(ax, ay);
			this.b = new Point(bx, by);
		}
		public Point a, b;
	}

	struct Point
	{
		public Point(int x, int y)
		{
			this.x = x;
			this.y = y;
		}
		public int x, y;
	}
}
