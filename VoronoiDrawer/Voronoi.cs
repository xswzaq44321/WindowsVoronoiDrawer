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
				bool xout = false, yout = false;
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
		private int[] parentID;
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

	class SweepLine
	{
		public SweepLine()
		{
			beachParabolas = new List<Parabola>();
			siteEvents = new List<Event>();
			circleEvents = new List<Event>();
		}
		public SweepLine(ref VoronoiStruct.Voronoi vmap) :
			this()
		{
			this.vmap = vmap;
			for (int i = 0; i < this.vmap.polygons.Count; i++)
			{
				var bar = this.vmap.polygons[i];
				bar.id = i;
				if (i < vmap.polygons.Count - 1 && vmap.polygons[i].focus.x == vmap.polygons[i + 1].focus.x)
				{
					bar.focus.x++;
				}
				siteEvents.Add(new Event(bar));
			}
		}

		// L := current location of sweepLine
		public double L;
		public List<Parabola> beachParabolas;
		public List<Event> siteEvents;
		public List<Event> circleEvents;
		public VoronoiStruct.Voronoi vmap;

		// moving forward swppeLine and deal with events
		public double nextEvent()
		{
			L = double.MaxValue;
			if (siteEvents.Count + circleEvents.Count < 0)
			{ // no more events to do
				return L;
			}
			Event nextEvent = null;
			nextEvent = siteEvents.Min();
			if (nextEvent == null || nextEvent.X <= circleEvents.Min()?.X)
				nextEvent = circleEvents.Min();
			if (nextEvent == null)
			{
				return L;
			}
			L = nextEvent.X;
			if (!nextEvent.isCircle)
			{
				beachAdd(nextEvent.relevant[0]);
				siteEvents.Remove(nextEvent);
			}
			else
			{
				var point = nextEvent.center;
				int pi = nextEvent.relevant[0].id;
				int pj = nextEvent.relevant[1].id;
				int pk = nextEvent.relevant[2].id;
				// usually pj is the one that'll delete from beachLine
				foreach (var edge in vmap.polygons[pi].edges)
				{
					if (edge.getParent().Contains(pj))
					{
						if (edge.isAbstract())
						{
							edge.deAbstract();
							edge.line.a = new Point(point);
						}
						else
						{
							edge.line.b = new Point(point);
						}
					}
				}
				foreach (var edge in vmap.polygons[pk].edges)
				{
					if (edge.getParent().Contains(pj))
					{
						if (edge.isAbstract())
						{
							edge.deAbstract();
							edge.line.a = new Point(point);
						}
						else
						{
							edge.line.b = new Point(point);
						}
					}
				}
				foreach (var edge in vmap.polygons[pj].edges)
				{
					if (edge.getParent().Contains(pi) || edge.getParent().Contains(pk))
					{
						if (edge.isAbstract())
						{
							edge.deAbstract();
							edge.line.a = new Point(point);
						}
						else
						{
							edge.line.b = new Point(point);
						}
					}
				}
				Edge bar = new Edge(pi, pk);
				bar.line.a = new Point(point);
				vmap.polygons[pi].edges.Add(new Edge(bar));
				vmap.polygons[pk].edges.Add(new Edge(bar));
				dealAfterCircleEvent(nextEvent);
			}
			return L;
		}

		public void beachAdd(Polygon p)
		{
			int pos = 0;
			Parabola pj = null;
			// find which arc to insert pi
			for (int i = 0; i < beachParabolas.Count - 2; i++)
			{
				var intersect = beachParabolas[i].getIntersect(beachParabolas[i + 1], L);
				var intersect2 = beachParabolas[i + 1].getIntersect(beachParabolas[i + 2], L);
				if (intersect.Y < p.focus.y && intersect2.Y > p.focus.y)
				{
					pos = i + 1;
					break;
				}
				else if (i == 0 && intersect.Y > p.focus.y)
				{
					// first one
					pos = i;
					break;
				}
				else if (i == beachParabolas.Count - 2 - 1)
				{
					// last one
					pos = beachParabolas.Count - 1;
					break;
				}
			}
			if (beachParabolas.Count == 0)
			{
				// beach is empty
				beachParabolas.Add(new Parabola(p));
				return;
			}
			pj = beachParabolas[pos];
			beachParabolas.Insert(pos + 1, new Parabola(p));
			beachParabolas.Insert(pos + 2, new Parabola(pj, false));
			// now p is on pos + 1
			pos = pos + 1;

			// add to edges
			vmap.polygons[p.id].edges.Add(new Edge(p.id, pj.parent.id, true));
			pj.parent.edges.Add(new Edge(p.id, pj.parent.id, true));

			// remove old pj circle event
			circleEvents.Remove(pj.e);
			pj.e = null;

			// adding new triple involving p (which is left 2 & right 2 of pi with pi)
			for (int j = pos - 1; j <= pos + 1; j += 2)
			{
				//if ((j - 1) < 0 || (j + 1) >= beachParabolas.Count)
				//	continue;
				checkCircleEvent(j);
			}
		}

		// get parabola's x position on y = y
		private double parabolaX(VoronoiStruct.Point focus, double y)
		{
			double k = focus.y;
			double h = (L + focus.x) / 2.0;
			double c = -(L - focus.x) / 2.0;
			if (c >= 0)
			{
				return double.MinValue;
			}
			else
			{
				double x = Math.Pow(y - k, 2) / (4 * c) + h;
				return x;
			}
		}

		// delete e from circleEvents and corresponding p from beachLine
		public void dealAfterCircleEvent(Event e)
		{
			bool err = true;
			int p1 = e.relevant[0].id;
			int p2 = e.relevant[1].id;
			int p3 = e.relevant[2].id;
			int p2Pos = -1;
			for (int i = 0; i < beachParabolas.Count - 2; i++)
			{
				if (beachParabolas[i].e == e)
				{
					p2Pos = i;
					err = false;
					beachParabolas.RemoveAt(i);
					break;
				}
			}
			circleEvents.Remove(e);
			// check circle Event on left side of p2
			checkCircleEvent(p2Pos - 1);
			// check circle Event on right side of p2
			checkCircleEvent(p2Pos);

			if (err)
				System.Diagnostics.Debug.WriteLine("Error dealCircleEvent! no matching pattern in beach");
			return;
		}

		public void finishEdges()
		{
			// set L large enough
			L = 2 * vmap.width + 2 * vmap.height;
			for (int i = 0; i < beachParabolas.Count - 1; i++)
			{
				var p1 = beachParabolas[i];
				var p2 = beachParabolas[i + 1];
				PointF cross = p1.getIntersect(p2, L);
				foreach (var edge in p1.parent.edges)
				{
					if (edge.getParent().Contains(p2.parent.id))
					{
						edge.line.b = new Point(cross);
					}
				}
				foreach (var edge in p2.parent.edges)
				{
					if (edge.getParent().Contains(p1.parent.id))
					{
						edge.line.b = new Point(cross);
					}
				}
			}
		}

		// get points for drawing purpose
		public List<List<PointF>> getBeachLinePoints(double interval = 1.0)
		{
			List<List<PointF>> parabolas = new List<List<PointF>>();
			if (L == double.MaxValue)
			{
				return parabolas;
			}
			double y = -10;
			if (beachParabolas.Count == 1)
			{
				var poly = beachParabolas[0];
				parabolas.Add(new List<PointF>());
				double k = poly.focus.y;
				double h = (L + poly.focus.x) / 2.0;
				double c = -(L - poly.focus.x) / 2.0;
				if (c == 0)
				{
					for (double x = -10; x < poly.focus.x; x += interval)
					{
						parabolas[0].Add(new PointF((float)x, (float)poly.focus.y));
					}
				}
				else
				{
					for (; y < vmap.height + 10; y += interval)
					{
						double x = Math.Pow(y - k, 2) / (4 * c) + h;
						parabolas[0].Add(new PointF((float)x, (float)y));
					}
				}
			}
			else if (beachParabolas.Count > 1)
			{
				for (int i = 0; i < beachParabolas.Count - 1 && y < vmap.height + 10; i++)
				{
					var poly = beachParabolas[i];
					parabolas.Add(new List<PointF>());
					double k = poly.focus.y;
					double h = (L + poly.focus.x) / 2.0;
					double c = -(L - poly.focus.x) / 2.0;
					PointF intersect = beachParabolas[i].getIntersect(beachParabolas[i + 1], L);
					if (c == 0)
					{
						for (double x = -10; x < poly.focus.x; x += interval)
						{
							parabolas[i].Add(new PointF((float)x, (float)poly.focus.y));
						}
					}
					else
					{
						for (; y < intersect.Y; y += interval)
						{
							double x = Math.Pow(y - k, 2) / (4 * c) + h;
							parabolas[i].Add(new PointF((float)x, (float)y));
						}
					}
				}
				{
					var poly = beachParabolas.Last();
					double k = poly.focus.y;
					double h = (L + poly.focus.x) / 2.0;
					double c = -(L - poly.focus.x) / 2.0;
					if (c == 0)
					{
						for (double x = -10; x < poly.focus.x; x += interval)
						{
							parabolas.Last().Add(new PointF((float)x, (float)poly.focus.y));
						}
					}
					else
					{
						for (; y < vmap.height + 10; y += interval)
						{
							double x = Math.Pow(y - k, 2) / (4 * c) + h;
							parabolas.Last().Add(new PointF((float)x, (float)y));
						}
					}
				}
			}
			return parabolas;
		}

		public List<Rectangle> getCirclePoints()
		{
			List<Rectangle> points = new List<Rectangle>();
			foreach (var circleEvent in circleEvents)
			{
				double r = circleEvent.X - circleEvent.center.X;
				Rectangle bar = new Rectangle((int)(circleEvent.center.X - r), (int)(circleEvent.center.Y - r),
					(int)(2 * r), (int)(2 * r));
				points.Add(bar);
			}
			return points;
		}

		public void checkCircleEvent(int posOnBeach)
		{
			if (posOnBeach <= 0 || posOnBeach >= beachParabolas.Count - 1)
				return;
			Parabola p1 = beachParabolas[posOnBeach - 1];
			Parabola p2 = beachParabolas[posOnBeach];
			Parabola p3 = beachParabolas[posOnBeach + 1];
			// b to a cross c to a is smaller than zero means it's a left turn
			if ((p2.focus.x - p1.focus.x) * (p3.focus.y - p1.focus.y) - (p3.focus.x - p1.focus.x) * (p2.focus.y - p1.focus.y) < 0)
			{
				Event circleEve = new Event(p1.parent, p2.parent, p3.parent);
				if (circleEve.X >= L)
				{
					circleEvents.Remove(p2.e);
					p2.e = circleEve;
					circleEvents.Add(circleEve);
				}
			}
		}
	}

	class Event : IComparable
	{
		// site event constructor
		public Event(Polygon rel)
		{
			relevant.Add(rel);
			X = rel.focus.x;
			isCircle = false;
		}
		// circle event constructor
		public Event(Polygon r1, Polygon r2, Polygon r3)
		{
			relevant.Add(r1);
			relevant.Add(r2);
			relevant.Add(r3);
			double[,] X = new double[3, 3];
			double[,] Y = new double[3, 3];
			double[,] D = new double[3, 3];
			X[0, 0] = (Math.Pow(r1.focus.x, 2) + Math.Pow(r1.focus.y, 2));
			X[1, 0] = r1.focus.y;
			X[2, 0] = 1;
			Y[0, 0] = r1.focus.x;
			Y[1, 0] = (Math.Pow(r1.focus.x, 2) + Math.Pow(r1.focus.y, 2));
			Y[2, 0] = 1;
			D[0, 0] = r1.focus.x;
			D[1, 0] = r1.focus.y;
			D[2, 0] = 1;
			X[0, 1] = (Math.Pow(r2.focus.x, 2) + Math.Pow(r2.focus.y, 2));
			X[1, 1] = r2.focus.y;
			X[2, 1] = 1;
			Y[0, 1] = r2.focus.x;
			Y[1, 1] = (Math.Pow(r2.focus.x, 2) + Math.Pow(r2.focus.y, 2));
			Y[2, 1] = 1;
			D[0, 1] = r2.focus.x;
			D[1, 1] = r2.focus.y;
			D[2, 1] = 1;
			X[0, 2] = (Math.Pow(r3.focus.x, 2) + Math.Pow(r3.focus.y, 2));
			X[1, 2] = r3.focus.y;
			X[2, 2] = 1;
			Y[0, 2] = r3.focus.x;
			Y[1, 2] = (Math.Pow(r3.focus.x, 2) + Math.Pow(r3.focus.y, 2));
			Y[2, 2] = 1;
			D[0, 2] = r3.focus.x;
			D[1, 2] = r3.focus.y;
			D[2, 2] = 1;
			double DX = calculator.det(X);
			double DY = calculator.det(Y);
			double DD = calculator.det(D);
			center = new PointF((float)(DX / (2 * DD)), (float)(DY / (2 * DD)));
			double r = calculator.distance(r1.focus, center);
			this.X = center.X + r;
			isCircle = true;
		}
		public Event(Event old)
		{
			this.relevant = new List<Polygon>(relevant);
			this.X = old.X;
			this.center = old.center;
			this.isCircle = old.isCircle;
		}

		public List<Polygon> relevant = new List<Polygon>();
		public double X;
		public PointF center;
		public bool isCircle;

		public int CompareTo(object obj)
		{
			double val = this.X - ((Event)obj).X;
			return Math.Sign(val) * (int)Math.Ceiling(Math.Abs(val));
		}
	}

	class Parabola
	{
		public Parabola(Polygon parent)
		{
			this.focus = parent.focus;
			this.parent = parent;
		}
		public Parabola(Parabola old, bool withEvent = true)
		{
			this.focus = old.focus;
			this.parent = old.parent;
			if (withEvent)
			{
				this.e = new Event(old.e);
			}
		}

		public VoronoiStruct.Point focus;
		public Event e;
		public Polygon parent;

		public PointF getIntersect(Parabola obj, double L)
		{
			double ka = this.focus.y;
			double ha = (L + this.focus.x) / 2.0;
			double ca = -(L - this.focus.x) / 2.0;
			double kb = obj.focus.y;
			double hb = (L + obj.focus.x) / 2.0;
			double cb = -(L - obj.focus.x) / 2.0;
			double a = cb - ca;
			double b = -2 * (cb * ka - ca * kb);
			double c = -(4 * ca * cb * (hb - ha) - cb * ka * ka + ca * kb * kb);
			double distance = b * b - 4 * a * c;
			if (a == 0)
			{
				if (b == 0)
				{
					return new PointF();
				}
				double y = -c / b;
				double x = Math.Pow(y - ka, 2) / (4 * ca) + ha;
				return new PointF((float)x, (float)y);
			}
			else
			{
				double[] root = new double[2];
				root[0] = (-b + Math.Sqrt(distance)) / (2 * a);
				root[1] = (-b - Math.Sqrt(distance)) / (2 * a);
				Array.Sort(root);
				double y;
				if (this.focus.x < obj.focus.x)
				{
					y = root[0];
				}
				else
				{
					y = root[1];
				}
				double x = Math.Pow(y - ka, 2) / (4 * ca) + ha;
				return new PointF((float)x, (float)y);
			}
		}
	}

	static class calculator
	{
		public static double det(double[,] m)
		{
			return (m[0, 0] * m[1, 1] * m[2, 2] + m[0, 1] * m[1, 2] * m[2, 0] + m[0, 2] * m[1, 0] * m[2, 1])
				- (m[0, 2] * m[1, 1] * m[2, 0] + m[0, 1] * m[1, 0] * m[2, 2] + m[0, 0] * m[1, 2] * m[2, 1]);
		}

		public static double distance(VoronoiStruct.Point a, PointF b)
		{
			return Math.Sqrt(Math.Pow(a.x - b.X, 2) + Math.Pow(a.y - b.Y, 2));
		}

		public static double distance(PointF a, PointF b)
		{
			return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
		}
	}
}
