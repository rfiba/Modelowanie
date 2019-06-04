using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelowanie_GUI
{
    class AdvancedCell
    {
        private int state;
        public double xCenter { get; set; }
        public double yCenter { get; set; }
        private int energy;
        bool recrystallizated;
        private double dislocationDensity;
        public AdvancedCell()
        {
            this.state = 0;
            this.energy = 0;
            this.recrystallizated = false;
        }

        public AdvancedCell(double xCenter, double yCenter) {
            this.state = 0;
            this.xCenter = xCenter;
            this.yCenter = yCenter;
        }

        public int State{
            get { return this.state; }
            set { this.state = value; }}
        public int Energy
        {
            get { return this.energy; }
            set { this.energy = value; }
        }

        public double DislocationDensity
        {
            get { return this.dislocationDensity; }
            set { this.dislocationDensity = value; }
        }

        public bool Recrystallizated
        {
            get { return this.recrystallizated; }
            set { this.recrystallizated = value; }
        }
    }
}
