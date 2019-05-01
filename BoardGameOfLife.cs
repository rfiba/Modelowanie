using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelowanie_GUI
{
    class BoardGameOfLife
    {
        Board boardA;
        Board boardB;
        int sizeX;
        int sizeY;
        int rule; // 0 - Newton's rule

        public BoardGameOfLife(int sizeX, int sizeY, int shape, int rule = 0)
        {
            this.rule = rule;
            this.sizeX = sizeX;
            this.sizeY = sizeY;

            int center = boardA.sizeX / 2;
            switch (shape)
            {
                case 0:
                    boardA.prepareBeeHive();
                    break;
                case 1:
                    boardA.prepareBlinker();
                    break;
                case 2:
                    boardA.prepareGlider();
                    break;
                case 3:
                    boardA.prepareRandom();
                    break;
            }
        }
    }
}
