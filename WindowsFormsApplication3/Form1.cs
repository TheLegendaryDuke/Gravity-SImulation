using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication3
{
    
    public partial class Form1 : Form
    {
        Star s1, s2;
        int clicked;
        double g;
        public Form1()
        {
            InitializeComponent();
            g = 6.674;
            timer1.Tick += tick;
        }
        private double findA(double x, double y)
        {
            if (x == 0)
            {
                if (y > 0)
                {
                    return Math.PI / 2;
                }
                else
                {
                    return -Math.PI / 2;
                }
            }
            if (y == 0)
            {
                if (y > 0)
                {
                    return 0;
                }
                else
                {
                    return Math.PI;
                }
            }
            if (x > 0 && y > 0)
            {
                return Math.Atan(y / x);
            }
            else if (x < 0)
            {
                return Math.PI + Math.Atan(y / x);
            }
            else
            {
                return Math.PI * 2 + Math.Atan(y / x);
            }
        }
        void tick(object sender, EventArgs e)
        {
            s1.x = s1.x + s1.sx;
            s1.y = s1.sy + s1.y;
            s2.x = s2.x + s2.sx;
            s2.y = s2.y + s2.sy;
            //1 pixel is set to 10^6 m
            double d = Math.Sqrt(Math.Pow(s2.x - s1.x, 2) + Math.Pow(s2.y - s1.y, 2));
            if (checkBox1.Checked)
            {
                if (d <= s1.r + s2.r)
                {
                    timer1.Stop();
                    return;
                }
            }
            //Fg in 10^23N
            double fg = g * s1.m * s2.m / d / d;
            double angle = findA(s2.x - s1.x, s2.y - s1.y);
            double fx = Math.Cos(angle) * fg;
            double fy = Math.Sin(angle) * fg;
            //acceleration in ms^-2
            double ax1 = fx / s1.m;
            double ay1 = fy / s1.m;
            double ax2 = -fx / s2.m;
            double ay2 = -fy / s2.m;
            //assume each tick is 10^6s
            s1.sx = (ax1 / 100 + s1.sx);
            s1.sy = (ay1 / 100 + s1.sy);
            s2.sx = (s2.sx + ax2 / 100);
            s2.sy = (s2.sy + ay2 / 100);
            pictureBox1.Invalidate();
        }
        private void draw(object sender, PaintEventArgs e)
        {
            int r1 = s1.r;
            int r2 = s2.r;
            e.Graphics.DrawEllipse(new Pen(Color.Red), (int)Math.Round(s1.x - r1), (int)Math.Round(s1.y - r1), r1 * 2, r1 * 2);
            e.Graphics.DrawEllipse(new Pen(Color.Blue), (int)Math.Round(s2.x - r2), (int)Math.Round(s2.y - r2), r2 * 2, r2 * 2);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //mass in 10^23 kg
            int m1 = Int32.Parse(textBox1.Text);
            int m2 = Int32.Parse(textBox4.Text);
            s1 = new Star(m1);
            s2 = new Star(m2);
            /*s1.x = s1.r;
            s1.y = s1.r;
            s2.x = pictureBox1.Size.Width;
            s2.y = pictureBox1.Size.Height;*/
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.draw);
            pictureBox1.Invalidate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            clicked = 1;
            pictureBox1.Click += pictureBox1_Click;
        }

        void pictureBox1_Click(object sender, EventArgs e)
        {
            MouseEventArgs ea = (MouseEventArgs)e;
            int x = ea.Location.X;
            int y = ea.Location.Y;
            if (clicked == 1)
            {
                s1.x = x;
                s1.y = y;
            }
            else
            {
                s2.x = x;
                s2.y = y;
            }
            pictureBox1.Invalidate();
            pictureBox1.Click -= pictureBox1_Click;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            clicked = 2;
            pictureBox1.Click += pictureBox1_Click;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //speed in 100m/s
            s1.sx = double.Parse(textBox2.Text);
            s1.sy = double.Parse(textBox3.Text);
            s2.sx = double.Parse(textBox5.Text);
            s2.sy = double.Parse(textBox6.Text);
            timer1.Start();
        }
    }

    public class Star
    {
        public int m,r;
        public double x, y, sx, sy;
        public Star(int m)
        {
            this.m = m;
            //radius in 10^6 m, assuming desity is 5g/cm^3 and it's a ball
            r = (int)Math.Pow(300 * m / 20 / Math.PI, 0.33333333333333333333333);
            x = 0;
            y = 0;
        }
    }
}
