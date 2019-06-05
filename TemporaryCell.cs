using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelowanie_GUI
{
    class TemporaryCell
    {
        public double dislocationDensity;
        public bool recrystalizated;

        public TemporaryCell(double dislocationDensityToAdd, bool recrystalizatedToAdd) {
            dislocationDensity = dislocationDensityToAdd;
            recrystalizated = recrystalizatedToAdd;
        }
    }
}
