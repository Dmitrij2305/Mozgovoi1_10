using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Mozgovoi1_10
{
    abstract class Command : ICommand
    {
        protected PointF lastPosition;
        protected double lastAngle;

        protected Command(PointF lastPosition, double lastAngle)
        {
            this.lastPosition = lastPosition;
            this.lastAngle = lastAngle;
        }

        public abstract void Do();
        public abstract void Undo();
    }
}
