using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelowanie_GUI
{
    class AdvancedBoard
    {
        private AdvancedCell[,] board;
        private int sizeM;
        private int sizeN;
        private Random rnd;

        public AdvancedBoard(int sizeM, int sizeN)
        {
            rnd = new Random();
            this.sizeM = sizeM;
            this.sizeN = sizeN;
            board = new AdvancedCell[sizeM, sizeN];
            for (int i = 0; i < sizeM; i++)
            {
                for (int j = 0; j < sizeN; j++)
                    board[i, j] = new AdvancedCell(rnd.NextDouble(), rnd.NextDouble());
            }
        }

        public int SizeM => sizeM;
        public int SizeN => sizeN;
        public int getValue(int m, int n)
        {
            return board[m, n].State;
        }

        public int getEnergy(int m, int n) {
            return board[m, n].Energy;
        }

        public void setValue(int m, int n, int value)
        {
            board[m, n].State = value;
        }

        public void setEnergy(int m, int n, int energy) {
            board[m, n].Energy = energy;
        }

        public double getXValueCenter(int m, int n) {
            return board[m, n].xCenter;
        }

        public double getYValueCenter(int m, int n) {
            return board[m, n].yCenter;
        }

        public void addDislocationDensity(int m, int n, double dislocationDensity) {
            board[m, n].DislocationDensity += dislocationDensity;
        }

        public bool checkRecrystalizated(int m, int n) {
            return board[m, n].Recrystallizated;
        }

        public void makeRecrystalizated(int m, int n) {
            board[m, n].Recrystallizated = true;
        }

        public double getDislocationDensity(int m, int n) {
            return board[m, n].DislocationDensity ;
        }

        public void setDislocationDensity(int m, int n, double dislocationDensity) {
            board[m, n].DislocationDensity = dislocationDensity;
        }

        public TemporaryCell getDislocationDensityAndRecrystalizated(int m, int n) {
            TemporaryCell result = new TemporaryCell(board[m, n].DislocationDensity, board[m, n].Recrystallizated);
            return result;
        }

        public double getMaxDislocationDensity() {
            double max = 0;
            for(int i = 0; i < sizeM; i++)
            {
                for(int j = 0; j < sizeN; j++)
                    max = max < board[i, j].DislocationDensity ? board[i, j].DislocationDensity : max;
            }
            return max;
        }

        public int getMaxEnergy()
        {
            int max = 0;
            for (int i = 0; i < sizeM; i++)
            {
                for (int j = 0; j < sizeN; j++)
                    max = max < board[i, j].Energy ? board[i, j].Energy : max;
            }
            return max;
        }



    }
}
