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
        public GameOfLife()
        {
            InitializeComponent();
            Grid grid = new Grid(10);
            Bitmap image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Pen pen = new Pen(Color.Black, 1f);
            Graphics graphics = Graphics.FromImage(image);
            grid.draw(pictureBox1.Width, pictureBox1.Height, graphics, pen);
            pictureBox1.Image = image;
        }
    }
}
