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
        public int numberOfIteration { get; }
        public int numberOfCells { get; }
        int rule;

        public Board(int numberOfIteration, int numberOfCells, int rule)
        {
            this.numberOfIteration = numberOfIteration;
            this.numberOfCells = numberOfCells;
            this.rule = rule;
            board = new Cell[numberOfIteration + 1, numberOfCells];
            for (int i = 0; i < numberOfIteration; i++)
            {
                for (int j = 0; j < numberOfCells; j++)
                    board[i, j] = new Cell();
            }
            board[0, 4].State = true;
        }

        public string prepareBoard()
        {
            string result ="";
            for (int i = 1; i < numberOfIteration; i++)
            {
                for (int j = 0; j < numberOfCells; j++)
                {
                    int triple = boolsToDecimal(board[i - 1, j].State, board[i - 1, (j + 1) % numberOfCells].State, board[i - 1, (j + 2) % numberOfCells].State);

                    if (checkBit(triple, rule))
                    {
                        board[i, (j + 1) % numberOfCells].State = true;
                    }
                }
            }

            for (int i = 0; i < numberOfIteration; i++)
            {
                for (int j = 0; j < numberOfCells; j++)
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



        public void drawOnGraphics(SolidBrush brush, Graphics graphics)
        {
            for(int i = 0; i < numberOfCells; i++)
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
