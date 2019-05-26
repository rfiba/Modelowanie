﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Modelowanie_GUI
{
    public partial class CAWindow : Form
    {
        private Grid grid;
        private Bitmap image;
        private Pen pen;
        private BoardCA board;
        private SolidBrush brush;
        private Graphics graphics;
        private bool manualMode = false;
        static Timer timer;
        private int boardCounter = 0;
        private int offset = 0;
        private bool radioButtonIsChecked = false;
        private bool advacedMode;
        private int neighbourhood;
        private Random rnd;
        private int radius = 0;

        public CAWindow(bool advacedMode = false) {
            InitializeComponent();
            pen = new Pen(Color.Black, 1f);
            image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(image);
            brush = new SolidBrush(Color.Red);
            pictureBox1.Image = image;
            timer = new Timer();
            timer.Tick += OnTimedEvent;
            timer.Interval = 700;            
            this.advacedMode = advacedMode;
            rnd = new Random();
        }

        private void OnTimedEvent(object sender, EventArgs e){
            timer.Stop();
            if (board.ChangesFlag == false){
                stop();
                return;
            }
            pictureBox1.Image = image;
            image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(image);
            grid.drawSpecificNumberOfCells((int)OX.Value, (int)OY.Value, graphics, pen);
            pictureBox1.Refresh();
            pictureBox1.Image = image;
            if (offset>0)
                board.computeStepAbsorbingBoundaryCondition(boardCounter % 2, neighbourhood);
            else
                board.computeStepPeriodicBoundaryCondition(boardCounter % 2, radius, neighbourhood);
            board.drawOnGraphics(brush, graphics, pictureBox1, grid, boardCounter % 2);
            boardCounter++;
            timer.Start();
        }

        private void pictureBox1_Click(object sender, EventArgs e){
            if (manualMode == true){
                MouseEventArgs me = (MouseEventArgs)e;
                if (me.Button == MouseButtons.Left){
                    if (me.X >= board.SizeN * (grid.cellSize-offset) || me.Y >= board.SizeM * (grid.cellSize-offset))
                        return;
                    if (offset > 0 && (me.X < grid.cellSize || me.Y < grid.cellSize))
                        return;
                    board.setValueBasedOnCoordinates(me.X, me.Y, (int)D2.Value, grid, boardCounter % 2);
                    image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                    graphics = Graphics.FromImage(image);
                    board.drawOnGraphics(brush, graphics, pictureBox1, grid, boardCounter % 2);
                    grid.drawSpecificNumberOfCells((int)OX.Value, (int)OY.Value, graphics, pen);
                    pictureBox1.Image = image;
                }

                if (me.Button == MouseButtons.Right){
                    if (me.X >= board.SizeN * (grid.cellSize - offset) || me.Y >= board.SizeM * (grid.cellSize - offset))
                        return;

                    if (offset > 0 && (me.X < grid.cellSize || me.Y < grid.cellSize))
                        return;

                    if (board.getValueBasedOnCoordinates(me.X, me.Y, grid, boardCounter % 2) > 0){
                        board.setValueBasedOnCoordinates(me.X, me.Y, 0, grid, boardCounter % 2);
                        image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                        graphics = Graphics.FromImage(image);
                        board.drawOnGraphics(brush, graphics, pictureBox1, grid, boardCounter % 2);
                        grid.drawSpecificNumberOfCells((int)OX.Value, (int)OY.Value, graphics, pen);
                        pictureBox1.Image = image;
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e){ //generuj plansze
            button2.Enabled = true;
            radioButton1.Enabled = false;
            board = new BoardCA((int)OX.Value, (int)OY.Value);
            image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(image);
            grid = new Grid(pictureBox1.Width, pictureBox1.Height, (int)OX.Value, (int)OY.Value);

            if (radioButton1.Checked)
                offset = 1;
            else
                offset = 0;

            if (listBox1.SelectedItem != null){
                if (listBox1.SelectedItem.ToString() == "Ręczny wybór pozycji"){
                    D2.Maximum = D1.Value;
                    D2.Enabled = true;
                    manualMode = true;
                }
                else if (listBox1.SelectedItem.ToString() == "Losowe"){
                    Random rnd = new Random();
                    int x, y;
                    for (int i = 0; i < (int)D1.Value; i++){
                        x = rnd.Next(offset, board.SizeN - offset);
                        y = rnd.Next(offset, board.SizeM - offset);
                        board.setValue(x, y, i + 1, boardCounter % 2);
                    }

                    board.drawOnGraphics(brush, graphics, pictureBox1, grid, boardCounter % 2);
                }
                else if (listBox1.SelectedItem.ToString() == "Jednorodne"){
                    int xDistance = (board.SizeN - 2 * offset) / (int)D1.Value;
                    int yDistance = (board.SizeM - 2 * offset) / (int)D2.Value;

                    int distance = 0;
                    if (yDistance < xDistance)
                        distance = yDistance;
                    else
                        distance = xDistance;
                    if (board.SizeN - (int)D1.Value * xDistance >= (int)D1.Value)
                        xDistance++;
                    if (board.SizeM - (int)D2.Value * yDistance >= (int)D2.Value)
                        yDistance++;
                    if (true){
                        for (int i = 0, x = offset, y = offset; i < ((int)D1.Value * (int)D2.Value); i++){

                            board.setValue(x, y, i + 1, boardCounter % 2);
                            if ((i + 1) % (int)D1.Value == 0){
                                y += yDistance;
                                x = offset;
                            }
                            else
                                x += xDistance;
                        }
                        board.drawOnGraphics(brush, graphics, pictureBox1, grid, boardCounter % 2);
                    }
                }
                else if(listBox1.SelectedItem.ToString() == "Z promieniem") {
                    D2.Enabled = true;
                    
                    int x, y, unsuccess = 0;
                                      
                    for (int i = 0; i < (int)D1.Value; i++){
                        bool result = true;                        
                        for (int j = 0; j < 10; j++){
                            x = rnd.Next(offset, board.SizeN - offset);
                            y = rnd.Next(offset, board.SizeM - offset);
                            result = board.setValueWithRadian(x, y, i + 1, boardCounter % 2, (int)D2.Value, grid.cellSize);

                            if (result)
                                break;
                        }
                        if(result == false)
                            unsuccess++;
                    }
                    if(unsuccess>0)
                        MessageBox.Show($"Nie udało sie wygenerowac pozycji {unsuccess} ziaren.");
                    board.drawOnGraphics(brush, graphics, pictureBox1, grid, boardCounter % 2);
                }
            }
            grid.drawSpecificNumberOfCells((int)OX.Value, (int)OY.Value, graphics, pen);
            pictureBox1.Image = image;
            D1.Enabled = false;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e){
            if (listBox1.SelectedItem.ToString() == "Ręczny wybór pozycji"){
                label4.Text = "Ilośc rodzajów ziaren";
                D2.Enabled = false;
            }
            else if (listBox1.SelectedItem.ToString() == "Z promieniem"){
                label4.Text = "Ilośc rodzajów ziaren do wylosowania";
                label5.Text = "Promień";
                D2.Enabled = true;
            }
            else if (listBox1.SelectedItem.ToString() == "Jednorodne"){
                label4.Text = "Ilośc ziaren w wierszu";
                label5.Text = "Ilośc ziaren w kolumnie";
                D2.Enabled = true;
            }
            else if (listBox1.SelectedItem.ToString() == "Losowe"){
                label4.Text = "Ilośc rodzajów ziaren do wylosowania";
                D2.Enabled = false;
            }
            label5.Refresh();
            label4.Refresh();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e){
            radioButtonIsChecked = radioButton1.Checked;
        }

        private void radioButton1_Clicked(object sender, EventArgs e){
            if(radioButton1.Checked && !radioButtonIsChecked)
                radioButton1.Checked = false;
            else{
                radioButton1.Checked = true;
                radioButtonIsChecked = false;
            }
        }

        private void button2_Click(object sender, EventArgs e){ //start
            if (listBox2.SelectedItem != null){
                if (listBox2.SelectedItem.ToString() == "von Neumann")
                    neighbourhood = 0;
                else if (listBox2.SelectedItem.ToString() == "Moore")
                    neighbourhood = 1;
                else if (listBox2.SelectedItem.ToString() == "Heksagonalne lewe")
                    neighbourhood = 2;
                else if (listBox2.SelectedItem.ToString() == "Heksagonalne prawe")
                    neighbourhood = 3;
                else if (listBox2.SelectedItem.ToString() == "Heksagonalne losowe")
                    neighbourhood = rnd.Next() % 2 + 2;
                else if (listBox2.SelectedItem.ToString() == "Pentagonalne losowe")
                    neighbourhood = rnd.Next() % 2 + 4;
                else if (listBox2.SelectedItem.ToString() == "Z promieniem")
                {
                    neighbourhood = 11;
                    label7.Visible = true;
                    numericUpDown1.Visible = true;
                }
            }
            button1.Enabled = false;
            button3.Enabled = true;
            button4.Enabled = false;
            listBox1.Enabled = false;
            radioButton1.Enabled = false;
            OX.Enabled = false;
            OY.Enabled = false;
            D1.Enabled = false;
            D2.Enabled = false;
            manualMode = false;
            button2.Enabled = false;
            timer.Start();
            image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(image);
            
            if (neighbourhood == 11)
                radius = (int)numericUpDown1.Value;
            if (offset>0)
                board.computeStepAbsorbingBoundaryCondition(boardCounter % 2, neighbourhood);
            else
                board.computeStepPeriodicBoundaryCondition(boardCounter % 2,radius, neighbourhood);
            board.drawOnGraphics(brush, graphics, pictureBox1, grid, boardCounter % 2);
            grid.drawSpecificNumberOfCells((int)OX.Value, (int)OY.Value, graphics, pen);
            pictureBox1.Image = image;
            boardCounter++;
        }

        private void button3_Click(object sender, EventArgs e){ //stop
            stop();
        }

        private void stop(){
            timer.Stop();
            button4.Enabled = true;
            button2.Enabled = true;
            boardCounter--;
        }
        
        private void button4_Click(object sender, EventArgs e){ //ustaw
            button3.Enabled = false;
            button1.Enabled = true;
            listBox1.Enabled = true;
            radioButton1.Enabled = true;
            OX.Enabled = true;
            OY.Enabled = true;
            radioButton1.Enabled = true;
            D1.Enabled = true;
            offset = 0;
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e) {
            if (listBox2.SelectedItem.ToString() == "Z promieniem")
            {
                label7.Visible = true;
                numericUpDown1.Visible = true;
            }
            else {
                label7.Visible = false;
                numericUpDown1.Visible = false;
            }
        }
    }
}
