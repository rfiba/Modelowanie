﻿using System;
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
        public Grid(int panelWidth, int numberOfCells)
        {
            this.cellSize = panelWidth/numberOfCells;
        }

        public void draw(int panelWidth, int panelHeight, Graphics canva, Pen pen)
        {
            for (int it = 0; it < panelWidth; it += cellSize)
                canva.DrawLine(pen, it, 0, it, panelHeight);

            for (int it = 0; it < panelHeight; it += cellSize)
                canva.DrawLine(pen, 0, it, panelWidth, it);
        }
    }
}
