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
    }
}
