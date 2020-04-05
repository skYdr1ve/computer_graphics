using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace CG_3
{
    public partial class Form1 : Form
    {
        private Bitmap _sBitmap = null;

        public Form1()
        {
            InitializeComponent();
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            comboBox1.Items.Add("Filter 1");
            comboBox1.Items.Add("Filter 2");
            comboBox1.Items.Add("Filter 3");
            comboBox1.Items.Add("Filter 4");
            comboBox1.Items.Add("LoG"); 
        }

        public static List<int[]> imageHistogram(Bitmap input)
        {
            int[] rhistogram = new int[256];
            int[] ghistogram = new int[256];
            int[] bhistogram = new int[256];

            for (int i = 0; i < input.Width; i++)
            {
                for (int j = 0; j < input.Height; j++)
                {

                    int red = input.GetPixel(i, j).R;
                    int green = input.GetPixel(i, j).G;
                    int blue = input.GetPixel(i, j).B;

                    rhistogram[red]++; ghistogram[green]++; bhistogram[blue]++;
                }
            }

            List<int[]> hist = new List<int[]>();
            hist.Add(rhistogram);
            hist.Add(ghistogram);
            hist.Add(bhistogram);

            return hist;
        }

        public Bitmap changeContrast(Bitmap image)
        {
            Bitmap newImage = new Bitmap(image.Width, image.Height);
            List<int[]> imageHist = imageHistogram(image);

            var rhistogram = imageHist[0];
            var ghistogram = imageHist[1];
            var bhistogram = imageHist[2];

            var discard_ratio = 0.01;
            var hists = new int[3][];

            hists[0] = rhistogram;
            hists[1] = ghistogram;
            hists[2] = bhistogram;

            var total = image.Width * image.Height;

            var vmin = new int[3];
            var vmax = new int[3];

            for (var i = 0; i < 3; ++i)
            {
                for (var j = 0; j < 255; ++j)
                {
                    hists[i][j + 1] += hists[i][j];
                }
                vmin[i] = 0;
                vmax[i] = 255;
                while (hists[i][vmin[i]] < discard_ratio * total)
                    vmin[i] += 1;
                while (hists[i][vmax[i]] > (1 - discard_ratio) * total)
                    vmax[i] -= 1;
                if (vmax[i] < 255 - 1)
                    vmax[i] += 1;
            }


            for (var y = 0; y < image.Height; ++y)
            {
                for (var x = 0; x < image.Width; ++x)
                {
                    var rgbValues = new int[3];

                    for (var j = 0; j < 3; ++j)
                    {
                        var val = 0;
                        int red = image.GetPixel(x, y).R;
                        int green = image.GetPixel(x, y).G;
                        int blue = image.GetPixel(x, y).B;

                        if (j == 0) val = red;
                        if (j == 1) val = green;
                        if (j == 2) val = blue;

                        if (val < vmin[j])
                            val = vmin[j];
                        if (val > vmax[j])
                            val = vmax[j];

                        rgbValues[j] = (int)((val - vmin[j]) * 255.0 / (vmax[j] - vmin[j]));
                    }

                    int alpha = image.GetPixel(x, y).A;
                    newImage.SetPixel(x, y, Color.FromArgb(alpha, rgbValues[0], rgbValues[1], rgbValues[2]));
                }
            }

            return newImage;
        }

        public Bitmap equalize(Bitmap original)
        {
            var histLUT = histogramEqualizationLUT(original);
            var histogramEQ = new Bitmap(original.Width, original.Height);

            for (var i = 0; i < original.Height; i++)
            {
                for (var j = 0; j < original.Width; j++)
                {
                    int alpha = original.GetPixel(j, i).A;
                    int red = original.GetPixel(j, i).R;
                    int green = original.GetPixel(j, i).G;
                    int blue = original.GetPixel(j, i).B;

                    red = histLUT[0][red];
                    green = histLUT[1][green];
                    blue = histLUT[2][blue];

                    histogramEQ.SetPixel(j, i, Color.FromArgb(colorToRGB(alpha, red, green, blue)));
                }
            }

            return histogramEQ;
        }

        private static List<int[]> histogramEqualizationLUT(Bitmap input)
        {
            var imageHist = imageHistogram(input);

            var imageLUT = new List<int[]>();

            var rhistogram = new int[256];
            var ghistogram = new int[256];
            var bhistogram = new int[256];

            for (int i = 0; i < rhistogram.Length; i++)
            {
                rhistogram[i] = 0;
                ghistogram[i] = 0;
                bhistogram[i] = 0;
            }

            long sumr = 0;
            long sumg = 0;
            long sumb = 0;

            var scale_factor = (float)(255.0 / (input.Width * input.Height));

            for (int i = 0; i < rhistogram.Length; i++)
            {
                sumr += imageHist[0][i];
                int valr = (int)(sumr * scale_factor);
                if (valr > 255)
                {
                    rhistogram[i] = 255;
                }
                else rhistogram[i] = valr;

                sumg += imageHist[1][i];
                int valg = (int)(sumg * scale_factor);
                if (valg > 255)
                {
                    ghistogram[i] = 255;
                }
                else ghistogram[i] = valg;

                sumb += imageHist[2][i];
                int valb = (int)(sumb * scale_factor);
                if (valb > 255)
                {
                    bhistogram[i] = 255;
                }
                else bhistogram[i] = valb;
            }

            imageLUT.Add(rhistogram);
            imageLUT.Add(ghistogram);
            imageLUT.Add(bhistogram);

            return imageLUT;

        }

        public static int colorToRGB(int alpha, int red, int green, int blue)
        {
            return alpha << 24 | red << 16 | green << 8 | blue;
        }

        public static float[] getCumulative(float[] hists)
        {
            for (int i = 0; i < hists.Length - 1; ++i)
            {
                hists[i + 1] += hists[i];
            }
            return hists;
        }

        public static Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }

        public Bitmap equalizeHSV(Bitmap original)
        {
            var histogramEQ = new Bitmap(original.Width, original.Height);

            var pixels = new Color[original.Width * original.Height];
            for (var x = 0; x < original.Height; x++)
            {
                for (var y = 0; y < original.Width; y++)
                {
                    pixels[(x * original.Width) + y] = original.GetPixel(y,x);
                }
            }

            var hsbColors = new float[original.Width * original.Height, 3];
            var hist = new float[100];

            for (var i = 0; i < pixels.Length; i++)
            {
                var hsb = new float[] { pixels[i].GetHue(), pixels[i].GetSaturation(), pixels[i].GetBrightness()};
                hsbColors[i, 0] = hsb[0];
                hsbColors[i, 1] = hsb[1];
                hsbColors[i, 2] = hsb[2];
                ++hist[(int)(hsb[2] * (100 - 1))];
            }
            hist = getCumulative(hist);

            float minHist = 0;
            for (int i = 0; i < 100; ++i)
            {
                if (hist[i] != 0)
                {
                    minHist = hist[i];
                    break;
                }
            }
            for (int i = 0; i < 100; ++i)
            {
                hist[i] = (hist[i] - minHist) / (original.Width * original.Height - minHist);
            }

            for (int x = 0; x < original.Width; ++x)
            {
                for (int y = 0; y < original.Height; ++y)
                {
                    int alpha = original.GetPixel(x, y).A;
                    float[] hsb =
                    {
                        hsbColors[y * original.Width + x, 0], 
                        hsbColors[y * original.Width + x, 1],
                        hsbColors[y * original.Width + x, 2]
                    };
                    var newBrightness = hist[(int)(hsb[2] * (100 - 1))];
                    var newColor = ColorFromHSV(hsb[0], hsb[1], newBrightness);

                    histogramEQ.SetPixel(x, y, Color.FromArgb(colorToRGB(alpha, newColor.R, newColor.G, newColor.B)));
                }
            }

            return histogramEQ;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
        
            {
                return;
            }

            pictureBox2.Image = changeContrast((Bitmap)pictureBox2.Image);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                return;
            }

            pictureBox2.Image = equalize((Bitmap)pictureBox2.Image);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                return;
            }

            pictureBox2.Image = equalizeHSV((Bitmap)pictureBox2.Image);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                return;
            }

            double[,] kernel = null;

            switch (comboBox1.SelectedItem.ToString())
            {
                case "Filter 1":
                    kernel = new double[,]
                    {
                        {-1, -1, -1},
                        {-1, 9, -1},
                        {-1, -1, -1}
                    };
                    break;
                case "Filter 2":
                    kernel = new double[,]
                    {
                        {0, -1, 0},
                        {-1, 5, -1},
                        {0, -1, 0}
                    };
                    break;
                case "Filter 3":
                    kernel = new double[,]
                    {
                        {1, -2, 1},
                        {-2, 5, -2},
                        {1, -2, 1}
                    };
                    break;
                case "Filter 4":
                    kernel = new double[,]
                    {
                        {0, -1, 0},
                        {-1, 20, -1},
                        {0, -1, 0}
                    };
                    break;
                case "LoG":
                    kernel = new double[,]
                    {
                        {0, 0, -1, 0, 0},
                        {0, -1, -2, -1, 0},
                        {-1, -2, 16, -2, -1},
                        {0, -1, -2, -1, 0},
                        {0, 0, -1, 0, 0},
                    };
                    break;
            }

            var tBitmap = (Bitmap) pictureBox2.Image.Clone();
            Convolution(tBitmap, kernel);
            pictureBox2.Image = tBitmap;
        }

        private void Convolution(Bitmap img, double[,] matrix)
        {
            var w = matrix.GetLength(0);
            var h = matrix.GetLength(1);

            using (var wr = new ImageWrapper(img))
                foreach (var p in wr)
                {
                    double r = 0d, g = 0d, b = 0d;

                    for (int i = 0; i < w; i++)
                    for (int j = 0; j < h; j++)
                    {
                        var pixel = wr[p.X + i - 1, p.Y + j - 1];
                        r += matrix[j, i] * pixel.R;
                        g += matrix[j, i] * pixel.G;
                        b += matrix[j, i] * pixel.B;
                    }
                    wr.SetPixel(p, r, g, b);
                }
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var op = new OpenFileDialog();
            var fileName = string.Empty;

            if (op.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            fileName = op.FileName;
            _sBitmap = new Bitmap(Image.FromFile(fileName));
            pictureBox1.Image = (Image) _sBitmap.Clone();
            pictureBox2.Image = (Image) _sBitmap.Clone();
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                return;
            }
            pictureBox2.Image = (Image)_sBitmap.Clone();
        }

        private void saveToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (pictureBox2.Image == null)
            {
                return;
            }

            var saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif";
            saveFileDialog1.Title = "Save an Image File";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                System.IO.FileStream fs =
                    (System.IO.FileStream) saveFileDialog1.OpenFile();

                switch (saveFileDialog1.FilterIndex)
                {
                    case 1:
                        pictureBox2.Image.Save(fs, ImageFormat.Jpeg);
                        break;
                    case 2:
                        pictureBox2.Image.Save(fs, ImageFormat.Bmp);
                        break;
                    case 3:
                        pictureBox2.Image.Save(fs, ImageFormat.Gif);
                        break;
                }

                fs.Close();
            }
        }
    }
}
