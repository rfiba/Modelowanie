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
            Board board = new Board((int)numericUpDown1.Value, (int)numericUpDown2.Value,(int)numericUpDown3.Value);
            //board.prepareBoard();           
            //MessageBox.Show(board.prepareBoard());
            Grid grid = new Grid((int)numericUpDown4.Value);
            Pen pen = new Pen(Color.Black, 1f);
            Graphics graphics = panel1.CreateGraphics();
            grid.draw(panel1.Width, panel1.Height, graphics, pen);
            for(int i = 0; i < board.numberOfIteration; i++)
            {
                for(int j = 0; j < board.numberOfCells; j++)
                {
                    if (board.getValue(i, j) == true)
                    {

                    }
                    else
                        j++;
                }
            }
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            
        }
    }
}
