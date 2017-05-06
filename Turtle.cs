using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Mozgovoi1_10
{
    abstract class Turtle: ITurtle
    {
        protected Graphics canvas;
        protected Pen drawPen;
        protected Pen erasePen;

        protected PointF currentPosition;
        protected double currentAngle;

        protected bool isPenDown;

        protected Stack<ICommand> doneCommands = new Stack<ICommand>();

        public PointF CurrentPosition
        {
            get { return currentPosition; }
        }

        protected Turtle(Graphics canvas, Point startPosition, double startAngle)
        {
            this.canvas = canvas;
            this.drawPen = new Pen(Color.Black, 2);
            this.erasePen = new Pen(Color.FromKnownColor(KnownColor.Desktop), 2);

            this.currentPosition = startPosition;

            this.currentAngle = startAngle;
            this.isPenDown = false;
        }

        protected class GoCommand : Command
        {
            private Turtle turtle;
            private int distance;

            public GoCommand(Turtle turtle, int distance)
                : base(turtle.currentPosition, turtle.currentAngle)
            {
                this.turtle = turtle;
                this.distance = distance;
            }

            public override void Do()
            {
                turtle.currentPosition.X += distance * (float)Math.Cos(turtle.currentAngle);
                turtle.currentPosition.Y -= distance * (float)Math.Sin(turtle.currentAngle);
            }

            public override void Undo()
            {
                turtle.currentPosition = lastPosition;
            }

            public override string ToString()
            {
                return String.Format("Go from {0} (angle = {1} rad) on {2}", lastPosition, lastAngle, distance);
            }
        }

        protected class DrawCommand : Command
        {
            private Turtle turtle;
            private int distance;

            public DrawCommand(Turtle turtle, int distance)
                : base(turtle.currentPosition, turtle.currentAngle)
            {
                this.turtle = turtle;
                this.distance = distance;
            }

            public override void Do()
            {
                PointF oldPosition = turtle.currentPosition;

                turtle.currentPosition.X += distance * (float)Math.Cos(turtle.currentAngle);
                turtle.currentPosition.Y -= distance * (float)Math.Sin(turtle.currentAngle);

                turtle.canvas.DrawLine(turtle.drawPen, oldPosition, turtle.currentPosition);
            }

            public override void Undo()
            {
                PointF oldPosition = turtle.currentPosition;
                turtle.canvas.DrawLine(turtle.erasePen, oldPosition, lastPosition);
                turtle.currentPosition = lastPosition;
            }

            public override string ToString()
            {
                return String.Format("Draw from {0} (angle = {1} rad) on {2}", lastPosition, lastAngle, distance);
            }
        }

        protected class TurnCommand : Command
        {
            private Turtle turtle;
            private double angle;

            public TurnCommand(Turtle turtle, double angle)
                : base(turtle.currentPosition, turtle.currentAngle)
            {
                this.turtle = turtle;
                this.angle = angle;
            }

            public override void Do()
            {
                turtle.currentAngle += angle;
            }

            public override void Undo()
            {
                turtle.currentAngle = lastAngle;
            }

            public override string ToString()
            {
                return String.Format("Turn (angle = {0} rad)", angle);
            }
        }


        public abstract void PenUp();
        public abstract void PenDown();

        public abstract void Forward(int distance);

        public abstract void TurnLeft(double angle);
        public abstract void TurnRight(double angle);
        

        public void Undo(int count)
        {
            while (count-- > 0)
            {
                if (doneCommands.Count == 0)
                    throw new ArgumentOutOfRangeException("count");

                ICommand command = doneCommands.Pop();
                command.Undo();
            }
        }

        public void Undo()
        {
            if (doneCommands.Count == 0)
                throw new ArgumentOutOfRangeException("count");

            ICommand command = doneCommands.Pop();
            command.Undo();
        }
    }
}
