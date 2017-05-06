using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Mozgovoi1_10
{
    class SimpleTurtle : Turtle
    {
        #region Constructors

        public SimpleTurtle(Graphics canvas, Point startPosition, double startAngle)
            : base(canvas, startPosition, startAngle)
        {
        }

        public SimpleTurtle(Graphics canvas, int x, int y, double startAngle)
            : base(canvas, new Point(x, y), startAngle)
        {
        }

        public SimpleTurtle(Graphics canvas, Point startPosition)
            : base(canvas, startPosition, Math.PI / 2)
        {
        }

        public SimpleTurtle(Graphics canvas, int x, int y)
            : base(canvas, new Point(x, y), Math.PI / 2)
        {
        }

        public SimpleTurtle(Graphics canvas)
            : base(canvas, new Point(0, 0), Math.PI / 2)
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
            ICommand command = null;
            if (isPenDown)
                command = new DrawCommand(this, distance);
            else
                command = new GoCommand(this, distance);

            command.Do();
            doneCommands.Push(command);
        }

        public override void TurnLeft(double angleInDegrees)
        {
            double angle = angleInDegrees * Math.PI / 180;
            ICommand command = new TurnCommand(this, angle);

            command.Do();
            doneCommands.Push(command);
        }

        public override void TurnRight(double angleInDegrees)
        {
            double angle = angleInDegrees * Math.PI / 180;
            ICommand command = new TurnCommand(this, -angle);

            command.Do();
            doneCommands.Push(command);
        }
    }
}
