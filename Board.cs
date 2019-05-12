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
        private Cell[,] board;
        public int sizeM { get; }
        public int sizeN { get; }
        private int rule;

        public Board(int numberOfIteration, int numberOfCells, int rule)
        {
            this.sizeM = numberOfIteration;
            this.sizeN = numberOfCells;
            this.rule = rule;
            board = new Cell[numberOfIteration + 1, numberOfCells];
            for (int i = 0; i < numberOfIteration; i++)
            {
                for (int j = 0; j < numberOfCells; j++)
                    board[i, j] = new Cell();
            }
            board[0, numberOfCells/2].State = true;
        }

        public Board(int sizeM, int sizeN)
        {
            this.sizeM = sizeM;
            this.sizeN = sizeN;
            board = new Cell[this.sizeM, this.sizeN];
            for (int i = 0; i < this.sizeM; i++)
            {
                for (int j = 0; j < this.sizeN; j++)
                    board[i, j] = new Cell();
            }
        }

        public void set(int x, int y, bool value)
        {
            board[x, y].State = value;
        }

        public string prepareBoard()
        {
            string result ="";
            for (int i = 1; i < sizeM; i++)
            {
                for (int j = 0; j < sizeN; j++)
                {
                    int triple = boolsToDecimal(board[i - 1, j].State, board[i - 1, (j + 1) % sizeN].State, board[i - 1, (j + 2) % sizeN].State);

                    if (checkBit(triple, rule))
                    {
                        board[i, (j + 1) % sizeN].State = true;
                    }
                }
            }

            for (int i = 0; i < sizeM; i++)
            {
                for (int j = 0; j < sizeN; j++)
                {
                    result += boolToInt(board[i, j].State);
                    result += " ";
                }
                result += "\n";
            }
            return result;
        }

        public void prepareBeeHive() {
            Random random = new Random();
            int m = random.Next(0, sizeM -3);
            int n = random.Next(0, sizeN -3);
            board[m, n + 1].State = board[m, n + 2].State = true;
            board[m+1, n].State = board[m + 1, n + 3].State = true;
            board[m+2, n + 1].State = board[m + 2, n + 2].State = true;
        }

        public void prepareBlinker()
        {
            Random random = new Random();
            int m = random.Next(0, sizeM - 3);
            int n = random.Next(0, sizeN - 3);
            board[m, n].State = board[m+1, n].State = board[m+2, n].State = true;
        }

        public void prepareGlider()
        {
            Random random = new Random();
            int m = random.Next(0, sizeM - 3);
            int n = random.Next(0, sizeN - 3);
            board[m, n + 1].State = board[m, n + 2].State = true;
            board[m+1, n].State = board[m+1, n + 1].State = true;
            board[m+2, n + 2].State = true;
        }

        public void prepareRandom(int numberOfCells = 15)
        {
            int m,n;
            Random random = new Random();
            for (int i = 0; i < numberOfCells; i++)
            {
                m = random.Next(0, sizeM);
                n = random.Next(0, sizeN);
                board[m, n].State = true;
            }
        }

        public bool getValue(int n, int m)
        {
            return board[n, m].State;
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
