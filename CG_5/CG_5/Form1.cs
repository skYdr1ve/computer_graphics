using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CG_5
{
    public partial class Form1 : Form
    {
        private Bitmap _sBitmap;
        private List<Segment> _lines;
        private Rectangle _rectangle;
        private List<Point[]> _polygons;
        private int _flag = 0;

        public Form1()
        {
            _sBitmap = new Bitmap(600, 600);
            _lines = new List<Segment>();
            _polygons = new List<Point[]>();
            InitializeComponent();
            comboBox1.Items.Add("Lines");
            comboBox1.Items.Add("Polygons");
            DrawCoordinateSystem();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClearArea();

            var op = new OpenFileDialog();

            if (op.ShowDialog(this) == DialogResult.Cancel)
            {
                return;
            }

            var path = op.FileName;

            var g = Graphics.FromImage(_sBitmap);
            var myPen = new Pen(System.Drawing.Color.White, 3);

            string line;
            var size = 0;

            using (var sr = new StreamReader(path, Encoding.Default))
            {
                switch (comboBox1.SelectedItem)
                {
                    case "Lines":
                    {
                        _flag = 1;
                        if ((line = sr.ReadLine()) != null)
                        {
                            size = Convert.ToInt32(line);
                        }
                        string[] paramsList = null;
                        for (var i = 0; i < size; i++)
                        {
                            line = sr.ReadLine();
                            paramsList = line?.Split();
                            _lines.Add(new Segment(Convert.ToInt32(paramsList[0]), Convert.ToInt32(paramsList[1]), Convert.ToInt32(paramsList[2]), Convert.ToInt32(paramsList[3])));
                        }
                        line = sr.ReadLine();
                        paramsList = line?.Split();
                        _rectangle = new Rectangle(Convert.ToInt32(paramsList[0]), Convert.ToInt32(paramsList[1]),
                            Convert.ToInt32(paramsList[2]), Convert.ToInt32(paramsList[3]));
                        g.DrawRectangle(new Pen(Color.LawnGreen, 5), _rectangle);
                        foreach (var i in _lines)
                        {
                            g.DrawLine(myPen, i.X1, i.Y1, i.X2, i.Y2);
                        }

                        break;
                    }
                    case "Polygons":
                    {
                        _flag = 2;
                        while ((line = sr.ReadLine()) != null)
                        {
                            size = Convert.ToInt32(line);
                            var points = new Point[size];
                            for (var i = 0; i < size; i++)
                            {
                                line = sr.ReadLine();
                                var paramsList = line?.Split();
                                points[i] = new Point(Convert.ToInt32(paramsList[0]), Convert.ToInt32(paramsList[1]));
                            }
                            _polygons.Add(points);
                            g.DrawPolygon(myPen, points);
                        }

                        break;
                    }
                    default:
                        return;
                }
            }

            pictureBox1.Image = _sBitmap;
        }

        private void MiddlePointClipping(Segment segment, Graphics g)
        {
            if (Math.Sqrt(Math.Pow(segment.X2-segment.X1,2)+ Math.Pow(segment.Y2 - segment.Y1, 2)) <= 2)
            {
                return;
            }

            if (!IntersectionRectangle(segment) && !InSide(segment))
            {
                return;   
            }

            if (InSide(segment))
            {
                g.DrawLine(new Pen(Color.Blue, 5), segment.X1, segment.Y1, segment.X2, segment.Y2);
                return;
            }

            MiddlePointClipping(
                new Segment(segment.X1, segment.Y1, (segment.X1 + segment.X2) / 2, (segment.Y1 + segment.Y2) / 2), g);
            MiddlePointClipping(
                new Segment((segment.X1 + segment.X2) / 2, (segment.Y1 + segment.Y2) / 2, segment.X2, segment.Y2), g);
        }

        private bool IntersectionRectangle(Segment segment)
        {
            float x = _rectangle.X;
            float y = _rectangle.Y;
            float sx1 = segment.X1;
            float sy1 = segment.Y1;
            float sx2 = segment.X2;
            float sy2 = segment.Y2;

            var p = Intersection(new PointF(x, y), new PointF(x + _rectangle.Width, y), new PointF(sx1, sy1),
                new PointF(sx2, sy2));

            if (PointInSide(p, new Segment(sx1, sy1, sx2, sy2), new Segment(x, y, x + _rectangle.Width, y)))
            {
                return true;
            }

            p = Intersection(new PointF(x + _rectangle.Width, y), new PointF(x + _rectangle.Width, y + _rectangle.Height), new PointF(sx1, sy1),
                new PointF(sx2, sy2));

            if (PointInSide(p, new Segment(sx1, sy1, sx2, sy2), new Segment(x + _rectangle.Width, y, x + _rectangle.Width, y + _rectangle.Height)))
            {
                return true;
            }

            p = Intersection(new PointF(x, y + _rectangle.Height), new PointF(x + _rectangle.Width, y + _rectangle.Height), new PointF(sx1, sy1),
                new PointF(sx2, sy2));

            if (PointInSide(p, new Segment(sx1, sy1, sx2, sy2), new Segment(x, y + _rectangle.Height, x + _rectangle.Width, y + _rectangle.Height)))
            {
                return true;
            }

            p = Intersection(new PointF(x, y), new PointF(x, y + _rectangle.Height), new PointF(sx1, sy1),
                new PointF(sx2, sy2));

            if (PointInSide(p, new Segment(sx1, sy1, sx2, sy2), new Segment(x, y, x, y + _rectangle.Height)))
            {
                return true;
            }

            return false;
        }

        PointF Intersection(PointF s1, PointF e1, PointF s2, PointF e2)
        {
            var a1 = e1.Y - s1.Y;
            var b1 = s1.X - e1.X;
            var c1 = a1 * s1.X + b1 * s1.Y;

            var a2 = e2.Y - s2.Y;
            var b2 = s2.X - e2.X;
            var c2 = a2 * s2.X + b2 * s2.Y;

            var delta = a1 * b2 - a2 * b1;

            return delta == 0 ? new PointF(float.NaN, float.NaN)
                : new PointF((b2 * c1 - b1 * c2) / delta, (a1 * c2 - a2 * c1) / delta);
        }

        private static bool PointInSide(PointF point, Segment segment1, Segment segment2)
        {
            if (!(point.X >= segment1.X1 && point.Y >= segment1.Y1 && point.X <= segment1.X2 && point.Y <= segment1.Y2))
            {
                return false;
            }

            if (!(point.X >= segment2.X1 && point.Y >= segment2.Y2 && point.X <= segment2.X2 && point.Y <= segment2.Y2))
            {
                return false;
            }

            return true;
        }

        private bool InSide(Segment segment)
        {
            var x1 = _rectangle.X;
            var y1 = _rectangle.Y;
            var x2 = _rectangle.X + _rectangle.Width;
            var y2 = _rectangle.Y + _rectangle.Height;

            if (!(x1 < segment.X1 && y1 < segment.Y1 && x2 > segment.X1 && y2 > segment.Y1))
            {
                return false;
            }

            if (!(x1 < segment.X2 && y1 < segment.Y2 && x2 > segment.X2 && y2 > segment.Y2))
            {
                return false;
            }

            return true;
        }
        private void ClearArea()
        {
            _flag = 0;
            _sBitmap = new Bitmap(600, 600);
            _lines = new List<Segment>();
            _rectangle = new Rectangle();
            _polygons = new List<Point[]>();
            DrawCoordinateSystem();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (_flag==0 || _flag == 2)
            {
                return;
            }
            var g = Graphics.FromImage(_sBitmap);
            foreach (var i in _lines)
            {
                MiddlePointClipping(i, g);
            }
            pictureBox1.Image = _sBitmap;
        }

        private class Edge
        {
            public Edge(Point from, Point to)
            {
                this.From = from;
                this.To = to;
            }

            public readonly Point From;
            public readonly Point To;
        }

        private Point[] SutherlandHodgman(IReadOnlyCollection<Point> subjectPoly, Point[] clipPoly)
        {
            if (subjectPoly.Count < 3 || clipPoly.Length < 3)
            {
                throw new ArgumentException(
                    $"The polygons passed in must have at least 3 points: subject={subjectPoly.Count.ToString()}, clip={clipPoly.Length.ToString()}");
            }

            var outputList = subjectPoly.ToList();

            foreach (var clipEdge in IterateEdgesClockwise(clipPoly))
            {
                var inputList = outputList.ToList();
                outputList.Clear();

                if (inputList.Count == 0)
                {
                    break;
                }

                var S = inputList[inputList.Count - 1];

                foreach (var E in inputList)
                {
                    if (IsInside(clipEdge, E))
                    {
                        if (!IsInside(clipEdge, S))
                        {
                            var point = GetIntersect(S, E, clipEdge.From, clipEdge.To);
                            if (point == null)
                            {
                                throw new ApplicationException("Line segments don't intersect");
                            }
                            else
                            {
                                outputList.Add(point.Value);
                            }
                        }

                        outputList.Add(E);
                    }
                    else if (IsInside(clipEdge, S))
                    {
                        var point = GetIntersect(S, E, clipEdge.From, clipEdge.To);
                        if (point == null)
                        {
                            throw new ApplicationException("Line segments don't intersect");
                        }
                        else
                        {
                            outputList.Add(point.Value);
                        }
                    }

                    S = E;
                }
            }

            return outputList.ToArray();
        }

        private static bool IsInside(Edge edge, Point test)
        {
            var isLeft = IsLeftOf(edge, test);
            if (isLeft == null)
            {
                return true;
            }

            return !isLeft.Value;
        }

        private static bool? IsLeftOf(Edge edge, Point test)
        {
            var tmp1 = new Point(edge.To.X - edge.From.X, edge.To.Y - edge.From.Y);
            var tmp2 = new Point(test.X - edge.To.X, test.Y - edge.To.Y);

            double x = (tmp1.X * tmp2.Y) - (tmp1.Y * tmp2.X); 

            if (x < 0)
            {
                return false;
            }
            else if (x > 0)
            {
                return true;
            }
            else
            {
                return null;
            }
        }

        private static Point? GetIntersect(Point line1From, Point line1To, Point line2From, Point line2To)
        {
            var direction1 = new Point(line1To.X - line1From.X, line1To.Y - line1From.Y);
            var direction2 = new Point(line2To.X - line2From.X, line2To.Y - line2From.Y);
            double dotPerp = (direction1.X * direction2.Y) - (direction1.Y * direction2.X);

            if (IsNearZero(dotPerp))
            {
                return null;
            }

            var c = new Point(line2From.X - line1From.X, line2From.Y - line1From.Y);
            var t = (c.X * direction2.Y - c.Y * direction2.X) / dotPerp;
            return new Point((int)(line1From.X + (t * direction1.X)), (int)(line1From.Y + (t * direction1.Y)));
        }

        private static bool IsNearZero(double testValue)
        {
            return Math.Abs(testValue) <= .000000001d;
        }

        private static IEnumerable<Edge> IterateEdgesClockwise(IReadOnlyList<Point> polygon)
        {
            if (IsClockwise(polygon))
            {
                #region Already clockwise

                for (int cntr = 0; cntr < polygon.Count - 1; cntr++)
                {
                    yield return new Edge(polygon[cntr], polygon[cntr + 1]);
                }

                yield return new Edge(polygon[polygon.Count - 1], polygon[0]);

                #endregion
            }
            else
            {
                #region Reverse

                for (int cntr = polygon.Count - 1; cntr > 0; cntr--)
                {
                    yield return new Edge(polygon[cntr], polygon[cntr - 1]);
                }

                yield return new Edge(polygon[0], polygon[polygon.Count - 1]);

                #endregion
            }
        }

        private static bool IsClockwise(IReadOnlyList<Point> polygon)
        {
            for (int cntr = 2; cntr < polygon.Count; cntr++)
            {
                bool? isLeft = IsLeftOf(new Edge(polygon[0], polygon[1]), polygon[cntr]);
                if (isLeft != null)		//	some of the points may be colinear.  That's ok as long as the overall is a polygon
                {
                    return !isLeft.Value;
                }
            }

            throw new ArgumentException("All the points in the polygon are colinear");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (_flag == 0 || _flag == 1)
            {
                return;
            }
            var g = Graphics.FromImage(_sBitmap);

            foreach (var i in _polygons.TakeWhile(i => i != _polygons.Last()))
            {
                var points = SutherlandHodgman(_polygons.Last(), i);
                g.DrawPolygon(new Pen(Color.Blue, 5), points);
            }

            pictureBox1.Image = _sBitmap;
        }

        private void DrawCoordinateSystem()
        {
            var g = Graphics.FromImage(_sBitmap);
            var pen = new Pen(Color.FromArgb(15, 187, 217), 1);
            for (var i = 0; i < _sBitmap.Height; i += 50)
            {
                g.DrawLine(pen, i, 0, i, _sBitmap.Height);
                g.DrawLine(pen, 0, i, _sBitmap.Width, i);
                using (var myFont = new Font("Calibri", 8))
                {
                    g.DrawString($"{i}", myFont, new SolidBrush(Color.FromArgb(15, 187, 217)) , new Point(i, 0));
                    g.DrawString($"{i}", myFont, new SolidBrush(Color.FromArgb(15, 187, 217)), new Point(0, i));
                }
            }

            pictureBox1.Image = _sBitmap;
        }
    }
}
