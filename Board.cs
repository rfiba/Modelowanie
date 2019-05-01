using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelowanie_GUI
{
    class Board
    {
        Cell[,] board;
        public int sizeY { get; }
        public int sizeX { get; }
        int rule;

        public Board(int numberOfIteration, int numberOfCells, int rule)
        {
            this.sizeY = numberOfIteration;
            this.sizeX = numberOfCells;
            this.rule = rule;
            board = new Cell[numberOfIteration + 1, numberOfCells];
            for (int i = 0; i < numberOfIteration; i++)
            {
                for (int j = 0; j < numberOfCells; j++)
                    board[i, j] = new Cell();
            }
            board[0, 4].State = true;
        }

       

        public void set(int x, int y, bool value)
        {
            board[x, y].State = value;
        }

        public string prepareBoard()
        {
            string result ="";
            for (int i = 1; i < sizeY; i++)
            {
                for (int j = 0; j < sizeX; j++)
                {
                    int triple = boolsToDecimal(board[i - 1, j].State, board[i - 1, (j + 1) % sizeX].State, board[i - 1, (j + 2) % sizeX].State);

                    if (checkBit(triple, rule))
                    {
                        board[i, (j + 1) % sizeX].State = true;
                    }
                }
            }

            for (int i = 0; i < sizeY; i++)
            {
                for (int j = 0; j < sizeX; j++)
                {
                    result += boolToInt(board[i, j].State);
                    //Console.Write(boolToInt(board[i, j].State));
                    //Console.Write(" ");
                    result += " ";
                }
                //Console.WriteLine();
                result += "\n";
            }
            return result;
        }

        public void prepareBeeHive() {
            int start = sizeX / 2;
            board[0, start + 1].State = board[0, start + 2].State = true;
            board[1, start].State = board[1, start + 3].State = true;
            board[2, start + 1].State = board[2, start + 2].State = true;
        }

        public void prepareBlinker()
        {
            int start = sizeX / 2;
            board[0, start].State = board[1, start].State = board[2, start].State = true;
        }

        public void prepareGlider()
        {
            int start = sizeX / 2;
            board[0, start + 1].State = board[0, start + 2].State = true;
            board[1, start].State = board[1, start + 1].State = true;
            board[3, start + 2].State = true;
        }

        public void prepareRandom(int numberOfCells = 15)
        {
            int x, y;
            Random random = new Random();
            for (int i = 0; i < numberOfCells; i++)
            {
                x = random.Next(0, sizeX);
                y = random.Next(0, sizeY);
                board[x, y].State = true;
            }
        }

        public void drawOnGraphics(SolidBrush brush, Graphics graphics)
        {
            for(int i = 0; i < sizeX; i++)
            {
                
            }
        }
        public bool getValue(int x, int y)
        {
            return board[x, y].State;
        }

        static int boolsToDecimal(bool a, bool b, bool c)
        {
            return 4 * boolToInt(a) + 2 * boolToInt(b) + 1 * boolToInt(c);
        }

        static int boolToInt(bool a)
        {
            return a ? 1 : 0;
        }

        static bool checkBit(int n, int toCheck)
        {
            int tmp = 1;
            int checker = tmp << n;
            return (toCheck & checker) == 0 ? false : true;
        }
    }
}
