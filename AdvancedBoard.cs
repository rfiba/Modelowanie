using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelowanie_GUI
{
    class AdvancedBoard
    {
        private AdvancedCell[,] board;
        private int sizeM;
        private int sizeN;

        public AdvancedBoard(int numberOfXCells, int numberOfYCells)
        {
            this.sizeM = numberOfYCells;
            this.sizeN = numberOfXCells;
            board = new AdvancedCell[sizeM, sizeN];
        }
    }
}
