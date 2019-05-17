using System;
using System.Drawing;
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
        bool additionMode = false;
        static Timer timer;

        public GameOfLife()
        {
            InitializeComponent();
            pen = new Pen(Color.Black, 1f);
            image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(image);
            brush = new SolidBrush(Color.Red);
            pictureBox1.Image = image;
            timer = new Timer();
            timer.Tick +=  OnTimedEvent;
            timer.Interval = 500;
        }

        private void button3_Click(object sender, EventArgs e) //wstrzymaj
        {
            timer.Stop();
            listBox1.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
            manualMode = true;
            additionMode = true;
            boardCounter--;
        }

        private void button2_Click(object sender, EventArgs e) //start/wznów
        {
            manualMode = false;   
            timer.Start();
            button3.Enabled = true;
            button4.Enabled = false;
            button5.Enabled = false;
            listBox1.Enabled = false;
            image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(image);
            board.computeStep(boardCounter % 2);
            board.drawOnGraphics(brush, graphics, pictureBox1, grid, boardCounter % 2);
            grid.drawSpecificNumberOfCells((int)numericUpDown1.Value, (int)numericUpDown2.Value, graphics, pen);
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

                    board.setValueBasedOnCoordinates(me.X, me.Y, true, grid, boardCounter % 2);
                    image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                    graphics = Graphics.FromImage(image);
                    board.drawOnGraphics(brush, graphics, pictureBox1, grid, boardCounter % 2);
                    grid.drawSpecificNumberOfCells((int)numericUpDown1.Value, (int)numericUpDown2.Value, graphics, pen);
                    pictureBox1.Image = image;
                }

                if (me.Button == MouseButtons.Right)
                {
                    if (me.X >= board.sizeN * grid.cellSize || me.Y >= board.sizeM * grid.cellSize)
                        return;

                    if (board.getValueBasedOnCoordinates(me.X, me.Y, grid, boardCounter % 2) == true)
                    {
                        board.setValueBasedOnCoordinates(me.X, me.Y, false, grid, boardCounter % 2);
                        image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                        graphics = Graphics.FromImage(image);
                        board.drawOnGraphics(brush, graphics, pictureBox1, grid, boardCounter % 2);
                        grid.drawSpecificNumberOfCells((int)numericUpDown1.Value, (int)numericUpDown2.Value, graphics, pen);
                        pictureBox1.Image = image;
                    }
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void OnTimedEvent(Object source, EventArgs e)
        {
            timer.Stop();
            
            pictureBox1.Image = image;
            image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(image);
            grid.drawSpecificNumberOfCells((int)numericUpDown1.Value, (int)numericUpDown2.Value, graphics, pen);
            pictureBox1.Refresh();

            board.drawOnGraphics(brush, graphics, pictureBox1, grid, boardCounter%2);
            pictureBox1.Image = image;
            board.computeStep(boardCounter % 2);
            boardCounter++;
            timer.Start();
        }

        private void button4_Click(object sender, EventArgs e) //generuj plansze
        {
            button2.Enabled = true;
            manualMode = true;
            additionMode = true;
            image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(image);
            grid = new Grid(pictureBox1.Width, pictureBox1.Height, (int)numericUpDown1.Value, (int)numericUpDown2.Value);
            grid.drawSpecificNumberOfCells((int)numericUpDown1.Value, (int)numericUpDown2.Value, graphics, pen);
            pictureBox1.Image = image;
            board = new BoardGameOfLife((int)numericUpDown2.Value, (int)numericUpDown1.Value);
        }

        private void button5_Click(object sender, EventArgs e) //Dodaj
        {
            if (additionMode && listBox1.SelectedItem != null)
            {
                board.drawShape(boardCounter % 2, listBox1.SelectedItem.ToString());
                image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                graphics = Graphics.FromImage(image);
                grid.drawSpecificNumberOfCells((int)numericUpDown1.Value, (int)numericUpDown2.Value, graphics, pen);
                pictureBox1.Refresh();
                board.drawOnGraphics(brush, graphics, pictureBox1, grid, boardCounter % 2);
                pictureBox1.Image = image;
            }
        }

        private void GameOfLife_Load(object sender, EventArgs e)
        {

        }
    }
}
