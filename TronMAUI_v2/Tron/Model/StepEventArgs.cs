using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tron.Model
{
    public class StepEventArgs : EventArgs
    {
        private int blueX;
        private int blueY;
        private int redX;
        private int redY;

        public int BlueX { get { return blueX; } }
        public int BlueY { get { return blueY; } }
        public int RedX {  get { return redX; } }
        public int RedY { get { return redY; } }

        public StepEventArgs(int blueX, int blueY, int redX, int redY)
        {
            this.blueX = blueX;
            this.blueY = blueY;
            this.redX = redX;
            this.redY = redY;
        }

    }
}
