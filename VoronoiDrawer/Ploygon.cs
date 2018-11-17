using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoronoiStruct;
using System.Drawing;

namespace VoronoiDrawer
{
	class Polygon
	{
		public Polygon()
		{
			edges = new List<Edge>();
		}
		public List<Edge> edges;
		public VoronoiStruct.Point focus;
	}

	class Edge
	{
		public Edge()
		{
			parents = new Parabola[2];
			line = new Line();
		}
		public Parabola[] parents;
		public Line line;
	}

	class Parabola
	{
		public Parabola(VoronoiStruct.Point focus, int n, Rectangle regin, int id)
		{
			points = new List<PointF>();
			this.k = focus.y;
			this.h = (n + focus.x) / 2.0;
			this.c = -(n - focus.x) / 2.0;
			this.focus = focus;
			this.id = id;
			if (this.c > 0)
			{
				this.points.Add(new PointF(focus.x, focus.y));
				this.points.Add(new PointF(focus.x, focus.y));
			}
			else if (this.c == 0)
			{
				for (float x = focus.x; x >= 0; --x)
				{
					this.points.Add(new PointF(x, focus.y));
				}
			}
			else
			{
				for (float y = regin.Top; y <= regin.Bottom; ++y)
				{
					float x = (float)(Math.Pow(y - this.k, 2) / (4 * this.c) + this.h);
					this.points.Add(new PointF(x, y));
				}
			}
		}
		public int id;
		public List<PointF> points;
		public VoronoiStruct.Point focus;
		public double k, h, c;

		public void dealIntersect(Parabola obj)
		{
			var intersections = this.intersect(obj);
			if (intersections != null && intersections.GetLength(0) == 2)
			{
				bool gotErased = false;
				points.RemoveAll((PointF point) =>
				{
					if (this.focus.x < obj.focus.x && point.Y > intersections[0].Y && point.Y < intersections[1].Y)
					{
						gotErased = true;
						return true;
					}
					else
						return false;
				});
				if (gotErased)
				{
					obj.points.RemoveAll((PointF point) =>
					{
						if (point.Y < intersections[0].Y || point.Y > intersections[1].Y)
							return true;
						else
							return false;
					});
				}
			}
		}
		public PointF[] intersect(Parabola obj)
		{
			if (this == obj || this.c > 0 || obj.c > 0)
				return null;
			double A = obj.c - c;
			double B = -2 * (obj.c * k - c * obj.k);
			double C = -(4 * c * obj.c * (obj.h - h) - obj.c * k * k + c * obj.k * obj.k);
			double distance = B * B - 4 * A * C;
			if (A == 0)
			{
				if(B == 0)
				{
					return null;
				}
				double y = -C / B;
				double x = Math.Pow(y - k, 2) / (4 * c) + h;
				PointF[] s = new PointF[1];
				s[0] = new PointF((float)x, (float)y);
				return s;
			}
			else
			{
				if (distance < 0)
					return null;
				else if (distance == 0)
				{
					double y = (-B + Math.Sqrt(distance)) / (2 * A);
					double x = Math.Pow(y - k, 2) / (4 * c) + k;
					PointF[] s = new PointF[1];
					s[0] = new PointF((float)x, (float)y);
					return s;
				}
				else
				{
					double y1 = (-B + Math.Sqrt(distance)) / (2 * A);
					double y2 = (-B - Math.Sqrt(distance)) / (2 * A);
					double x1 = Math.Pow(y1 - k, 2) / (4 * c) + k;
					double x2 = Math.Pow(y2 - k, 2) / (4 * c) + k;
					PointF[] s = new PointF[2];
					s[0] = new PointF((float)x1, (float)y1);
					s[1] = new PointF((float)x2, (float)y2);
					Array.Sort(s, (PointF a, PointF b) =>
					{
						float x = a.Y - b.Y;
						// round away from zero
						return (int)(Math.Sign(x)*Math.Ceiling(Math.Abs(x)));
					});
					return s;
				}
			}
		}
	}

	class Node
	{

	}
}
