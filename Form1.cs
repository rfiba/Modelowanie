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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
            Board board = new Board((int)numericUpDown1.Value, (int)numericUpDown2.Value,(int)numericUpDown3.Value);
            board.prepareBoard();           
            //MessageBox.Show(board.prepareBoard());
            
            Bitmap image = new Bitmap(844, 350);
            Grid grid = new Grid(pictureBox1.Width, (int)numericUpDown2.Value);
            Pen pen = new Pen(Color.Black, 1f);
            SolidBrush brush = new SolidBrush(Color.Red);
            Graphics graphics = Graphics.FromImage(image);
            grid.draw(pictureBox1.Width, pictureBox1.Height, graphics, pen);

            for (int i = 0; i < board.numberOfIteration; i++)
            {
                for (int j = 0; j < board.numberOfCells; j++)
                {
                    if (board.getValue(i, j) == true)
                    {
                        graphics.FillRectangle(brush, j * grid.cellSize + 1, i * grid.cellSize + 1, grid.cellSize - 1, grid.cellSize - 1);
                    }
                }
            }
            

            pictureBox1.Image = image;
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
