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

        public int cellSize { get; set; }
        public Grid(int panelWidth, int panelHeight, int numberOfXCells, int numberOfYCells)
        {
            int tmpX = panelWidth / numberOfXCells;
            int tmpY = panelHeight / numberOfYCells;
            this.cellSize = tmpX < tmpY ? tmpX : tmpY;
        }

        public Grid(int cellSize)
        {
            this.cellSize = cellSize;
        }

        public Grid(int panelWidth, int numberOfCells)
        {
            this.cellSize = panelWidth / numberOfCells;
        }

        public void draw(int panelWidth, int panelHeight, Graphics canva, Pen pen)
        {
            int realHeight = (panelHeight / cellSize)*cellSize;
            int realWidth = (panelWidth / cellSize)* cellSize;
            for (int it = 0; it < panelWidth; it += cellSize)
                canva.DrawLine(pen, it, 0, it, realHeight);

            for (int it = 0; it < panelHeight; it += cellSize)
                canva.DrawLine(pen, 0, it, realWidth, it);
        }

        public void drawSpecificNumberOfCells(int numberOfXCells, int numberOfYCells, Graphics canva, Pen pen)
        {
            int realHeight = cellSize * numberOfYCells;
            int realWidth = cellSize * numberOfXCells;
            //MessageBox.Show($"Komorka {cellSize} X {numberOfXCells} Y {numberOfYCells} rh {realHeight} rw {realWidth}");
            for (int it = 0, cord = 0; it <= numberOfXCells; it++, cord += cellSize)
                canva.DrawLine(pen, cord, 0, cord, realHeight);

            for (int it = 0, cord = 0; it <= numberOfYCells; it++, cord += cellSize)
                canva.DrawLine(pen, 0, cord, realWidth, cord);
        }
    }
}
