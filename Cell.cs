using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelowanie_GUI
{
    class Cell
    {
        private bool state;
        public Cell()
        {
            state = false;
        }
        public bool State
        {
            get
            {
                return state;
            }

            set
            {
                state = value;
            }
        }
    }
}
