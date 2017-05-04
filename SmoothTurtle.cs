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
        class GoCommand : ICommand
        {
            private SmoothTurtle turtle;
            private int distance;

            public GoCommand(SmoothTurtle turtle, int distance)
            {
                this.turtle = turtle;
                this.distance = distance;
            }

            public void Do()
            {
                turtle.currentPosition.X += distance * (float)Math.Cos(turtle.currentAngle);
                turtle.currentPosition.Y -= distance * (float)Math.Sin(turtle.currentAngle);
            }

            public void Undo()
            {
                throw new NotImplementedException();
            }
        }

        class DrawCommand : ICommand
        {
            private SmoothTurtle turtle;
            private int distance;

            public DrawCommand(SmoothTurtle turtle, int distance)
            {
                this.turtle = turtle;
                this.distance = distance;
            }

            public void Do()
            {
                PointF oldPosition = turtle.currentPosition;

                turtle.currentPosition.X += distance * (float)Math.Cos(turtle.currentAngle);
                turtle.currentPosition.Y -= distance * (float)Math.Sin(turtle.currentAngle);

                turtle.canvas.DrawLine(turtle.pen, oldPosition, turtle.currentPosition);
            }

            public void Undo()
            {
                throw new NotImplementedException();
            }
        }

        class TurnCommand : ICommand
        {
            private SmoothTurtle turtle;
            private double angle;

            public TurnCommand(SmoothTurtle turtle, double angle)
            {
                this.turtle = turtle;
                this.angle = angle;
            }

            public void Do()
            {
                turtle.currentAngle += angle;
            }

            public void Undo()
            {
                throw new NotImplementedException();
            }
        }

        private static readonly int stepLength = 10;
        private static readonly double stepAngle = Math.PI / 12;

        private static readonly int sizeTurtle = 10;

        private Timer timer;
        private Queue<ICommand> queue;

        private Graphics canvas;
        private Pen pen;
        
        private PointF currentPosition;
        private double currentAngle;

        private bool isPenDown;

        public PointF CurrentPosition
        {
            get { return currentPosition; }
        }

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

            canvas.DrawLine(pen, x1, y1, x2, y2);
            canvas.DrawLine(pen, x2, y2, x3, y3);
            canvas.DrawLine(pen, x3, y3, x1, y1);
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

        public SmoothTurtle(Graphics canvas, Point startPosition, double startAngle, bool isPenDown)
        {
            this.canvas = canvas;
            this.pen = new Pen(Color.Black, 2);

            this.currentPosition = startPosition;

            this.currentAngle = startAngle;
            this.isPenDown = isPenDown;

            timer = new Timer();
            timer.Interval = 100;

            timer.Tick += timer_Tick;
            timer.Enabled = true;

            queue = new Queue<ICommand>();
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

        #endregion

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

        public void TurnLeft(double angleInDegrees)
        {
            double angle = angleInDegrees * Math.PI / 180;

            int count = (int)(angle / stepAngle);
            for (int _ = 0; _ < count; _++)
                queue.Enqueue(new TurnCommand(this, stepAngle));

            double restAngle = angle - count * stepAngle;
            if (restAngle > 0)
                queue.Enqueue(new TurnCommand(this, restAngle));
        }

        public void TurnRight(double angleInDegrees)
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
