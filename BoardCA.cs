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

        public void computeStepPeriodicBoundaryCondition(int numberOfBoard, int absorbingBoundaryCondition = 0, bool Moore=false) {
            for (int i = absorbingBoundaryCondition; i < boards[numberOfBoard].SizeM - absorbingBoundaryCondition; i++)
            {
                for (int j = absorbingBoundaryCondition; j < boards[numberOfBoard].SizeN- absorbingBoundaryCondition; j++)
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
                    if (mode != 0 || (maxCount == 4 && !Moore) || (maxCount == 8 && Moore))
                        boards[(numberOfBoard + 1) % 2].setValue(i, j, mode);
                    else
                        boards[(numberOfBoard + 1) % 2].setValue(i, j, arr.Max());

                }
            }
        }

        public void computeStepAbsorbingBoundaryCondition(int numberOfBoard, bool Moore = false)
        {
            for (int i = 1; i < boards[numberOfBoard].SizeM -1; i++)
            {
                for (int j = 1; j < boards[numberOfBoard].SizeN -1; j++)
                {
                    int[] arr = new int[8];

                    arr[0] = boards[numberOfBoard].getValue(i - 1, j);
                    arr[1] = boards[numberOfBoard].getValue(i + 1, j);
                    arr[2] = boards[numberOfBoard].getValue(i, j + 1);
                    arr[3] = boards[numberOfBoard].getValue(i, j - 1);
                    if (Moore)
                    {
                        arr[4] = boards[numberOfBoard].getValue(i - 1, j - 1);
                        arr[5] = boards[numberOfBoard].getValue(i + 1, j - 1);
                        arr[6] = boards[numberOfBoard].getValue(i - 1, j + 1);
                        arr[7] = boards[numberOfBoard].getValue(i + 1, j + 1);
                    }
                    var groups = arr.GroupBy(v => v);
                    int maxCount = groups.Max(g => g.Count());
                    int mode = groups.First(g => g.Count() == maxCount).Key;
                    if (mode != 0 || (maxCount == 4 && !Moore) || (maxCount == 8 && Moore))
                        boards[(numberOfBoard + 1) % 2].setValue(i, j, mode);
                    else
                        boards[(numberOfBoard + 1) % 2].setValue(i, j, arr.Max()); 
                }
            }
        }
    }
}
