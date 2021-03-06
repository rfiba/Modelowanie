﻿using System;
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
        private bool recrystalizationBoardPrepared;
        private List<Point> points;
        Dictionary<int, Color> colors;
        Random rnd;
        int exponent;
        double previousRo = 0;
        bool exponentCalculated;
        private bool[,,] recrystalizationBoard;
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
            recrystalizationBoard = new bool[2,sizeM, sizeN];
    }

        public void computeStepPeriodicBoundaryCondition(int numberOfBoard, int radius = 0, int neighbourhood = 0) {
            int skippingCounter = 0;
            int firstNeighbourhood = neighbourhood;
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

                    if (firstNeighbourhood == 10)
                        neighbourhood = rnd.Next() % 2 + 2;
                    else if (firstNeighbourhood == 12)
                        neighbourhood = rnd.Next() % 2 + 4;
                    var arr = new List<int>();
                   
                    
                    if (neighbourhood == 0 || neighbourhood == 1 || neighbourhood == 2 || neighbourhood == 3)
                    {
                        arr.Add(boards[numberOfBoard].getValue(BoardGameOfLife.mod(i - 1, boards[numberOfBoard].SizeM), j));
                        arr.Add(boards[numberOfBoard].getValue(BoardGameOfLife.mod(i + 1, boards[numberOfBoard].SizeM), j));
                        arr.Add(boards[numberOfBoard].getValue(i, BoardGameOfLife.mod(j + 1, boards[numberOfBoard].SizeN)));
                        arr.Add(boards[numberOfBoard].getValue(i, BoardGameOfLife.mod(j - 1, boards[numberOfBoard].SizeN)));
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
                    else if(neighbourhood == 4 || neighbourhood == 5){
                        int multiplier = (int)Math.Pow(-1, generateExponent());
                        if (neighbourhood == 4){
                            arr.Add(boards[numberOfBoard].getValue(BoardGameOfLife.mod(i - 1, boards[numberOfBoard].SizeM), j));
                            arr.Add(boards[numberOfBoard].getValue(BoardGameOfLife.mod(i + 1, boards[numberOfBoard].SizeM), j));
                            arr.Add(boards[numberOfBoard].getValue(i, BoardGameOfLife.mod(j + multiplier, boards[numberOfBoard].SizeN)));
                            arr.Add(boards[numberOfBoard].getValue(BoardGameOfLife.mod(i - 1, boards[numberOfBoard].SizeM),
                                BoardGameOfLife.mod(j + multiplier, boards[numberOfBoard].SizeN)));
                            arr.Add(boards[numberOfBoard].getValue(BoardGameOfLife.mod(i + 1, boards[numberOfBoard].SizeM),
                                BoardGameOfLife.mod(j + multiplier, boards[numberOfBoard].SizeN)));
                        }
                        else {
                            arr.Add(boards[numberOfBoard].getValue(i, BoardGameOfLife.mod(j - 1, boards[numberOfBoard].SizeN)));
                            arr.Add(boards[numberOfBoard].getValue(i, BoardGameOfLife.mod(j + 1, boards[numberOfBoard].SizeN)));
                            arr.Add(boards[numberOfBoard].getValue(BoardGameOfLife.mod(i + multiplier, boards[numberOfBoard].SizeM), j));
                            arr.Add(boards[numberOfBoard].getValue(BoardGameOfLife.mod(i + multiplier, boards[numberOfBoard].SizeM),
                                BoardGameOfLife.mod(j - 1, boards[numberOfBoard].SizeN)));
                            arr.Add(boards[numberOfBoard].getValue(BoardGameOfLife.mod(i + multiplier, boards[numberOfBoard].SizeM),
                                BoardGameOfLife.mod(j + 1, boards[numberOfBoard].SizeN)));
                        }
                    }
                    else{
                        double xLength, yLength, distance;
                        int modK, modL;
                        for(int k = BoardGameOfLife.mod(i - radius, boards[numberOfBoard].SizeM), it = 0; it < 2* radius; k =BoardGameOfLife.mod(k+1, boards[numberOfBoard].SizeM), it++){
                            for(int l = BoardGameOfLife.mod(j - radius, boards[numberOfBoard].SizeN), it2 = 0; it2 < 2* radius; l =BoardGameOfLife.mod(l+1, boards[numberOfBoard].SizeN), it2++){
                                modK = BoardGameOfLife.mod(k, boards[numberOfBoard].SizeM);
                                modL = BoardGameOfLife.mod(l, boards[numberOfBoard].SizeN);
                                xLength = calclulateDistanceBetweenCentres(i, modK, boards[numberOfBoard].getXValueCenter(i, j), boards[numberOfBoard].getXValueCenter(modK, modL));
                                yLength = calclulateDistanceBetweenCentres(j, modL, boards[numberOfBoard].getYValueCenter(i, j), boards[numberOfBoard].getYValueCenter(modK, modL));
                                distance = Math.Sqrt(xLength * xLength + yLength * yLength);
                                if (distance < radius)
                                    arr.Add(boards[numberOfBoard].getValue(k, l));
                            }
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

        public void computeStepAbsorbingBoundaryCondition(int numberOfBoard, int radius = 0, int neighbourhood = 0)
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
                    
                    if (neighbourhood == 0 || neighbourhood == 1 || neighbourhood == 2 || neighbourhood == 3)
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
                    else if (neighbourhood == 4 || neighbourhood == 5)
                    {
                        
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
                    else {
                        double xLength, yLength, distance;
                        int modK, modL;
                        for (int k = modForAbsorbingCondition(i - radius, true), it = 0; it < 2 * radius && k < sizeM - 1; k = modForAbsorbingCondition(k + 1,true), it++)
                        {
                            for (int l = modForAbsorbingCondition(j - radius, false), it2 = 0; it2 < 2 * radius && l < sizeM - 1; l = modForAbsorbingCondition(l + 1, false), it2++)
                            {
                                modK = modForAbsorbingCondition(k, true);
                                modL = modForAbsorbingCondition(l, false);
                                xLength = calclulateDistanceBetweenCentres(i, modK, boards[numberOfBoard].getXValueCenter(i, j), boards[numberOfBoard].getXValueCenter(modK, modL));
                                yLength = calclulateDistanceBetweenCentres(j, modL, boards[numberOfBoard].getYValueCenter(i, j), boards[numberOfBoard].getYValueCenter(modK, modL));
                                distance = Math.Sqrt(xLength * xLength + yLength * yLength);
                                if (distance < radius)
                                    arr.Add(boards[numberOfBoard].getValue(k, l));
                            }
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
            for (int i = 0; i < boards[numberOfBoard].SizeM; i++){
                for (int j = 0; j < boards[numberOfBoard].SizeN; j++){
                    var tmp = boards[numberOfBoard].getValue(i, j);
                    
                    if (tmp >0){
                        brush.Color = getColorForValue(tmp);
          
                        graphics.FillRectangle(brush, j * grid.cellSize + 1, i * grid.cellSize + 1, grid.cellSize - 1, grid.cellSize - 1);
                    }
                }
            }
        }

        public void drawEnergyOnGraphicsPeriodicCondition(SolidBrush brush, Graphics graphics, PictureBox pictureBox, Grid grid, int numberOfBoard) {
            int factor = 255 / 8;
            for (int i = 0; i < boards[numberOfBoard].SizeM; i++){
                for (int j = 0; j < boards[numberOfBoard].SizeN; j++){
                    var tmp = boards[numberOfBoard].getEnergy(i, j);
                    
                    brush.Color = Color.FromArgb(factor * (8 - tmp), 0, 0);
                    graphics.FillRectangle(brush, j * grid.cellSize + 1, i * grid.cellSize + 1, grid.cellSize - 1, grid.cellSize - 1);
                    
                }
            }
        }

        public void drawDislocationDensityOnGraphicsPeriodicCondition(SolidBrush brush, Graphics graphics, PictureBox pictureBox, Grid grid, int numberOfBoard) {
            double max = boards[numberOfBoard].getMaxDislocationDensity();
            if (max == 0)
                max = 0.001;
            for (int i = 0; i < boards[numberOfBoard].SizeM; i++){
                for (int j = 0; j < boards[numberOfBoard].SizeN; j++){
                    var tmp = boards[numberOfBoard].getDislocationDensity(i, j);
                    brush.Color = Color.FromArgb((int)(Math.Round((tmp/max),1)*255), 0, 0);
                    graphics.FillRectangle(brush, j * grid.cellSize + 1, i * grid.cellSize + 1, grid.cellSize - 1, grid.cellSize - 1);
                }
            }
        }

        public void drawEnergyOnGraphicsAbsorbingCondition(SolidBrush brush, Graphics graphics, PictureBox pictureBox, Grid grid, int numberOfBoard) {
            int factor = 255 / 8;
            for (int i = 1; i < boards[numberOfBoard].SizeM-1; i++){
                for (int j = 1; j < boards[numberOfBoard].SizeN-1; j++){
                    var tmp = boards[numberOfBoard].getEnergy(i, j);

                    brush.Color = Color.FromArgb(factor * (8-tmp), 0, 0);
                    graphics.FillRectangle(brush, j * grid.cellSize + 1, i * grid.cellSize + 1, grid.cellSize - 1, grid.cellSize - 1);
                }
            }
        }

        public void drawRecrystalizationOnGraphicsPeriodicCondition(SolidBrush brush, Graphics graphics, PictureBox pictureBox, Grid grid, int numberOfBoard)
        {
            int factor = 255 / 8;
            brush.Color = Color.Aqua;
            for (int i = 0; i < boards[numberOfBoard].SizeM ; i++)
            {
                for (int j = 0; j < boards[numberOfBoard].SizeN ; j++)
                {
                    var tmp = boards[numberOfBoard].getValue(i, j);



                    if (boards[numberOfBoard].checkRecrystalizated(i, j))
                        brush.Color = Color.Black;//getColorForValue(tmp + 200);
                    else if (tmp > 0)
                        brush.Color = getColorForValue(tmp);
                    graphics.FillRectangle(brush, j * grid.cellSize + 1, i * grid.cellSize + 1, grid.cellSize - 1, grid.cellSize - 1);
                }
            }
        }

        public void computeMonteCarloPeriodicCondition(int numberOfBoard, double ktFactor) {
            bool[,] checkedArray = new bool[sizeM, sizeN];
            int i;
            int j;
            for (int k = 0; k < sizeN * sizeM; k++){
                do{
                    i = rnd.Next(1, sizeM - 1);
                    j = rnd.Next(1, sizeN - 1);
                } while (checkedArray[i, j]);
                i = rnd.Next(sizeM);
                j = rnd.Next(sizeN);
                var arr = new List<int>();
                arr.Add(boards[numberOfBoard].getValue(BoardGameOfLife.mod(i - 1, boards[numberOfBoard].SizeM), j));
                arr.Add(boards[numberOfBoard].getValue(BoardGameOfLife.mod(i + 1, boards[numberOfBoard].SizeM), j));
                arr.Add(boards[numberOfBoard].getValue(i, BoardGameOfLife.mod(j + 1, boards[numberOfBoard].SizeN)));
                arr.Add(boards[numberOfBoard].getValue(i, BoardGameOfLife.mod(j - 1, boards[numberOfBoard].SizeN)));
                arr.Add(boards[numberOfBoard].getValue(BoardGameOfLife.mod(i + 1, boards[numberOfBoard].SizeM),
                                    BoardGameOfLife.mod(j - 1, boards[numberOfBoard].SizeN)));
                arr.Add(boards[numberOfBoard].getValue(BoardGameOfLife.mod(i - 1, boards[numberOfBoard].SizeM),
                    BoardGameOfLife.mod(j + 1, boards[numberOfBoard].SizeN)));
                arr.Add(boards[numberOfBoard].getValue(BoardGameOfLife.mod(i + 1, boards[numberOfBoard].SizeM),
                                    BoardGameOfLife.mod(j + 1, boards[numberOfBoard].SizeN)));
                arr.Add(boards[numberOfBoard].getValue(BoardGameOfLife.mod(i - 1, boards[numberOfBoard].SizeM),
                    BoardGameOfLife.mod(j - 1, boards[numberOfBoard].SizeN)));

                int energy = arr.Count(x => x != boards[numberOfBoard].getValue(i, j));
                int temporaryValue = arr[rnd.Next(0, arr.Count - 1)];
                int temporaryEnergy = arr.Count(x => x != temporaryValue);
                int deltaEnergy = temporaryEnergy - energy;
                double p = rnd.NextDouble();
                if (deltaEnergy < 0)
                    boards[numberOfBoard].setValue(i, j, temporaryValue);
                else if (p < Math.Exp(-((deltaEnergy) / (ktFactor))))
                    boards[numberOfBoard].setValue(i, j, temporaryValue);
            }
            
        }

        public void computeMonteCarloAbsorbingCondition(int numberOfBoard, double ktFactor) {
            bool[,] checkedArray= new bool[sizeM, sizeN];
            int i; 
            int j;
            for (int k = 0; k < (sizeN - 2) * (sizeM - 2); k++){
                do{
                    i = rnd.Next(1, sizeM - 1);
                    j = rnd.Next(1, sizeN - 1);
                } while (checkedArray[i, j]);

                var arr = new List<int>();
                arr.Add(boards[numberOfBoard].getValue(i - 1, j));
                arr.Add(boards[numberOfBoard].getValue(i + 1, j));
                arr.Add(boards[numberOfBoard].getValue(i, j + 1));
                arr.Add(boards[numberOfBoard].getValue(i, j - 1));
                arr.Add(boards[numberOfBoard].getValue(i + 1, j + 1));
                arr.Add(boards[numberOfBoard].getValue(i - 1, j - 1));
                arr.Add(boards[numberOfBoard].getValue(i + 1, j - 1));
                arr.Add(boards[numberOfBoard].getValue(i - 1, j + 1));
                int energy = arr.Count(x => x != boards[numberOfBoard].getValue(i, j));
                int temporaryValue = arr[rnd.Next(0, arr.Count - 1)];
                int temporaryEnergy = arr.Count(x => x != temporaryValue);
                int deltaEnergy = temporaryEnergy - energy;
                double p = rnd.NextDouble();
                if (deltaEnergy<0)
                    boards[numberOfBoard].setValue(i, j, temporaryValue);
                else if (p < Math.Exp(-((deltaEnergy) / (ktFactor))))
                    boards[numberOfBoard].setValue(i, j, temporaryValue);
            }
            for(i = 0; i< sizeM; i++)
            {
                for (j = 0; j < sizeN; j++)
                    boards[(numberOfBoard+1)%2].setEnergy(i, j, boards[numberOfBoard].getEnergy(i, j)); 
            }
        }
        public int getValueBasedOnCoordinates(int x, int y, Grid grid, int numberOfBoard){
            return boards[numberOfBoard].getValue(y / grid.cellSize, x / grid.cellSize);
        }

        public void setValueBasedOnCoordinates(int x, int y, int value, Grid grid, int numberOfBoard){
            boards[numberOfBoard].setValue(y / grid.cellSize, x / grid.cellSize, value);
        }

        public void setValue(int x, int y, int value, int numberOfBoard){
            boards[numberOfBoard].setValue(y , x , value);
        }

        public bool setValueWithRadian(int x, int y, int value, int numberOfBoard, int radian, int cellSize){
            if (points.Count == 0){
                boards[numberOfBoard].setValue(y, x, value);
                points.Add(new Point(x, y));
            }
            else{
                foreach(var i in points){
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

        private double calclulateDistanceBetweenCentres(int n1, int n2, double n1Center, double n2Center) {
            double result;
            if (n1 - n2 > 0)
                result = n1Center + 1 - n2Center + n1 - n2 - 1;
            else if (n1 - n2 == 0)
                result = Math.Abs(n1Center-n2Center);//jeden nad drugim;
            else
                result = 1 - n1Center + n2Center + n2 - n1 - 1;//x po prawej
            return result;
        }

        private int modForAbsorbingCondition(int i, bool sizeM) {
            if (i < 1)
                return 1;
            else if (sizeM && i > this.sizeM - 2)
                return this.sizeM - 2;
            else if(!sizeM && i > this.sizeN - 2)
                return this.sizeM - 2;
            else
                return i;
        }

        public void calculateEnergyAbsorbingCondition(int numberOfBoard) {
            for(int i = 1; i < sizeM - 1; i++){
                for(int j = 1; j < sizeN -1; j++){
                    var arr = new List<int>();
                    if(i != 1)
                        arr.Add(boards[numberOfBoard].getValue(i - 1, j));
                    if(i != sizeM-2)
                        arr.Add(boards[numberOfBoard].getValue(i + 1, j));
                    if(j != sizeN - 2)
                        arr.Add(boards[numberOfBoard].getValue(i, j + 1));
                    if(j!=1)
                        arr.Add(boards[numberOfBoard].getValue(i, j - 1));
                    if(i != sizeM -2 && j != sizeN - 2)
                        arr.Add(boards[numberOfBoard].getValue(i + 1, j + 1));
                    if(i !=1 && j!=1)
                        arr.Add(boards[numberOfBoard].getValue(i - 1, j - 1));
                    if(i != sizeM -2 && j!=1)
                        arr.Add(boards[numberOfBoard].getValue(i + 1, j - 1));
                    if(i!=1 && j != sizeN-2)
                        arr.Add(boards[numberOfBoard].getValue(i - 1, j + 1));
                    boards[numberOfBoard].setEnergy(i,j,arr.Count(x => x != boards[numberOfBoard].getValue(i, j)));
                }
            }
        }

        public void calculateEnergyPeriodicCondition(int numberOfBoard) {
            for (int i = 0; i < sizeM; i++){
                for (int j = 0; j < sizeN; j++){
                    var arr = new List<int>();
                    arr.Add(boards[numberOfBoard].getValue(BoardGameOfLife.mod(i - 1, boards[numberOfBoard].SizeM), j));
                    arr.Add(boards[numberOfBoard].getValue(BoardGameOfLife.mod(i + 1, boards[numberOfBoard].SizeM), j));
                    arr.Add(boards[numberOfBoard].getValue(i, BoardGameOfLife.mod(j + 1, boards[numberOfBoard].SizeN)));
                    arr.Add(boards[numberOfBoard].getValue(i, BoardGameOfLife.mod(j - 1, boards[numberOfBoard].SizeN)));
                    arr.Add(boards[numberOfBoard].getValue(BoardGameOfLife.mod(i + 1, boards[numberOfBoard].SizeM),
                                        BoardGameOfLife.mod(j - 1, boards[numberOfBoard].SizeN)));
                    arr.Add(boards[numberOfBoard].getValue(BoardGameOfLife.mod(i - 1, boards[numberOfBoard].SizeM),
                        BoardGameOfLife.mod(j + 1, boards[numberOfBoard].SizeN)));
                    arr.Add(boards[numberOfBoard].getValue(BoardGameOfLife.mod(i + 1, boards[numberOfBoard].SizeM),
                                        BoardGameOfLife.mod(j + 1, boards[numberOfBoard].SizeN)));
                    arr.Add(boards[numberOfBoard].getValue(BoardGameOfLife.mod(i - 1, boards[numberOfBoard].SizeM),
                        BoardGameOfLife.mod(j - 1, boards[numberOfBoard].SizeN)));
                    boards[numberOfBoard].setEnergy(i, j, arr.Count(x => x != boards[numberOfBoard].getValue(i, j)));
                }
            }
        }

        private double calculateRo(double A, double B, double time) {
            return A / B + (1 - A / B) * Math.Exp(-B * time);
        }

        private void scatterAverageRoToBoardPeriodicCondition(int numberOfBoard, double averageRo) {
            for (int i = 0; i < sizeM; i++){
                for (int j = 0; j < sizeN; j++)
                    boards[numberOfBoard].addDislocationDensity(i, j, averageRo);
            }
        }

        private void scatterAverageRoToBoardAbsorbingCondition(int numberOfBoard, double averageRo)
        {
            for (int i = 1; i < sizeM-1; i++)
            {
                for (int j = 1; j < sizeN-1; j++)
                    boards[numberOfBoard].addDislocationDensity(i, j, averageRo);
            }
        }

        private void scatterRoWithProbabilityPeriodicCondition(int numberOfBoard, double ro) {
            int numberOfPackages = (int)(ro / 100000);
            int i, j;
            int randomed;
            for(int k = 0; k < numberOfPackages; k++)
            {
                i = rnd.Next(0, sizeM);
                j = rnd.Next(0, sizeN);
                randomed = rnd.Next(0,100);
                var tmp = boards[numberOfBoard].getEnergy(i, j);
                if (tmp != 0){
                    
                    if (randomed >= 20)
                        boards[numberOfBoard].addDislocationDensity(i, j, 100000);
                    else {
                        k--;
                        continue;
                    }
                }
                else{
                    if(randomed < 20)
                        boards[numberOfBoard].addDislocationDensity(i, j, 100000);
                    else {
                        k--;
                        continue;
                    }
                }
            }
        }

        private void scatterRoWithProbabilityAbsorbingCondition(int numberOfBoard, double ro)
        {
            int numberOfPackages = (int)(ro / 100000);
            int i, j;
            int randomed;
            for (int k = 0; k < numberOfPackages; k++)
            {
                i = rnd.Next(1, sizeM-1);
                j = rnd.Next(1, sizeN-1);
                randomed = rnd.Next(0, 100);
                var tmp = boards[numberOfBoard].getEnergy(i, j);
                if (tmp != 0)
                {

                    if (randomed >= 20)
                        boards[numberOfBoard].addDislocationDensity(i, j, 100000);
                    else {
                        k--;
                        continue;
                    }
                }
                else {
                    if (randomed < 20)
                        boards[numberOfBoard].addDislocationDensity(i, j, 100000);
                    else {
                        k--;
                        continue;
                    }
                }
            }
        }
        public void scatterRoToBoardPeriodicCondition(int numberOfBoard, int numberOfSteps, double A, double B, double timeFactor, double xPercentage) {
            double previousRo = 0, deltaRo, tmp, roForEqualScatter; ;
            for (int i = 0; i < numberOfSteps; i++)
            {
                tmp = calculateRo(A, B, timeFactor * i);
                deltaRo = tmp - previousRo;
                roForEqualScatter = (deltaRo / (sizeM * sizeN)) * xPercentage;
                scatterAverageRoToBoardPeriodicCondition(numberOfBoard, roForEqualScatter);

                previousRo = tmp;
            }
        }

        public double computeRecrystalizationStepPeriodicCondition(int numberOfBoard, double A, double B, double timeStep, double xPercentage, double criticalRo, int neighbourhood) {
            bool[,] tmpRecrystalizationBoard = new bool[sizeM, sizeN]; 
            var arr = new List<TemporaryCell>();
            var arrRecrystalizated = new List<bool>();
            double ro = calculateRo(A, B, timeStep);
            double deltaRo = ro - previousRo;
            double averageRo = deltaRo / (sizeM * sizeN);
            scatterAverageRoToBoardPeriodicCondition(numberOfBoard, averageRo * xPercentage);
            scatterRoWithProbabilityPeriodicCondition(numberOfBoard, averageRo - averageRo * xPercentage);
            for (int i = 0; i < sizeM; i++)
            {
                for(int j = 0; j < sizeN; j++)
                {
                    
                    if(boards[numberOfBoard].getDislocationDensity(i,j) > criticalRo)
                    {
                        boards[(numberOfBoard+1)%2].setDislocationDensity(i, j, 0);
                        boards[(numberOfBoard + 1) % 2].makeRecrystalizated(i, j);
                        tmpRecrystalizationBoard[i, j] = true;
                        continue;
                    }
                    if (isAnyNeighbourRecrystlizatedPeriodicCondition(i, j, 0, numberOfBoard))
                    {
                        if (neighbourhood == 0 || neighbourhood == 1 || neighbourhood == 2 || neighbourhood == 3)
                        {
                            arr.Add(boards[numberOfBoard].getDislocationDensityAndRecrystalizated(BoardGameOfLife.mod(i - 1, sizeM), j));
                            arr.Add(boards[numberOfBoard].getDislocationDensityAndRecrystalizated(BoardGameOfLife.mod(i + 1, boards[numberOfBoard].SizeM), j));
                            arr.Add(boards[numberOfBoard].getDislocationDensityAndRecrystalizated(i, BoardGameOfLife.mod(j + 1, boards[numberOfBoard].SizeN)));
                            arr.Add(boards[numberOfBoard].getDislocationDensityAndRecrystalizated(i, BoardGameOfLife.mod(j - 1, boards[numberOfBoard].SizeN)));
                            if (neighbourhood == 1 || neighbourhood == 2)
                            {
                                arr.Add(boards[numberOfBoard].getDislocationDensityAndRecrystalizated(BoardGameOfLife.mod(i + 1, boards[numberOfBoard].SizeM),
                                    BoardGameOfLife.mod(j - 1, boards[numberOfBoard].SizeN)));
                                arr.Add(boards[numberOfBoard].getDislocationDensityAndRecrystalizated(BoardGameOfLife.mod(i - 1, boards[numberOfBoard].SizeM),
                                    BoardGameOfLife.mod(j + 1, boards[numberOfBoard].SizeN)));
                            }

                            if (neighbourhood == 1 || neighbourhood == 3)
                            {
                                arr.Add(boards[numberOfBoard].getDislocationDensityAndRecrystalizated(BoardGameOfLife.mod(i + 1, boards[numberOfBoard].SizeM),
                                    BoardGameOfLife.mod(j + 1, boards[numberOfBoard].SizeN)));
                                arr.Add(boards[numberOfBoard].getDislocationDensityAndRecrystalizated(BoardGameOfLife.mod(i - 1, boards[numberOfBoard].SizeM),
                                    BoardGameOfLife.mod(j - 1, boards[numberOfBoard].SizeN)));
                            }
                        }
                    }
                    boards[(numberOfBoard + 1) % 2].setDislocationDensity(i, j, boards[numberOfBoard].getDislocationDensity(i, j));
                }

                
            }

            //for (int i = 0; i < sizeM; i++)
            //{
            //    for (int j = 0; j < sizeN; j++)
            //    {
            //        recrystalizationBoard[i, j] = tmpRecrystalizationBoard[i, j];
            //    }
            //}
                    //recrystalizationBoard = tmpRecrystalizationBoard;

            previousRo = deltaRo;
            return ro;
        }

        private bool isAnyNeighbourRecrystlizatedPeriodicCondition(int i , int j, int neighbourhood, int numberOfBoard) {
            if (recrystalizationBoard[numberOfBoard,BoardGameOfLife.mod(i - 1, sizeM), j]) return true;
            if (recrystalizationBoard[numberOfBoard,BoardGameOfLife.mod(i + 1, sizeM), j]) return true;
            if (recrystalizationBoard[numberOfBoard,i, BoardGameOfLife.mod(j + 1, sizeN)]) return true;
            if (recrystalizationBoard[numberOfBoard,i, BoardGameOfLife.mod(j - 1, sizeN)]) return true;
            if (neighbourhood == 1 || neighbourhood == 2)
            {
                if (recrystalizationBoard[numberOfBoard, BoardGameOfLife.mod(i + 1, boards[numberOfBoard].SizeM),
                    BoardGameOfLife.mod(j - 1, boards[numberOfBoard].SizeN)]) return true;
                if (recrystalizationBoard[numberOfBoard, BoardGameOfLife.mod(i - 1, boards[numberOfBoard].SizeM),
                    BoardGameOfLife.mod(j + 1, boards[numberOfBoard].SizeN)]) return true;
            }

            if (neighbourhood == 1 || neighbourhood == 3)
            {
                if (recrystalizationBoard[numberOfBoard, BoardGameOfLife.mod(i + 1, boards[numberOfBoard].SizeM),
                    BoardGameOfLife.mod(j + 1, boards[numberOfBoard].SizeN)]) return true;
                if (recrystalizationBoard[numberOfBoard, BoardGameOfLife.mod(i - 1, boards[numberOfBoard].SizeM),
                    BoardGameOfLife.mod(j - 1, boards[numberOfBoard].SizeN)]) return true;
            }
            return false;
        }

        public double computeRecrystalizationStepAbsorbingCondition(int numberOfBoard, double A, double B, double timeStep, double xPercentage, double criticalRo, int neighbourhood)
        {
            bool[,] tmpRecrystalizationBoard = new bool[sizeM, sizeN];
            var arr = new List<TemporaryCell>();
            var arrRecrystalizated = new List<bool>();
            double ro = calculateRo(A, B, timeStep);
            double deltaRo = ro - previousRo;
            double averageRo = deltaRo / ((sizeM-2) * (sizeN-2));
            scatterAverageRoToBoardAbsorbingCondition(numberOfBoard, averageRo * xPercentage);
            scatterRoWithProbabilityAbsorbingCondition(numberOfBoard, averageRo - averageRo * xPercentage);
            for (int i = 0; i < sizeM; i++)
            {
                for (int j = 0; j < sizeN; j++)
                {

                    if (boards[numberOfBoard].getDislocationDensity(i, j) > criticalRo)
                    {
                        boards[(numberOfBoard + 1) % 2].setDislocationDensity(i, j, 0);
                        boards[(numberOfBoard + 1) % 2].makeRecrystalizated(i, j);
                        tmpRecrystalizationBoard[i, j] = true;
                        continue;
                    }
                    if (isAnyNeighbourRecrystlizatedPeriodicCondition(i, j, 0, numberOfBoard))
                    {
                        if (neighbourhood == 0 || neighbourhood == 1 || neighbourhood == 2 || neighbourhood == 3)
                        {
                            arr.Add(boards[numberOfBoard].getDislocationDensityAndRecrystalizated(i - 1, j));
                            arr.Add(boards[numberOfBoard].getDislocationDensityAndRecrystalizated(i + 1, j));
                            arr.Add(boards[numberOfBoard].getDislocationDensityAndRecrystalizated(i, j + 1));
                            arr.Add(boards[numberOfBoard].getDislocationDensityAndRecrystalizated(i, j - 1));
                            if (neighbourhood == 1 || neighbourhood == 2)
                            {
                                arr.Add(boards[numberOfBoard].getDislocationDensityAndRecrystalizated(i + 1, j - 1));
                                arr.Add(boards[numberOfBoard].getDislocationDensityAndRecrystalizated(i - 1,j + 1));
                            }

                            if (neighbourhood == 1 || neighbourhood == 3)
                            {
                                arr.Add(boards[numberOfBoard].getDislocationDensityAndRecrystalizated(i + 1, j + 1));
                                arr.Add(boards[numberOfBoard].getDislocationDensityAndRecrystalizated(i - 1, j - 1));
                            }
                        }
                    }
                    boards[(numberOfBoard + 1) % 2].setDislocationDensity(i, j, boards[numberOfBoard].getDislocationDensity(i, j));
                }


            }

            //for (int i = 0; i < sizeM; i++)
            //{
            //    for (int j = 0; j < sizeN; j++)
            //    {
            //        recrystalizationBoard[i, j] = tmpRecrystalizationBoard[i, j];
            //    }
            //}
            //recrystalizationBoard = tmpRecrystalizationBoard;

            previousRo = deltaRo;
            return ro;
        }

        private bool isAnyNeighbourRecrystlizatedAbsorbingCondition(int i, int j, int neighbourhood, int numberOfBoard)
        {
            if (recrystalizationBoard[numberOfBoard, i - 1, j]) return true;
            if (recrystalizationBoard[numberOfBoard, i + 1, j]) return true;
            if (recrystalizationBoard[numberOfBoard, i, j + 1]) return true;
            if (recrystalizationBoard[numberOfBoard, i, j - 1]) return true;
            if (neighbourhood == 1 || neighbourhood == 2)
            {
                if (recrystalizationBoard[numberOfBoard,i + 1, j - 1]) return true;
                if (recrystalizationBoard[numberOfBoard, i - 1, j + 1]) return true;
            }

            if (neighbourhood == 1 || neighbourhood == 3)
            {
                if (recrystalizationBoard[numberOfBoard, i + 1,j + 1]) return true;
                if (recrystalizationBoard[numberOfBoard, i - 1, j - 1]) return true;
            }
            return false;
        }

        public int getMaxEnergy(int numberOfBoard)
        {
            return boards[numberOfBoard].getMaxEnergy();
        }

        public void setEnergyBetweenBoard(int numberOfBoard)
        {
            for (int i = 0; i < sizeM; i++)
            {
                for (int j = 0; j < sizeN; j++)
                    boards[(numberOfBoard + 1) % 2].setEnergy(i, j, boards[numberOfBoard].getEnergy(i, j));
            }
        }
    }
}
