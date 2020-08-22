using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Data.SqlClient;

namespace yuz_tanima
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        FilterInfoCollection filin;
        VideoCaptureDevice vcd;
        private void Form1_Load(object sender, EventArgs e)
        {
            filin = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach(FilterInfo f in filin)
            {
                comboBox1.Items.Add(f.Name);
            }
            comboBox1.SelectedIndex = 0;
            vcd = new VideoCaptureDevice();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            vcd = new VideoCaptureDevice(filin[comboBox1.SelectedIndex].MonikerString);
            vcd.NewFrame += Vcd_NewFrame;
            vcd.Start();
        }
        static readonly CascadeClassifier cascadeClassifier = new CascadeClassifier("haarcascade_frontalface_alt_tree.xml");
        private void Vcd_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();
            Image<Bgr , byte> grayImage = new Image<Bgr, byte>(bitmap);
           
            Rectangle[] rectangles=cascadeClassifier.DetectMultiScale(grayImage, 1.2,1);
            foreach (Rectangle rectangle in rectangles)
            {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    using (Pen pen = new Pen(Color.GreenYellow, 7))
                    {
                        graphics.DrawRectangle(pen, rectangle);
                    }
                }
            }
            pic.Image = bitmap;
            //pic.Image = pictureBox1.Image;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (vcd.IsRunning)
                vcd.Stop();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }
    }
}
