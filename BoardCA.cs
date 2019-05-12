using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelowanie_GUI
{
    class BoardCA
    {
        AdvancedBoard[] boards;
        int sizeM;
        int sizeN;

        public BoardCA(int sizeM, int sizeN)
        {
            boards = new AdvancedBoard[2];
            this.sizeM = sizeM;
            this.sizeN = sizeN;
            boards[0] = new AdvancedBoard(sizeM, sizeN);
            boards[1] = new AdvancedBoard(sizeM, sizeN);
        }

        public void computeStepPeriodicBoundaryCondition(int numberOfBoard) {
            for (int i = 0; i < boards[numberOfBoard].SizeM; i++)
            {
                for (int j = 0; j < boards[numberOfBoard].SizeN; j++)
                {
                    int alives = 0;

                    var tmp = boards[numberOfBoard].getValue(BoardGameOfLife.mod(i - 1, boards[numberOfBoard].SizeM), j) > 0 ? alives++ : 0;
                    tmp = boards[numberOfBoard].getValue(BoardGameOfLife.mod(i + 1, boards[numberOfBoard].SizeM), j) > 0? alives++ : 0;
                    tmp = boards[numberOfBoard].getValue(i, BoardGameOfLife.mod(j + 1, boards[numberOfBoard].SizeN)) >0? alives++ : 0;
                    tmp = boards[numberOfBoard].getValue(i, BoardGameOfLife.mod(j - 1, boards[numberOfBoard].SizeN)) >0? alives++ : 0;

                    
                }
            }
        }
    }
}
