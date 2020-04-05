using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG_3
{
    public class ImageWrapper : IDisposable, IEnumerable<Point>
    {
        public int Width { get; private set; }

        public int Height { get; private set; }

        public Color DefaultColor { get; set; }

        private byte[] data;
        private byte[] outData;
        private int stride;
        private BitmapData bmpData;
        private Bitmap bmp;

        public ImageWrapper(Bitmap bmp, bool copySourceToOutput = false)
        {
            Width = bmp.Width;
            Height = bmp.Height;
            this.bmp = bmp;

            bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            stride = bmpData.Stride;

            data = new byte[stride * Height];
            System.Runtime.InteropServices.Marshal.Copy(bmpData.Scan0, data, 0, data.Length);

            outData = copySourceToOutput ? (byte[])data.Clone() : new byte[stride * Height];
        }

        public Color this[int x, int y]
        {
            get
            {
                var i = GetIndex(x, y);
                return i < 0 ? DefaultColor : Color.FromArgb(data[i + 3], data[i + 2], data[i + 1], data[i]);
            }

            set
            {
                var i = GetIndex(x, y);
                if (i >= 0)
                {
                    outData[i] = value.B;
                    outData[i + 1] = value.G;
                    outData[i + 2] = value.R;
                    outData[i + 3] = value.A;
                };
            }
        }

        public Color this[Point p]
        {
            get { return this[p.X, p.Y]; }
            set { this[p.X, p.Y] = value; }
        }

        public void SetPixel(Point p, double r, double g, double b)
        {
            if (r < 0) r = 0;
            if (r >= 256) r = 255;
            if (g < 0) g = 0;
            if (g >= 256) g = 255;
            if (b < 0) b = 0;
            if (b >= 256) b = 255;

            this[p.X, p.Y] = Color.FromArgb((int)r, (int)g, (int)b);
        }

        int GetIndex(int x, int y)
        {
            return (x < 0 || x >= Width || y < 0 || y >= Height) ? -1 : x * 4 + y * stride;
        }

        public void Dispose()
        {
            System.Runtime.InteropServices.Marshal.Copy(outData, 0, bmpData.Scan0, outData.Length);
            bmp.UnlockBits(bmpData);
        }

        public IEnumerator<Point> GetEnumerator()
        {
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    yield return new Point(x, y);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void SwapBuffers()
        {
            var temp = data;
            data = outData;
            outData = temp;
        }
    }
}
