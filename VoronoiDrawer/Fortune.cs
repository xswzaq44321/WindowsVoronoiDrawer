using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoronoiStruct;

namespace VoronoiDrawer
{
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
			L = double.MaxValue;
			if (siteEvents.Count + circleEvents.Count == 0)
			{
				return L;
			}
			Event nextEvent = null;
			nextEvent = siteEvents.Min();
			if (nextEvent == null || nextEvent.X >= circleEvents.Min()?.X)
				nextEvent = circleEvents.Min();
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
							edge.line.a = new VoronoiStruct.Point(point);
						}
						else
						{
							edge.line.b = new VoronoiStruct.Point(point);
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
							edge.line.a = new VoronoiStruct.Point(point);
						}
						else
						{
							edge.line.b = new VoronoiStruct.Point(point);
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
							edge.line.a = new VoronoiStruct.Point(point);
						}
						else
						{
							edge.line.b = new VoronoiStruct.Point(point);
						}
					}
				}
				Edge bar = new Edge(pi, pk);
				bar.line.a = new VoronoiStruct.Point(point);
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
			if (pj.focus.x == pi.focus.x)
			{
				// special case, first few point on same x pos
				if (pj.focus.y > pi.focus.y)
					beachPolys.Insert(pos, pi);
				else
					beachPolys.Insert(pos + 1, pi);
				var bar = new Edge(pi.id, pj.id);
				bar.line.a = new VoronoiStruct.Point(-vmap.width, (pj.focus.y + pi.focus.y) / 2);
				pj.edges.Add(new Edge(bar));
				pi.edges.Add(new Edge(bar));
				return;
			}
			else if (beachPolys.Count == 2)
			{ // special case, on third point insertion if first two point is on same x pos
				if (pi.focus.y < beachPolys[0].focus.y)
					pos = 0;
				else if (pi.focus.y > beachPolys[0].focus.y && pi.focus.y < beachPolys[1].focus.y)
					if (pi.focus.y >= getIntersect(beachPolys[0].focus, beachPolys[1].focus).Y)
					{
						pos = 1;
					}
					else
					{
						pos = 0;
					}
				else if (pi.focus.y > beachPolys[1].focus.y)
					pos = 1;
				pj = beachPolys[pos];
			}
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
			pi.edges.Add(new Edge(pi.id, pj.id, true));
			pj.edges.Add(new Edge(pi.id, pj.id, true));

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
						edge.line.b = new VoronoiStruct.Point(cross);
					}
				}
				foreach (var edge in p2.edges)
				{
					if (edge.getParent().Contains(p1.id))
					{
						edge.line.b = new VoronoiStruct.Point(cross);
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

		private PointF getIntersect(VoronoiStruct.Point A, VoronoiStruct.Point B)
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
					return new PointF(float.MinValue, (A.y + B.y) / 2);
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

	class Event : IComparable
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
			// find max radius to avoid errors
			double r = Math.Max(calculator.distance(r1.focus, center), calculator.distance(r2.focus, center));
			r = Math.Max(r, calculator.distance(r3.focus, center));
			this.X = center.X + r;
			isCircle = true;
		}
		public Event(Event old)
		{
			this.relevant = new List<Polygon>(old.relevant);
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
			if (this.X < ((Event)obj).X)
				return -1;
			else if (this.X == ((Event)obj).X)
				return 0;
			else
				return 1;
		}
	}

	class Arc
	{
		public Arc(Polygon parent)
		{
			this.focus = parent.focus;
			this.parent = parent;
		}
		public Arc(Arc old, bool withEvent = true)
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
		public PointF getIntersect(Arc obj, double L)
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
		// 向量oa與向量ob進行叉積，判斷oa到ob的旋轉方向。
		public static double cross(VoronoiStruct.Point o, VoronoiStruct.Point a, VoronoiStruct.Point b)
		{
			return (double)(a.x - o.x) * (b.y - o.y) - (double)(a.y - o.y) * (b.x - o.x);
		}
		// 點與線段已確定共線，判斷相交。
		public static bool intersect(VoronoiStruct.Point p1, VoronoiStruct.Point p2, VoronoiStruct.Point p)
		{
			return p.x >= Math.Min(p1.x, p2.x)
				&& p.x <= Math.Max(p1.x, p2.x)
				&& p.y >= Math.Min(p1.y, p2.y)
				&& p.y <= Math.Max(p1.y, p2.y);
		}
		public static bool intersect(VoronoiStruct.Point a1, VoronoiStruct.Point a2, VoronoiStruct.Point b1, VoronoiStruct.Point b2)
		{
			double c1 = cross(a1, a2, b1);
			double c2 = cross(a1, a2, b2);
			double c3 = cross(b1, b2, a1);
			double c4 = cross(b1, b2, a2);
			// 端點不共線
			if (c1 * c2 < 0 && c3 * c4 < 0) return true;
			// 端點共線
			if (c1 == 0 && intersect(a1, a2, b1)) return true;
			if (c2 == 0 && intersect(a1, a2, b2)) return true;
			if (c3 == 0 && intersect(b1, b2, a1)) return true;
			if (c4 == 0 && intersect(b1, b2, a2)) return true;
			return false;
		}
		public static bool intersect(Line a, Line b)
		{
			return intersect(a.a, a.b, b.a, b.b);
		}
		public static bool intersect(Rectangle regin, Line line)
		{
			Line top = new Line(regin.Left, regin.Top, regin.Right, regin.Top);
			Line bottom = new Line(regin.Left, regin.Bottom, regin.Right, regin.Bottom);
			Line left = new Line(regin.Left, regin.Top, regin.Left, regin.Bottom);
			Line right = new Line(regin.Right, regin.Top, regin.Right, regin.Bottom);
			return intersect(top, line) || intersect(bottom, line)
				|| intersect(left, line) || intersect(right, line);
		}
	}
}
