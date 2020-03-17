using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using DmitryBrant.ImageFormats;

namespace CG_2
{
    public partial class Form1 : Form
    {
        readonly List<ImageInfo> image_info_list = new List<ImageInfo>();

        public Form1()
        {
            InitializeComponent();
        }


        private static short GetCompressionType(Image image)
        {
            var compressionTagIndex = Array.IndexOf(image.PropertyIdList, 0x103);
            var compressionTag = image.PropertyItems[compressionTagIndex];
            return BitConverter.ToInt16(compressionTag.Value, 0);
        }

        readonly Dictionary<long, string> CompressType = new Dictionary<long, string>()
        {
            {1, "Uncompressed" } ,
            {2, "CCITT modified Huffman RLE"},
            {32773, "PackBits"},
            {3, "CCITT3"},
            {4, "CCITT4"},
            {5, "LZW"},
            {6, "JPEG_old"},
            {7, "JPEG_new"},
            {32946, "DeflatePKZIP"},
            {8, "DeflateAdobe"},
            {9, "JBIG_85"},
            {10, "JBIG_43"},
            {11, "JPEG"},
            {12, "JPEG"},
            {32766, "RLE_NeXT"},
            {32809, "RLE_ThunderScan"},
            {32895, "RasterPadding"},
            {32896, "RLE_LW"},
            {32897, "RLE_HC"},
            {32947, "RLE_BL"},
            {34661, "JBIG"},
            {34713, "Nikon_NEF"},
            {34712,"JPEG2000"}
        };

        private void ViewInfo(string[] files)
        {
            foreach (string s in files)
            {
                var str = "";
                var fileInfo = new FileInfo(s);
                Image image;

                if (fileInfo.Extension.ToLower() == ".pcx")
                {
                    image = PcxReader.Load(s);
                }
                else
                {
                    image = Image.FromFile(s);
                }

                var res = string.Empty;
                if (fileInfo.Extension.ToLower() == ".tif")
                {
                    var temp_res = GetCompressionType(image);
                    var final_res = temp_res < 0 ? 3 : temp_res;
                    res = CompressType[final_res];
                }
                else if (fileInfo.Extension.ToLower() == ".jpg")
                {
                    res = "Baseline";
                }
                else if (fileInfo.Extension.ToLower() == ".png")
                {
                    res = "Deflate";
                }
                else if (fileInfo.Extension.ToLower() == ".png")
                {
                    res = "None";
                }

                var size = image.Height * image.Width;

                string pixelFormat = Image.GetPixelFormatSize(image.PixelFormat).ToString();
                image_info_list.Add(new ImageInfo(fileInfo.Name, size, Convert.ToInt32(image.HorizontalResolution), pixelFormat, res));
                image.Dispose();
            }
        }

        private void ViewInfoParallel(string s)
        {
            var str = "";
            var fileInfo = new FileInfo(s);
            Image image;

            if (fileInfo.Extension.ToLower() == ".pcx")
            {
                image = PcxReader.Load(s);
            }
            else
            {
                image = Image.FromFile(s);
            }

            var res = string.Empty;
            if (fileInfo.Extension.ToLower() == ".tif")
            {
                var temp_res = GetCompressionType(image);
                var final_res = temp_res < 0 ? 3 : temp_res;
                res = CompressType[final_res];
            }
            else if (fileInfo.Extension.ToLower() == ".jpg")
            {
                res = "Baseline";
            }
            else if (fileInfo.Extension.ToLower() == ".png")
            {
                res = "Deflate";
            }
            else if (fileInfo.Extension.ToLower() == ".png")
            {
                res = "None";
            }

            var size = image.Height * image.Width;

            string pixelFormat = Image.GetPixelFormatSize(image.PixelFormat).ToString();
            image_info_list.Add(new ImageInfo(fileInfo.Name, size, Convert.ToInt32(image.HorizontalResolution), pixelFormat, res));
            image.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var op = new FolderBrowserDialog();
            var dirName = string.Empty;
            var sw = new Stopwatch();

            image_info_list.Clear();

            if (op.ShowDialog(this) == DialogResult.OK)
            {
                dirName = op.SelectedPath;
                sw.Start();
            }

            if (Directory.Exists(dirName))
            {
                var files = Directory.GetFiles(dirName);
                Parallel.ForEach(files, ViewInfoParallel);
            }
            
            dataGridView1.DataSource = image_info_list;

            sw.Stop();

            label1.Text = sw.Elapsed.ToString();
        }
    }
}
