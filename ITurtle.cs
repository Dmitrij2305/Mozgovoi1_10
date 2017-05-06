using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mozgovoi1_10
{
    interface ITurtle
    {
        void PenUp();
        void PenDown();

        void Forward(int distance);

        void TurnLeft(double angle);
        void TurnRight(double angle);

        void Undo(int count);
        void Undo();
    }
}
