using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;


namespace size_of
{
    public partial class Form1 : Form
    {
        const int n = 5;
        int[] E = { 1, 5, 7, 35, 101};
        string path = "image.png";
        double[] X = new double[n];
        double[] Y = new double[n];
        int h;
        int w;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Bitmap img = new Bitmap(path);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.Image = img;
            h = img.Height;
            w = img.Width;
            textBox1.Text = h.ToString() + "*" + w.ToString();
            textBox2.Text = capacitiveDimension().ToString();
        }

        private double capacitiveDimension()
        {
            int b = 0;
            Image<Gray, Byte> img = new Image<Gray, Byte>(path);
            for (int i = 0; i < n; i++) 
            {
                X[i] = Math.Log(1.0 / E[i]);
                Y[i] = Math.Log(Ne(img, E[i]));
            }

            return LS();
        }

        private int Ne(Image<Gray, Byte> img, int e)
        {
            int res = 0;
            bool hasBlack;
            for (int i = 0; i < w; i += e) 
            {
                for (int j = 0; j < h; j += e) 
                {
                    hasBlack = false;
                    for (int x = 0; x < e && !hasBlack && i + x < w; x++) 
                    {
                        for (int y = 0; y < e && !hasBlack && j + y < h; y++) 
                        {
                            if (img.Data[i + x, j + y, 0] < 127) 
                            {
                                hasBlack = true;
                            }
                        }
                    }
                    if (hasBlack)
                    {
                        res++;
                    }
                }
            }
            return res;
        }
        private double LS()
        {
            double sumx = 0;
            double sumy = 0;
            double sumx2 = 0;
            double sumxy = 0;

            for (int i = 0; i < n; i++)
            {
                sumx += X[i];
                sumy += Y[i];
                sumx2 += X[i] * X[i];
                sumxy += X[i] * Y[i];
            }
            sumx = sumx / n;
            sumy = sumy / n;
            sumx2 = sumx2 / n;
            sumxy = sumxy / n;
            return (sumxy - sumx * sumy) / (sumx2 - sumx * sumx);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }

}
