using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Modelowanie_GUI
{
    class BoardGameOfLife
    {
        Board []board;
        Board boardB;
        int sizeM;
        int sizeN;
        int rule; // 0 - Moore neighbourhood
        //short currentBoard = 0;


        public BoardGameOfLife(int sizeM, int sizeN, string shape, int rule = 0)
        {
            board = new Board[2];
            this.rule = rule;
            this.sizeM = sizeM;
            this.sizeN = sizeN;
            board[0] = new Board(this.sizeM, this.sizeN);
            board[1] = new Board(this.sizeM, this.sizeN);
            switch (shape)
            {
                case "Niezmienny":
                    board[0].prepareBeeHive();
                    break;
                case "Oscylator":
                    board[0].prepareBlinker();
                    break;
                case "Glider":
                    board[0].prepareGlider();
                    break;
                case "Losowy":
                    board[0].prepareRandom();
                    break;
            }
        }

        public BoardGameOfLife(int sizeM, int sizeN, int rule = 0)
        {
            board = new Board[2];
            this.rule = rule;
            this.sizeM = sizeM;
            this.sizeN = sizeN;
            board[0] = new Board(this.sizeM, this.sizeN);
            board[1] = new Board(this.sizeM, this.sizeN);           
        }

        public void computeStep(int numberOfBoard, int rule=0)
        {
            for (int i = 0; i < board[numberOfBoard].sizeM; i++)
            {
                for (int j = 0; j < board[numberOfBoard].sizeN; j++)
                {
                    int alives = 0;
                   
                    var tmp = board[numberOfBoard].getValue(mod(i - 1, board[numberOfBoard].sizeM), j) ? alives++ : 0;
                    tmp = board[numberOfBoard].getValue(mod(i - 1, board[numberOfBoard].sizeM), mod(j + 1, board[numberOfBoard].sizeN)) ? alives++ : 0;
                    tmp = board[numberOfBoard].getValue(mod(i - 1, board[numberOfBoard].sizeM), mod(j - 1, board[numberOfBoard].sizeN)) ? alives++ : 0;
                    tmp = board[numberOfBoard].getValue(mod(i + 1, board[numberOfBoard].sizeM), j) ? alives++ : 0;
                    tmp = board[numberOfBoard].getValue(mod(i + 1, board[numberOfBoard].sizeM), mod(j + 1, board[numberOfBoard].sizeN)) ? alives++ : 0;
                    tmp = board[numberOfBoard].getValue(mod(i + 1, board[numberOfBoard].sizeM), mod(j - 1, board[numberOfBoard].sizeN)) ? alives++ : 0;
                    tmp = board[numberOfBoard].getValue(i, mod(j + 1, board[numberOfBoard].sizeN)) ? alives++ : 0;
                    tmp = board[numberOfBoard].getValue(i, mod(j - 1, board[numberOfBoard].sizeN)) ? alives++ : 0;

                    if (board[numberOfBoard].getValue(i,j))
                    {
                        if (alives != 2 && alives != 3)
                        {
                            board[(numberOfBoard+1)%2].set(i, j,false); 
                            continue;
                        }
                    }
                    else {
                        if (alives == 3)
                        {
                            board[(numberOfBoard + 1) % 2].set(i, j, true);
                            continue;
                        }
                    }

                    board[(numberOfBoard + 1) % 2].set(i, j, board[numberOfBoard].getValue(i, j));
                }
            }
        }

        public bool getValueBasedOnCoordinates(int x, int y, Grid grid, int numberOfBoard)
        {
            return board[numberOfBoard].getValue(y / grid.cellSize, x / grid.cellSize);
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
        
        public void drawOnGraphics(SolidBrush brush, Graphics graphics, PictureBox pictureBox, Grid grid, int numberOfBoard)
        {
            for (int i = 0; i < board[numberOfBoard].sizeM; i++)
            {
                if ((pictureBox.Height / grid.cellSize) * grid.cellSize < i * grid.cellSize + 1)
                    ;//continue;

                for (int j = 0; j < board[numberOfBoard].sizeN; j++)
                {
                    if (board[numberOfBoard].getValue(i, j) == true)
                    {
                        graphics.FillRectangle(brush, j * grid.cellSize + 1, i * grid.cellSize + 1, grid.cellSize - 1, grid.cellSize - 1);
                    }
                }
            }
        }

        public void setValue(int m, int n, bool value, int numberOfBoard)
        {
            board[numberOfBoard].set(m, n, value);
        }

        public void setValueBasedOnCoordinates(int x, int y, bool value, Grid grid, int numberOfBoard)
        {
            board[numberOfBoard].set(y / grid.cellSize,x / grid.cellSize, value);
        }
    }
}
