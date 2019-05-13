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
    public partial class ChoiceWindow : Form
    {
        public ChoiceWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Automaton automaton = new Automaton();
            automaton.Closed += (s, args) => this.Close();
            automaton.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GameOfLife gameOfLife = new GameOfLife();
            gameOfLife.Closed += (s, args) => this.Close();
            gameOfLife.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CAWindow cAWindow = new CAWindow();
            cAWindow.Closed += (s, args) => this.Close();
            cAWindow.Show();
            this.Hide();
        }
    }
}
