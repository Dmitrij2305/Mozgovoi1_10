using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Mozgovoi1_10
{
    class SmoothTurtle : ITurtle
    {
        private static readonly int stepLength = 10;

        private Timer timer;
        private Queue<Size> queue;

        private Graphics canvas;
        private Pen pen;
        
        private Point currentPosition;
        private Point targetPosition;

        private double currentAngle;
        private bool isPenDown;

        public Point CurrentPosition
        {
            get { return currentPosition; }
        }
        
        void timer_Tick(object sender, EventArgs e)
        {
            if (currentPosition != targetPosition)
            {
                int dx = targetPosition.X - currentPosition.X;
                int dy = targetPosition.Y - currentPosition.Y;
                
                Point oldPosition = currentPosition;

                double distanceToTarget = Math.Sqrt(dx * dx + dy * dy);
                if (distanceToTarget > stepLength)
                {                    
                    currentPosition.Offset((int)(dx * stepLength / distanceToTarget), 
                                           (int)(dy * stepLength / distanceToTarget));
                }
                else
                {
                    currentPosition = targetPosition;
                }

                if (isPenDown)
                    canvas.DrawLine(pen, oldPosition, currentPosition);
            }
            else if (queue.Count > 0)
            {
                Size offset = queue.Dequeue();
                targetPosition.Offset(offset.Width, offset.Height);
            }
            //canvas.DrawRectangle(pen, currentPosition.X - 10, currentPosition.Y - 10, 20, 20);
        }

        public SmoothTurtle(Graphics canvas, Point startPosition, double startAngle, bool isPenDown)
        {
            this.canvas = canvas;
            this.pen = new Pen(Color.Black, 2);

            this.currentPosition = startPosition;
            this.targetPosition = startPosition;

            this.currentAngle = startAngle;
            this.isPenDown = isPenDown;

            timer = new Timer();
            timer.Interval = 100;

            timer.Tick += timer_Tick;
            timer.Enabled = true;

            queue = new Queue<Size>();
        }

        public SmoothTurtle(Graphics canvas, int x, int y, double startAngle, bool isPenDown)
            : this(canvas, new Point(x, y), startAngle, isPenDown)
        {
        }

        public SmoothTurtle(Graphics canvas, Point startPosition)
            : this(canvas, startPosition, Math.PI / 2, false)
        {
        }

        public SmoothTurtle(Graphics canvas, int x, int y)
            : this(canvas, new Point(x, y), Math.PI / 2, false)
        {
        }

        public SmoothTurtle(Graphics canvas)
            : this(canvas, 0, 0, Math.PI / 2, false)
        {
        }

        public void PenDown()
        {
            isPenDown = true;
        }

        public void PenUp()
        {
            isPenDown = false;
        }

        public void Go(int distance)
        {
            int dx = (int)(distance * Math.Cos(currentAngle));
            int dy = (int)(distance * Math.Sin(currentAngle));
            
            queue.Enqueue(new Size(dx, -dy));
        }

        public void TurnLeft(double angle)
        {
            this.currentAngle += angle; 
        }

        public void TurnRight(double angle)
        {
            this.currentAngle -= angle;
        }
    }
}
