using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
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
        private bool changesFlag;
        private List<Point> points;
        Dictionary<int, Color> colors;
        Random rnd;
        int exponent;
        bool exponentCalculated;
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
            rnd = new Random();
            changesFlag = true;
            points = new List<Point>();
            colors = new Dictionary<int, Color>();
            exponentCalculated = false;
        }

        public void computeStepPeriodicBoundaryCondition(int numberOfBoard, int neighbourhood = 0) {
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
                    
                    var arr = new List<int>();
                   
                    arr.Add(boards[numberOfBoard].getValue(BoardGameOfLife.mod(i - 1, boards[numberOfBoard].SizeM), j) );
                    arr.Add(boards[numberOfBoard].getValue(BoardGameOfLife.mod(i + 1, boards[numberOfBoard].SizeM), j) );
                    arr.Add(boards[numberOfBoard].getValue(i, BoardGameOfLife.mod(j + 1, boards[numberOfBoard].SizeN)));
                    arr.Add(boards[numberOfBoard].getValue(i, BoardGameOfLife.mod(j - 1, boards[numberOfBoard].SizeN)));
                    if (neighbourhood == 1 || neighbourhood == 2 || neighbourhood == 3)
                    {

                        if (neighbourhood == 1 || neighbourhood == 2)
                        {
                            arr.Add(boards[numberOfBoard].getValue(BoardGameOfLife.mod(i + 1, boards[numberOfBoard].SizeM),
                                BoardGameOfLife.mod(j - 1, boards[numberOfBoard].SizeN)));
                            arr.Add(boards[numberOfBoard].getValue(BoardGameOfLife.mod(i - 1, boards[numberOfBoard].SizeM),
                                BoardGameOfLife.mod(j + 1, boards[numberOfBoard].SizeN)));
                        }

                        if (neighbourhood == 1 || neighbourhood == 3) { 
                            arr.Add(boards[numberOfBoard].getValue(BoardGameOfLife.mod(i + 1, boards[numberOfBoard].SizeM),
                                BoardGameOfLife.mod(j + 1, boards[numberOfBoard].SizeN)));
                            arr.Add(boards[numberOfBoard].getValue(BoardGameOfLife.mod(i - 1, boards[numberOfBoard].SizeM),
                                BoardGameOfLife.mod(j - 1, boards[numberOfBoard].SizeN)));
                        }
                    }
                    var groups = arr.GroupBy(v => v);
                    int maxCount = groups.Max(g => g.Count());
                    int mode = groups.First(g => g.Count() == maxCount).Key;
                    var tmp = arr.Max();
                    if (mode != 0 && (maxCount == arr.Count))
                        boards[(numberOfBoard + 1) % 2].setValue(i, j, mode);
                    else
                        boards[(numberOfBoard + 1) % 2].setValue(i, j, arr.Max());
                }
            }
            if (skippingCounter == sizeM * sizeN)
                changesFlag = false;

        }

        public void computeStepAbsorbingBoundaryCondition(int numberOfBoard, int neighbourhood = 0)
        {
            int skippingCounter = 0;
            for (int i = 1; i < boards[numberOfBoard].SizeM -1; i++){
                for (int j = 1; j < boards[numberOfBoard].SizeN -1; j++){

                    if (boards[numberOfBoard].getValue(i, j) != 0){
                        skippingCounter++;
                        boards[(numberOfBoard + 1) % 2].setValue(i, j, boards[numberOfBoard].getValue(i, j));
                        continue;
                    }
                    
                    var arr = new List<int>();
                    
                    if (neighbourhood == 1 || neighbourhood == 2 || neighbourhood == 3)
                    {
                        arr.Add(boards[numberOfBoard].getValue(i - 1, j));
                        arr.Add(boards[numberOfBoard].getValue(i + 1, j));
                        arr.Add(boards[numberOfBoard].getValue(i, j + 1));
                        arr.Add(boards[numberOfBoard].getValue(i, j - 1));
                        if (neighbourhood == 1 || neighbourhood == 2) {
                            arr.Add(boards[numberOfBoard].getValue(i + 1, j + 1));
                            arr.Add(boards[numberOfBoard].getValue(i - 1, j - 1));
                        }

                        if (neighbourhood == 1 || neighbourhood == 3){
                            arr.Add(boards[numberOfBoard].getValue(i + 1, j - 1));
                            arr.Add(boards[numberOfBoard].getValue(i - 1, j + 1));
                        }
                    }
                    else{
                        
                        int multiplier = (int)Math.Pow(-1, generateExponent());
                        if (neighbourhood == 4){
                            arr.Add(boards[numberOfBoard].getValue(i - 1, j));
                            arr.Add(boards[numberOfBoard].getValue(i + 1, j));
                            arr.Add(boards[numberOfBoard].getValue(i, j + multiplier));
                            arr.Add(boards[numberOfBoard].getValue(i - 1, j + multiplier ));
                            arr.Add(boards[numberOfBoard].getValue(i + 1, j + multiplier));
                        }
                        else{
                            arr.Add(boards[numberOfBoard].getValue(i, j - 1));
                            arr.Add(boards[numberOfBoard].getValue(i, j + 1));
                            arr.Add(boards[numberOfBoard].getValue(i + multiplier, j));
                            arr.Add(boards[numberOfBoard].getValue(i + multiplier, j - 1));
                            arr.Add(boards[numberOfBoard].getValue(i + multiplier, j + 1));
                        }
                    }
                    var groups = arr.GroupBy(v => v);
                    int maxCount = groups.Max(g => g.Count());
                    int mode = groups.First(g => g.Count() == maxCount).Key;
                    if (mode != 0 && (maxCount == arr.Count))
                        boards[(numberOfBoard + 1) % 2].setValue(i, j, mode);                    
                    else
                        boards[(numberOfBoard + 1) % 2].setValue(i, j, arr.Max());
                }
            }
            if (skippingCounter == (sizeM - 2) * (sizeN - 2))
                changesFlag = false;
        }

        

        public void drawOnGraphics(SolidBrush brush, Graphics graphics, PictureBox pictureBox, Grid grid, int numberOfBoard)
        {
            for (int i = 0; i < boards[numberOfBoard].SizeM; i++)
            {
                for (int j = 0; j < boards[numberOfBoard].SizeN; j++)
                {
                    var tmp = boards[numberOfBoard].getValue(i, j);
                    
                    if (tmp >0)
                    {
                        brush.Color = getColorForValue(tmp);
          
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

        public bool setValueWithRadian(int x, int y, int value, int numberOfBoard, int radian, int cellSize)
        {
            if (points.Count == 0)
            {
                boards[numberOfBoard].setValue(y, x, value);
                points.Add(new Point(x, y));
            }
            else
            {
                foreach(var i in points)
                {
                    double tmp = Math.Sqrt(Math.Pow(i.X - x, 2) + Math.Pow(i.Y - y, 2));
                    if (tmp <= radian)
                        return false;
                }
                boards[numberOfBoard].setValue(y, x, value);
                points.Add(new Point(x, y));
            }
            return true;
        }

        private Color getColorForValue(int value){
            if (colors.ContainsKey(value)){
                return colors[value];
            }
            else{
                int r, g, b;
                Color color;
                do{
                    r = rnd.Next(0, 255);
                    g = rnd.Next(0, 255);
                    b = rnd.Next(0, 255);
                    color = Color.FromArgb(r, g, b);
                } while (colors.ContainsValue(color));
                colors.Add(value, color);
                return color;
            }
        }

        private int generateExponent() {
            if (exponentCalculated)
                return exponent;
            else{
                exponentCalculated = true;
                return rnd.Next() % 2 + 1;
            }
        }
    }
}
