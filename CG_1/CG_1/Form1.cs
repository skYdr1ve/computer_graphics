using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CG_1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        void PrintOut() {
            label1.Text = "Out of bounds";
        }

        double FtoXYZ(double x) {
            if (x >= 0.04045) {
                return Math.Pow(((x + 0.055) / 1.055), 2.4);
            }
            return x / 12.92;
        }

        double FtoRGB(double x)
        {
            if (x >= 0.0031308)
            {
                return 1.055 * Math.Pow(x, 1 / 2.4) - 0.055;
            }
            return x * 12.92;
        }

        double FtoLAB(double x) {
            if (x >= 0.008856)
            {
                return Math.Pow(x, 1 / 3d);
            }
            return 7.787 * x + 16d / 116;
        }

        double FLABtoXYZ(double x)
        {
            if (Math.Pow(x, 3) >= 0.008856)
            {
                return Math.Pow(x, 3);
            }
            return  (x - 16d / 116d) / 7.878;
        }
        
        double[] ConvertRGBToXYZ(int _red, int _green, int _blue) {
            var var_R = FtoXYZ(_red / 255d) * 100;
            var var_G = FtoXYZ(_green / 255d) * 100;
            var var_B = FtoXYZ(_blue / 255d) * 100;
            double X = var_R * 0.4124 + var_G * 0.3576 + var_B * 0.1805;
            double Y = var_R * 0.2126 + var_G * 0.7152 + var_B * 0.0722;
            double Z = var_R * 0.0193 + var_G * 0.1192 + var_B * 0.9505;
            double[] result = new double[] { Math.Round(X, 4), Math.Round(Y, 4), Math.Round(Z, 4)};
            return result;
        }

        int[] ConvertXYZToRGB(double X, double Y, double Z)
        {
            double var_X = X / 100;
            double var_Y = Y / 100;
            double var_Z = Z / 100;

            double var_R = var_X * 3.2406 + var_Y * -1.5372 + var_Z * -0.4986;
            double var_G = var_X * -0.9689 + var_Y * 1.8758 + var_Z * 0.0415;
            double var_B = var_X * 0.0557 + var_Y * -0.2040 + var_Z * 1.0570;
            int sR = Convert.ToInt32(FtoRGB(var_R) * 255);
            int sG = Convert.ToInt32(FtoRGB(var_G) * 255);
            int sB = Convert.ToInt32(FtoRGB(var_B) * 255);
            if (sR < 0) {
                sR = 0;
            }
            if (sG < 0) {
                sG = 0;
            }
            if (sB < 0) {
                sB = 0;
            }
            if (sR > 255) {
                sR = 255;
                PrintOut();
            }
            if (sG > 255)
            {
                sG = 255;
                PrintOut();
            }
            if (sB > 255)
            {
                sB = 255;
                PrintOut();
            }
            int[] result = new int[] { sR, sG, sB };
            return result;
        }

        int[] ConvertXYZtoLAB(double X, double Y, double Z) {
            double rX = 95.047d;
            double rY = 100d;
            double rZ = 108.883d;
            double var_X = X / rX;
            double var_Y = Y / rY;
            double var_Z = Z / rZ;

            Debug.WriteLine(var_X + " " + var_Y + " " + var_Z);

            int L = Convert.ToInt32(Math.Floor(116d * FtoLAB(var_Y) - 16d));
            int A = Convert.ToInt32(Math.Floor(500d * (FtoLAB(var_X) - FtoLAB(var_Y))));
            int B = Convert.ToInt32(Math.Floor(200d * (FtoLAB(var_Y) - FtoLAB(var_Z))));
            int[] result = new int[] { L, A, B };
            return result;
        }

        double[] ConvertLABtoXYZ(double L, double A, double B)
        {
            double rX = 95.047d;
            double rY = 100d;
            double rZ = 108.883d;

            double Y = FLABtoXYZ((L + 16) / 116d) * rY;
            double X = FLABtoXYZ(A / 500d + (L + 16) / 116d) * rX;
            double Z = FLABtoXYZ((L + 16) / 116d - B / 200d) * rZ;
            double[] result = new double[] { X, Y, Z };
            return result;
        }

        double[] CheackXYZ(double[] xyz)
        {
            bool flag = true;
            if (xyz[0] > 95)
            {
                xyz[0] = 95;
                PrintOut();
                flag = false;
            }
            else if (xyz[0] < 0)
            {
                xyz[0] = 0;
                PrintOut();
                flag = false;
            }
            if (xyz[1] > 100)
            {
                xyz[1] = 100;
                PrintOut();
                flag = false;
            }
            else if (xyz[1] < 0)
            {
                xyz[1] = 0;
                PrintOut();
                flag = false;
            }
            if (xyz[2] > 108)
            {
                xyz[2] = 108;
                PrintOut();
                flag = false;
            }
            else if (xyz[2] < 0)
            {
                xyz[2] = 0;
                PrintOut();
                flag = false;
            }
            if (flag)
            {
                label1.Text = string.Empty;
            }
            return xyz;
        }

        int[] CheackLab(int[] lab) {
            bool flag = true;
            if(lab[0] > 100)
            {
                lab[0] = 100;
                PrintOut();
                flag = false;
            }
            if (lab[1] > 128)
            {
                lab[1] = 128;
                PrintOut();
                flag = false;
            }
            else if (lab[1] < -128)
            {
                lab[1] = -128;
                PrintOut();
                flag = false;
            }
            if (lab[2] > 128)
            {
                lab[2] = 128;
                PrintOut();
                flag = false;
            }
            else if (lab[2] < -128)
            {
                lab[2] = -128;
                PrintOut();
                flag = false;
            }
            if (flag)
            {
                label1.Text = string.Empty;
            }
            return lab;
        }

        private void Display_rgb() {
            label1.Text = string.Empty;
            panel1.BackColor = Color.FromArgb(trackRed.Value, trackGreen.Value, trackBlue.Value);
            textBoxRed.Text = trackRed.Value.ToString();
            textBoxGreen.Text = trackGreen.Value.ToString();
            textBoxBlue.Text = trackBlue.Value.ToString();
            double[] XYZ = ConvertRGBToXYZ(trackRed.Value, trackGreen.Value, trackBlue.Value);
            buttonColorPicker.BackColor = panel1.BackColor;
            trackBarX.Value = Convert.ToInt32(Math.Floor(XYZ[0]));
            trackBarY.Value = Convert.ToInt32(Math.Floor(XYZ[1]));
            trackBarZ.Value = Convert.ToInt32(Math.Floor(XYZ[2]));
            textBoxX.Text = Convert.ToInt32(Math.Floor(XYZ[0])).ToString();
            textBoxY.Text = Convert.ToInt32(Math.Floor(XYZ[1])).ToString();
            textBoxZ.Text = Convert.ToInt32(Math.Floor(XYZ[2])).ToString();
            int[] lab = ConvertXYZtoLAB(XYZ[0], XYZ[1], XYZ[2]);
            trackBarL.Value = lab[0];
            trackBarA.Value = lab[1];
            trackBarB.Value = lab[2];
            textBoxL.Text = lab[0].ToString();
            textBoxA.Text = lab[1].ToString();
            textBoxB.Text = lab[2].ToString();
        }

        private void Display_xyz()
        {
            double x = trackBarX.Value;
            double y = trackBarY.Value;
            double z = trackBarZ.Value;
            textBoxX.Text = ((int)x).ToString();
            textBoxY.Text = ((int)y).ToString();
            textBoxZ.Text = ((int)z).ToString();
            int[] rgb = ConvertXYZToRGB(x, y, z);
            trackRed.Value = rgb[0];
            trackGreen.Value = rgb[1];
            trackBlue.Value = rgb[2];
            textBoxRed.Text = rgb[0].ToString();
            textBoxGreen.Text = rgb[1].ToString();
            textBoxBlue.Text = rgb[2].ToString(); 
            panel1.BackColor = Color.FromArgb(trackRed.Value, trackGreen.Value, trackBlue.Value);
            buttonColorPicker.BackColor = panel1.BackColor;
            int[] lab = ConvertXYZtoLAB(x, y, z);
            lab = CheackLab(lab);
            textBoxL.Text = lab[0].ToString();
            textBoxA.Text = lab[1].ToString();
            textBoxB.Text = lab[2].ToString();
            trackBarL.Value = lab[0];
            trackBarA.Value = lab[1];
            trackBarB.Value = lab[2];
        }

        private void Display_lab()
        {
            int l = trackBarL.Value;
            int a = trackBarA.Value;
            int b = trackBarB.Value;
            textBoxL.Text = l.ToString();
            textBoxA.Text = a.ToString();
            textBoxB.Text = b.ToString();
            double[] xyz = ConvertLABtoXYZ(l, a, b);
            
            int[] rgb = ConvertXYZToRGB(xyz[0], xyz[1], xyz[2]);
            textBoxRed.Text = rgb[0].ToString();
            textBoxGreen.Text = rgb[1].ToString();
            textBoxBlue.Text = rgb[2].ToString();
            trackRed.Value = rgb[0];
            trackGreen.Value = rgb[1];
            trackBlue.Value = rgb[2];
            xyz = CheackXYZ(xyz);
            xyz[0] = Math.Floor(xyz[0]);
            xyz[1] = Math.Floor(xyz[1]);
            xyz[2] = Math.Floor(xyz[2]);
            textBoxX.Text = xyz[0].ToString();
            textBoxY.Text = xyz[1].ToString();
            textBoxZ.Text = xyz[2].ToString();
            trackBarX.Value = (int)xyz[0];
            trackBarY.Value = (int)xyz[1];
            trackBarZ.Value = (int)xyz[2];
            panel1.BackColor = Color.FromArgb(trackRed.Value, trackGreen.Value, trackBlue.Value);
            buttonColorPicker.BackColor = panel1.BackColor;
        }

        private void trackRed_Scroll(object sender, EventArgs e)
        {
            Display_rgb();
        }

        private void trackGreen_Scroll(object sender, EventArgs e)
        {
            Display_rgb();
        }

        private void trackBlue_Scroll(object sender, EventArgs e)
        {
            Display_rgb();
        }

        private void trackBarX_Scroll(object sender, EventArgs e)
        {
            Display_xyz();
        }

        private void trackBarY_Scroll(object sender, EventArgs e)
        {
            Display_xyz();
        }

        private void trackBarZ_Scroll(object sender, EventArgs e)
        {
            Display_xyz();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK) {
                buttonColorPicker.BackColor = cd.Color;
                trackRed.Value = cd.Color.R;
                trackGreen.Value = cd.Color.G;
                trackBlue.Value = cd.Color.B;
                Display_rgb();
            }
        }

        private void buttonRGB_Click(object sender, EventArgs e)
        {
            int red_value;
            int green_value;
            int blue_value;
            try
            {
                red_value = Convert.ToInt32(textBoxRed.Text);
                green_value = Convert.ToInt32(textBoxGreen.Text);
                blue_value = Convert.ToInt32(textBoxBlue.Text);
            }
            catch (Exception ex)
            {
                label1.Text = "Wrong input";
                return;
            }
            if (red_value < 0 || red_value > 255 || green_value < 0 || green_value > 255 || blue_value < 0 || blue_value > 255)
            {
                label1.Text = "Wrong input";
                return;
            }
            else
            {
                label1.Text = string.Empty;
            }

            trackRed.Value = red_value;
            trackGreen.Value = green_value;
            trackBlue.Value = blue_value;

            Display_rgb();
       
        }

        private void buttonXYZ_Click(object sender, EventArgs e)
        {
            int X;
            int Y;
            int Z;
            try
            {
                X = Convert.ToInt32(textBoxX.Text);
                Y = Convert.ToInt32(textBoxY.Text);
                Z = Convert.ToInt32(textBoxZ.Text);
            }
            catch (Exception ex)
            {
                label1.Text = "Wrong input";
                return;
            }
            if (X < 0 || X > 95 || Y < 0 || Y > 100 || Z < 0 || Z > 108)
            {
                label1.Text = "Wrong input";
                return;
            }
            else
            {
                label1.Text = string.Empty;
            }
            trackBarX.Value = X;
            trackBarY.Value = Y;
            trackBarZ.Value = Z;

            Display_xyz();
        }

        private void trackBarL_Scroll(object sender, EventArgs e)
        {
            Display_lab();
        }

        private void trackBarA_Scroll(object sender, EventArgs e)
        {
            Display_lab();
        }

        private void trackBarB_Scroll(object sender, EventArgs e)
        {
            Display_lab();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int l;
            int a;
            int b;
            try
            {
                l = Convert.ToInt32(textBoxL.Text);
                a = Convert.ToInt32(textBoxA.Text);
                b = Convert.ToInt32(textBoxB.Text);
            }
            catch (Exception ex)
            {
                label1.Text = "Wrong input";
                return;
            }
            if (l < 0 || l > 100 || a < -128 || a > 128 || b < -128 || b > 128)
            {
                label1.Text = "Wrong input";
                return;
            }
            else {
                label1.Text = string.Empty;
            }

            trackBarL.Value = l;
            trackBarA.Value = a;
            trackBarB.Value = b;

            Display_lab();
        }
    }
}
