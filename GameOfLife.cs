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
    public partial class GameOfLife : Form
    {
        Grid grid;
        Bitmap image;
        Pen pen;
        BoardGameOfLife board;
        SolidBrush brush;
        Graphics graphics;
        int boardCounter = 0;
        bool manualMode;
        static Timer timer;

        public GameOfLife()
        {
            InitializeComponent();
            grid = new Grid(10);
            pen = new Pen(Color.Black, 1f);
            image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(image);
            grid.draw(pictureBox1.Width, pictureBox1.Height, graphics, pen);
            brush = new SolidBrush(Color.Red);
            pictureBox1.Image = image;
            board = new BoardGameOfLife(pictureBox1.Height / 10, pictureBox1.Width / 10);
            timer = new Timer();
            timer.Tick +=  OnTimedEvent;
            timer.Interval = 1000;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            boardCounter = 0;
            if(board == null)
                board = new BoardGameOfLife(pictureBox1.Height / 10, pictureBox1.Width / 10, listBox1.SelectedItem.ToString());
            else
            {
                if (manualMode == false)
                    board.setDefaultShape(listBox1.SelectedItem.ToString());
            }
            timer.Start();
            button1.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer.Stop();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer.Start();
            image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(image);
            board.computeStep(boardCounter % 2);
            board.drawOnGraphics(brush, graphics, pictureBox1, grid, boardCounter % 2);
            grid.draw(pictureBox1.Width, pictureBox1.Height, graphics, pen);
            pictureBox1.Image = image;
            boardCounter++;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (manualMode == true)
            {
                MouseEventArgs me = (MouseEventArgs)e;
                if (me.Button == MouseButtons.Left)
                {
                    if (me.X >= board.sizeN * grid.cellSize || me.Y >= board.sizeM * grid.cellSize)
                        return;
                    //MessageBox.Show($"{me.X} {me.Y}");
                    board.setValueBasedOnCoordinates(me.X, me.Y, true, grid, boardCounter % 2);
                    image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                    graphics = Graphics.FromImage(image);
                    board.drawOnGraphics(brush, graphics, pictureBox1, grid, boardCounter % 2);
                    grid.draw(pictureBox1.Width, pictureBox1.Height, graphics, pen);
                    pictureBox1.Image = image;
                }

                if (me.Button == MouseButtons.Right)
                {
                    MessageBox.Show($"{me.X} {me.Y}");
                    if (me.X >= board.sizeN * grid.cellSize || me.Y >= board.sizeM * grid.cellSize)
                        return;

                    if (board.getValueBasedOnCoordinates(me.X, me.Y, grid, boardCounter % 2) == true)
                    {
                        board.setValueBasedOnCoordinates(me.X, me.Y, false, grid, boardCounter % 2);
                        image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                        graphics = Graphics.FromImage(image);
                        board.drawOnGraphics(brush, graphics, pictureBox1, grid, boardCounter % 2);
                        grid.draw(pictureBox1.Width, pictureBox1.Height, graphics, pen);
                        pictureBox1.Image = image;
                    }
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem.ToString() == "Ręczna definicja")
                manualMode = true;
        }

        private void OnTimedEvent(Object source, EventArgs e)
        {
            timer.Stop();
            pictureBox1.Image = image;
            image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(image);
            grid.draw(pictureBox1.Width, pictureBox1.Height, graphics, pen);
            pictureBox1.Refresh();

            board.drawOnGraphics(brush, graphics, pictureBox1, grid, boardCounter%2);
            pictureBox1.Image = image;
            board.computeStep(boardCounter % 2);
            boardCounter++;
            
            timer.Start();
        }
    }
}
