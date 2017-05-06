using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Mozgovoi1_10
{
    class SmoothTurtle : Turtle
    {
        private static readonly int stepLength = 10;
        private static readonly double stepAngle = Math.PI / 12;

        private static readonly int sizeTurtle = 10;

        private Timer timer;
        private Queue<ICommand> queue;
        
        private void DrawTurtle()
        {
            int x1 = (int)(currentPosition.X + (sizeTurtle * Math.Cos(currentAngle) / Math.Sqrt(3)));
            int y1 = (int)(currentPosition.Y - (sizeTurtle * Math.Sin(currentAngle) / Math.Sqrt(3)));
            int x2 = (int)(currentPosition.X - ((sizeTurtle * Math.Cos(currentAngle)) /
                (2 * Math.Sqrt(3)) - sizeTurtle * Math.Sin(currentAngle) / 2));
            int y2 = (int)(currentPosition.Y + ((sizeTurtle * Math.Sin(currentAngle)) /
                (2 * Math.Sqrt(3)) + sizeTurtle * Math.Cos(currentAngle) / 2));
            int x3 = (int)(currentPosition.X - ((sizeTurtle * Math.Cos(currentAngle)) /
                (2 * Math.Sqrt(3)) + sizeTurtle * Math.Sin(currentAngle) / 2));
            int y3 = (int)(currentPosition.Y + ((sizeTurtle * Math.Sin(currentAngle)) /
                (2 * Math.Sqrt(3)) - sizeTurtle * Math.Cos(currentAngle) / 2));

            canvas.DrawLine(drawPen, x1, y1, x2, y2);
            canvas.DrawLine(drawPen, x2, y2, x3, y3);
            canvas.DrawLine(drawPen, x3, y3, x1, y1);
        }
        
        void timer_Tick(object sender, EventArgs e)
        {
            if (queue.Count > 0)
            {
                ICommand command = queue.Dequeue();
                command.Do();

                DrawTurtle();
            }
        }

        #region Constructors

        public SmoothTurtle(Graphics canvas, Point startPosition, double startAngle)
            : base(canvas, startPosition, startAngle)
        {
            timer = new Timer();
            timer.Interval = 100;

            timer.Tick += timer_Tick;
            timer.Enabled = true;

            queue = new Queue<ICommand>();
        }

        public SmoothTurtle(Graphics canvas, int x, int y, double startAngle)
            : this(canvas, new Point(x, y), startAngle)
        {
        }

        public SmoothTurtle(Graphics canvas, Point startPosition)
            : this(canvas, startPosition, Math.PI / 2)
        {
        }

        public SmoothTurtle(Graphics canvas, int x, int y)
            : this(canvas, new Point(x, y), Math.PI / 2)
        {
        }

        public SmoothTurtle(Graphics canvas)
            : this(canvas, 0, 0, Math.PI / 2)
        {
        }

        #endregion

        public override void PenDown()
        {
            isPenDown = true;
        }

        public override void PenUp()
        {
            isPenDown = false;
        }

        public override void Forward(int distance)
        {
            if (isPenDown)
            {
                for (int _ = 0; _ < distance / stepLength; _++)
                    queue.Enqueue(new DrawCommand(this, stepLength));

                int rest = distance % stepLength;
                if (rest > 0)
                    queue.Enqueue(new DrawCommand(this, rest));
            }
            else
            {
                for (int _ = 0; _ < distance / stepLength; _++)
                    queue.Enqueue(new GoCommand(this, stepLength));

                int rest = distance % stepLength;
                if (rest > 0)
                    queue.Enqueue(new GoCommand(this, rest));
            }
        }

        public override void TurnLeft(double angleInDegrees)
        {
            double angle = angleInDegrees * Math.PI / 180;

            int count = (int)(angle / stepAngle);
            for (int _ = 0; _ < count; _++)
                queue.Enqueue(new TurnCommand(this, stepAngle));

            double restAngle = angle - count * stepAngle;
            if (restAngle > 0)
                queue.Enqueue(new TurnCommand(this, restAngle));
        }

        public override void TurnRight(double angleInDegrees)
        {
            double angle = angleInDegrees * Math.PI / 180;

            int count = (int)(angle / stepAngle);
            for (int _ = 0; _ < count; _++)
                queue.Enqueue(new TurnCommand(this, -stepAngle));

            double restAngle = angle - count * stepAngle;
            if (restAngle > 0)
                queue.Enqueue(new TurnCommand(this, -restAngle));
        }
    }
}
