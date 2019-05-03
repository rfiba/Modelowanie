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
        int rule; // 0 - Moore neighbourhood

        public BoardGameOfLife(int sizeX, int sizeY, int shape, int rule = 0)
        {
            this.rule = rule;
            this.sizeX = sizeX;
            this.sizeY = sizeY;

            int center = boardA.sizeN / 2;
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

        public void computeStep(int rule)
        {
            for (int i = 0; i < boardA.sizeM; i++)
            {
                for (int j = 0; j < boardA.sizeN; j++)
                {
                    int alives = 0;
                   
                    var tmp = boardA.getValue(mod(i - 1,boardA.sizeM), j) ? alives++ : 0;
                    tmp = boardA.getValue(mod(i - 1, boardA.sizeM), mod(j + 1, boardA.sizeN)) ? alives++ : 0;
                    tmp = boardA.getValue(mod(i - 1, boardA.sizeM), mod(j - 1, boardA.sizeN)) ? alives++ : 0;
                    tmp = boardA.getValue(mod(i + 1, boardA.sizeM), j) ? alives++ : 0;
                    tmp = boardA.getValue(mod(i + 1, boardA.sizeM), mod(j + 1, boardA.sizeN)) ? alives++ : 0;
                    tmp = boardA.getValue(mod(i + 1, boardA.sizeM), mod(j - 1, boardA.sizeN)) ? alives++ : 0;
                    tmp = boardA.getValue(i, mod(j + 1, boardA.sizeN)) ? alives++ : 0;
                    tmp = boardA.getValue(i, mod(j - 1, boardA.sizeN)) ? alives++ : 0;

                    if (boardA.getValue(i,j))
                    {
                        if (alives != 2 && alives != 3)
                        {
                            boardB.set(i, j,false); 
                            continue;
                        }
                    }
                    else {
                        if (alives == 3)
                        {
                            boardB.set(i, j, true);
                            continue;
                        }
                    }

                    boardB.set(i, j, boardA.getValue(i, j));
                }
            }
        }

    int mod(int x, int max)
    {
        if (x >= 0 && x < max)
            return x;
        else if (x < 0)
            return max - 1;
        else
            return (x - max);
    }
    }
}
