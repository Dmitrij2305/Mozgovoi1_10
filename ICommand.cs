using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mozgovoi1_10
{
    interface ICommand
    {
        void Do();
        void Undo();
    }
}
