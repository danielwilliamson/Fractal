using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment_1
{
    public partial class Fractal : Form
    {
        private const int MAX = 256;      // max iterations
        private const double SX = -2.025; // start value real
        private const double SY = -1.125; // start value imaginary
        private const double EX = 0.6;    // end value real
        private const double EY = 1.125;  // end value imaginary
        private static int x1, y1, xs, ys, xe, ye;
        private static double xstart, ystart, xende, yende, xzoom, yzoom;
        private static bool action, rectangle, finished, changegrey, change, reset;
        private static float xy;
        //private Image picture;
        Graphics g1;


        private Cursor c1, c2;
        Pen whitePen = new Pen(Color.FromArgb(255, 255, 255, 255), 1);
        private HSB HSBcol = new HSB();


        public void init() // all instances will be prepared
        {

            HSBcol = new HSB();
            this.Size = new System.Drawing.Size(640, 550);
            finished = false;
            x1 = this.Width;
            y1 = this.Height;
            xy = (float)x1 / (float)y1;
            pictureBox1.Image = new Bitmap(640, 480); //create new Bitmap image to display
            pictureBox1.Visible = false;
            g1 = Graphics.FromImage(pictureBox1.Image);//get image from picturbox1 and set the g1 graphics to the image
            finished = true;


        }

        public void start()
        {

            action = false;
            rectangle = false;
            initvalues();
            xzoom = (xende - xstart) / (double)x1;
            yzoom = (yende - ystart) / (double)y1;
            mandelbrot();
        }
        //HSB Class
        #region
        class HSB
        {//djm added, it makes it simpler to have this code in here than in the C#
            public float rChan, gChan, bChan;
            public HSB()
            {
                rChan = gChan = bChan = 0;
            }
            public void fromHSB(float h, float s, float b)
            {
                float red = b;
                float green = b;
                float blue = b;
                if (s != 0)
                {
                    float max = b;
                    float dif = b * s / 255f;
                    float min = b - dif;

                    float h2 = h * 360f / 255f;

                    if (h2 < 60f)
                    {
                        red = max;
                        green = h2 * dif / 60f + min;
                        blue = min;
                    }
                    else if (h2 < 120f)
                    {
                        red = -(h2 - 120f) * dif / 60f + min;
                        green = max;
                        blue = min;
                    }
                    else if (h2 < 180f)
                    {
                        red = min;
                        green = max;
                        blue = (h2 - 120f) * dif / 60f + min;
                    }
                    else if (h2 < 240f)
                    {
                        red = min;
                        green = -(h2 - 240f) * dif / 60f + min;
                        blue = max;
                    }
                    else if (h2 < 300f)
                    {
                        red = (h2 - 240f) * dif / 60f + min;
                        green = min;
                        blue = max;
                    }
                    else if (h2 <= 360f)
                    {
                        red = max;
                        green = min;
                        blue = -(h2 - 360f) * dif / 60 + min;
                    }
                    else
                    {
                        red = 0;
                        green = 0;
                        blue = 0;
                    }
                }

                rChan = (float)Math.Round(Math.Min(Math.Max(red, 0f), 255));
                gChan = (float)Math.Round(Math.Min(Math.Max(green, 0), 255));
                bChan = (float)Math.Round(Math.Min(Math.Max(blue, 0), 255));

            }
        }
        #endregion
        //

        //HSBColor class
        #region
        public struct HSBColor
        {
            float h;
            float s;
            float b;
            int a;

            public HSBColor(float h, float s, float b)
            {
                this.a = 0xff;
                this.h = Math.Min(Math.Max(h, 0), 255);
                this.s = Math.Min(Math.Max(s, 0), 255);
                this.b = Math.Min(Math.Max(b, 0), 255);
            }

            public HSBColor(int a, float h, float s, float b)
            {
                this.a = a;
                this.h = Math.Min(Math.Max(h, 0), 255);
                this.s = Math.Min(Math.Max(s, 0), 255);
                this.b = Math.Min(Math.Max(b, 0), 255);
            }

            public float H
            {
                get { return h; }
            }

            public float S
            {
                get { return s; }
            }

            public float B
            {
                get { return b; }
            }

            public int A
            {
                get { return a; }
            }

            public Color Color
            {
                get
                {
                    return FromHSB(this);
                }
            }

            public static Color FromHSB(HSBColor hsbColor)
            {
                float r = hsbColor.b;
                float g = hsbColor.b;
                float b = hsbColor.b;
                if (hsbColor.s != 0)
                {
                    float max = hsbColor.b;
                    float dif = hsbColor.b * hsbColor.s / 255f;
                    float min = hsbColor.b - dif;

                    float h = hsbColor.h * 360f / 255f;

                    if (h < 60f)
                    {
                        r = max;
                        g = h * dif / 60f + min;
                        b = min;
                    }
                    else if (h < 120f)
                    {
                        r = -(h - 120f) * dif / 60f + min;
                        g = max;
                        b = min;
                    }
                    else if (h < 180f)
                    {
                        r = min;
                        g = max;
                        b = (h - 120f) * dif / 60f + min;
                    }
                    else if (h < 240f)
                    {
                        r = min;
                        g = -(h - 240f) * dif / 60f + min;
                        b = max;
                    }
                    else if (h < 300f)
                    {
                        r = (h - 240f) * dif / 60f + min;
                        g = min;
                        b = max;
                    }
                    else if (h <= 360f)
                    {
                        r = max;
                        g = min;
                        b = -(h - 360f) * dif / 60 + min;
                    }
                    else
                    {
                        r = 0;
                        g = 0;
                        b = 0;
                    }
                }

                return Color.FromArgb
                    (
                        hsbColor.a,
                        (int)Math.Round(Math.Min(Math.Max(r, 0), 255)),
                        (int)Math.Round(Math.Min(Math.Max(g, 0), 255)),
                        (int)Math.Round(Math.Min(Math.Max(b, 0), 255))
                        );
            }

        }
        #endregion
        //
        private void mandelbrot() // calculate all points
        {

            int x, y;

            float h, b, alt = 0.0f;



            action = false;

            for (x = 0; x < x1; x += 2)

                for (y = 0; y < y1; y++)
                {

                    h = pointcolour(xstart + xzoom * (double)x, ystart + yzoom * (double)y); // color value

                    if (h != alt)
                    {

                        b = 1.0f - h * h; // brightness

                        Color color = HSBColor.FromHSB(new HSBColor(h * trackBar3.Value, 0.8f * trackBar2.Value, b * trackBar1.Value));

                        Pen pen = new Pen(color);
                        g1.DrawLine(pen, x, y, x + 1, y);

                    }

                    if (reset) //reset button
                    {


                        b = 1.0f - h * h; // brightness

                        Color color = HSBColor.FromHSB(new HSBColor(h * 255, 0.8f * 255, b * 255));

                        Pen pen = new Pen(color);
                        g1.DrawLine(pen, x, y, x + 1, y);
                        trackBar1.Value = 255;
                        trackBar2.Value = 255;
                        trackBar3.Value = 255;
                        reset = false;

                    }


                }


            action = true;

        }



        private float pointcolour(double xwert, double ywert) // color value from 0.0 to 1.0 by iterations
        {
            double r = 0.0, i = 0.0, m = 0.0;
            int j = 0;

            while ((j < MAX) && (m < 4.0))
            {
                j++;
                m = r * r - i * i;
                i = 2.0 * r * i + ywert;
                r = m + xwert;
            }
            return (float)j / (float)MAX;
        }

        private void initvalues() // reset start values
        {
            xstart = SX;
            ystart = SY;
            xende = EX;
            yende = EY;
            if ((float)((xende - xstart) / (yende - ystart)) != xy)
                xstart = xende - (yende - ystart) * (double)xy;
        }

        public Fractal()
        {
            InitializeComponent();
            init();
            start();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        //drawing to the form & picturebox
        private void Fractal_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(pictureBox1.Image, 0, 54);

            Rectangle rect1 = new Rectangle(xs, ys, (xe - xs), (ye - ys));
            Rectangle rect2 = new Rectangle(xs, ye, (xe - xs), (ys - ye));
            Rectangle rect3 = new Rectangle(xe, ys, (xs - xe), (ye - ys));
            Rectangle rect4 = new Rectangle(xe, ye, (xs - xe), (ys - ye));
            if (rectangle)
            {

                if (xs < xe)
                {
                    if (ys < ye) e.Graphics.DrawRectangle(whitePen, rect1);
                    else e.Graphics.DrawRectangle(whitePen, rect2);
                }
                else
                {
                    if (ys < ye) e.Graphics.DrawRectangle(whitePen, rect3);
                    else e.Graphics.DrawRectangle(whitePen, rect4);
                }
            }


        }

        private void Fractal_Load(object sender, EventArgs e)
        {


        }
        //mouse controls
        private void Fractal_MouseDown(object sender, MouseEventArgs e)
        {

            if (action)
            {
                xs = e.X;
                ys = e.Y;
            }
        }

        private void Fractal_MouseUp(object sender, MouseEventArgs e)
        {
            int z, w;


            if (action)
            {
                xe = e.X;
                ye = e.Y;
                if (xs > xe)
                {
                    z = xs;
                    xs = xe;
                    xe = z;
                }
                if (ys > ye)
                {
                    z = ys;
                    ys = ye;
                    ye = z;
                }
                w = (xe - xs);
                z = (ye - ys);
                if ((w < 2) && (z < 2)) initvalues();
                else
                {
                    if (((float)w > (float)z * xy)) ye = (int)((float)ys + (float)w / xy);
                    else xe = (int)((float)xs + (float)z * xy);
                    xende = xstart + xzoom * (double)xe;
                    yende = ystart + yzoom * (double)ye;
                    xstart += xzoom * (double)xs;
                    ystart += yzoom * (double)ys;
                }
                xzoom = (xende - xstart) / (double)x1;
                yzoom = (yende - ystart) / (double)y1;
                mandelbrot();
                rectangle = false;

                this.Invalidate(); //update graphics
            }
        }




        private void Fractal_MouseMove(object sender, MouseEventArgs e)
        {
            if (action & e.Button == MouseButtons.Left)
            {
                xe = e.X;
                ye = e.Y;
                rectangle = true;
                this.Invalidate(); //update graphics
            }
        }
        //save to clipboard method
        private void Fractal_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.C)
            {

                Clipboard.SetImage(pictureBox1.Image);
            }
        }

        //trackbar H,S,B Updates to the fractal
        private void trackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            this.Invalidate(); //update graphics
            mandelbrot();


        }


        private void trackBar2_MouseUp(object sender, MouseEventArgs e)
        {
            this.Invalidate(); //update graphics
            mandelbrot();
        }

        private void trackBar3_MouseUp(object sender, MouseEventArgs e)
        {
            this.Invalidate(); //update graphics
            mandelbrot();

        }
        //


        // reset button click
        private void button3_MouseUp(object sender, MouseEventArgs e)
        {
            this.Invalidate(); //update graphics
            mandelbrot();
            reset = true;

        }

        private void button1_MouseUp(object sender, MouseEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Images|*.png;*.bmp;*.jpg";
            ImageFormat format = ImageFormat.Png;
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string ext = System.IO.Path.GetExtension(sfd.FileName);
                switch (ext)
                {
                    case ".jpg":
                        format = ImageFormat.Jpeg;
                        break;
                    case ".bmp":
                        format = ImageFormat.Bmp;
                        break;
                }
                pictureBox1.Image.Save(sfd.FileName, format);
            }
        }

        private void button2_MouseUp(object sender, MouseEventArgs e)
        {
            var form = new Form2();
            form.Show();
        }












    }
}

