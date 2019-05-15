using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Modelowanie_GUI
{
    class BoardCA
    {
        private AdvancedBoard[] boards;
        private int sizeM;
        private int sizeN;
        private Color[] colorTable;
        private bool changesFlag;

        public bool ChangesFlag { get { return changesFlag; } }

        public int SizeM
        {
            get
            {
                return sizeM;
            }
            set
            {
                sizeM = value;
            }
        }

        public int SizeN
        {
            get
            {
                return sizeN;
            }
            set
            {
                sizeN = value;
            }
        }

        public BoardCA(int xCells, int yCells)
        {
            boards = new AdvancedBoard[2];
            this.sizeM = yCells;
            this.sizeN = xCells;
            boards[0] = new AdvancedBoard(sizeM, sizeN);
            boards[1] = new AdvancedBoard(sizeM, sizeN);
            colorTable = new Color[5];
            colorTable[0] = Color.Aqua;
            colorTable[1] = Color.Brown;
            colorTable[2] = Color.Red;
            colorTable[3] = Color.Yellow;
            colorTable[4] = Color.Green;
            changesFlag = true;

        }

        public void computeStepPeriodicBoundaryCondition(int numberOfBoard, bool Moore=false) {
            int skippingCounter = 0;
            for (int i = 0; i < boards[numberOfBoard].SizeM; i++)
            {
                for (int j = 0; j < boards[numberOfBoard].SizeN; j++)
                {
                    if (boards[numberOfBoard].getValue(i, j) != 0)
                    {
                        skippingCounter++;
                        boards[(numberOfBoard + 1) % 2].setValue(i, j, boards[numberOfBoard].getValue(i, j));
                        continue;
                    }
                    int[] arr;
                    if (Moore)
                        arr = new int[8];
                    else
                        arr = new int[4];

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
                    var tmp = arr.Max();
                    if (mode != 0 && ((maxCount == 4 && !Moore) || (maxCount == 8 && Moore)))
                        boards[(numberOfBoard + 1) % 2].setValue(i, j, mode);
                    else
                        boards[(numberOfBoard + 1) % 2].setValue(i, j, arr.Max());
                }
            }
            if (skippingCounter == sizeM * sizeN)
                changesFlag = false;

        }

        public void computeStepAbsorbingBoundaryCondition(int numberOfBoard, bool Moore = false)
        {
            int skippingCounter = 0;
            for (int i = 1; i < boards[numberOfBoard].SizeM -1; i++)
            {
                for (int j = 1; j < boards[numberOfBoard].SizeN -1; j++)
                {

                    if (boards[numberOfBoard].getValue(i, j) != 0)
                    {
                        skippingCounter++;
                        boards[(numberOfBoard + 1) % 2].setValue(i, j, boards[numberOfBoard].getValue(i, j));
                        continue;
                    }
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
                    if (mode != 0 && ((maxCount == 4 && !Moore) || (maxCount == 8 && Moore)))
                        boards[(numberOfBoard + 1) % 2].setValue(i, j, mode);                    
                    else
                        boards[(numberOfBoard + 1) % 2].setValue(i, j, arr.Max());
                }
            }
            if (skippingCounter == (sizeM - 2) * (sizeN - 2))
                changesFlag = false;
        }

        

        public void drawOnGraphics(SolidBrush brush, Graphics graphics, PictureBox pictureBox, Grid grid, int numberOfBoard, int colorOffset)
        {
            for (int i = 0; i < boards[numberOfBoard].SizeM; i++)
            {
                for (int j = 0; j < boards[numberOfBoard].SizeN; j++)
                {
                    var tmp = boards[numberOfBoard].getValue(i, j);
                    if (tmp >0)
                    {
                        brush.Color = colorTable[tmp % 5];
                        //byte red = (byte)(tmp & 0x000000FF);
                        //byte green = (byte)((tmp & 0x0000FF00) >> 08);
                        //byte blue = (byte)((tmp & 0x00FF0000) >> 16);
                        //brush.Color = Color.FromArgb(red, green, blue);

                        graphics.FillRectangle(brush, j * grid.cellSize + 1, i * grid.cellSize + 1, grid.cellSize - 1, grid.cellSize - 1);
                    }
                }
            }
        }

        public int getValueBasedOnCoordinates(int x, int y, Grid grid, int numberOfBoard)
        {
            return boards[numberOfBoard].getValue(y / grid.cellSize, x / grid.cellSize);
        }

        public void setValueBasedOnCoordinates(int x, int y, int value, Grid grid, int numberOfBoard)
        {
            boards[numberOfBoard].setValue(y / grid.cellSize, x / grid.cellSize, value);
        }

        public void setValue(int x, int y, int value, int numberOfBoard)
        {
            boards[numberOfBoard].setValue(y , x , value);
        }
    }
}
