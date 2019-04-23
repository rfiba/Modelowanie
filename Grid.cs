using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Modelowanie_GUI
{
    class Grid
    {
        private int horizontalCells;
        private int verticalCells ;
        private int cellSize;
        public Grid(int horizontalCells, int verticalCells, int cellSize=10)
        {
            this.horizontalCells = horizontalCells;
            this.verticalCells = verticalCells;
            this.cellSize = cellSize;
        }

        public void draw(int panelWidth, int panelHeight, Graphics canva, Pen pen)
        {
            for (int it = cellSize; it < panelWidth; it += cellSize)
                canva.DrawLine(pen, it, 0, it, panelHeight);

            for (int it = cellSize; it < panelHeight; it += cellSize)
                canva.DrawLine(pen, 0, it, panelWidth, it);
        }
    }
}
