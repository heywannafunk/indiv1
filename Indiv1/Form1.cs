using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Indiv1
{
    public partial class Form1 : Form
    {
        //Артем Голенищев: 1. Построение выпуклой оболочки при пошаговом вводе
        Graphics g;
        List<PointF> points = new List<PointF>();
        List<PointF> convexHull = new List<PointF>();

        public PointF GetLeftmost(List<PointF> points) 
        {
            PointF leftmost = new PointF(float.MaxValue, 0.0f);
            for (int i = 0; i < points.Count(); i++)
            {
                if (points[i].X < leftmost.X)
                    leftmost = points[i];
            }
            return leftmost;
        }

        private bool leftTest(PointF p1, PointF p2, PointF p)
        {
            float test = (p2.X - p1.X) * (p.Y - p1.Y) - (p.X - p1.X) * (p2.Y - p1.Y);

            if (test > 0)
                return true;  // p слева от p1p2
            return false;
        }

        public void buildConvexHull()
        {
            convexHull.Clear();
            
            PointF startPoint = GetLeftmost(points);
            PointF finishPoint;

            do
            {
                convexHull.Add(startPoint);
                finishPoint = points[0];

                for (int i = 1; i < points.Count; i++)
                {
                    if ((startPoint == finishPoint) || leftTest(startPoint, finishPoint, points[i]))
                    {
                        finishPoint = points[i];
                    }
                }
                startPoint = finishPoint;
            }
            while (finishPoint != convexHull[0]);
        }

        public void drawHull() 
        {
            for (int i = 0; i < convexHull.Count()-1; i++)
            {
                g.DrawLine(Pens.GreenYellow, convexHull[i], convexHull[i + 1]);
            }
            g.DrawLine(Pens.GreenYellow, convexHull[convexHull.Count()-1], convexHull[0]);
        }

        public void drawPoints() 
        {
            for (int i = 0; i < points.Count(); i++)
            {
                g.DrawEllipse(Pens.LightSkyBlue, points[i].X - 3, points[i].Y - 3, 7, 7);
            }
        }

        public void GiftWrapping() 
        {
            buildConvexHull();
            drawHull();
        }

        public Form1()
        {
            InitializeComponent();
            g = CreateGraphics();
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            points.Add(new PointF(e.X, e.Y));
            drawPoints();

            if (points.Count() > 2) 
            {
                g.Clear(BackColor);
                drawPoints();
                GiftWrapping();
            }
        }

        private void Form1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            points.Clear();
            g.Clear(BackColor);
        }
    }
}
