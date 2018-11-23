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
				if(degree < -Math.PI / 2 || degree > Math.PI / 2)
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
			beachPolys = new List<Polygon>();
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
				addSite(new Event(bar));
			}
		}

		// current location of sweepLine
		public double L;
		public List<Polygon> beachPolys;
		public List<Event> siteEvents;
		public List<Event> circleEvents;
		public VoronoiStruct.Voronoi vmap;

		public void addSite(Event e)
		{
			siteEvents.Add(e);
		}

		// moving forward swppeLine and deal with events
		public double nextEvent()
		{
			if (siteEvents.Count + circleEvents.Count < 0)
			{
				return double.MaxValue;
			}
			L = double.MaxValue;
			Event nextEvent = null;
			foreach (var e in siteEvents.Union(circleEvents))
			{
				if (e.X < L)
				{
					L = e.X;
					nextEvent = e;
				}
			}
			if (nextEvent == null)
			{
				return L;
			}
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
				dealCircleEvent(nextEvent);
			}
			return L;
		}

		public void beachAdd(Polygon pi)
		{
			int pos = 0;
			Polygon pj = null;
			// find which arc to insert pi
			for (int i = 0; i < beachPolys.Count - 2; i++)
			{
				var intersect = getIntersect(beachPolys[i].focus, beachPolys[i + 1].focus);
				var intersect2 = getIntersect(beachPolys[i + 1].focus, beachPolys[i + 2].focus);
				if (intersect.Y < pi.focus.y && intersect2.Y > pi.focus.y)
				{
					pos = i + 1;
					break;
				}
				else if (i == 0 && intersect.Y > pi.focus.y)
				{
					// first one
					pos = i;
					break;
				}
				else if (i == beachPolys.Count - 2 - 1)
				{
					// last one
					pos = beachPolys.Count - 1;
					break;
				}
			}
			if (beachPolys.Count == 0)
			{
				// beach is empty
				beachPolys.Add(pi);
				return;
			}
			pj = beachPolys[pos];
			Polygon pj_l = null;
			Polygon pj_r = null;
			if (pos - 1 >= 0)
				pj_l = beachPolys[pos - 1];
			if (pos + 1 < beachPolys.Count)
				pj_r = beachPolys[pos + 1];
			beachPolys.Insert(pos + 1, pi);
			beachPolys.Insert(pos + 2, pj);
			// now pi is on pos + 1
			pos = pos + 1;

			// add to edges
			vmap.polygons[pi.id].edges.Add(new Edge(pi.id, pj.id, true));
			vmap.polygons[pj.id].edges.Add(new Edge(pi.id, pj.id, true));

			// remove old triple involving pj
			circleEvents.RemoveAll((e) =>
			{
				/* implement this code */
				/* find pos - 1, pos, pos + 1 pattern */
				if (e.relevant[0] == pj_l && e.relevant[1] == pj && e.relevant[2] == pj_r)
					return true;
				else
					return false;
			});
			// adding new triple involving pi (which is left 2 & right 2 of pi with pi)
			for (int j = pos - 1; j <= pos + 1; j += 2)
			{
				if ((j - 1) < 0 || (j + 1) >= beachPolys.Count)
					continue;
				Polygon p1 = beachPolys[j - 1];
				Polygon p2 = beachPolys[j];
				Polygon p3 = beachPolys[j + 1];
				// b to a cross c to a is smaller than zero means it's a left turn
				if ((p2.focus.x - p1.focus.x) * (p3.focus.y - p1.focus.y) - (p3.focus.x - p1.focus.x) * (p2.focus.y - p1.focus.y) < 0)
				{
					Event circleEve = new Event(p1, p2, p3);
					if (circleEve.X >= L)
					{
						circleEvents.Add(circleEve);
					}
				}
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
		public void dealCircleEvent(Event e)
		{
			bool err = true;
			int p1 = e.relevant[0].id;
			int p2 = e.relevant[1].id;
			int p3 = e.relevant[2].id;
			for (int i = 0; i < beachPolys.Count - 2; i++)
			{
				if (beachPolys[i].id == p1 && beachPolys[i + 1].id == p2 && beachPolys[i + 2].id == p3)
				{
					if (i > 0)
					{
						var pp1 = beachPolys[i - 1];
						var pp2 = beachPolys[i];
						var pp3 = beachPolys[i + 2];
						if ((pp2.focus.x - pp1.focus.x) * (pp3.focus.y - pp1.focus.y) - (pp3.focus.x - pp1.focus.x) * (pp2.focus.y - pp1.focus.y) < 0)
						{
							Event cirEve = new Event(pp1, pp2, pp3);
							if (cirEve.X >= L)
							{
								circleEvents.Add(cirEve);
							}
						}
					}
					if (i < beachPolys.Count - 3)
					{
						var pp1 = beachPolys[i];
						var pp2 = beachPolys[i + 2];
						var pp3 = beachPolys[i + 3];
						if ((pp2.focus.x - pp1.focus.x) * (pp3.focus.y - pp1.focus.y) - (pp3.focus.x - pp1.focus.x) * (pp2.focus.y - pp1.focus.y) < 0)
						{
							Event cirEve = new Event(pp1, pp2, pp3);
							if (cirEve.X >= L)
							{
								circleEvents.Add(cirEve);
							}
						}
					}
					err = false;
					beachPolys.RemoveAt(i + 1);
					break;
				}
			}
			circleEvents.Remove(e);
			circleEvents.RemoveAll((e2) =>
			{
				if (e2.relevant[0] == e.relevant[1] && e2.relevant[1] == e.relevant[2])
					return true;
				if (e2.relevant[1] == e.relevant[0] && e2.relevant[2] == e.relevant[1])
					return true;
				return false;
			});

			if (err)
				System.Diagnostics.Debug.WriteLine("Error dealCircleEvent! no matching pattern in beach");
			return;
		}

		public void finishEdges()
		{
			// set L large enough
			L = 2 * vmap.width + 2 * vmap.height;
			for (int i = 0; i < beachPolys.Count - 1; i++)
			{
				var p1 = beachPolys[i];
				var p2 = beachPolys[i + 1];
				PointF cross = getIntersect(p1.focus, p2.focus);
				foreach (var edge in p1.edges)
				{
					if (edge.getParent().Contains(p2.id))
					{
						edge.line.b = new Point(cross);
					}
				}
				foreach (var edge in p2.edges)
				{
					if (edge.getParent().Contains(p1.id))
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
			if (beachPolys.Count == 1)
			{
				var poly = beachPolys[0];
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
			else if (beachPolys.Count > 1)
			{
				for (int i = 0; i < beachPolys.Count - 1 && y < vmap.height + 10; i++)
				{
					var poly = beachPolys[i];
					parabolas.Add(new List<PointF>());
					double k = poly.focus.y;
					double h = (L + poly.focus.x) / 2.0;
					double c = -(L - poly.focus.x) / 2.0;
					PointF intersect = getIntersect(beachPolys[i].focus, beachPolys[i + 1].focus);
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
					var poly = beachPolys.Last();
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

		private PointF getIntersect(Point A, Point B)
		{
			double ka = A.y;
			double ha = (L + A.x) / 2.0;
			double ca = -(L - A.x) / 2.0;
			double kb = B.y;
			double hb = (L + B.x) / 2.0;
			double cb = -(L - B.x) / 2.0;
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
				if (A.x < B.x)
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

	class Event
	{
		public Event(Polygon rel)
		{
			relevant.Add(rel);
			X = rel.focus.x;
			isCircle = false;
		}
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

		public List<Polygon> relevant = new List<Polygon>();
		public double X;
		public PointF center;
		public bool isCircle;
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
