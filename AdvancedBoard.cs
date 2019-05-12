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

        public int SizeM => sizeM;
        public int SizeN => sizeN;
        public int getValue(int m, int n)
        {
            return board[m, n].State;
        }

        public void setValue(int m, int n, int value)
        {
            board[m, n].State = value;
        }

    }
}
