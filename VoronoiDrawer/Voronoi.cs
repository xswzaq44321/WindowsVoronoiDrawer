using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoronoiStruct
{
	class Voronoi
	{
		public Voronoi()
		{
			polygons = new List<Polygon>();
		}
		public Voronoi(int width, int height)
		{
			polygons = new List<Polygon>();
			this.width = width;
			this.height = height;
		}
		public Voronoi(Voronoi old)
		{
			this.width = old.width;
			this.height = old.height;
			this.polygons = old.polygons.Select(bar => new Polygon(bar)).ToList();
		}
		public int width, height;
		public List<Polygon> polygons;
	}

	class Polygon
	{
		public Polygon()
		{
			edges = new List<Edge>();
			focus = new Point();
		}
		public Polygon(VoronoiStruct.Point focus)
		{
			edges = new List<Edge>();
			this.focus = focus;
		}
		public Polygon(List<Edge> edges, VoronoiStruct.Point focus)
		{
			this.edges = new List<Edge>();
			this.focus = focus;
		}
		public Polygon(Polygon old)
		{
			this.edges = old.edges.Select(bar => new Edge(bar)).ToList();
			this.focus = old.focus;
			this.id = old.id;
		}

		public List<Edge> edges;
		public VoronoiStruct.Point focus;
		public int id;

		public void optimize(Rectangle regin)
		{
			// fixing edge lines
			List<VoronoiStruct.Point> borderEdges = new List<VoronoiStruct.Point>();
			for (int i = 0; i < edges.Count; i++)
			{
				var edge = edges[i];
				if (!regin.Contains(edge.line.a.x, edge.line.a.y) && !regin.Contains(edge.line.b.x, edge.line.b.y))
				{
					// edge is out of regin
					edges.Remove(edge);
					--i;
					continue;
				}
				if (regin.Contains(edge.line.a.x, edge.line.a.y) && regin.Contains(edge.line.b.x, edge.line.b.y))
				{
					continue;
				}
				double mx = edge.line.b.x - edge.line.a.x;
				double my = edge.line.b.y - edge.line.a.y;
				double y0 = 0, ym = 0;
				if (mx != 0)
				{
					y0 = edge.line.a.y + (0 - edge.line.a.x) * (my / mx);
					ym = edge.line.a.y + (regin.Right - 1 - edge.line.a.x) * (my / mx);
				}
				if (edge.line.a.x < 0)
				{
					edge.line.a = new Point(0, (int)y0);
				}
				else if (edge.line.b.x < 0)
				{
					edge.line.b = new Point(0, (int)y0);
				}
				else if (edge.line.a.x >= regin.Right)
				{
					edge.line.a = new Point(regin.Right - 1, (int)ym);
				}
				else if (edge.line.b.x >= regin.Right)
				{
					edge.line.b = new Point(regin.Right - 1, (int)ym);
				}
				double x0 = 0, xm = 0;
				if (my != 0)
				{
					x0 = edge.line.a.x + (0 - edge.line.a.y) * (mx / my);
					xm = edge.line.a.x + (regin.Bottom - 1 - edge.line.a.y) * (mx / my);
				}
				if (edge.line.a.y < 0)
				{
					edge.line.a = new Point((int)x0, 0);
				}
				else if (edge.line.b.y < 0)
				{
					edge.line.b = new Point((int)x0, 0);
				}
				else if (edge.line.a.y >= regin.Bottom)
				{
					edge.line.a = new Point((int)xm, regin.Bottom - 1);
				}
				else if (edge.line.b.y >= regin.Bottom)
				{
					edge.line.b = new Point((int)xm, regin.Bottom - 1);
				}
				if (edge.line.a.x == 0 || edge.line.a.x == regin.Bottom - 1)
				{
					borderEdges.Add(edge.line.a);
				}
				else if (edge.line.b.x == 0 || edge.line.b.x == regin.Bottom - 1)
				{
					borderEdges.Add(edge.line.b);
				}
				if (edge.line.a.y == 0 || edge.line.a.y == regin.Bottom - 1)
				{
					borderEdges.Add(edge.line.a);
				}
				else if (edge.line.b.y == 0 || edge.line.b.y == regin.Bottom - 1)
				{
					borderEdges.Add(edge.line.b);
				}
			}
			if (borderEdges.Count == 2)
			{
				if (borderEdges[0].x == borderEdges[1].x)
				{
					var bar = new Edge();
					bar.deAbstract();
					bar.line.a = borderEdges[0];
					bar.line.b = borderEdges[1];
					this.edges.Add(bar);
				}
				else if (borderEdges[0].y == borderEdges[1].y)
				{
					var bar = new Edge();
					bar.deAbstract();
					bar.line.a = borderEdges[0];
					bar.line.b = borderEdges[1];
					this.edges.Add(bar);
				}
				else
				{
					var endPoint = new VoronoiStruct.Point();
					if (borderEdges[0].x == 0 || borderEdges[0].x == regin.Bottom - 1)
					{
						endPoint.x = borderEdges[0].x;
					}
					else
					{
						endPoint.x = borderEdges[1].x;
					}
					if (borderEdges[0].y == 0 || borderEdges[0].y == regin.Bottom - 1)
					{
						endPoint.y = borderEdges[0].y;
					}
					else
					{
						endPoint.y = borderEdges[1].y;
					}
					var bar1 = new Edge();
					var bar2 = new Edge();
					bar1.deAbstract();
					bar2.deAbstract();
					bar1.line.a = borderEdges[0];
					bar1.line.b = endPoint;
					bar2.line.a = borderEdges[1];
					bar2.line.b = endPoint;
					this.edges.Add(bar1);
					this.edges.Add(bar2);
				}
			}
			sortEdges();
		}
		private void sortEdges()
		{
			double[] edgeDegrees = new double[edges.Count];
			Edge[] edge = edges.ToArray();
			for (int i = 0; i < edges.Count; i++)
			{
				// add average degree of edge to array for sorting uses later
				double ta = Math.Atan2(edges[i].line.a.y - focus.y, edges[i].line.a.x - focus.x);
				double tb = Math.Atan2(edges[i].line.b.y - focus.y, edges[i].line.b.x - focus.x);
				double degree = (ta + tb) / 2;
				if (Math.Abs(ta - tb) > Math.PI)
				{
					degree += Math.PI;
					for (; degree > Math.PI; degree -= 2 * Math.PI) ;
					for (; degree < -Math.PI; degree += 2 * Math.PI) ;
				}
				edgeDegrees[i] = degree;

				// sort a, b
				if (degree < -Math.PI / 2 || degree > Math.PI / 2)
				{ // angle of edge is behind focus
					if (ta < 0 && tb > 0)
						ta += 2 * Math.PI;
					else if (ta > 0 && tb < 0)
						tb += 2 * Math.PI;
				}
				if (ta >= tb)
				{
					var temp = edges[i].line.a;
					edge[i].line.a = edges[i].line.b;
					edge[i].line.b = temp;
				}
			}
			Array.Sort(edgeDegrees, edge);
			edges = new List<Edge>(edge);
		}
	}

	class Edge
	{
		public Edge()
		{
			line = new Line(-1, -1, -1, -1);
			parentID = null;
			is_abstract = true;
		}
		public Edge(int id1, int id2, bool is_abstract = false)
		{
			line = new Line(-1, -1, -1, -1);
			parentID = new int[2];
			parentID[0] = id1;
			parentID[1] = id2;
			this.is_abstract = is_abstract;
		}
		public Edge(Edge old)
		{
			line = new Line();
			line.a = old.line.a;
			line.b = old.line.b;
			parentID = new int[2];
			parentID[0] = old.parentID[0];
			parentID[1] = old.parentID[1];
			this.is_abstract = old.is_abstract;
		}

		public Line line;
		public int[] parentID;
		private bool is_abstract;

		public int[] getParent()
		{
			return parentID;
		}
		public void setParentID(int[] IDs)
		{
			if (IDs.Count() != 2)
				System.Diagnostics.Debug.WriteLine("Error setParentID!");
			parentID = IDs;
		}
		public bool isAbstract()
		{
			return is_abstract;
		}
		public void deAbstract()
		{
			this.is_abstract = false;
		}
	}

	struct Line
	{
		public Line(VoronoiStruct.Point a, VoronoiStruct.Point b)
		{
			this.a = a;
			this.b = b;
		}
		public Line(PointF a, PointF b)
		{
			this.a = new VoronoiStruct.Point((int)a.X, (int)a.Y);
			this.b = new VoronoiStruct.Point((int)b.X, (int)b.Y);
		}
		public Line(int ax, int ay, int bx, int by)
		{
			this.a = new VoronoiStruct.Point(ax, ay);
			this.b = new VoronoiStruct.Point(bx, by);
		}

		public VoronoiStruct.Point a, b;
	}

	struct Point
	{
		public Point(PointF p)
		{
			x = (int)p.X;
			y = (int)p.Y;
		}
		public Point(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public int x, y;
	}
}
