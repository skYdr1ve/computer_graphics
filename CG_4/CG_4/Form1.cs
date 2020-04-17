using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace CG_4
{
    public partial class Form1 : Form
    {

        private Bitmap _sLessBitmap;
        private Bitmap _sMoreBitmap;
        private readonly Stopwatch _stopWatch = new Stopwatch();

        public Form1()
        {
            InitializeComponent();
            _sLessBitmap = new Bitmap(30, 30);
            _sMoreBitmap = new Bitmap(600, 600);
            InitGrid();
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            comboBox1.Items.Add("Step-by-step");
            comboBox1.Items.Add("DDA");
            comboBox1.Items.Add("Bresenham`s line");
            comboBox1.Items.Add("Bresenham`s circle");
        }

        private void InitGrid()
        {
            for (var y = 0; y < _sMoreBitmap.Height; y++)
            {
                for (var x = 0; x < _sMoreBitmap.Width; x++)
                {
                    if (x % 20 == 0 || y % 20 == 0)
                        _sMoreBitmap.SetPixel(x, y, Color.Black);
                }
            }

            pictureBox1.Image = _sMoreBitmap;
            pictureBox1.Invalidate();
        }

        private void ClearImage()
        {
            _sLessBitmap = new Bitmap(30, 30);
            _sMoreBitmap = new Bitmap(600, 600);
            InitGrid();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label7.Text = "Time(ns): 0.0";
            ClearImage();
            _stopWatch.Start();
            if (comboBox1.SelectedItem == "Step-by-step") 
            {
                pictureBox1.Image = StepByStep();
            }
            else if (comboBox1.SelectedItem == "DDA")
            {
                pictureBox1.Image = Dda();
            }
            else if (comboBox1.SelectedItem == "Bresenham`s line")
            {
                pictureBox1.Image = DrawLineBres();
            }
            else if (comboBox1.SelectedItem == "Bresenham`s circle")
            {
                pictureBox1.Image = CircleBres();
            }
            _stopWatch.Stop();
            label7.Text = $"Time(ns): {_stopWatch.Elapsed.Ticks}";
            _stopWatch.Reset();
        }

        public Bitmap FillBitmap()
        {
            var diffHeight = _sMoreBitmap.Height / _sLessBitmap.Height;
            var diffWidth = _sMoreBitmap.Width / _sLessBitmap.Width;
            for (var y = 0; y < _sLessBitmap.Height; y++)
            {
                for (var x = 0; x < _sLessBitmap.Width; x++)
                {
                    if (Color.Black.ToArgb() != _sLessBitmap.GetPixel(x, y).ToArgb()) continue;

                    for (var i = 0; i < diffHeight; i++)
                    {
                        for (var j = 0; j < diffWidth; j++)
                        {
                            _sMoreBitmap.SetPixel(x * diffHeight + i, y * diffWidth + j, Color.Black);
                        }
                    }
                }
            }

            return _sMoreBitmap;
        }

        private Bitmap StepByStep()
        {
            var x0 = Convert.ToInt32(textBox1.Text);
            var y0 = Convert.ToInt32(textBox2.Text);
            var x1 = Convert.ToInt32(textBox3.Text);
            var y1 = Convert.ToInt32(textBox4.Text);

            CheckArguments(ref x0, ref y0, ref x1, ref y1);

            int x, y;
            double k;
            if (Math.Abs(x1 - x0) >= Math.Abs(y1 - y0))
            {
                k = 1.0 * (y1 - y0) / (x1 - x0);
                for (x = Math.Min(x0, x1); x <= Math.Max(x0, x1); ++x)
                {
                    y = (int)Math.Round(k * (x - x0) + y0);
                    _sLessBitmap.SetPixel(x, y, Color.Black);
                }
            }
            else
            {
                k = 1.0 * (x1 - x0) / (y1 - y0);
                for (y = Math.Min(y0, y1); y <= Math.Max(y0, y1); ++y)
                {
                    x = (int)Math.Round(k * (y - y0) + x0);
                    _sLessBitmap.SetPixel(x, y, Color.Black);
                }
            }

            FillBitmap();

            return _sMoreBitmap;        
        }

        private Bitmap Dda()
        {
            var x0 = Convert.ToInt32(textBox1.Text);
            var y0 = Convert.ToInt32(textBox2.Text);
            var x1 = Convert.ToInt32(textBox3.Text);
            var y1 = Convert.ToInt32(textBox4.Text);

            CheckArguments(ref x0, ref y0, ref x1, ref y1);

            var dx = x1 - x0;
            var dy = y1 - y0;

            var steps = Math.Abs(dx) > Math.Abs(dy) ? Math.Abs(dx) : Math.Abs(dy);

            var xinc = dx / (float)steps;
            var yinc = dy / (float)steps;

            var x = (float)x0;
            var y = (float)y0;
            for (var i = 0; i <= steps; i++)
            {
                _sLessBitmap.SetPixel(Convert.ToInt32(x), Convert.ToInt32(y), Color.Black);
                x += xinc;
                y += yinc;
            }

            FillBitmap();

            return _sMoreBitmap;
        }

        private Bitmap DrawLineBres()
        {
            var x0 = Convert.ToInt32(textBox1.Text);
            var y0 = Convert.ToInt32(textBox2.Text);
            var x1 = Convert.ToInt32(textBox3.Text);
            var y1 = Convert.ToInt32(textBox4.Text);

            CheckArguments(ref x0, ref y0, ref x1, ref y1);

            int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
            int dy = Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
            int err = (dx > dy ? dx : -dy) / 2, e2;
            for (; ; )
            {
                _sLessBitmap.SetPixel(x0, y0, Color.Black);
                if (x0 == x1 && y0 == y1) break;
                e2 = err;
                if (e2 > -dx) { err -= dy; x0 += sx; }

                if (e2 >= dy) continue;
                err += dx; y0 += sy;
            }

            FillBitmap();

            return _sMoreBitmap;
        }

        private bool CheckBorder(int x, int y)
        {
            return x <= 29 && y <= 29 && x >= 0 && y >= 0;
        }

        private void DrawCircle(int xc, int yc, int x, int y)
        {
            if (CheckBorder(xc + x, yc + y))
            {
                _sLessBitmap.SetPixel(xc + x, yc + y, Color.Black);
            }
            if (CheckBorder(xc - x, yc + y))
            {
                _sLessBitmap.SetPixel(xc - x, yc + y, Color.Black);
            }
            if (CheckBorder(xc + x, yc - y))
            {
                _sLessBitmap.SetPixel(xc + x, yc - y, Color.Black);
            }
            if (CheckBorder(xc - x, yc - y))
            {
                _sLessBitmap.SetPixel(xc - x, yc - y, Color.Black);
            }
            if (CheckBorder(xc + y, yc + x))
            {
                _sLessBitmap.SetPixel(xc + y, yc + x, Color.Black);
            }
            if (CheckBorder(xc - y, yc + x))
            {
                _sLessBitmap.SetPixel(xc - y, yc + x, Color.Black);
            }
            if (CheckBorder(xc + y, yc - x))
            {
                _sLessBitmap.SetPixel(xc + y, yc - x, Color.Black);
            }
            if (CheckBorder(xc - y, yc - x))
            {
                _sLessBitmap.SetPixel(xc - y, yc - x, Color.Black);
            }
        }

        private Bitmap CircleBres()
        {
            var xc = Convert.ToInt32(textBox1.Text);
            var yc = Convert.ToInt32(textBox2.Text);
            var r = Convert.ToInt32(textBox6.Text);

            CheckArguments(ref xc, ref yc, ref xc, ref yc);

            int x = 0, y = r;
            var d = 3 - 2 * r;

            DrawCircle(xc, yc, x, y);

            while (y >= x)
            {
                x++;

                if (d > 0)
                {
                    y--;
                    d = d + 4 * (x - y) + 10;
                }
                else
                {
                    d = d + 4 * x + 6;
                }

                DrawCircle(xc, yc, x, y);
            }

            FillBitmap();

            return _sMoreBitmap;
        }

        private void CheckArguments(ref int x0, ref int y0, ref int x1, ref int y1)
        {
            if (x0 < 0)
            {
                x0 = 0;
            }
            else if (x0 > 29)
            {
                x0 = 29;
            }

            if (x1 < 0)
            {
                x1 = 0;
            }
            else if (x1 > 29)
            {
                x1 = 29;
            }

            if (y0 < 0)
            {
                y0 = 0;
            }
            else if (y1 > 29)
            {
                y1 = 29;
            }

            if (y1 < 0)
            {
                y1 = 0;
            }
            else if (y1 > 29)
            {
                y1 = 29;
            }
        }

    }
}
