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
    public partial class GameOfLife : Form
    {
        Grid grid;
        Bitmap image;
        Pen pen;

        public GameOfLife()
        {
            InitializeComponent();
            grid = new Grid(10);
            pen = new Pen(Color.Black, 1f);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics graphics = Graphics.FromImage(image);
            grid.draw(pictureBox1.Width, pictureBox1.Height, graphics, pen);
            pictureBox1.Image = image;
            pictureBox1.Refresh();
            BoardGameOfLife board = new BoardGameOfLife(pictureBox1.Width / 10, pictureBox1.Height / 10, listBox1.SelectedItem.ToString());
            SolidBrush brush = new SolidBrush(Color.Red);
            board.drawOnGraphics(brush, graphics, pictureBox1, grid, 0);
            pictureBox1.Image = image;
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
