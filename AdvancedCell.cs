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
        public AdvancedCell()
        {
            this.state = 0;
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
    }
}
