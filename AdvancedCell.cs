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

        public AdvancedCell()
        {
            this.state = 0;
        }

        public int State
        {
            get { return this.state; }
            set { this.state = value; }
        }
    }
}
