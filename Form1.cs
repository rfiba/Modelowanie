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
            MessageBox.Show(board.prepareBoard());
        }
    }
}
