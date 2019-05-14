using System;
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
        private bool settingMode = true;
        static Timer timer;
        private int boardCounter = 0;
        private int offset = 0;
        private bool radioButtonIsChecked = false;
        public CAWindow()
        {
            InitializeComponent();
            pen = new Pen(Color.Black, 1f);
            image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(image);
            brush = new SolidBrush(Color.Red);
            pictureBox1.Image = image;
            timer = new Timer();
            timer.Tick += OnTimedEvent;
            timer.Interval = 500;
        }

        private void OnTimedEvent(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
            
            if (manualMode == true)
            {
                MouseEventArgs me = (MouseEventArgs)e;
                if (me.Button == MouseButtons.Left)
                {
                    if (me.X >= board.SizeN * (grid.cellSize-offset) || me.Y >= board.SizeM * (grid.cellSize-offset))
                        return;
                    if (offset > 0 && (me.X < grid.cellSize || me.Y < grid.cellSize))
                        return;
                    board.setValueBasedOnCoordinates(me.X, me.Y, 5, grid, boardCounter % 2);
                    image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                    graphics = Graphics.FromImage(image);
                    board.drawOnGraphics(brush, graphics, pictureBox1, grid, boardCounter % 2);
                    grid.drawSpecificNumberOfCells((int)numericUpDown1.Value, (int)numericUpDown2.Value, graphics, pen);
                    pictureBox1.Image = image;
                }

                if (me.Button == MouseButtons.Right)
                {
                    if (me.X >= board.SizeN * (grid.cellSize - offset) || me.Y >= board.SizeM * (grid.cellSize - offset))
                        return;

                    if (offset > 0 && (me.X < grid.cellSize || me.Y < grid.cellSize))
                        return;

                    if (board.getValueBasedOnCoordinates(me.X, me.Y, grid, boardCounter % 2) > 0)
                    {
                        board.setValueBasedOnCoordinates(me.X, me.Y, 0, grid, boardCounter % 2);
                        image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                        graphics = Graphics.FromImage(image);
                        board.drawOnGraphics(brush, graphics, pictureBox1, grid, boardCounter % 2);
                        grid.drawSpecificNumberOfCells((int)numericUpDown1.Value, (int)numericUpDown2.Value, graphics, pen);
                        pictureBox1.Image = image;
                    }
                }

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
                offset = 1;
            
            //button2.Enabled = true;
            manualMode = true;
            //additionMode = true;
            board = new BoardCA((int)numericUpDown1.Value, (int)numericUpDown2.Value);
            image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(image);
            grid = new Grid(pictureBox1.Width, pictureBox1.Height, (int)numericUpDown1.Value, (int)numericUpDown2.Value);
            grid.drawSpecificNumberOfCells((int)numericUpDown1.Value, (int)numericUpDown2.Value, graphics, pen);
            pictureBox1.Image = image;
            //board = new BoardGameOfLife((int)numericUpDown2.Value, (int)numericUpDown1.Value);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex.ToString() == "Ręczny wybór pozycji")
                throw new NotImplementedException();
        }

        private void CAWindow_Load(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            radioButtonIsChecked = radioButton1.Checked;
        }

        private void radioButton1_Clicked(object sender, EventArgs e)
        {
            if(radioButton1.Checked && !radioButtonIsChecked)
                radioButton1.Checked = false;
            else
            {
                radioButton1.Checked = true;
                radioButtonIsChecked = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button4.Enabled = false;
            listBox1.Enabled = false;
            radioButton1.Enabled = false;
            numericUpDown1.Enabled = false;
            numericUpDown2.Enabled = false;
            manualMode = false;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            button4.Enabled = true;
        }

        
        private void button4_Click(object sender, EventArgs e)
        {
            button3.Enabled = false;
            button1.Enabled = true;
            listBox1.Enabled = true;
            radioButton1.Enabled = true;
            numericUpDown1.Enabled = true;
            numericUpDown2.Enabled = true;
            radioButton1.Enabled = true;
            manualMode = true;
        } 
    }
}
