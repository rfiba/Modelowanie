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

        public void computeStepPeriodicBoundaryCondition(int numberOfBoard, bool Moore=false) {
            for (int i = 0; i < boards[numberOfBoard].SizeM; i++)
            {
                for (int j = 0; j < boards[numberOfBoard].SizeN; j++)
                {
                    int[] arr = new int[8];

                    arr[0] = boards[numberOfBoard].getValue(BoardGameOfLife.mod(i - 1, boards[numberOfBoard].SizeM), j) ;
                    arr[1] = boards[numberOfBoard].getValue(BoardGameOfLife.mod(i + 1, boards[numberOfBoard].SizeM), j) ;
                    arr[2] = boards[numberOfBoard].getValue(i, BoardGameOfLife.mod(j + 1, boards[numberOfBoard].SizeN));
                    arr[3] = boards[numberOfBoard].getValue(i, BoardGameOfLife.mod(j - 1, boards[numberOfBoard].SizeN)) ;
                    if (Moore)
                    {
                        arr[4] = boards[numberOfBoard].getValue(BoardGameOfLife.mod(i - 1, boards[numberOfBoard].SizeM),
                            BoardGameOfLife.mod(j - 1, boards[numberOfBoard].SizeN));
                        arr[5] = boards[numberOfBoard].getValue(BoardGameOfLife.mod(i + 1, boards[numberOfBoard].SizeM),
                            BoardGameOfLife.mod(j - 1, boards[numberOfBoard].SizeN));
                        arr[6] = boards[numberOfBoard].getValue(BoardGameOfLife.mod(i - 1, boards[numberOfBoard].SizeM),
                            BoardGameOfLife.mod(j + 1, boards[numberOfBoard].SizeN));
                        arr[7] = boards[numberOfBoard].getValue(BoardGameOfLife.mod(i + 1, boards[numberOfBoard].SizeM),
                            BoardGameOfLife.mod(j + 1, boards[numberOfBoard].SizeN));
                    }
                    var groups = arr.GroupBy(v => v);
                    int maxCount = groups.Max(g => g.Count());
                    int mode = groups.First(g => g.Count() == maxCount).Key;
                    boards[(numberOfBoard + 1) % 2].setValue(i,j, mode);
                }
            }
        }

        public void computeStepAbsorbingBoundaryCondition(int numberOfBoard, bool Moore = false)
        {
            //Nie będzie działać dla rogów
            for (int i = 1; i < boards[numberOfBoard].SizeM-1; i++)
            {
                for (int j = 1; j < boards[numberOfBoard].SizeN-1; j++)
                {
                    int[] arr = new int[8];

                    if(i -1 > 0)
                        arr[0] = boards[numberOfBoard].getValue(BoardGameOfLife.mod(i - 1, boards[numberOfBoard].SizeM), j);
                    if (i + 1 < boards[numberOfBoard].SizeM - 1)
                        arr[1] = boards[numberOfBoard].getValue(BoardGameOfLife.mod(i + 1, boards[numberOfBoard].SizeM), j);
                    if (j + 1 < boards[numberOfBoard].SizeN - 1)
                        arr[2] = boards[numberOfBoard].getValue(i, BoardGameOfLife.mod(j + 1, boards[numberOfBoard].SizeN));
                    if (j - 1 > 0)
                        arr[3] = boards[numberOfBoard].getValue(i, BoardGameOfLife.mod(j - 1, boards[numberOfBoard].SizeN));
                    if (Moore)
                    {
                        if (i - 1 > 0 && j - 1 > 0)
                            arr[4] = boards[numberOfBoard].getValue(BoardGameOfLife.mod(i - 1, boards[numberOfBoard].SizeM),
                            BoardGameOfLife.mod(j - 1, boards[numberOfBoard].SizeN));
                        if (i + 1 < boards[numberOfBoard].SizeM - 1  && j - 1 > 0)
                            arr[5] = boards[numberOfBoard].getValue(BoardGameOfLife.mod(i + 1, boards[numberOfBoard].SizeM),
                            BoardGameOfLife.mod(j - 1, boards[numberOfBoard].SizeN));
                        if (i - 1 > 0 && j + 1 < boards[numberOfBoard].SizeN - 1)
                            arr[6] = boards[numberOfBoard].getValue(BoardGameOfLife.mod(i - 1, boards[numberOfBoard].SizeM),
                            BoardGameOfLife.mod(j + 1, boards[numberOfBoard].SizeN));
                        if (i + 1 < boards[numberOfBoard].SizeM - 1 && j + 1 < boards[numberOfBoard].SizeN - 1)
                            arr[7] = boards[numberOfBoard].getValue(BoardGameOfLife.mod(i + 1, boards[numberOfBoard].SizeM),
                            BoardGameOfLife.mod(j + 1, boards[numberOfBoard].SizeN));
                    }
                    var groups = arr.GroupBy(v => v);
                    int maxCount = groups.Max(g => g.Count());
                    int mode = groups.First(g => g.Count() == maxCount).Key;
                    boards[(numberOfBoard + 1) % 2].setValue(i, j, mode);
                }
            }
        }

        ch
    }
}
