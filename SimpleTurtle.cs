using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Mozgovoi1_10
{
    class SimpleTurtle : ITurtle
    {
        Graphics canvas;

        private Point currentPosition;
        private double currentAngle;
        private bool isPenDown;
        private Pen pen;

        public Point CurrentPosition
        {
            get { return currentPosition; }
        }

        public SimpleTurtle(Graphics canvas, Point startPosition, double startAngle, bool isPenDown)
        {
            this.canvas = canvas;
            this.pen = new Pen(Color.Black, 2);

            this.currentPosition = startPosition;

            this.currentAngle = startAngle;
            this.isPenDown = isPenDown;
        }

        public SimpleTurtle(Graphics canvas, int x, int y, double startAngle, bool isPenDown)
            : this(canvas, new Point(x, y), startAngle, isPenDown)
        {
        }

        public SimpleTurtle(Graphics canvas, Point startPosition)
            : this(canvas, startPosition, Math.PI / 2, false)
        {
        }

        public SimpleTurtle(Graphics canvas, int x, int y)
            : this(canvas, new Point(x, y), Math.PI / 2, false)
        {
        }

        public SimpleTurtle(Graphics canvas)
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

        public void Forward(int distance)
        {
            int dx = (int)(distance * Math.Cos(currentAngle));
            int dy = -(int)(distance * Math.Sin(currentAngle));

            Point oldPosition = currentPosition;
            currentPosition.Offset(dx, dy);
            
            if (isPenDown)
                canvas.DrawLine(pen, oldPosition, currentPosition);
        }

        public void TurnLeft(double angleInDegrees)
        {
            this.currentAngle += angleInDegrees * Math.PI / 180; 
        }

        public void TurnRight(double angleInDegrees)
        {
            this.currentAngle -= angleInDegrees * Math.PI / 180;
        }
    }
}
